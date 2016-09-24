using Main.Nodes;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BoolNodes
{
    public class NodeAnd : Main.Nodes.Node
    {
        public static readonly new string typeName = "/standart/boolean/and";

        public override List<Tuple<string,object>> Calculate()
        {
            List<Tuple<string,object>> res = new List<Tuple<string,object>>();
            Main.Ports.Port in1 = this.PortManager.GetPort("in1");
            Main.Ports.Port in2 = this.PortManager.GetPort("in2");
            res.Add(new Tuple<string, object>("out", (bool)in1.GetValue() && (bool)in2.GetValue()));
            return res;
        }

        public NodeAnd(string name, NodeType _nodeType) : base(name, _nodeType)
        {
            this.PortManager.CreatePort(new Main.Ports.Port("in1", Main.Ports.PortType.Input, typeof(bool)));
            this.PortManager.CreatePort(new Main.Ports.Port("in2", Main.Ports.PortType.Input, typeof(bool)));
            this.PortManager.CreatePort(new Main.Ports.Port("out", Main.Ports.PortType.Output, typeof(bool)));

            this.PortManager.GetPort("in1").SetValue(false);
            this.PortManager.GetPort("in2").SetValue(false);
        }
    }

    public class NodeOr : Main.Nodes.Node
    {
        public static readonly new string typeName = "/standart/boolean/or";

        public override List<Tuple<string, object>> Calculate()
        {
            List<Tuple<string, object>> res = new List<Tuple<string, object>>();
            Main.Ports.Port in1 = this.PortManager.GetPort("in1");
            Main.Ports.Port in2 = this.PortManager.GetPort("in2");
            res.Add(new Tuple<string, object>("out", (bool)in1.GetValue() || (bool)in2.GetValue()));
            return res;
        }

        public NodeOr(string name, NodeType _nodeType)
            : base(name, _nodeType)
        {
            this.PortManager.CreatePort(new Main.Ports.Port("in1", Main.Ports.PortType.Input, typeof(bool)));
            this.PortManager.CreatePort(new Main.Ports.Port("in2", Main.Ports.PortType.Input, typeof(bool)));
            this.PortManager.CreatePort(new Main.Ports.Port("out", Main.Ports.PortType.Output, typeof(bool)));

            this.PortManager.GetPort("in1").SetValue(false);
            this.PortManager.GetPort("in2").SetValue(false);
        }
    }

    public class NodeXor : Main.Nodes.Node
    {
        public static readonly new string typeName = "/standart/boolean/xor";

        public override List<Tuple<string, object>> Calculate()
        {
            List<Tuple<string, object>> res = new List<Tuple<string, object>>();
            Main.Ports.Port in1 = this.PortManager.GetPort("in1");
            Main.Ports.Port in2 = this.PortManager.GetPort("in2");
            res.Add(new Tuple<string, object>("out", (bool)in1.GetValue() ^ (bool)in2.GetValue()));
            return res;
        }

        public NodeXor(string name, NodeType _nodeType)
            : base(name, _nodeType)
        {
            this.PortManager.CreatePort(new Main.Ports.Port("in1", Main.Ports.PortType.Input, typeof(bool)));
            this.PortManager.CreatePort(new Main.Ports.Port("in2", Main.Ports.PortType.Input, typeof(bool)));
            this.PortManager.CreatePort(new Main.Ports.Port("out", Main.Ports.PortType.Output, typeof(bool)));

            this.PortManager.GetPort("in1").SetValue(false);
            this.PortManager.GetPort("in2").SetValue(false);
        }
    }

    public class NodeNot : Main.Nodes.Node
    {
        public static readonly new string typeName = "/standart/boolean/not";

        public override List<Tuple<string, object>> Calculate()
        {
            List<Tuple<string, object>> res = new List<Tuple<string, object>>();
            Main.Ports.Port in1 = this.PortManager.GetPort("in1");
            res.Add(new Tuple<string, object>("out", !(bool)in1.GetValue()));
            return res;
        }

        public NodeNot(string name, NodeType _nodeType)
            : base(name, _nodeType)
        {
            this.PortManager.CreatePort(new Main.Ports.Port("in1", Main.Ports.PortType.Input, typeof(bool)));
            this.PortManager.CreatePort(new Main.Ports.Port("out", Main.Ports.PortType.Output, typeof(bool)));

            this.PortManager.GetPort("in1").SetValue(false);
        }
    }
}
