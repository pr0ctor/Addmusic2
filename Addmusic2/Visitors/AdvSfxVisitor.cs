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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Addmusic2.Visitors
{
    internal class AdvSfxVisitor : SfxBaseVisitor<ISongNode>, ISfxVisitor<ISongNode>
    {

        #region General Items

        /*public ISongNode Visit(IParseTree tree)
        {
            throw new NotImplementedException();
        }*/

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

        public override ISongNode VisitSoundEffect([NotNull] SfxParser.SoundEffectContext context)
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

        public override ISongNode VisitSoundEffectElement([NotNull] SfxParser.SoundEffectElementContext context)
        {
            return Visit(context.GetChild(0));
        }

        /*public ISongNode VisitErrorNode(IErrorNode node)
        {
            throw new NotImplementedException();
        }*/

        /*public ISongNode VisitTerminal(ITerminalNode node)
        {
            throw new NotImplementedException();
        }*/

        #endregion


        #region Special Directives

        public override ISongNode VisitSpecialDirective([NotNull] SfxParser.SpecialDirectiveContext context)
        {
            return Visit(context.GetChild(0));
        }

        public override ISongNode VisitAsm([NotNull] SfxParser.AsmContext context)
        {
            var asmText = context.GetText();
            var jsrLabel = context.JsrIdentifier().GetText();
            var asmContentText = context.AsmTextBlock().GetText();

            // gets everything between the curly braces
            var asmContent = asmContentText.Trim()[1..^1].Trim()
                .Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var asmBuilder = new StringBuilder();

            foreach( var line in asmContent)
            {
                var trimmedLine = line.Trim();
                if(trimmedLine.Length == 0 )
                {
                    continue;
                }

                var commentIndex = trimmedLine.IndexOf(";");

                if( commentIndex != -1 )
                {
                    var lineWithouComment = trimmedLine[..commentIndex].Trim();
                    asmBuilder.AppendLine(lineWithouComment);
                }
                else
                {
                    asmBuilder.AppendLine(trimmedLine);
                }
            }

            var asmPayload = new SfxAsmPayload
            {
                JsrLabelName = jsrLabel,
                AsmContentText = asmBuilder.ToString(),
            };
            var asmNode = new DirectiveNode
            {
                NodeType = SongNodeType.Asm,
                NodeSource = asmText,
                Payload = asmPayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
            return asmNode;
        }

        public override ISongNode VisitJsr([NotNull] SfxParser.JsrContext context)
        {
            var jsrText = context.GetText();
            var jsrLabel = context.JsrIdentifier().GetText();

            var jsrPayload = new SfxJsrPayload
            {
                JsrLabelName = jsrLabel,
            };
            var jsrNode = new DirectiveNode
            {
                NodeType = SongNodeType.Jsr,
                NodeSource = jsrText,
                Payload = jsrPayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
            return jsrNode;
        }

        #endregion


        #region Atomics

        public override ISongNode VisitAtomics([NotNull] SfxParser.AtomicsContext context)
        {
            return Visit(context.GetChild(0));
        }

        public override ISongNode VisitDefaultLength([NotNull] SfxParser.DefaultLengthContext context)
        {
            var defaultLengthText = context.GetText();
            var lengthValue = int.Parse(defaultLengthText[1..]);
            var defaultLengthPayload = new DefaultLengthPayload(lengthValue);
            var defaultLengthNode = new AtomicNode
            {
                NodeType = SongNodeType.DefaultLength,
                NodeSource = defaultLengthText,
                Payload = defaultLengthPayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };

            return defaultLengthNode;
        }

        public override ISongNode VisitInstrument([NotNull] SfxParser.InstrumentContext context)
        {
            var instrumentText = context.GetText();
            var instrumentNumberText = instrumentText[1..];

            var splitData = instrumentNumberText.Split(",");

            var instrumentPayload = new SfxInstrumentPayload
            {
                InstrumentNumber = int.Parse(splitData[0]),
            };

            if(splitData.Length > 1 )
            {
                instrumentPayload.NoiseHexValue = splitData[1];
            }

            var instrumentNode = new AtomicNode
            {
                NodeType = SongNodeType.Instrument,
                NodeSource = instrumentText,
                Payload = instrumentPayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };

            return instrumentNode;
        }

        public override ISongNode VisitLowerOctave([NotNull] SfxParser.LowerOctaveContext context)
        {
            return new AtomicNode
            {
                NodeType = SongNodeType.LowerOctave,
                NodeSource = context.GetText(),
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
        }

        public override ISongNode VisitNakedTie([NotNull] SfxParser.NakedTieContext context)
        {
            var tieText = context.GetText();
            var hasEquals = (tieText.IndexOf('=') != -1) ? true : false;
            var dotCount = tieText.Count(t => t == '.');
            var duration = tieText.Replace(".", "")[((hasEquals) ? 2 : 1)..];
            var tiePayload = new TiePayload
            {
                Duration = int.Parse(duration),
                DotCount = dotCount,
            };
            var tieNode = new AtomicNode
            {
                NodeType = SongNodeType.Tie,
                NodeSource = tieText,
                Payload = tiePayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
            return tieNode;
        }

        public override ISongNode VisitNote([NotNull] SfxParser.NoteContext context)
        {
            var noteText = context.GetText();
            var notePayload = new NotePayload();
            var noteRegex = Regexes.NoteRegex();
            var matches = noteRegex.Match(noteText);

            // skip the first group since its the full match value
            var groups = matches.Groups.Values.ToList().Skip(1);
            notePayload.NoteValue = groups.First().Value;
            foreach (var group in groups.Skip(1))
            {
                var groupValue = group.Value;
                if (groupValue.Length == 0)
                {
                    continue;
                }
                if (groupValue == "+" || groupValue == "-")
                {
                    notePayload.Accidental = (groupValue.Contains("+")) ? NotePayload.Accidentals.Sharp : NotePayload.Accidentals.Flat;
                }
                else if (groupValue.Contains("^"))
                {
                    var ties = groupValue.Split("^").Where(v => v.Length > 0).ToList();
                    foreach (var tie in ties)
                    {
                        var duration = tie.Replace(".", "");
                        var dotCount = tie.Count(t => t == '.');
                        notePayload.ConnectedTies.Add(new AtomicNode
                        {
                            NodeType = SongNodeType.Tie,
                            NodeSource = tie,
                            Payload = new TiePayload
                            {
                                Duration = int.Parse(duration),
                                DotCount = dotCount
                            },
                            LineNumber = context.Start.Line,
                            ColumnNumber = context.Start.Column,
                        });
                    }
                }
                else if (groupValue.Contains(".") && !groupValue.Contains("^"))
                {
                    var dotCount = groupValue.Count(t => t == '.');
                    notePayload.DotCount = dotCount;
                }
                else
                {
                    var durationValue = groupValue.Replace("=", "");
                    notePayload.Duration = int.Parse(durationValue);
                }
            }

            var noteNode = new AtomicNode
            {
                NodeType = SongNodeType.Note,
                NodeSource = noteText,
                Payload = notePayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
            return noteNode;
        }

        public override ISongNode VisitOctave([NotNull] SfxParser.OctaveContext context)
        {
            var octaveText = context.GetText();
            var octaveNumber = int.Parse(octaveText[1..]);
            var octavePayload = new OctavePayload(octaveNumber);
            var octaveNode = new AtomicNode
            {
                NodeType = SongNodeType.Octave,
                NodeSource = octaveText,
                Payload = octavePayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
            return octaveNode;
        }

        public override ISongNode VisitRaiseOctave([NotNull] SfxParser.RaiseOctaveContext context)
        {
            return new AtomicNode
            {
                NodeType = SongNodeType.RaiseOctave,
                NodeSource = context.GetText(),
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
        }

        public override ISongNode VisitRest([NotNull] SfxParser.RestContext context)
        {
            var restText = context.GetText();
            var restPayload = new NotePayload();
            var restRegex = Regexes.RestRegex();
            var matches = restRegex.Match(restText);

            // skip the first group since its the full match value
            var groups = matches.Groups.Values.ToList().Skip(1);
            restPayload.NoteValue = groups.First().Value;
            foreach (var group in groups.Skip(1))
            {
                var groupValue = group.Value;
                if (groupValue.Length == 0)
                {
                    continue;
                }

                if (groupValue.Contains("^"))
                {
                    var ties = groupValue.Split("^").Where(v => v.Length > 0).ToList();
                    foreach (var tie in ties)
                    {
                        var duration = tie.Replace(".", "");
                        var dotCount = tie.Count(t => t == '.');
                        restPayload.ConnectedTies.Add(new AtomicNode
                        {
                            NodeType = SongNodeType.Tie,
                            NodeSource = tie,
                            Payload = new TiePayload
                            {
                                Duration = int.Parse(duration),
                                DotCount = dotCount
                            },
                            LineNumber = context.Start.Line,
                            ColumnNumber = context.Start.Column,
                        });
                    }
                }
                else if (groupValue.Contains(".") && !groupValue.Contains("^"))
                {
                    var dotCount = groupValue.Count(t => t == '.');
                    restPayload.DotCount = dotCount;
                }
                else
                {
                    var durationValue = groupValue.Replace("=", "");
                    restPayload.Duration = int.Parse(durationValue);
                }
            }

            var noteNode = new AtomicNode
            {
                NodeType = SongNodeType.Rest,
                NodeSource = restText,
                Payload = restPayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
            return noteNode;
        }

        public override ISongNode VisitVolume([NotNull] SfxParser.VolumeContext context)
        {
            var volumeText = context.GetText();
            var sfxVolumePayload = new SfxVolumePayload();
            
            if(volumeText.Contains(','))
            {
                var volumeValues = volumeText[1..].Split(',').ToList();
                sfxVolumePayload.LeftVolumeValue = int.Parse(volumeValues[0]);
                sfxVolumePayload.RightVolumeValue = int.Parse(volumeValues[1]);
            }
            else
            {
                sfxVolumePayload.Volume = int.Parse(volumeText[1..]);
            }

            var volumeNode = new AtomicNode
            {
                NodeType = SongNodeType.SfxVolume,
                NodeSource = volumeText,
                Payload = sfxVolumePayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
            return volumeNode;
        }


        #endregion


        #region Composites

        public override ISongNode VisitPitchslide([NotNull] SfxParser.PitchslideContext context)
        {
            var pitchslideText = context.GetText();
            var pitchslidePayload = new PitchSlidePayload();
            foreach (var childIndex in Enumerable.Range(0, context.ChildCount))
            {
                var child = context.GetChild(childIndex);
                if (child.GetText().Equals("&"))
                {
                    pitchslidePayload.Nodes.Add(new SongNode
                    {
                        NodeType = SongNodeType.Empty
                    });
                }
                else
                {
                    var childNode = Visit(child);
                    pitchslidePayload.Nodes.Add(childNode);
                }
            }
            var pitchslideNode = new CompositeNode
            {
                NodeType = SongNodeType.PitchSlide,
                NodeSource = pitchslideText,
                Payload = pitchslidePayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
            return pitchslideNode;
        }

        public override ISongNode VisitTriplet([NotNull] SfxParser.TripletContext context)
        {
            var tripletText = context.GetText();
            var rangeLowerBound = 1;
            var rangeUpperBound = context.ChildCount - 2;
            var childRange = new Range(rangeLowerBound, rangeUpperBound);
            var childNodes = VisitChildren(context, childRange);
            var tripletNode = new CompositeNode
            {
                NodeType = SongNodeType.Triplet,
                NodeSource = tripletText,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
                Children = childNodes,
            };
            return tripletNode;
        }

        #endregion


        #region HexCommands

        public override ISongNode VisitHexCommands([NotNull] SfxParser.HexCommandsContext context)
        {
            return Visit(context.GetChild(0));
        }

        public override ISongNode VisitHexNumber([NotNull] SfxParser.HexNumberContext context)
        {
            var hexNumberText = context.GetText();
            var hexNumberPayload = new HexNumberPayload
            {
                HexValue = hexNumberText,
            };

            var hexNumberNode = new CompositeNode
            {
                NodeType = SongNodeType.HexCommand,
                NodeSource = hexNumberText,
                Payload = hexNumberPayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };

            return hexNumberNode;
        }

        public override ISongNode VisitDdPitchBlendCommand([NotNull] SfxParser.DdPitchBlendCommandContext context)
        {
            var dbPitchBlendText = context.GetText();
            var hexCommandValue = context.NDD().GetText();
            var values = context.hexNumber().Select(h => h.GetText()).ToList();
            var noteData = new List<ISongNode>();
            var blendItems = context.ddPitchBlendItems();
            if(blendItems != null)
            {
                foreach (var index in Enumerable.Range(0, blendItems.ChildCount))
                {
                    noteData.Add(Visit(blendItems.GetChild(index)));
                }
            }
            return new HexNode
            {
                NodeType = SongNodeType.Hex,
                NodeSource = dbPitchBlendText,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
                CommandType = HexCommands.DDPitchBlend,
                HexCommand = hexCommandValue,
                HexValues = values,
                Children = noteData,
            };
        }

        public override ISongNode VisitE0SfxPriority([NotNull] SfxParser.E0SfxPriorityContext context)
        {
            var eaVibratoFadeText = context.GetText();
            var hexCommandValue = context.NE0().GetText();
            var values = new List<string>
            {
                context.HexNumber().GetText(),
            };
            return new HexNode
            {
                NodeType = SongNodeType.Hex,
                NodeSource = eaVibratoFadeText,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
                CommandType = HexCommands.E0SfxPriority,
                HexCommand = hexCommandValue,
                HexValues = values,
            };
        }


        #endregion

    }
}
