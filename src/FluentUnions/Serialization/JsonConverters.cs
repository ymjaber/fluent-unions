using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentUnions.Serialization;

/// <summary>
/// Provides JSON serialization and deserialization for <see cref="Option{T}"/> types.
/// </summary>
/// <typeparam name="T">The type of the value in the option.</typeparam>
/// <remarks>
/// This converter serializes Some values as their underlying value and None as null.
/// When deserializing, null values become None and non-null values become Some.
/// </remarks>
public class OptionJsonConverter<T> : JsonConverter<Option<T>>
    where T : notnull
{
    /// <summary>
    /// Reads and converts the JSON to an <see cref="Option{T}"/>.
    /// </summary>
    /// <param name="reader">The reader to read from.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <returns>An option containing the deserialized value or None if the JSON value is null.</returns>
    public override Option<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return Option<T>.None;

        var value = JsonSerializer.Deserialize<T>(ref reader, options);
        return value is null ? Option.None : Option.Some(value);
    }

    /// <summary>
    /// Writes an <see cref="Option{T}"/> as JSON.
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The option value to convert.</param>
    /// <param name="options">Options to control the serialization behavior.</param>
    /// <remarks>
    /// Some values are written as their underlying value. None values are written as null.
    /// </remarks>
    public override void Write(Utf8JsonWriter writer, Option<T> value, JsonSerializerOptions options)
    {
        if (value.IsNone)
            writer.WriteNullValue();
        else
            JsonSerializer.Serialize(writer, value.Value, options);
    }
}

/// <summary>
/// Provides JSON serialization and deserialization for <see cref="Result{T}"/> types.
/// </summary>
/// <typeparam name="T">The type of the value in the result.</typeparam>
/// <remarks>
/// This converter serializes results as objects with the following structure:
/// - For success: { "isSuccess": true, "value": ... }
/// - For failure: { "isSuccess": false, "error": ... }
/// </remarks>
public class ResultJsonConverter<T> : JsonConverter<Result<T>>
{
    /// <summary>
    /// Reads and converts the JSON to a <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="reader">The reader to read from.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <returns>A result containing either the deserialized value or error.</returns>
    /// <exception cref="JsonException">The JSON is not in the expected format.</exception>
    public override Result<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        bool isSuccess = false;
        T? value = default;
        Error? error = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();

            string propertyName = reader.GetString()!;
            reader.Read();

            switch (propertyName.ToLower())
            {
                case "issuccess":
                    isSuccess = reader.GetBoolean();
                    break;
                case "value":
                    value = JsonSerializer.Deserialize<T>(ref reader, options);
                    break;
                case "error":
                    error = JsonSerializer.Deserialize<Error>(ref reader, options);
                    break;
            }
        }

        if (isSuccess)
        {
            if (value == null)
                throw new JsonException("Success result must have a value");
            return Result.Success(value);
        }
        else
        {
            if (error == null)
                error = new Error("DESERIALIZATION_ERROR", "Missing error information in failure result");
            return Result.Failure<T>(error);
        }
    }

    /// <summary>
    /// Writes a <see cref="Result{T}"/> as JSON.
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The result value to convert.</param>
    /// <param name="options">Options to control the serialization behavior.</param>
    /// <remarks>
    /// The output format includes an "isSuccess" property and either a "value" property (for success)
    /// or an "error" property (for failure).
    /// </remarks>
    public override void Write(Utf8JsonWriter writer, Result<T> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteBoolean("isSuccess", value.IsSuccess);
        
        if (value.IsSuccess)
        {
            writer.WritePropertyName("value");
            JsonSerializer.Serialize(writer, value.Value, options);
        }
        else
        {
            writer.WritePropertyName("error");
            JsonSerializer.Serialize(writer, value.Error, options);
        }
        
        writer.WriteEndObject();
    }
}

