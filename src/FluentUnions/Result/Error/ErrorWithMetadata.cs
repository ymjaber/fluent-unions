namespace FluentUnions;

/// <summary>
/// Represents an error that can contain additional metadata for contextual information.
/// This abstract class extends the base Error class to provide metadata support for specific error types.
/// </summary>
[Serializable]
public abstract class ErrorWithMetadata : Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorWithMetadata"/> class with a code, message, and metadata.
    /// </summary>
    /// <param name="code">A unique identifier for the error type.</param>
    /// <param name="message">A human-readable description of the error.</param>
    /// <param name="metadata">Additional contextual information about the error.</param>
    protected ErrorWithMetadata(string code, string message, IDictionary<string, object> metadata)
        : base(code, message)
    {
        Metadata = metadata.AsReadOnly();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorWithMetadata"/> class with a code and message.
    /// </summary>
    /// <param name="code">A unique identifier for the error type.</param>
    /// <param name="message">A human-readable description of the error.</param>
    protected ErrorWithMetadata(string code, string message)
        : this(code, message, new Dictionary<string, object>())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorWithMetadata"/> class with only a message.
    /// The error code will be set to an empty string.
    /// </summary>
    /// <param name="message">A human-readable description of the error.</param>
    protected ErrorWithMetadata(string message)
        : this(string.Empty, message, new Dictionary<string, object>())
    {
    }

    /// <summary>
    /// Gets additional contextual information about the error.
    /// </summary>
    public IReadOnlyDictionary<string, object> Metadata { get; }

    /// <summary>
    /// Determines whether the specified object is equal to the current error.
    /// </summary>
    /// <param name="obj">The object to compare with the current error.</param>
    /// <returns><see langword="true"/> if the specified object is equal to the current error; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// Two errors are considered equal if they have the same type, code, message, and metadata.
    /// </remarks>
    public override bool Equals(object? obj)
    {
        if (!base.Equals(obj)) return false;
        if (obj is not ErrorWithMetadata other) return false;

        return MetadataEquals(other.Metadata);
    }

    /// <summary>
    /// Returns the hash code for this error.
    /// </summary>
    /// <returns>A hash code for the current error based on its type and code.</returns>
    public override int GetHashCode() => base.GetHashCode();

    private bool MetadataEquals(IReadOnlyDictionary<string, object> other)
    {
        if (Metadata.Count != other.Count) return false;

        foreach (var kvp in Metadata)
        {
            if (!other.TryGetValue(kvp.Key, out var value) ||
                !Equals(kvp.Value, value))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Returns a string representation of the error.
    /// </summary>
    /// <returns>A string containing the error type, code, message, and metadata if present.</returns>
    public override string ToString()
    {
        string result = base.ToString();

        if (Metadata.Count > 0)
        {
            var metadata = string.Join(", ", Metadata.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
            result += $" - Metadata: {metadata}";
        }

        return result;
    }
}