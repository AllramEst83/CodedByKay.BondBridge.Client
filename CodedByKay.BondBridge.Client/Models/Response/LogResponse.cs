namespace CodedByKay.BondBridge.Client.Models.Response
{
    /// <summary>
    /// Represents a logged response.
    /// </summary>
    public class LogResponse
    {
        /// <summary>
        /// Gets or sets the ID of the log.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the log.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the log level.
        /// </summary>
        public string Level { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the message logged.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the exception message, if an exception occurred.
        /// </summary>
        public string ExceptionMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the stack trace, if an exception occurred.
        /// </summary>
        public string StackTrace { get; set; } = string.Empty;
    }

}
