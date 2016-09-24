using Main.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StandratNodes
{
    public class NodeSum : Main.Nodes.Node
    {
        public static readonly new string typeName = "/standart/integer/sum";

        public override List<Tuple<string, object>> Calculate()
        {
            List<Tuple<string, object>> res = new List<Tuple<string, object>>();
            Main.Ports.Port in1 = this.PortManager.GetPort("in1");
            Main.Ports.Port in2 = this.PortManager.GetPort("in2");
            res.Add(new Tuple<string, object>("out", (int)in1.GetValue() + (int)in2.GetValue()));
            return res;
        }

        public NodeSum(string name, NodeType _nodeType)
            : base(name, _nodeType)
        {
            this.PortManager.CreatePort(new Main.Ports.Port("in1", Main.Ports.PortType.Input, typeof(int)));
            this.PortManager.CreatePort(new Main.Ports.Port("in2", Main.Ports.PortType.Input, typeof(int)));
            this.PortManager.CreatePort(new Main.Ports.Port("out", Main.Ports.PortType.Output, typeof(int)));

            this.PortManager.GetPort("in1").SetValue(0);
            this.PortManager.GetPort("in2").SetValue(0);
        }
    }

    public class NodeSub : Main.Nodes.Node
    {
        public static readonly new string typeName = "/standart/integer/sub";

        public override List<Tuple<string, object>> Calculate()
        {
            List<Tuple<string, object>> res = new List<Tuple<string, object>>();
            Main.Ports.Port in1 = this.PortManager.GetPort("in1");
            Main.Ports.Port in2 = this.PortManager.GetPort("in2");
            res.Add(new Tuple<string, object>("out", (int)in1.GetValue() - (int)in2.GetValue()));
            return res;
        }

        public NodeSub(string name, NodeType _nodeType)
            : base(name, _nodeType)
        {
            this.PortManager.CreatePort(new Main.Ports.Port("in1", Main.Ports.PortType.Input, typeof(int)));
            this.PortManager.CreatePort(new Main.Ports.Port("in2", Main.Ports.PortType.Input, typeof(int)));
            this.PortManager.CreatePort(new Main.Ports.Port("out", Main.Ports.PortType.Output, typeof(int)));

            this.PortManager.GetPort("in1").SetValue(0);
            this.PortManager.GetPort("in2").SetValue(0);
        }
    }

    
    public class NodeDiv : Main.Nodes.Node
    {
        public static readonly new string typeName = "/standart/integer/div";

        public override List<Tuple<string, object>> Calculate()
        {
            List<Tuple<string, object>> res = new List<Tuple<string, object>>();
            Main.Ports.Port in1 = this.PortManager.GetPort("in1");
            Main.Ports.Port in2 = this.PortManager.GetPort("in2");
            res.Add(new Tuple<string, object>("out", (int)in1.GetValue() / (int)in2.GetValue()));
            return res;
        }

        public NodeDiv(string name, NodeType _nodeType)
            : base(name, _nodeType)
        {
            this.PortManager.CreatePort(new Main.Ports.Port("in1", Main.Ports.PortType.Input, typeof(int)));
            this.PortManager.CreatePort(new Main.Ports.Port("in2", Main.Ports.PortType.Input, typeof(int)));
            this.PortManager.CreatePort(new Main.Ports.Port("out", Main.Ports.PortType.Output, typeof(int)));

            this.PortManager.GetPort("in1").SetValue(0);
            this.PortManager.GetPort("in2").SetValue(1);
        }
    }
}
