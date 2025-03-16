using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pidgin;

namespace Addmusic2.Parsers
{
    internal enum MusicNodeTypes
    {
        Directive,
        DirectievGroup,
        Channel,
        Instrument,
        Note,
        Triplet,
        Loop,
        SuperLoop,
        RemoteCodeDefinition,
        RemoteCodeCall,
        RemoteCodeStop,
        Replacement,
        HexCommand,


        Octave,
    }


    abstract class MusicNode { }
    internal class GeneralValueNode : MusicNode
    {
        public string Value { get; }
    }
    internal class ValueNode<T> : MusicNode
    {
        public T Value { get; }
    }
    internal class TextNode : ValueNode<string> { }
    internal class NumberNode : ValueNode<int> { }

    // overall parent
    internal class SongNode : MusicNode
    {
        public SongNode() : base()
        {
            Children = [];
        }
        public SongNode(MusicNode node) : base()
        {
            Children = ImmutableList.Create(node);
        }
        public SongNode(IEnumerable<MusicNode> nodes) : base()
        {
            Children = ImmutableList.CreateRange(nodes);
        }
        public ImmutableList<MusicNode> Children { get; }
    }

    // anything that has #<text>
    internal class DirectiveNode : MusicNode
    {
        public string Name { get; }
        public MusicNode Value { get; }
    }

    internal class OptionNode : DirectiveNode { }

    // list of items for a DirectiveNode. example:
    // #SPC
    // {
    //      #author "name"
    // }
    internal class ListableDirectiveNode : MusicNode
    {
        public ImmutableList<ListableNode> Children { get; }
    }

    // list item for the DirectiveNodes. examples:
    //      #author "name"
    //      "Sample01.brr"	$00 $00 $7F $03 $E0
    internal class ListableNode : MusicNode
    {
        public string ItemName { get; }
        public ImmutableList<MusicNode> Values { get; }
    }

    internal class ReplacementNode : MusicNode
    {
        public string Name { get; }
        public string ReplacementValue { get; }
    }

    // general nodes for data routing logic

    // single value nodes

    // #0-7
    internal class ChannelNode : SongNode
    {
        public string Value { get; }
        public ChannelNode(string value) : base() 
        { 
            Value = value[1].ToString();
        }
        public ChannelNode(string value, MusicNode node) : base(node)
        {
            Value = value[1].ToString();
        }

        public ChannelNode(string value, IEnumerable<MusicNode> nodes) : base(nodes)
        {
            Value = value[1].ToString();
        }
    }
    // notes: a,b,c,d,e,f,g, r, ^
    internal class NoteNode : GeneralValueNode { }
    // o<number>
    internal class OctaveNode : GeneralValueNode { }
    // @<number>
    internal class InstrumentNode : GeneralValueNode { }

    // basic control nodes

    // w<number>
    internal class GlobalVolumeNode : GeneralValueNode { }
    // v<number>
    internal class VolumeNode : GeneralValueNode { }
    // q<number>
    internal class QuantizationNode : GeneralValueNode { }
    // p<number
    internal class VibratoNode : GeneralValueNode { }
    // y<number>
    internal class PanNode : GeneralValueNode { }
    


    // symbol nodes

    // >
    internal class RaiseOctaveNode : GeneralValueNode { }
    // <
    internal class LowerOctaveNode : GeneralValueNode { }


    // logic control nodes

    internal class LoopNode : GeneralValueNode { }
    internal class TripletNode : GeneralValueNode { }
    internal class StarLoopCommand : GeneralValueNode { }

    // misc nodes

    internal class CommentNode : GeneralValueNode { }
    internal class NCommandNode : GeneralValueNode { }


    internal static class MusicNodeBuilder
    {
        public static MusicNode ChannelGroup(string name) => new ChannelNode(name);
    }
}
