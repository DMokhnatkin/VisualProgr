using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VisualProgr.GUI.elements;
using System.Windows.Controls;

namespace VisualProgr.data
{
    public class XML_SchemeSaver
    {
        public static void SaveScheme(WPF_SchemeView schemeView, string fileName)
        {
            XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartElement("Scheme");
            writer.WriteStartElement("nodes");
            foreach (WPF_Node node in schemeView.canvas.Children.OfType<WPF_Node>())
            {
                writer.WriteStartElement(node.Node.Name);
                writer.WriteAttributeString("type", node.Node.NodeTypeMetaData.Name);
                writer.WriteAttributeString("group", node.Node.NodeTypeMetaData.Group);
                writer.WriteAttributeString("left", node.GetValue(Canvas.LeftProperty).ToString());
                writer.WriteAttributeString("top", node.GetValue(Canvas.TopProperty).ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteStartElement("connections");
            foreach (WPF_Node node in schemeView.canvas.Children.OfType<WPF_Node>())
            {
                writer.WriteStartElement(node.Node.Name);
                writer.WriteStartElement("inputs");
                foreach (Input port in node.Node.Ports.Inputs)
                {
                    writer.WriteStartElement(port.Name);
                    if (!port.IsNullConnected)
                    {
                        writer.WriteAttributeString("IsNullConnected", false.ToString());
                        writer.WriteStartElement(port.Connected[0].ParentNode.Name);
                        writer.WriteValue(port.Connected[0].Name);
                        writer.WriteEndElement();
                    }
                    else
                    {
                        writer.WriteAttributeString("IsNullConnected", true.ToString());
                        writer.WriteValue(port.Value);
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteStartElement("settings");
                foreach (Input port in node.Node.Ports.Settings)
                {
                    writer.WriteStartElement(port.Name);
                    if (!port.IsNullConnected)
                    {
                        writer.WriteAttributeString("IsNullConnected", false.ToString());
                        writer.WriteStartElement(port.Connected[0].ParentNode.Name);
                        writer.WriteValue(port.Connected[0].Name);
                        writer.WriteEndElement();
                    }
                    else
                    {
                        writer.WriteAttributeString("IsNullConnected", true.ToString());
                        writer.WriteValue(port.Value);
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.Close();
        }
    }
}
