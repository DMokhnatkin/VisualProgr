using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using VisualProgr.GUI.elements;
using System.Xml;
using System.Xml.XPath;
using System.Windows.Input;
using VisualProgr.GUI.elements.ports;

namespace VisualProgr.data
{
    public class XML_SchemeLoader
    {
        public static void Load(WPF_SchemeView shemeView, string fileName)
        {
            shemeView.Clear();
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            foreach (XmlElement node in doc.DocumentElement.SelectNodes("nodes/*"))
            {
                shemeView.CreateNode(NodeTypes.GetNodeMetaData(node.GetAttribute("type"), node.GetAttribute("group")), 
                    Convert.ToDouble(node.GetAttribute("left")), Convert.ToDouble(node.GetAttribute("top")),
                    node.Name).UpdateLayout();
            }
            foreach(XmlElement node in doc.DocumentElement.SelectNodes("connections/*"))
            {
                foreach (XmlElement el in node.SelectNodes("inputs/*"))
                {
                    if (el.GetAttribute("IsNullConnected") != true.ToString())
                    {
                        VIS_Port first = shemeView.GetVisEl(node.Name).GetVisPort(el.Name);
                        VIS_Port second = shemeView.GetVisEl(el.SelectSingleNode("*").Name).GetVisPort(el.SelectSingleNode("*").InnerText);
                        shemeView.Connect(first, second);
                    }
                    else
                    {
                        var port = Scheme.Cur.GetNode(node.Name).Ports[el.Name];
                        port.Value = Convert.ChangeType(el.InnerText, port.Value.GetType());
                        port.IsNullConnected = true;
                    }

                }
            }
        }
    }
}
