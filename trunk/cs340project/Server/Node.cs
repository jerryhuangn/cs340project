using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Server
{
    class Node
    {
        #region Global set of nodes; debugging

        static Dictionary<uint, Node> AllNodes = new Dictionary<uint, Node>();

        public static void DumpAllNodes()
        {
            foreach (uint id in Node.AllNodes.Keys)
            {
                Debug.WriteLine(id);
                Debug.WriteLine(Node.AllNodes[id].ToString());
                Debug.WriteLine("------------------------------------");
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }

        #endregion

        /// <summary>
        /// The binary representation of the node's ID number in the
        /// hyperweb.
        /// </summary>
        public uint Id { get; private set; }

        /// <summary>
        /// Construct a new Node in the hyperweb.
        /// </summary>
        /// <param name="id">The new node's ID number.</param>
        public Node(uint id)
        {
            Id = id;
            Node.AllNodes[id] = this;
        }


        #region Basic insertion algorithm

        #region References to other nodes

        public List<Node> Neighbors = new List<Node>();
        public Node Fold { get; set; }
        public Node OldFold { get; set; }

        #endregion

        /// <summary>
        /// Inserts a new node in the hyperweb, and returns it.
        /// If this is the insertion point, insert it here, otherwise
        /// send it closer to the insertion point.
        /// </summary>
        /// <returns>The newly inserted node.</returns>
        public Node InsertNode()
        {
            return null;
        }

        #endregion
    }
}
