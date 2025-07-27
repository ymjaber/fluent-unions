namespace FluentUnions;

/// <summary>
/// Represents an error that can occur during an operation, containing a code and message.
/// </summary>
[Serializable]
public class Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class with a code and message.
    /// </summary>
    /// <param name="code">A unique identifier for the error type.</param>
    /// <param name="message">A human-readable description of the error.</param>
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class with only a message.
    /// The error code will be set to an empty string.
    /// </summary>
    /// <param name="message">A human-readable description of the error.</param>
    public Error(string message) : this(string.Empty, message)
    {
    }

    /// <summary>
    /// Gets the unique identifier for this error type.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the human-readable description of the error.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Determines whether two error instances are equal.
    /// </summary>
    /// <param name="left">The first error to compare.</param>
    /// <param name="right">The second error to compare.</param>
    /// <returns><see langword="true"/> if the errors are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Error? left, Error? right) => left?.Equals(right) ?? right is null;

    /// <summary>
    /// Determines whether two error instances are not equal.
    /// </summary>
    /// <param name="left">The first error to compare.</param>
    /// <param name="right">The second error to compare.</param>
    /// <returns><see langword="true"/> if the errors are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Error? left, Error? right) => !(left == right);

    /// <summary>
    /// Determines whether the specified object is equal to the current error.
    /// </summary>
    /// <param name="obj">The object to compare with the current error.</param>
    /// <returns><see langword="true"/> if the specified object is equal to the current error; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// Two errors are considered equal if they have the same type, code, and message.
    /// </remarks>
    public override bool Equals(object? obj)
    {
        if (obj is not Error other) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;

        return Code == other.Code &&
               Message == other.Message;
    }

    /// <summary>
    /// Returns the hash code for this error.
    /// </summary>
    /// <returns>A hash code for the current error based on its type and code.</returns>
    public override int GetHashCode() => HashCode.Combine(GetType(), Code);

    /// <summary>
    /// Returns a string representation of the error.
    /// </summary>
    /// <returns>A string containing the error type, code, and message.</returns>
    public override string ToString()
    {
        return Code == string.Empty
            ? $"{GetType().Name}: {Message}"
            : $"{GetType().Name}: {Code} - {Message}";
    }

    /// <summary>
    /// Implicitly converts a string message to an Error instance.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>A new Error instance with the provided message and an empty code.</returns>
    public static implicit operator Error(string message) => new Error(message);
}