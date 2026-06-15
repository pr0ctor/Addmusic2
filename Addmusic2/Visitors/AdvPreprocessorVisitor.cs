using Addmusic2.Model.Constants;
using Addmusic2.Model.Interfaces;
using Addmusic2.Model.SongTree;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addmusic2.Visitors
{
    internal class AdvPreprocessorVisitor : PreprocessorBaseVisitor<ISongNode>, IPreprocessorVisitor<ISongNode>
    {

        #region General Items

        public List<ISongNode> VisitChildren(ParserRuleContext context)
        {
            var nodes = new List<ISongNode>();
            for (int i = 0; i < context.ChildCount; i++)
            {
                var child = context.GetChild(i);
                var childNode = Visit(child);
                nodes.Add(childNode);
            }
            return nodes;
        }

        public List<ISongNode> VisitChildren(ParserRuleContext context, Range elementRange)
        {
            var nodes = new List<ISongNode>();
            for (int i = elementRange.Start.Value; i < elementRange.End.Value; i++)
            {
                var child = context.GetChild(i);
                var childNode = Visit(child);
                nodes.Add(childNode);
            }
            return nodes;
        }

        public override ISongNode VisitSong([NotNull] PreprocessorParser.SongContext context)
        {
            var children = new List<ISongNode>();

            for (int i = 0; i < context.ChildCount; i++)
            {
                var child = context.GetChild(i);
                var childNode = Visit(child);
                children.Add(childNode);
            }

            var songNode = new SongNode
            {
                NodeType = SongNodeType.Root,
                Children = children,
            };
            return songNode;
        }

        public override ISongNode VisitPreprocessorElement([NotNull] PreprocessorParser.PreprocessorElementContext context)
        {
            return Visit(context.GetChild(0));
        }


        #endregion


        #region Atomics

        public ISongNode VisitReplacements([NotNull] PreprocessorParser.ReplacementsContext context)
        {
            throw new NotImplementedException();
        }

        public ISongNode VisitArbitraryText([NotNull] PreprocessorParser.ArbitraryTextContext context)
        {
            throw new NotImplementedException();
        }


        #endregion


        #region Preprocessor Items


        public override ISongNode VisitDefines([NotNull] PreprocessorParser.DefinesContext context)
        {
            return Visit(context.GetChild(0));
        }

        public ISongNode VisitDefine([NotNull] PreprocessorParser.DefineContext context)
        {
            throw new NotImplementedException();
        }

        public ISongNode VisitUndefine([NotNull] PreprocessorParser.UndefineContext context)
        {
            throw new NotImplementedException();
        }

        public ISongNode VisitError([NotNull] PreprocessorParser.ErrorContext context)
        {
            throw new NotImplementedException();
        }

        public override ISongNode VisitConditionalLogics([NotNull] PreprocessorParser.ConditionalLogicsContext context)
        {
            return Visit(context.GetChild(0));
        }

        public ISongNode VisitSingleIfStatement([NotNull] PreprocessorParser.SingleIfStatementContext context)
        {
            throw new NotImplementedException();
        }

        public ISongNode VisitIfStatementWithElseIfs([NotNull] PreprocessorParser.IfStatementWithElseIfsContext context)
        {
            throw new NotImplementedException();
        }

        public ISongNode VisitElseIf([NotNull] PreprocessorParser.ElseIfContext context)
        {
            throw new NotImplementedException();
        }

        public ISongNode VisitElse([NotNull] PreprocessorParser.ElseContext context)
        {
            throw new NotImplementedException();
        }

        public ISongNode VisitIfdef([NotNull] PreprocessorParser.IfdefContext context)
        {
            throw new NotImplementedException();
        }

        public ISongNode VisitIfndef([NotNull] PreprocessorParser.IfndefContext context)
        {
            throw new NotImplementedException();
        }

        
        #endregion


        #region Amk Items

        public ISongNode VisitGeneralAmk([NotNull] PreprocessorParser.GeneralAmkContext context)
        {
            throw new NotImplementedException();
        }

        public ISongNode VisitGeneralAmkVersion([NotNull] PreprocessorParser.GeneralAmkVersionContext context)
        {
            throw new NotImplementedException();
        }

        public ISongNode VisitAmmVersion([NotNull] PreprocessorParser.AmmVersionContext context)
        {
            throw new NotImplementedException();
        }

        public ISongNode VisitAm4Version([NotNull] PreprocessorParser.Am4VersionContext context)
        {
            throw new NotImplementedException();
        }

        public ISongNode VisitAmkV1Version([NotNull] PreprocessorParser.AmkV1VersionContext context)
        {
            throw new NotImplementedException();
        }

        public ISongNode VisitAmm([NotNull] PreprocessorParser.AmmContext context)
        {
            throw new NotImplementedException();
        }

        public ISongNode VisitAm4([NotNull] PreprocessorParser.Am4Context context)
        {
            throw new NotImplementedException();
        }

        public ISongNode VisitAmkV1([NotNull] PreprocessorParser.AmkV1Context context)
        {
            throw new NotImplementedException();
        }


        #endregion

    }
}
