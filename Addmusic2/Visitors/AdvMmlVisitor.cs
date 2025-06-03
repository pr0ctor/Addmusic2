using Addmusic2.Helpers;
using Addmusic2.Model;
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
    internal class AdvMmlVisitor : MmlBaseVisitor<ISongNode>//, IMmlVisitor<ISongNode>
    {
        #region General Items

       /* public override ISongNode VisitChildren(IRuleNode node)
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
            for(int i = elementRange.Start.Value; i < elementRange.End.Value; i++)
            {
                var child = context.GetChild(i);
                var childNode = Visit(child);
                nodes.Add(childNode);
            }
            return nodes;
        }

        /*public override ISongNode VisitErrorNode(IErrorNode node)
        {
            throw new NotImplementedException();
        }*/

        public override ISongNode VisitSong([NotNull] MmlParser.SongContext context)
        {
            var children = VisitChildren(context);
            var songNode = new SongNode
            {
                NodeType = SongNodeType.Root,
                Children = children,
            };
            return songNode;
        }

        public override ISongNode VisitSongElement([NotNull] MmlParser.SongElementContext context)
        {
            return Visit(context.GetChild(0));
        }

        /*public override ISongNode VisitTerminal(ITerminalNode node)
        {
            throw new NotImplementedException();
        }*/

        /*ISongNode IParseTreeVisitor<ISongNode>.Visit(IParseTree tree)
        {
            throw new NotImplementedException();
        }*/

        #endregion


        #region Special Directives

        /*public ISongNode VisitAmk([NotNull] MmlParser.AmkContext context)
        {
            return Visit(context.GetChild(0));
        }*/

        public override ISongNode VisitGeneralAmkVersion([NotNull] MmlParser.GeneralAmkVersionContext context)
        {
            var amkVersionText = context.GetText();
            var amkPayload = new AmkVersionPayload();
            var amkVersion = context.amkVersion();
            amkPayload.AmkVersionType = AmkVersionPayload.AmkType.Amk;
            if(amkVersionText.Contains("="))
            {
                amkPayload.AmkVersion = "1";
            }
            else
            {
                amkPayload.AmkVersion = amkVersion.NUMBERS().GetText();
            }
            var amkVersionNode = new DirectiveNode
            {
                NodeType = SongNodeType.Amk,
                NodeSource = amkVersionText,
                Payload = amkPayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
            return amkVersionNode;
        }

        public override ISongNode VisitAmmVersion([NotNull] MmlParser.AmmVersionContext context)
        {
            var ammVersionText = context.GetText();
            var amkPayload = new AmkVersionPayload();
            amkPayload.AmkVersionType = AmkVersionPayload.AmkType.Amm;
            var amkVersionNode = new DirectiveNode
            {
                NodeType = SongNodeType.Amk,
                NodeSource = ammVersionText,
                Payload = amkPayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
            return amkVersionNode;
        }

        public override ISongNode VisitAm4Version([NotNull] MmlParser.Am4VersionContext context)
        {
            var am4VersionText = context.GetText();
            var amkPayload = new AmkVersionPayload();
            amkPayload.AmkVersionType = AmkVersionPayload.AmkType.Amk;
            amkPayload.AmkVersion = "4";
            var amkVersionNode = new DirectiveNode
            {
                NodeType = SongNodeType.Amk,
                NodeSource = am4VersionText,
                Payload = amkPayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
            return amkVersionNode;
        }

        /*public override ISongNode VisitAmm([NotNull] MmlParser.AmmContext context)
        {
            throw new NotImplementedException();
        }

        public override ISongNode VisitAm4([NotNull] MmlParser.Am4Context context)
        {
            throw new NotImplementedException();
        }*/

        public override ISongNode VisitHalvetempo([NotNull] MmlParser.HalvetempoContext context)
        {
            return new DirectiveNode
            {
                NodeType = SongNodeType.Halvetempo,
                NodeSource = context.GetText(),
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
        }
        
        public override ISongNode VisitInstruments([NotNull] MmlParser.InstrumentsContext context)
        {
            var instrumentsText = context.GetText();
            var instruments = new List<InstrumentDefinition>();
            var instrumentsPayload = new InstrumentsPayload();
            // Children will always contain  " #instruments { } " and if they arent there or there isnt anything contained, return an empty node
            if (context.ChildCount <= 3)
            {
                return new DirectiveNode
                {
                    NodeType = SongNodeType.Instruments,
                    NodeSource = instrumentsText,
                    Payload = instrumentsPayload,
                    LineNumber = context.Start.Line,
                    ColumnNumber = context.Start.Column,
                };
            }

            var instrumentList = context.instrumentsList();
            foreach (var instrument in instrumentList)
            {
                var instrumentDefinition = new InstrumentDefinition();

                if (instrument.GetType().Name == nameof(MmlParser.InstrumentListItemContext))
                {
                    var instr = instrument as MmlParser.InstrumentListItemContext;
                    var instrumentNode = VisitInstrument(instr.instrument());
                    instrumentDefinition.Type = InstrumentDefinition.InstrumentType.Number;
                    instrumentDefinition.InstrumentNumber = instrumentNode;
                    instrumentDefinition.HexSettings = instr.HexNumber().Select(h => h.GetText()).ToList();
                }
                else if(instrument.GetType().Name == nameof(MmlParser.NamedInstrumentListItemContext))
                {
                    var instr = instrument as MmlParser.NamedInstrumentListItemContext;
                    instrumentDefinition.Type = InstrumentDefinition.InstrumentType.Sample;
                    instrumentDefinition.SampleName = instr.StringLiteral().GetText();
                    instrumentDefinition.HexSettings = instr.HexNumber().Select(h => h.GetText()).ToList();
                }
                else if(instrument.GetType().Name == nameof(MmlParser.NoiseInstrumentListItemContext))
                {
                    var instr = instrument as MmlParser.NoiseInstrumentListItemContext;
                    var noiseNode = VisitNoiseNote(instr.noiseNote());
                    instrumentDefinition.Type = InstrumentDefinition.InstrumentType.Noise;
                    instrumentDefinition.NoiseData = noiseNode;
                    instrumentDefinition.HexSettings = instr.HexNumber().Select(h => h.GetText()).ToList();
                }
                else
                {
                    throw new Exception("Invalid Instrument");
                }

                instruments.Add(instrumentDefinition);
            }

            if(instruments.Count > 0)
            {
                instrumentsPayload.Instruments = instruments;
            }

            var instrumentsNode = new DirectiveNode
            {
                NodeType = SongNodeType.Instruments,
                NodeSource = context.GetText(),
                Payload = instrumentsPayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };

            return instrumentsNode;
        }

        // Don't need because instruments handles the necessary logic
        /*public override ISongNode VisitNamedInstrumentListItem([NotNull] MmlParser.NamedInstrumentListItemContext context)
        {
            throw new NotImplementedException();
        }*/

        // Don't need because instruments handles the necessary logic
        /*public override ISongNode VisitInstrumentListItem([NotNull] MmlParser.InstrumentListItemContext context)
        {
            throw new NotImplementedException();
        }*/

        // Don't need because instruments handles the necessary logic
        /*public override ISongNode VisitNoiseInstrumentListItem([NotNull] MmlParser.NoiseInstrumentListItemContext context)
        {
            throw new NotImplementedException();
        }*/

        public override ISongNode VisitOptionGroup([NotNull] MmlParser.OptionGroupContext context)
        {
            var optionGroupText = context.GetText();
            var itemRange = new Range(1, context.ChildCount - 3);
            var childNodes = VisitChildren(context, itemRange);
            var optionGroupNode = new DirectiveNode
            {
                NodeType = SongNodeType.OptionGroup,
                NodeSource = optionGroupText,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
                Children = childNodes
            };
            return optionGroupNode;
        }

        public override ISongNode VisitSingleOption([NotNull] MmlParser.SingleOptionContext context)
        {
            var optionItem = context.optionItem();
            var optionNode = VisitOptionItem(optionItem);
            return optionNode;
        }

        public override ISongNode VisitOptionItem([NotNull] MmlParser.OptionItemContext context)
        {
            var optionItemText = context.GetText();
            var payload = new OptionPayload();
            if (optionItemText.ToLower().Contains("tempoimmunity"))
            {
                payload.Option = OptionPayload.OptionType.TempoImmunity;
            }
            else if (optionItemText.ToLower().Contains("dividetempo"))
            {
                payload.Option = OptionPayload.OptionType.DivideTempo;
                payload.OptionValue = int.Parse(context.NUMBERS().GetText());
            }
            else if (optionItemText.ToLower().Contains("smwvtable"))
            {
                payload.Option = OptionPayload.OptionType.Smwvtable;
            }
            else if (optionItemText.ToLower().Contains("nspcvtable"))
            {
                payload.Option = OptionPayload.OptionType.Nspcvtable;
            }
            else if (optionItemText.ToLower().Contains("noloop"))
            {
                payload.Option = OptionPayload.OptionType.Noloop;
            }
            else if (optionItemText.ToLower().Contains("amk109hotpatch"))
            {
                payload.Option = OptionPayload.OptionType.Amk109hotpatch;
            }
            else
            {

            }

            return new DirectiveNode
            {
                NodeType = SongNodeType.Option,
                NodeSource = context.GetText(),
                Payload = payload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
        }

        public override ISongNode VisitPad([NotNull] MmlParser.PadContext context)
        {
            var padLength = context.HexNumber();
            var padNodePayload = new PadPayload(padLength.GetText());
            var padNode = new DirectiveNode
            {
                NodeType = SongNodeType.Pad,
                Payload = padNodePayload,
                NodeSource = context.GetText(),
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };

            return padNode;
        }

        public override ISongNode VisitPath([NotNull] MmlParser.PathContext context)
        {
            var pathText = context.StringLiteral();
            var pathNodePayload = new PathPayload(pathText.GetText());
            var pathNode = new DirectiveNode
            {
                NodeType = SongNodeType.Path,
                Payload = pathNodePayload,
                NodeSource = context.GetText(),
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };

            return pathNode;
        }

        // Don't need because replacements are handled in the preprocessing step
        // Handled anyway in order to preserve structure
        public override ISongNode VisitReplacements([NotNull] MmlParser.ReplacementsContext context)
        {
            return new SongNode
            {
                NodeType = SongNodeType.Replacement,
                NodeSource = context.GetText(),
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
        }

        public override ISongNode VisitSamples([NotNull] MmlParser.SamplesContext context)
        {
            var samplesText = context.GetText();
            var samplesPayload = new SamplesPayload();

            var sampleListContext = context.samplesList();
            var sampleOptimization = sampleListContext.SampleOptimization();
            var listOfSamples = sampleListContext.StringLiteral().Select(s => s.GetText()).ToList();

            samplesPayload.SampleGroupPath = (sampleOptimization != null) ? sampleOptimization.GetText() : "";
            samplesPayload.Samples = listOfSamples;

            var samplesNode = new DirectiveNode
            {
                NodeType = SongNodeType.Samples,
                Payload = samplesPayload,
                NodeSource = samplesText,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };

            return samplesNode;
        }

        // Don't need because Samples handles the smaples list
        public override ISongNode VisitSamplesList([NotNull] MmlParser.SamplesListContext context)
        {
            throw new NotImplementedException();
        }

        public override ISongNode VisitSoundChannel([NotNull] MmlParser.SoundChannelContext context)
        {
            var soundChannelText = context.GetText() ?? "";
            var soundChannelNumber = int.Parse(soundChannelText.Substring(1, 1));
            var channelPayload = new ChannelPayload
            {
                ChannelNumber = soundChannelNumber,
            };
            var childNodes = VisitChildren(context);
            var channelNode = new DirectiveNode
            {
                NodeType = SongNodeType.Channel,
                NodeSource = soundChannelText,
                Payload = channelPayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
                Children = childNodes,
            };
            return channelNode;
        }

        public override ISongNode VisitSpc([NotNull] MmlParser.SpcContext context)
        {
            var spcText = context.GetText();
            var spcPayload = new SpcPayload();

            // Children will always contain  " #spc { } " and if they arent there or there isnt anything contained, return an empty node
            if(context.ChildCount <= 3)
            {
                return new DirectiveNode
                {
                    NodeType = SongNodeType.SPC,
                    NodeSource = spcText,
                    Payload = spcPayload,
                    LineNumber = context.Start.Line,
                    ColumnNumber = context.Start.Column,
                };
            }

            // Start at the third item and continue for the number of children found
            foreach ( var childIndex in Enumerable.Range(2, context.ChildCount - 3 ))
            {
                var child = context.GetChild(childIndex);
                // Guard against invalid types. Should never happen due to Range check and antlr grammer enforcement
                if(child.GetType().Name != nameof(MmlParser.SpcListContext))
                {
                    continue;
                }

                spcPayload.AssignValue(child.GetChild(0).GetText(), child.GetChild(1).GetText() ?? "");
            }

            var spcNode = new DirectiveNode
            {
                NodeType = SongNodeType.SPC,
                NodeSource = spcText,
                Payload = spcPayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };

            return spcNode;
        }

        // Don't need because VisitSpc handles the list items
        public override ISongNode VisitSpcList([NotNull] MmlParser.SpcListContext context)
        {
            throw new NotImplementedException();
        }

        public override ISongNode VisitSpecialDirective([NotNull] MmlParser.SpecialDirectiveContext context)
        {
            return Visit(context.GetChild(0));
        }

        #endregion


        #region Atomics
        
        public override ISongNode VisitAtomics([NotNull] MmlParser.AtomicsContext context)
        {
            if(context.ChildCount > 1)
            {
                Console.WriteLine("atomic with more than one child.");
                return new SongNode();
            }

            return Visit(context.GetChild(0));
        }

        public override ISongNode VisitDefaultLength([NotNull] MmlParser.DefaultLengthContext context)
        {
            var defaultLengthText = context.GetText();
            var lengthValue = int.Parse(defaultLengthText.Substring(1));
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

        public override ISongNode VisitGlobalVolume([NotNull] MmlParser.GlobalVolumeContext context)
        {
            var globalVolumeText = context.GetText();
            var globalVolumePayload = new VolumePayload();
            var values = globalVolumeText.Substring(1).Split(',').ToList();
            globalVolumePayload.Volume = int.Parse(values.First());
            if(values.Count > 1)
            {
                globalVolumePayload.FadeValue = int.Parse(values[1]);
            }
            var globalVolumeNode = new AtomicNode
            {
                NodeType = SongNodeType.GlobalVolume,
                NodeSource = globalVolumeText,
                Payload = globalVolumePayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
            return globalVolumeNode;
        }

        public override ISongNode VisitInstrument([NotNull] MmlParser.InstrumentContext context)
        {
            var instrumentText = context.GetText();
            var instrumentNumber = int.Parse(instrumentText.Substring(1));
            var instrumentPayload = new InstrumentPayload(instrumentNumber);
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

        public override ISongNode VisitLowerOctave([NotNull] MmlParser.LowerOctaveContext context)
        {
            return new AtomicNode
            {
                NodeType = SongNodeType.LowerOctave,
                NodeSource = context.GetText(),
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
        }

        public override ISongNode VisitNakedTie([NotNull] MmlParser.NakedTieContext context)
        {
            var tieText = context.GetText();
            var hasEquals = (tieText.IndexOf('=') != -1) ? true : false;
            var dotCount = tieText.Count(t => t == '.');
            var duration = tieText.Replace(".","")[((hasEquals) ? 2 : 1)..];
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

        public override ISongNode VisitNoiseNote([NotNull] MmlParser.NoiseNoteContext context)
        {
            var noiseText = context.GetText();
            var noiseValue = noiseText.Substring(1);
            var noisePayload = new NoisePayload(noiseValue);
            var noiseNode = new AtomicNode
            {
                NodeType = SongNodeType.Noise,
                NodeSource = noiseText,
                Payload = noisePayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
            return noiseNode;
        }

        public override ISongNode VisitNoloopCommand([NotNull] MmlParser.NoloopCommandContext context)
        {
            return new AtomicNode
            {
                NodeType = SongNodeType.NoLoopCommand,
                NodeSource = context.GetText(),
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
        }

        public override ISongNode VisitNote([NotNull] MmlParser.NoteContext context)
        {
            var noteText = context.GetText();
            var notePayload = new NotePayload();
            var noteRegex = new Regex(@"([a-gA-G])(\+|\-)?\=?([0-9]*)(\.*)(\^[0-9]+\.*)*");
            var matches = noteRegex.Match(noteText);

            var groups = matches.Groups.Values.ToList();
            notePayload.NoteValue = groups.First().Value;
            foreach (var group in groups.Take(1))
            {
                var groupValue = group.Value;
                if (groupValue.Length == 0)
                {
                    continue;
                }
                if(groupValue == "+" || groupValue == "-")
                {
                    notePayload.Accidental = (groupValue.Contains("+")) ? NotePayload.Accidentals.Sharp : NotePayload.Accidentals.Flat;
                }
                else if(groupValue.Contains("^"))
                {
                    var ties = groupValue.Split("^").Where(v => v.Length > 0).ToList();
                    foreach(var tie in ties)
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
                else if(groupValue.Contains(".") && !groupValue.Contains("^"))
                {
                    var dotCount = groupValue.Count(t => t == '.');
                    notePayload.DotCount = dotCount;
                }
                else
                {
                    notePayload.Duration = int.Parse(groupValue);
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

        public override ISongNode VisitOctave([NotNull] MmlParser.OctaveContext context)
        {
            var octaveText = context.GetText();
            var octaveNumber = int.Parse(octaveText.Substring(1));
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

        public override ISongNode VisitPan([NotNull] MmlParser.PanContext context)
        {
            var panText = context.GetText();
            var panPayload = new PanPayload();
            var panValues = panText.Substring(1).Split(',');
            if(panValues.Length > 0)
            {
                panPayload.PanPosition = int.Parse(panValues[0]);
            }
            if(panValues.Length > 1)
            {
                if(panValues[1].Length > 0)
                {
                    panPayload.SurroundSoundLeft = int.Parse(panValues[1]);
                }
            }
            if(panValues.Length > 2)
            {
                if (panValues[2].Length > 0)
                {
                    panPayload.SurroundSoundRight = int.Parse(panValues[2]);
                }
            }
            var panNode = new AtomicNode
            {
                NodeType = SongNodeType.Pan,
                NodeSource = panText,
                Payload = panPayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
            return panNode;
        }

        public override ISongNode VisitQuantization([NotNull] MmlParser.QuantizationContext context)
        {
            var quantizationText = context.GetText();
            var quantizationPayload = new QuantizationPayload();
            quantizationPayload.DelayValue = int.Parse(quantizationText.Substring(1,1));
            if(quantizationText.IndexOf('v', StringComparison.OrdinalIgnoreCase) != -1)
            {
                var volumeAmount = int.Parse(quantizationText[quantizationText.IndexOf('v', StringComparison.OrdinalIgnoreCase)..]);
                quantizationPayload.VolumeNode = new AtomicNode
                {
                    NodeType = SongNodeType.Volume,
                    NodeSource = quantizationText,
                    Payload = new VolumePayload(volumeAmount),
                    LineNumber = context.Start.Line,
                    ColumnNumber = context.Start.Column + 2,
                };
            }
            else
            {
                quantizationPayload.VolumeValue = quantizationText.Substring(2, 1);
            }
            var quantizationNode = new AtomicNode
            {
                NodeType = SongNodeType.Quantization,
                NodeSource = quantizationText,
                Payload = quantizationPayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
            return quantizationNode;
        }

        public override ISongNode VisitQmark([NotNull] MmlParser.QmarkContext context)
        {
            var qmarkText = context.GetText();
            var qmarkPayload = new QuestionMarkPayload();
            var qmarkNumber = int.Parse(qmarkText[1..1]);
            qmarkPayload.MarkNumber = qmarkNumber;
            var qmarkNode = new AtomicNode
            {
                NodeType = SongNodeType.QuestionMark,
                NodeSource = qmarkText,
                Payload = qmarkPayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
            return qmarkNode;
        }

        public override ISongNode VisitRaiseOctave([NotNull] MmlParser.RaiseOctaveContext context)
        {
            return new AtomicNode
            {
                NodeType = SongNodeType.RaiseOctave,
                NodeSource = context.GetText(),
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
        }

        public override ISongNode VisitRest([NotNull] MmlParser.RestContext context)
        {
            var restText = context.GetText();
            var restPayload = new NotePayload();
            var restRegex = new Regex(@"([rR])\=?([0-9]*)(\.*)(\^[0-9]+\.*)*");
            var matches = restRegex.Match(restText);

            var groups = matches.Groups.Values.ToList();
            restPayload.NoteValue = groups.First().Value;
            foreach (var group in groups.Take(1))
            {
                var groupValue = group.Value;
                if(groupValue.Length == 0)
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
                    restPayload.Duration = int.Parse(groupValue);
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

        public override ISongNode VisitTempo([NotNull] MmlParser.TempoContext context)
        {
            var tempoText = context.GetText();
            var tempoPayload = new TempoPayload();
            var tempoValues = tempoText.Substring(1).Split(',').ToList();
            tempoPayload.Tempo = int.Parse(tempoValues[0]);
            if(tempoValues.Count > 1)
            {
                tempoPayload.FadeValue = int.Parse(tempoValues[1]);
            }
            var tempoNode = new AtomicNode
            {
                NodeType = SongNodeType.Tempo,
                NodeSource = tempoText,
                Payload = tempoPayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
            return tempoNode;
        }

        public override ISongNode VisitTune([NotNull] MmlParser.TuneContext context)
        {
            var tuneText = context.GetText();
            var tuneValue = int.Parse(tuneText.Substring(1));
            var tunePayload = new TunePayload(tuneValue);
            var tuneNode = new AtomicNode
            {
                NodeType = SongNodeType.Tune,
                NodeSource = tuneText,
                Payload = tunePayload,
                LineNumber = tuneValue,
                ColumnNumber = tuneValue,
            };
            return tuneNode;
        }

        public override ISongNode VisitVibrato([NotNull] MmlParser.VibratoContext context)
        {
            var vibratoText = context.GetText();
            var vibratoPayload = new VibratoPayload();
            var vibratoValues = vibratoText.Substring(1).Split(',').ToList();
            if(vibratoValues.Count > 2)
            {
                vibratoPayload.DelayDurationValue = int.Parse(vibratoValues[0]);
                vibratoPayload.RateValue = int.Parse(vibratoValues[1]);
                vibratoPayload.ExtentValue = int.Parse(vibratoValues[2]);
            }
            else
            {
                vibratoPayload.RateValue = int.Parse(vibratoValues[0]);
                vibratoPayload.ExtentValue = int.Parse(vibratoValues[1]);
            }
            var vibratoNode = new AtomicNode
            {
                NodeType = SongNodeType.Vibrato,
                NodeSource = vibratoText,
                Payload = vibratoPayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
            return vibratoNode;
        }

        public override ISongNode VisitVolume([NotNull] MmlParser.VolumeContext context)
        {
            var volumeText = context.GetText();
            var volumePayload = new VolumePayload();
            var volumeValues = volumeText.Substring(1).Split(',').ToList();
            volumePayload.Volume = int.Parse(volumeValues.First());
            if (volumeValues.Count > 1)
            {
                volumePayload.FadeValue = int.Parse(volumeValues[1]);
            }
            var volumeNode = new AtomicNode
            {
                NodeType = SongNodeType.Volume,
                NodeSource = volumeText,
                Payload = volumePayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
            return volumeNode;
        }

        #endregion


        #region Loops


        public override ISongNode VisitCallLoop([NotNull] MmlParser.CallLoopContext context)
        {
            var callLoopText = context.GetText();
            var loopName = context.LoopName().GetText();
            var iterationsToken = context.NUMBERS();
            int iterations = 0;
            if(iterationsToken != null && iterationsToken.GetText() != "")
            {
                iterations = int.Parse(iterationsToken.GetText());
            }
            var callLoopNode = new LoopNode
            {
                NodeType = SongNodeType.CallLoop,
                NodeSource = callLoopText,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
                LoopName = loopName,
                Iterations = iterations,
            };
            return callLoopNode;
        }

        public override ISongNode VisitCallPreviousLoop([NotNull] MmlParser.CallPreviousLoopContext context)
        {
            var callPreviousLoopText = context.GetText();
            var iterationsToken = context.NUMBERS();
            int iterations = 0;
            if (iterationsToken != null && iterationsToken.GetText() != "")
            {
                iterations = int.Parse(iterationsToken.GetText());
            }

            return new LoopNode
            {
                NodeType = SongNodeType.CallPreviousLoop,
                NodeSource = callPreviousLoopText,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
                Iterations = iterations,
            };
        }

        public override ISongNode VisitCallRemoteCode([NotNull] MmlParser.CallRemoteCodeContext context)
        {
            var callRemoteCode = context.GetText();
            var callRemoteCodePayload = new CallRemoteCodePayload();
            var remoteCodeContents = callRemoteCode.Replace("(", "").Replace(")", "").Replace("!", "");
            var remoteCodeValues = remoteCodeContents.Split(",").ToList();
            for(int i = 0; i < remoteCodeValues.Count; i++)
            {
                if(i == 0)
                {
                    callRemoteCodePayload.DefinitionName = remoteCodeValues[i];
                }
                else if(i == 1)
                {
                    callRemoteCodePayload.EventType = int.Parse(remoteCodeValues[i]);
                }
                else if(i == 2)
                {
                    if (remoteCodeValues[i].Contains("$"))
                    {
                        callRemoteCodePayload.HexArgument = remoteCodeValues[i];
                    }
                    else
                    {
                        callRemoteCodePayload.IntArgument = int.Parse(remoteCodeValues[i]);
                    }
                }
            }
            var callLoopNode = new LoopNode
            {
                NodeType = SongNodeType.CallRemoteCode,
                NodeSource = callRemoteCode,
                Payload = callRemoteCodePayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
            return callLoopNode;
        }

        public override ISongNode VisitLogicCalls([NotNull] MmlParser.LogicCallsContext context)
        {
            if (context.ChildCount > 1)
            {
                Console.WriteLine("logicCall with more than one child.");
                return new SongNode();
            }

            return Visit(context.GetChild(0));
        }

        public override ISongNode VisitLogicControls([NotNull] MmlParser.LogicControlsContext context)
        {
            if (context.ChildCount > 1)
            {
                Console.WriteLine("logicControl with more than one child.");
                return new SongNode();
            }

            return Visit(context.GetChild(0));
        }

        public override ISongNode VisitLoopers([NotNull] MmlParser.LoopersContext context)   
        {
            if (context.ChildCount > 1)
            {
                Console.WriteLine("loopers with more than one child.");
                return new SongNode();
            }

            return Visit(context.GetChild(0));
        }

        public override ISongNode VisitRemoteCode([NotNull] MmlParser.RemoteCodeContext context)
        {
            var remoteCodeDefinitionText = context.GetText();
            var remoteCodeName = context.RemoteCodeName().GetText();
            var remoteCodeContents = context.remoteCodeContents().ToList();
            var remoteCodeContentNodes = new List<ISongNode>();
            foreach( var content in remoteCodeContents )
            {
                var node = Visit(content);
                remoteCodeContentNodes.Add(node);
            }
            var remoteCodeDefinitionNode = new LoopNode 
            {
                NodeType = SongNodeType.RemoteCode,
                NodeSource = remoteCodeDefinitionText,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
                LoopName = remoteCodeName,
                Children = remoteCodeContentNodes,
            };
            return remoteCodeDefinitionNode;
        }

        public override ISongNode VisitRemoteCodeContents([NotNull] MmlParser.RemoteCodeContentsContext context)
        {
            return Visit(context.GetChild(0));
        }

        public override ISongNode VisitRemoteLogicCalls([NotNull] MmlParser.RemoteLogicCallsContext context)
        {
            if (context.ChildCount > 1)
            {
                Console.WriteLine("remoteLogicCall with more than one child.");
                return new SongNode();
            }

            return Visit(context.GetChild(0));
        }

        public override ISongNode VisitSimpleLoop([NotNull] MmlParser.SimpleLoopContext context)
        {
            var simpleLoopText = context.GetText();
            var loopNameNode = context.LoopName();
            var loopName = (loopNameNode == null) ? "" : loopNameNode.GetText();
            var iterationsText = context.NUMBERS();
            var iterations = (iterationsText == null) ? 0 : int.Parse(iterationsText.GetText());

            // Remove loopname (if exists) and start bracket
            var rangeLowerBound = (loopName.Length > 0) ? 2 : 1 ;
            // remove end bracket and iteration count (if exists) and get length wrt start
            var rangeUpperBound = (iterationsText == null) ? context.ChildCount - (rangeLowerBound + 2) : context.ChildCount - (rangeLowerBound + 1);
            var childrenRange = new Range(rangeLowerBound, rangeUpperBound);

            var childrenNodes = VisitChildren(context, childrenRange);

            var simpleLoopNode = new LoopNode
            {
                NodeType = SongNodeType.SimpleLoop,
                NodeSource = simpleLoopText,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
                LoopName = loopName,
                Iterations = iterations,
                LoopContents = childrenNodes,
            };

            return simpleLoopNode;
        }

        public override ISongNode VisitSimpleLoopContents([NotNull] MmlParser.SimpleLoopContentsContext context)
        {
            if (context.ChildCount > 1)
            {
                Console.WriteLine("simpleLoopContent with more than one child.");
                return new SongNode();
            }

            return Visit(context.GetChild(0));
        }

        public override ISongNode VisitStopRemoteCode([NotNull] MmlParser.StopRemoteCodeContext context)
        {
            var stopRemoteCodeText = context.GetText();
            var eventName = stopRemoteCodeText[2..^1];
            return new LoopNode
            {
                NodeType = SongNodeType.StopRemoteCode,
                NodeSource = stopRemoteCodeText,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
                LoopName = eventName,
            };
        }

        public override ISongNode VisitSuperLoop([NotNull] MmlParser.SuperLoopContext context)
        {
            var simpleLoopText = context.GetText();
            var iterationsText = context.NUMBERS();
            var iterations = (iterationsText == null) ? 0 : int.Parse(iterationsText.GetText());

            // Remove loopname (if exists) and start bracket
            var rangeLowerBound = 1;
            // remove end bracket and iteration count (if exists) and get length wrt start
            var rangeUpperBound = (iterationsText == null) ? context.ChildCount - (rangeLowerBound + 2) : context.ChildCount - (rangeLowerBound + 1);
            var childrenRange = new Range(rangeLowerBound, rangeUpperBound);

            var childrenNodes = VisitChildren(context, childrenRange);

            var superLoopNode = new LoopNode
            {
                NodeType = SongNodeType.SuperLoop,
                NodeSource = simpleLoopText,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
                Iterations = iterations,
                LoopContents = childrenNodes,
            };

            return superLoopNode;
        }

        public override ISongNode VisitSuperLoopContents([NotNull] MmlParser.SuperLoopContentsContext context)
        {
            if (context.ChildCount > 1)
            {
                Console.WriteLine("superLoopContent with more than one child.");
                return new SongNode();
            }

            return Visit(context.GetChild(0));
        }

        public override ISongNode VisitTerminalSimpleLoop([NotNull] MmlParser.TerminalSimpleLoopContext context)
        {
            var simpleLoopText = context.GetText();
            var loopNameNode = context.LoopName();
            var loopName = (loopNameNode == null) ? "" : loopNameNode.GetText();
            var iterationsText = context.NUMBERS();
            var iterations = (iterationsText == null) ? 0 : int.Parse(iterationsText.GetText());

            // Remove loopname (if exists) and start bracket
            var rangeLowerBound = (loopName.Length > 0) ? 2 : 1;
            // remove end bracket and iteration count (if exists) and get length wrt start
            var rangeUpperBound = (iterationsText == null) ? context.ChildCount - (rangeLowerBound + 2) : context.ChildCount - (rangeLowerBound + 1);
            var childrenRange = new Range(rangeLowerBound, rangeUpperBound);

            var childrenNodes = VisitChildren(context, childrenRange);

            var simpleLoopNode = new LoopNode
            {
                NodeType = SongNodeType.SimpleLoop,
                NodeSource = simpleLoopText,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
                LoopName = loopName,
                Iterations = iterations,
                LoopContents = childrenNodes,
            };

            return simpleLoopNode;
        }

        public override ISongNode VisitTerminalSimpleLoopContents([NotNull] MmlParser.TerminalSimpleLoopContentsContext context)
        {
            if (context.ChildCount > 1)
            {
                Console.WriteLine("terminalSimpleLoopContent with more than one child.");
                return new SongNode();
            }

            return Visit(context.GetChild(0));
        }

        public override ISongNode VisitTerminalSuperLoop([NotNull] MmlParser.TerminalSuperLoopContext context)
        {
            var simpleLoopText = context.GetText();
            var iterationsText = context.NUMBERS();
            var iterations = (iterationsText == null) ? 0 : int.Parse(iterationsText.GetText());

            // Remove loopname (if exists) and start bracket
            var rangeLowerBound = 1;
            // remove end bracket and iteration count (if exists) and get length wrt start
            var rangeUpperBound = (iterationsText == null) ? context.ChildCount - (rangeLowerBound + 2) : context.ChildCount - (rangeLowerBound + 1);
            var childrenRange = new Range(rangeLowerBound, rangeUpperBound);

            var childrenNodes = VisitChildren(context, childrenRange);

            var superLoopNode = new LoopNode
            {
                NodeType = SongNodeType.SuperLoop,
                NodeSource = simpleLoopText,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
                Iterations = iterations,
                LoopContents = childrenNodes,
            };

            return superLoopNode;
        }

        public override ISongNode VisitTerminalSuperLoopContents([NotNull] MmlParser.TerminalSuperLoopContentsContext context)
        {
            if (context.ChildCount > 1)
            {
                Console.WriteLine("terminalSuperLoopContent with more than one child.");
                return new SongNode();
            }

            return Visit(context.GetChild(0));
        }

        #endregion


        #region Composites

        public override ISongNode VisitHexNumber([NotNull] MmlParser.HexNumberContext context)
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

        public override ISongNode VisitIntroEnd([NotNull] MmlParser.IntroEndContext context)
        {
            return new CompositeNode
            {
                NodeType = SongNodeType.Intro,
                NodeSource = context.GetText(),
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
        }

        public override ISongNode VisitPitchslide([NotNull] MmlParser.PitchslideContext context)
        {
            var pitchslideText = context.GetText();
            var pitchslidePayload = new PitchSlidePayload();
            foreach (var childIndex in Enumerable.Range(0, context.ChildCount))
            {
                var child = context.GetChild(childIndex);
                if(child.GetText().Equals("&"))
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

        public override ISongNode VisitSampleLoad([NotNull] MmlParser.SampleLoadContext context)
        {
            var sampleLoadText = context.GetText();
            var sampleLoadComponents = sampleLoadText.Replace("(", "").Replace(")", "").Split(",");
            var sampleLoadPayload = new SampleLoadPayload
            {
                SampleName = sampleLoadComponents[0],
                TuningValue = sampleLoadComponents[1],
            };
            var sampleLoadNode = new CompositeNode
            {
                NodeType = SongNodeType.SampleLoad,
                NodeSource = sampleLoadText,
                Payload = sampleLoadPayload,
                LineNumber = context.Start.Line,
                ColumnNumber = context.Start.Column,
            };
            return sampleLoadNode;
        }

        public override ISongNode VisitTriplet([NotNull] MmlParser.TripletContext context)
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
    }
}
