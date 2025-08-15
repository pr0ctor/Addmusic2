using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Model
{
    internal class Argument
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }
        public bool DisplayInHelp { get; set; } = true;
        public int Order { get; set; }
        public List<string> Aliases { get; set; } = new();

        public Argument(string Name, int Order, bool IsRequired, string Description, List<string> Aliases, bool DisplayInHelp = true)
        {
            this.Name = Name;
            this.Order = Order;
            this.IsRequired = IsRequired;
            this.Description = Description;
            this.Aliases = Aliases;
            this.DisplayInHelp = DisplayInHelp;
        }
    }
}
