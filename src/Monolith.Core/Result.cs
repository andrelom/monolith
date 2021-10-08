using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace Monolith.Core
{
    /// <summary>
    /// It provides a protocol to standardize communication between local and remote services.
    /// </summary>
    public sealed class Result
    {
        /// <summary>
        /// Whether the process was successful.
        /// </summary>
        [JsonIgnore]
        public bool Succeeded { get; private set; }

        /// <summary>
        /// Error that occurred during the process.
        /// </summary>
        public string Error { get; private set; }

        /// <summary>
        /// If there were any validation errors during the process.
        /// </summary>
        public ImmutableArray<string>? Validations { get; private set; }

        public static Result Success()
        {
            return new()
            {
                Succeeded = true
            };
        }

        /// <summary>
        /// Create a new unsuccessful result.
        /// </summary>
        /// <param name="error">The resulting error.</param>
        /// <param name="validations">The validation errors.</param>
        /// <returns>A result.</returns>
        public static Result Failure(string error, params string[] validations)
        {
            return new()
            {
                Succeeded = false,
                Error = error,
                Validations = validations?.ToImmutableArray()
            };
        }
    }

    /// <summary>
    /// It provides a protocol to standardize communication between local and remote services.
    /// </summary>
    /// <typeparam name="TData">The resulting data type.</typeparam>
    public sealed class Result<TData>
    {
        /// <summary>
        /// Whether the process was successful.
        /// </summary>
        [JsonIgnore]
        public bool Succeeded { get; private set; }

        /// <summary>
        /// Data that was produced during the process.
        /// </summary>
        [JsonIgnore]
        public TData Data { get; private set; }

        /// <summary>
        /// Error that occurred during the process.
        /// </summary>
        public string Error { get; private set; }

        /// <summary>
        /// If there were any validation errors during the process.
        /// </summary>
        public ImmutableArray<string>? Validations { get; private set; }

        /// <summary>
        /// Create a new successful result.
        /// </summary>
        /// <param name="data">The resulting data.</param>
        /// <returns>A result.</returns>
        public static Result<TData> Success(TData data)
        {
            return new()
            {
                Succeeded = true,
                Data = data
            };
        }

        /// <summary>
        /// Create a new unsuccessful result.
        /// </summary>
        /// <param name="error">The resulting error.</param>
        /// <param name="validations">The validation errors.</param>
        /// <returns>A result.</returns>
        public static Result<TData> Failure(string error, params string[] validations)
        {
            return new()
            {
                Succeeded = false,
                Error = error,
                Validations = validations?.ToImmutableArray()
            };
        }
    }
}
