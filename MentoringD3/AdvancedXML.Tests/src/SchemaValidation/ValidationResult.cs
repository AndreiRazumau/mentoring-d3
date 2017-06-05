using System.Collections.Generic;

namespace AdvancedXML.Tests.SchemaValidation
{
    /// <summary>
    ///     Represents the validation result.
    /// </summary>
    public class ValidationResult
    {
        public ValidationResult()
        {
            this.Messages = new Queue<string>();
            this.IsSuccess = true;
        }

        /*
            Why Queue?
        */

        /// <summary>
        ///     Gets or sets the result messages
        /// </summary>
        public Queue<string> Messages { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether validation successful.
        /// </summary>
        public bool IsSuccess { get; set; }
    }
}
