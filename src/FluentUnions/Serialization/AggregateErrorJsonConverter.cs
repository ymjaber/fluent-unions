using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentUnions.Serialization;

/// <summary>
/// Provides JSON serialization and deserialization for <see cref="AggregateError"/> types.
/// </summary>
public class AggregateErrorJsonConverter : JsonConverter<AggregateError>
{
    /// <summary>
    /// Reads and converts the JSON to an <see cref="AggregateError"/>.
    /// </summary>
    public override AggregateError Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        List<Error>? errors = null;

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
                case "Errors":
                    errors = JsonSerializer.Deserialize<List<Error>>(ref reader, options);
                    break;
                // Skip other properties like Code, Message, $type since they're fixed for AggregateError
            }
        }

        if (errors == null || errors.Count == 0)
            throw new JsonException("AggregateError must have Errors property with at least one error");

        return new AggregateError(errors);
    }

    /// <summary>
    /// Writes an <see cref="AggregateError"/> as JSON.
    /// </summary>
    public override void Write(Utf8JsonWriter writer, AggregateError value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        
        writer.WriteString("$type", "AggregateError");
        writer.WriteString("Code", value.Code);
        writer.WriteString("Message", value.Message);
        
        writer.WritePropertyName("Errors");
        JsonSerializer.Serialize(writer, value.Errors, options);

        writer.WriteEndObject();
    }
}