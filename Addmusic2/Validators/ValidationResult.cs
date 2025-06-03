using Addmusic2.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Validators
{
    internal class ValidationResult : IValidationResult
    {
        public enum ResultType
        {
            Success,
            Warning,
            Failure,
            Error,
            Skip,
        }
        public ResultType Type { get; set; }
        public List<string> Message { get; set; }
        public ValidationResult() { }
    }
}
