using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Server
{
    public class Node
    {
        #region Global set of nodes; debugging

        public static Dictionary<uint, Node> AllNodes = new Dictionary<uint, Node>();

        public static string DumpAllNodes()
        {
            StringBuilder ret = new StringBuilder();
            foreach (uint id in Node.AllNodes.Keys)
            {
                ret.Append(Node.AllNodes[id].ToString());
                ret.AppendLine("------------------------------------");
            }
            return ret.ToString();
        }

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();
            ret.AppendLine("Node " + Id.ToString());
            
            ret.AppendLine("Neighbors:");
            foreach (Node n in Neighbors)
                ret.AppendLine(n.Id.ToString());

            ret.AppendLine("Up:");
            foreach (Node n in Up)
                ret.AppendLine(n.Id.ToString());

            ret.AppendLine("Down:");
            foreach (Node n in Down)
                ret.AppendLine(n.Id.ToString());

            if(Fold != null)
                ret.AppendLine("Fold: " + Fold.Id.ToString());

            if(OldFold != null)
                ret.AppendLine("OldFold: " + OldFold.Id.ToString());

            return ret.ToString();
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
        private Node(uint id)
        {
            Id = id;
            Node.AllNodes[id] = this;
        }

        /// <summary>
        /// Creates a new root node of a hyperweb.
        /// </summary>
        public Node()
        {
            Id = 0;
            Node.AllNodes[Id] = this;
        }


        #region Basic insertion algorithm

        #region References to other nodes

        List<Node> Neighbors = new List<Node>();
        List<Node> Up = new List<Node>();
        List<Node> Down = new List<Node>();
        Node Fold { get; set; }
        Node OldFold { get; set; }

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
