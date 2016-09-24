using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Main.Nodes
{
    /// <summary>
    /// Name of node type(with hierarchy) (e.g. /standart/boolean/)
    /// </summary>
    public class NodeType
    {
        public string FullName { get; private set; }

        /// <summary>
        /// Name of nodeType (e.g 
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Bundles in path (e.g. for /standart/boolean/ it will be ["standart","boolean"])
        /// Without leaf!!!
        /// </summary>
        public ReadOnlyCollection<string> Bundles { get; private set; }

        /// <summary>
        /// System type of nodeType
        /// </summary>
        public Type SystemType { get; private set; }

        public NodeType(string fullName, Type _systemType)
        {
            FullName = fullName;
            LinkedList<string> bundles = new LinkedList<string>(fullName.Split('/'));
            // fullName can be started with '/'. 
            // In this case first bundle will be empty. 
            // We must remove it from bundles 
            if (bundles.First.Value == "")
                bundles.RemoveFirst();
            // Last bundle is Name of nodeType
            Name = bundles.Last.Value;
            bundles.RemoveLast();
            // Just put other bundles into Bundles
            Bundles = new ReadOnlyCollection<string>(bundles.ToList());

            SystemType = _systemType;
        }
    }
}