/// <summary>
/// Provides JSON serialization and deserialization for <see cref="Error"/> types.
/// </summary>
/// <remarks>
/// This converter handles the Error base class and its derived types,
/// preserving type information for proper deserialization.
/// </remarks>
public class ErrorJsonConverter : JsonConverter<Error>
{
    /// <summary>
    /// Determines whether the converter can convert the specified type.
    /// </summary>
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(Error).IsAssignableFrom(typeToConvert);
    }

    /// <summary>
    /// Reads and converts the JSON to an <see cref="Error"/>.
    /// </summary>
    /// <param name="reader">The reader to read from.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <returns>An error object.</returns>
    /// <exception cref="JsonException">The JSON is not in the expected format.</exception>
    public override Error Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        string? code = null;
        string? message = null;
        Dictionary<string, object>? metadata = null;
        string? errorType = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();

            string propertyName = reader.GetString()!;
            reader.Read();

            switch (propertyName)
            {
                case "$type":
                    errorType = reader.GetString();
                    break;
                case "Code":
                    code = reader.GetString();
                    break;
                case "Message":
                    message = reader.GetString();
                    break;
                case "Metadata":
                    metadata = ReadMetadata(ref reader, options);
                    break;
            }
        }

        if (code == null || message == null)
            throw new JsonException("Error must have Code and Message properties");

        // Create the appropriate error type based on the $type field
        metadata ??= new Dictionary<string, object>();
        
        return errorType switch
        {
            "ValidationError" => metadata.Count > 0 ? new ValidationError(code, message, metadata) : new ValidationError(code, message),
            "NotFoundError" => metadata.Count > 0 ? new NotFoundError(code, message, metadata) : new NotFoundError(code, message),
            "ConflictError" => metadata.Count > 0 ? new ConflictError(code, message, metadata) : new ConflictError(code, message),
            "AuthenticationError" => metadata.Count > 0 ? new AuthenticationError(code, message, metadata) : new AuthenticationError(code, message),
            "AuthorizationError" => metadata.Count > 0 ? new AuthorizationError(code, message, metadata) : new AuthorizationError(code, message),
            "AggregateError" => throw new NotSupportedException("AggregateError should use AggregateErrorJsonConverter"),
            _ => new Error(code, message)
        };
    }

    /// <summary>
    /// Writes an <see cref="Error"/> as JSON.
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The error value to convert.</param>
    /// <param name="options">Options to control the serialization behavior.</param>
    public override void Write(Utf8JsonWriter writer, Error value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        
        // Write type discriminator for derived types
        var typeName = value.GetType().Name;
        if (typeName != "Error")
        {
            writer.WriteString("$type", typeName);
        }

        writer.WriteString("Code", value.Code);
        writer.WriteString("Message", value.Message);
        
        // Only write metadata for errors that support it
        if (value is ErrorWithMetadata errorWithMetadata && errorWithMetadata.Metadata.Count > 0)
        {
            writer.WritePropertyName("Metadata");
            WriteMetadata(writer, errorWithMetadata.Metadata, options);
        }

        // Handle special properties for derived types
        if (value is AggregateError aggregateError)
        {
            writer.WritePropertyName("Errors");
            JsonSerializer.Serialize(writer, aggregateError.Errors, options);
        }

        writer.WriteEndObject();
    }

    private Dictionary<string, object>? ReadMetadata(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        var metadata = new Dictionary<string, object>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();

            string key = reader.GetString()!;
            reader.Read();

            // Handle different value types
            object value = reader.TokenType switch
            {
                JsonTokenType.String => reader.GetString()!,
                JsonTokenType.Number => reader.TryGetInt64(out var longValue) ? longValue : reader.GetDouble(),
                JsonTokenType.True => true,
                JsonTokenType.False => false,
                JsonTokenType.Null => null!,
                JsonTokenType.StartObject => JsonSerializer.Deserialize<Dictionary<string, object>>(ref reader, options)!,
                JsonTokenType.StartArray => JsonSerializer.Deserialize<List<object>>(ref reader, options)!,
                _ => throw new JsonException($"Unexpected token type: {reader.TokenType}")
            };

            metadata[key] = value;
        }

        return metadata;
    }

    private void WriteMetadata(Utf8JsonWriter writer, IReadOnlyDictionary<string, object> metadata, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        
        foreach (var kvp in metadata)
        {
            writer.WritePropertyName(kvp.Key);
            JsonSerializer.Serialize(writer, kvp.Value, options);
        }
        
        writer.WriteEndObject();
    }
}
