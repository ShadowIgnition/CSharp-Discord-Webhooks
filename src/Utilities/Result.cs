
namespace SI.Discord.Webhooks.Utilities
{
    /// <summary>
    /// Represents a result containing a message of type TMessage.
    /// </summary>
    /// <typeparam name="TMessage">The type of the result message.</typeparam>
    public record Result<TMessage>
    {
        /// <summary>
        /// Implicitly converts a message of type TMessage to a Result of type TMessage.
        /// </summary>
        /// <param name="result">The message to convert.</param>
        /// <returns>A Result containing the provided message.</returns>
        public static implicit operator Result<TMessage>(TMessage result) => new(result);

        /// <summary>
        /// Implicitly converts a Result of type TMessage to a message of type TMessage.
        /// </summary>
        /// <param name="result">The Result to convert.</param>
        /// <returns>The message contained in the Result.</returns>
        public static implicit operator TMessage(Result<TMessage> result) => result.Message;

        /// <summary>
        /// Gets a static instance of Result representing a successful result.
        /// </summary>
        public static Result<TMessage> Success { get; } = new();

        /// <summary>
        /// Indicates if the result represents a failure.
        /// </summary>
        public bool Failed { get; }

        /// <summary>
        /// Indicates if the result represents a success.
        /// </summary>
        public bool Succeeded => !Failed;

        /// <summary>
        /// Gets the message contained in the result.
        /// </summary>
        public TMessage Message { get; }

        /// <summary>
        /// Initializes a new instance of the Result class representing a successful result.
        /// </summary>
        Result()
        {
            Failed = false;
            Message = default;
        }

        /// <summary>
        /// Initializes a new instance of the Result class representing a failure with the provided message.
        /// </summary>
        /// <param name="message">The message indicating the reason for the failure.</param>
        Result(TMessage message)
        {
            Failed = true;
            Message = message;
        }
    }
}
