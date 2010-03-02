using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Server
{
    public enum NodeState
    {
        Inner, Up, Down, Edge, Largest
    }
    /// <summary>
    ///  Author: Joel Day
    /// </summary>
    public partial class Node
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
            ret.AppendLine("Node:\t\t" + Id.ToString());
            ret.AppendLine("CurrState:\t\t" + CurrentState);

            ret.Append("Neighbors:\t");
            foreach (Node n in Neighbors)
                ret.Append(n.Id.ToString() + " ");
            ret.AppendLine();

            ret.Append("Up:\t\t");
            foreach (Node n in Up.Values)
                ret.Append(n.Id.ToString() + " ");
            ret.AppendLine();

            ret.Append("Down:\t\t");
            foreach (Node n in Down.Values)
                ret.Append(n.Id.ToString() + " ");
            ret.AppendLine();

            if (Fold != null)
                ret.AppendLine("Fold:\t\t" + Fold.Id.ToString());

            if (OldFold != null)
                ret.AppendLine("OldFold:\t\t" + OldFold.Id.ToString());

            return ret.ToString();
        }

        #endregion

        #region References to other nodes

        List<Node> AllNeighbors
        {
            get
            {
                List<Node> nodes = new List<Node>();
                nodes.AddRange(Neighbors);
                nodes.AddRange(Up.Values);
                nodes.AddRange(Down.Values);

                if (Fold != null)
                    nodes.Add(Fold);
                if (OldFold != null)
                    nodes.Add(OldFold);

                return nodes;
            }
        }
        List<Node> Neighbors = new List<Node>();
        Dictionary<uint, Node> Up = new Dictionary<uint, Node>();
        Dictionary<uint, Node> Down = new Dictionary<uint, Node>();
        Node Fold { get; set; }
        Node OldFold { get; set; }

        #endregion

        /// <summary>
        /// Gets the node's parent Id.
        /// </summary>
        /// <value>The parent's Id.</value>
        public uint ParentId
        {
            get
            {
                uint par = 0;
                for (uint i = 1; i <= Id; )
                {
                    par = Id - i;
                    i *= 2;
                }
                return par < 0 ? 0 : par;
            }
        }

        /// <summary>
        /// Gets the node's Child Id. Purely Logical, node may not exist
        /// </summary>
        /// <value>The child's Id.</value>
        public uint ChildId(uint callerId)
        {
            uint dim = 1;
            while (callerId != 0)
            {
                dim <<= 1;
                callerId >>= 1;
            }
            dim >>= 1;
            return dim + Id;
        }
        /// <summary>
        /// Gets the logical surrogate id.
        /// </summary>
        /// <value>The logical surrogate id.</value>
        public uint SurrogateId(uint callerId)
        {

            return ChildId(callerId);

        }

        /// <summary>
        /// Gets a value indicating whether this instance has a child.
        /// </summary>
        /// <value><c>true</c> if this instance has a child; otherwise, <c>false</c>.</value>
        public bool HasChild
        {
            get
            {
                if (Id == 0)
                    return true;

                var largestNeighbor = (from n in Neighbors
                                       orderby n.Id ascending
                                       select n).Last();

                if (largestNeighbor.Id.Dimension() == Id.Dimension())
                    return false;

                return largestNeighbor.CurrentState == NodeState.Down;
            }
        }

        /// <summary>
        /// Gets or sets the state of the current node.
        /// </summary>
        /// <value>The state of the current.</value>
        public NodeState CurrentState
        {
            get
            {
                if (Neighbors.Count == 0)
                {
                    Debug.Assert(Node.AllNodes.Count == 1);
                    return NodeState.Largest;
                }

                var largestNeighbor = (from n in Neighbors
                       orderby n.Id ascending
                       select n).Last();

                if (Down.Count > 0)
                    return NodeState.Down;
                if (Up.Count > 0)
                    return NodeState.Up;
                if (Neighbors.Count(n => n.Id > Id) == 0)
                    return NodeState.Largest;
                if (!HasChild && largestNeighbor.Id.Dimension() == Id.Dimension())
                    return NodeState.Edge;

                return NodeState.Inner;
            }

        }

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

        /// <summary>
        /// Creates a new root node of a hyperweb.
        /// </summary>
        public Node()
        {
            Id = 0;
            Node.AllNodes[Id] = this;
        }

    }
}
