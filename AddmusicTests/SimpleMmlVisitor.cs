using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddmusicTests
{
    internal class SimpleMmlVisitor : MmlBaseVisitor<object>
    {

        public override object Visit(IParseTree tree)
        {
            return base.Visit(tree);
        }

        /* public override object VisitSong([NotNull] MmlParser.SongContext context)
         {
             return base.VisitSong(context);
         }*/
    }
}
