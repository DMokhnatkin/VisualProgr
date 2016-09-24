using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Nodes
{
    /// <summary>
    /// Uses to collect all node types and acces to them by there path(fullName)
    /// </summary>
    public class NodeTypesManager
    {
        Dictionary<string, NodeType> _storage = new Dictionary<string, NodeType>(); 

        /// <summary>
        /// Create NodeTypesManager and collect all node types
        /// </summary>
        public NodeTypesManager(List<System.Reflection.Assembly> assemblys)
        {
            foreach (System.Reflection.Assembly assembly in assemblys)
            {
                foreach (Type t in assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Node))).ToList<Type>())
                {
                    string path = ((string)t.GetField("typeName").GetValue(null));
                    _storage.Add(path, new NodeType(path, t));
                }
            }
        }

        /// <summary>
        /// Get all nodeTypes
        /// </summary>
        /// <returns></returns>
        public List<NodeType> GetAll()
        {
            return _storage.Values.ToList();
        }

        /// <summary>
        /// Get nodeType or null
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public NodeType GetNodeType(string path)
        {
            if (!_storage.ContainsKey(path))
                return null;
            return _storage[path];
        }
    }
}
