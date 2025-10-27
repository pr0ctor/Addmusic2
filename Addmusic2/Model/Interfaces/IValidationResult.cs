using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Addmusic2.Model.ValidationResult;

namespace Addmusic2.Model.Interfaces
{
    internal interface IValidationResult
    {
        public ResultType Type { get; set; }
        public List<string> Message { get; set; }
    }
}
