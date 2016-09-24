using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Main.Structs
{

    public class Tree<T>
    {
        /// <summary>
        /// Vertex of tree
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class Vertex<T>
        {
            /// <summary>
            /// Data to store
            /// </summary>
            public T Data { get; private set; }

            public string Name { get; private set; }

            public bool HasChilds
            {
                get { return _childs.Count != 0; }
            }

            List<Vertex<T>> _childs = new List<Vertex<T>>();

            /// <summary>
            /// Add child to vertex
            /// </summary>
            /// <param name="child"></param>
            public void AddChild(Vertex<T> child)
            {
                _childs.Add(child);
            }

            /// <summary>
            /// Get child by name
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public Vertex<T> GetChild(string name)
            {
                return _childs.FirstOrDefault(x => x.Name == name);
            }

            public ReadOnlyCollection<Vertex<T>> GetAllChilds()
            {
                return new ReadOnlyCollection<Vertex<T>>(_childs);
            }

            public Vertex(string name, T data)
            {
                Data = data;
                Name = name;
            }
        }

        Vertex<T> _root = new Vertex<T>("", default(T));

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="path">Path to add</param>
        public void Add(string path, T data)
        {
            List<string> vertexs = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            string name = vertexs.Last();
            vertexs.RemoveAt(vertexs.Count - 1);
            Vertex<T> cur = _root;
            foreach (string vertexName in vertexs)
            {
                var t = cur.GetChild(vertexName);
                if (t == null)
                    cur.AddChild(new Vertex<T>(vertexName, default(T)));
                cur = cur.GetChild(vertexName);
            }
            cur.AddChild(new Vertex<T>(name, data));
        }

        /// <summary>
        /// Get all data in Tuple(path, data, hasChilds)
        /// </summary>
        /// <returns></returns>
        private void GetAll(Vertex<T> cur, List<Tuple<string, T, bool>> res, string curPath)
        {
            var childs = cur.GetAllChilds();
            foreach (Vertex<T> t in childs)
            {
                res.Add(new Tuple<string, T, bool>(curPath + "/" + t.Name, t.Data, t.HasChilds));
                GetAll(t, res, curPath + "/" + t.Name);
            }
        }

        /// <summary>
        /// Get data Tuple(name, data, hasChilds)
        /// if path == null return all data
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<Tuple<string, T, bool>> Get(string path = null)
        {
            List<Tuple<string, T, bool>> res = new List<Tuple<string, T, bool>>();
            Vertex<T> cur = _root;
            if (path != null)
            {
                if (path[0] != '/')
                    throw new ArgumentException(string.Format("You must start with '/' in '{0}'", path));
                List<string> vertexs = path.Split(new char[] { '/' }).ToList<string>();
                vertexs.RemoveAt(0); // Remove root
                string name = vertexs.Last(); // Get last
                vertexs.RemoveAt(vertexs.Count - 1);
                foreach (string vertexName in vertexs)
                {
                    var t = cur.GetChild(vertexName);
                    if (t == null)
                        throw new ArgumentException(string.Format("No such path '{1}' in tree", path));
                    cur = t;
                }
                if (name == "")
                {
                    // Return all childs
                    var arr = cur.GetAllChilds();
                    foreach (Vertex<T> vert in arr)
                    {
                        res.Add(new Tuple<string, T, bool>(path, vert.Data, vert.HasChilds));
                    }
                }
                else
                {
                    Vertex<T> vert = cur.GetChild(name);
                    res.Add(new Tuple<string, T, bool>(path, vert.Data, vert.HasChilds));
                }
            }
            else
                GetAll(cur, res, "");
            return res;
        }

        private void Find(Vertex<T> cur, T data, ref bool flag, ref string res, string curPath)
        {
            var childs = cur.GetAllChilds();
            foreach (Vertex<T> child in childs)
            {
                if (flag)
                    break;
                if (child.Data != null && child.Data.Equals(data))
                {
                    flag = true;
                    res = curPath + "/" + child.Name;
                    break;
                }
                Find(child, data, ref flag, ref res, curPath + "/" + child.Name);
            }
        }

        /// <summary>
        /// Get path of data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetPath(T data)
        {
            string res = "";
            bool flag = false;
            Find(_root, data, ref flag, ref res, "");
            return res;
        }
    }
}
