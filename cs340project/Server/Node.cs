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
    /// 
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
            ret.AppendLine("Node " + Id.ToString());

            ret.AppendLine("Neighbors:");
            foreach (Node n in Neighbors)
                ret.AppendLine(n.Id.ToString());

            ret.AppendLine("Up:");
            foreach (Node n in Up.Values)
                ret.AppendLine(n.Id.ToString());

            ret.AppendLine("Down:");
            foreach (Node n in Down.Values)
                ret.AppendLine(n.Id.ToString());

            if (Fold != null)
                ret.AppendLine("Fold: " + Fold.Id.ToString());

            if (OldFold != null)
                ret.AppendLine("OldFold: " + OldFold.Id.ToString());

            return ret.ToString();
        }

        #endregion

        #region References to other nodes

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
                for (uint i = 1; i < Id; )
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
        public uint ChildId
        {
            get
            {
                if (Id == 0)
                    return nextDimensionChild();
                uint i = 1;
                while (i < Id)
                    i *= 2;
                return i + Id;
            }
        }

        /// <summary>
        /// Gets the next dimension child Id.
        /// </summary>
        /// <returns>A child Id for the next dimension</returns>
        private uint nextDimensionChild()
        {
            uint dim = 1;
            while (Neighbors.Count(n => n.Id == dim) > 0)
                dim <<= 1;
            return dim;
        }

        /// <summary>
        /// Gets the logical surrogate id.
        /// </summary>
        /// <value>The logical surrogate id.</value>
        public uint SurrogateId
        {
            get
            {
                uint sur = Id;
                int count = 0;
                while (sur > 0)
                {
                    sur >>= 1;
                    count++;
                }
                sur = 1;
                count++;
                while (count > 0)
                {
                    sur <<= 1;
                }

                return sur + Id;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has a child.
        /// </summary>
        /// <value><c>true</c> if this instance has a child; otherwise, <c>false</c>.</value>
        public bool HasChild
        {
            get
            {
                return Neighbors.Count(n => n.Id == ChildId) == 1;
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
                if (Down.Count > 0)
                    return NodeState.Down;
                if (Up.Count > 0)
                    return NodeState.Up;
                if (Neighbors.Count(n => n.Id > Id) == 0)
                    return NodeState.Largest;
                if (!HasChild)
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

    }
}
