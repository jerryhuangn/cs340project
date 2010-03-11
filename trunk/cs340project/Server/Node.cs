using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Server
{
    /// <summary>
    /// Enumerator for the different states.
    /// 
    /// This is used to satisfy the "State Pattern"
    /// Portion of the project
    /// </summary>
    public enum NodeState
    {
        /// <summary>
        /// Inner Nodes are nodes that have children and are not
        /// Up Nodes
        /// </summary>
        Inner,
        /// <summary>
        /// Nodes that are Surrogates are in the Up Node State
        /// 
        /// Surrogates are used to fill in the gaps of the HypeerWeb
        /// </summary>
        Up,
        /// <summary>
        /// Nodes that are on the outter edge of the HypeerWeb, but
        /// not all their neighbors are physical nodes.  Neighbors that
        /// aren't represented by physical nodes are called Surrogates
        /// 
        /// Surrogates are used to fill in the gaps of the HypeerWeb
        /// </summary>
        Down,
        /// <summary>
        /// Nodes that have all their neighbors, but do not have children.
        /// They are on the outter edge of the HypeerWeb
        /// </summary>
        Edge,
        /// <summary>
        /// The node that are the biggest of all their neighbors
        /// </summary>
        Largest
    }
    /// <summary>
    /// A node is an element of the HypeerWeb.
    /// Nodes have all the built in functionality
    /// to add new nodes, delete nodes, and communicate
    /// between nodes all while maintaining the proper
    /// struction of th the HypeerWeb
    /// Author: Joel Day
    /// </summary>
    public partial class Node
    {

        #region Global set of nodes; debugging


        /// <summary>
        /// Dictionary that holds all the Nodes.  Used for 
        /// dumping the web as a string.
        /// </summary>
        public static Dictionary<uint, Node> AllNodes = new Dictionary<uint, Node>();


        /// <summary>
        /// Dumps all nodes into a string representation of the HypeerWeb.
        /// 
        /// PreCondition: True
        /// PostCondition: The web stays the same, only a list of the nodes
        /// is iterated over to produces a string representation of the web
        /// </summary>
        /// <returns>A string representation of the web</returns>
        public static string DumpAllNodes()
        {
            StringBuilder ret = new StringBuilder();
            var nodes = from n in Node.AllNodes.Values
                       orderby n.Id ascending
                       select n;
            foreach (var node in nodes)
            {
                ret.Append(node.ToString());
                ret.AppendLine("------------------------------------");
            }
            return ret.ToString();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the node instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents the node instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();
            ret.AppendLine("Node\t" + Id.ToString());
            ret.AppendLine("CurrState:\t" + CurrentState);

            ret.AppendLine("Neighbors:");

            var neigh = from ne in Neighbors
                        orderby ne.Id ascending
                        select ne;

            foreach (Node n in neigh)
                ret.AppendLine("\t" + n.Id.ToString());


            var up = from u in Up.Values
                     orderby u.Id ascending
                     select u;
            ret.AppendLine("Up:");
            foreach (Node n in up)
                ret.AppendLine("\t" + n.Id.ToString());


            var down = from d in Down.Values
                       orderby d.Id ascending
                       select d;
            ret.AppendLine("Down:");
            foreach (Node n in down)
                ret.AppendLine("\t" + n.Id.ToString());

            if (Fold != null)
                ret.AppendLine("Fold:\t" + Fold.Id.ToString());

            if (OldFold != null)
                ret.AppendLine("OldFold:\t" + OldFold.Id.ToString());

            return ret.ToString();
        }
        #endregion

        #region References to other nodes

        /// <summary>
        /// Gets the largest neighbor.
        /// </summary>
        /// <value>The largest neighbor.</value>
        Node LargestNeighbor
        {
            get
            {
                return (from n in Neighbors
                        orderby n.Id ascending
                        select n).Last();
            }
        }

        /// <summary>
        /// Gets the smallest neighbor.
        /// </summary>
        /// <value>The smallest neighbor.</value>
        /// <value>The smallest neighbor.</value>
        Node AbsoluteLargestNeighbor
        {
            get
            {
                Node temp = this;
                while (temp.LargestNeighbor.Id > temp.Id)
                    temp = temp.LargestNeighbor;

                return temp;
            }
        }

        Node SmallestNeighbor
        {
            get
            {
                return (from n in Neighbors
                        orderby n.Id ascending
                        select n).First();
            }
        }

        /// <summary>
        /// Gets all neighbors. This includes all the <see cref="Neighbors"/>, <see cref="Up"/>, <see cref="Down"/>,
        /// <see cref="Fold"/>, and <see cref="OldFold"/>
        /// </summary>
        /// <value>All neighbors.</value>
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
        /// <summary>
        /// A list of Neighboring Nodes
        /// </summary>
        List<Node> Neighbors = new List<Node>();
        /// <summary>
        /// A list of Up pointers for Surrogate Nodes
        /// </summary>
        Dictionary<uint, Node> Up = new Dictionary<uint, Node>();
        /// <summary>
        /// A list of Down pointers to Surrogate Nodes
        /// </summary>
        Dictionary<uint, Node> Down = new Dictionary<uint, Node>();
        /// <summary>
        /// Gets or sets the fold. The Fold is used as a short-cut to
        /// get to the other side of the web quickly
        /// </summary>
        /// <value>The fold.</value>
        Node Fold { get; set; }
        /// <summary>
        /// Gets or sets the old fold. The Fold is used as a short-cut to
        /// get to the other side of the web quickly
        /// </summary>
        /// <value>The old fold.</value>
        Node OldFold { get; set; }

        #endregion

        /// <summary>
        /// The Payload is the storage house for the visitor pattern.
        /// When the node is visited, the Payload is used to record the 
        /// actions.
        /// </summary>
        /// <value>The payload.</value>
        public Dictionary<uint, object> Payload { get; set; }

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
        /// Gets the next child id based on the current HypeerWeb.
        /// </summary>
        /// <value>The next child id.</value>
        public uint NextChildId
        {
            get
            {
                if (Id == 0)
                {
                    return Fold.Id + 1;
                }

                uint newId = Id;
                uint i = 0;
                for (i = 1; i <= newId; )
                    i <<= 1;
                newId += i;

                while (Neighbors.Count(n1 => n1.Id == newId) > 0)
                {
                    i <<= 1;
                    newId = Id + i;
                }
                return newId;
            }
        }

        /// <summary>
        /// Gets the node's Child Id. Purely Logical, node may not exist
        /// 
        /// PreCondition: The caller's id's <see cref="Server.Extensions.Dimension"/> is 
        /// greater-than or equal to the current node's id's <see cref="Server.Extensions.Dimension"/>
        /// Domain: All possible Node Ids
        /// PostCondition: The logical child in the same
        /// level of the HypeerWeb as the caller
        /// </summary>
        /// <param name="callerId">The callers id.</param>
        /// <returns></returns>
        /// <value>The child's Id.</value>
        public uint ChildId(uint callerId)
        {
            uint dim = (uint)(1 << (int)callerId.Dimension());
            return dim + Id;
        }

        /// <summary>
        /// Gets the logical surrogate id.
        /// Same as the ChidId
        /// </summary>
        /// <param name="callerId">The caller id.</param>
        /// <returns>The logical surrogate id. Same as ChildId</returns>
        public uint SurrogateId(uint callerId)
        {
            return ChildId(callerId >> 1);
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

                Node temp = AbsoluteLargestNeighbor;

                if (temp.Id.Dimension() == Id.Dimension())
                    return false;

                return temp.CurrentState == NodeState.Down;
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
                if (Down.Count > 0)
                    return NodeState.Down;
                if (Up.Count > 0)
                    return NodeState.Up;
                if (Neighbors.Count(n => n.Id > Id) == 0)
                    return NodeState.Largest;
                if (LargestNeighbor.Id.Dimension() == Id.Dimension())
                    return NodeState.Edge;

                return NodeState.Inner;
            }

        }

        /// <summary>
        /// The binary representation of the node's ID number in the
        /// hyperweb.
        /// 
        /// Domain: All positive 32 bit numbers
        /// </summary>
        public uint Id { get; private set; }

        /// <summary>
        /// Construct a new Node in the hyperweb.
        /// </summary>
        /// <param name="id">The new node's ID number.</param>
        public Node(uint id)
        {
            Payload = new Dictionary<uint, object>();
            Id = id;
            Node.AllNodes[id] = this;
        }

        /// <summary>
        /// Creates a new root node of a hyperweb.
        /// </summary>
        public Node()
        {
            Payload = new Dictionary<uint, object>();
            Id = 0;
            Node.AllNodes[Id] = this;
        }

        public void Visit(Visitor vis, object obj)
        {
            vis.Visit(obj);
        }
    }
}
