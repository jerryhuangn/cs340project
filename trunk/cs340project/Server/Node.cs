using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Net;
using cs340project;

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
    /// struction of the HypeerWeb
    /// Author: Joel Day
    /// 
    /// Domain: All nodes of a Hypeerweb
    /// Invariant: Node must be a valid member of a Hypeerweb
    /// </summary>
    [Serializable]
    public partial class Node : ISerializeMutator
    {
     

        #region Global set of nodes; debugging

        /// <summary>
        /// Gets the size of the web.
        /// </summary>
        /// <returns>The size of the HypeerWeb</returns>
        public virtual uint HypeerWebSize()
        {
            Node n = Node.insertionPoint(this);
            return n.NextChildId;
        }

        /// <summary>
        /// Dumps all nodes into a string representation of the HypeerWeb.
        /// 
        /// PreCondition: True
        /// PostCondition: The web stays the same, only a list of the nodes
        /// is iterated over to produces a string representation of the web
        /// </summary>
        /// <returns>A string representation of the web</returns>
        public virtual string DumpAllNodes()
        {
            StringBuilder ret = new StringBuilder();

            uint HypeerWebSize = this.HypeerWebSize();
            for (uint i = 0; i < HypeerWebSize; i++)
            {
                Node node = Node.getNode(i, this);
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

            List<string> Messages = (List<string>)Payload["Messages"];
            if (Messages.Count > 0)
            {
                ret.AppendLine("Payload:\t" + Payload.Count);
                foreach (var msg in Messages)
                    ret.AppendLine("\t" + msg);
            }
            return ret.ToString();
        }
        #endregion

        #region References to other nodes

        /// <summary>
        /// Gets the largest neighbor.
        /// </summary>
        /// <value>The largest neighbor.</value>
        public virtual Node LargestNeighbor
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
        public virtual Node AbsoluteLargestNeighbor
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

        public virtual Node ClosestNeighbor(uint p)
        {
            return (from node in AllNeighbors
                    orderby node.Id.Distance(p) ascending
                    select node).First();
        }

        /// <summary>
        /// A list of Neighboring Nodes
        /// </summary>
        List<Node> Neighbors = new List<Node>();
        /// <summary>
        /// A list of Up pointers for Surrogate Nodes
        /// </summary>
        Dictionary<uint, Node> Up = new Dictionary<uint, Node>();

        public virtual void AddUp(uint key, Node n)
        {
            Up.Add(key, n);
        }

        public virtual Node SmallestUp
        {
            get
            {
                return (from n1 in Up.Values
                        orderby n1.Id ascending
                        select n1).Last();
            }
        }
        /// <summary>
        /// A list of Down pointers to Surrogate Nodes
        /// </summary>
        Dictionary<uint, Node> Down = new Dictionary<uint, Node>();
        public virtual Node SmallestDown
        {
            get {
                return (from n1 in Down.Values
                        orderby n1.Id ascending
                        select n1).First();
            }
        }

        /// <summary>
        /// Gets or sets the fold. The Fold is used as a short-cut to
        /// get to the other side of the web quickly
        /// </summary>
        /// <value>The fold.</value>
        public virtual Node Fold { get; set; }
        /// <summary>
        /// Gets or sets the old fold. The Fold is used as a short-cut to
        /// get to the other side of the web quickly
        /// </summary>
        /// <value>The old fold.</value>
        public virtual Node OldFold { get; set; }

        #endregion

        /// <summary>
        /// The Payload is the storage house for the visitor pattern.
        /// When the node is visited, the Payload is used to record the 
        /// actions.
        /// </summary>
        /// <value>The payload.</value>
        public virtual Dictionary<string, object> Payload { get; set; }

        /// <summary>
        /// Accept a visitor and run it on this Node if it hasn't already been run here.
        /// </summary>
        /// <param name="v">The visitor to visit this Node</param>
        /// <returns>true if the visitor was called, false if it has already been here.</returns>
        public virtual bool Accept(Visitor v)
        {
            //First, quit if the visitor has already been here.
            foreach (Visitor old in RecentVisitors)
            {
                if (old.Id == v.Id)
                    return false;
            }

            //Then do the actual visit itself.
            v.Visit(this.Payload);

            //And add it to the history.
            this.RecentVisitors.Add(v);

            return true;
        }

        /// <summary>
        /// Gets the node's parent Id.
        /// </summary>
        /// <value>The parent's Id.</value>
        public virtual uint ParentId
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

        public virtual Node Parent
        {
            get
            {
                return Neighbors.First(n1 => n1.Id == ParentId);
            }
        }
        /// <summary>
        /// Gets the next child id based on the current HypeerWeb.
        /// </summary>
        /// <value>The next child id.</value>
        public virtual uint NextChildId
        {
            get
            {
                if (Id == 0)
                {
                    if (Fold == null)
                        return 1;
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

        public virtual Node GetNode(uint id)
        {
            try
            {
                return Node.getNode(id, this);
            }
            catch { return null; }
        }

        /// <summary>
        /// Gets the node's Child Id. Purely Logical, node may not exist
        /// 
        /// PreCondition: The caller's id's <see cref="Server.Extensions.Dimension"/> is 
        /// greater-than or equal to the current node's id's <see cref="Server.Extensions.Dimension"/>
        /// PostCondition: The logical child in the same
        /// level of the HypeerWeb as the caller
        /// </summary>
        /// <param name="callerId">The callers id.</param>
        /// <returns></returns>
        /// <value>The child's Id.</value>
        public virtual uint ChildId(uint callerId)
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
        public virtual uint SurrogateId(uint callerId)
        {
            return ChildId(callerId >> 1);
        }

        /// <summary>
        /// Gets a value indicating whether this instance has a child.
        /// </summary>
        /// <value><c>true</c> if this instance has a child; otherwise, <c>false</c>.</value>
        public virtual bool HasChild
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
        public virtual NodeState CurrentState
        {
            get
            {
                if (Neighbors.Count == 0)
                    return NodeState.Largest;
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
        /// </summary>
        uint _id = 0;
        public delegate void IdSet(Node n);
        public event IdSet OnIdSet = null;
        public virtual uint Id
        {
            get
            {
                return _id;
            }
            set {
                _id = value;
                if (OnIdSet != null)
                    OnIdSet(this);
            }
        }

        /// <summary>
        /// Visitors who recently visited this Node.
        /// 
        /// Used to prevent duplicate visits.
        /// </summary>
        public virtual List<Visitor> RecentVisitors { get; private set; }

        /// <summary>
        /// Construct a new Node in the hyperweb.
        /// </summary>
        /// <param name="id">The new node's ID number.</param>
        public Node(uint id, IPEndPoint ep)
        {
            //IMPORTANT: For proxy to work, no public methods can be called from the default constructor.
            if(id == uint.MaxValue)
                return;

            EndPoint = ep;

            Payload = new Dictionary<string, object>();
            Payload["Messages"] = new List<string>();

            RecentVisitors = new List<Visitor>();

            Id = id;
        }

        public IPEndPoint EndPoint { get; private set; }

        /// <summary>
        /// ONLY to be used by the proxy class.
        /// 
        /// Precondition: none
        /// Postcondition: this is the root node of a Hypeerweb
        /// </summary>
        public Node()
            : this(uint.MaxValue, null)
        {
            if(this.GetType().Name != "NodeProxy")
                throw new Exception("Default constructor only to be used from proxy.");
        }

        /// <summary>
        /// Creates a new root node of a hyperweb.
        /// </summary>
        public Node(IPEndPoint ep)
            : this(0, ep)
        {
        }

        public virtual void Send(Visitor v, uint id)
        {
            var node = getNode(id, this);
            node.Accept(v);
        }

        /// <summary>
        /// Send visitor V to all nodes in the Hypeerweb
        /// 
        /// PreCondition: none
        /// PostCondition: V has visited all nodes in the Hypeerweb
        /// </summary>
        /// <param name="v">Visitor to be sent to all nodes</param>
        public virtual void Broadcast(Visitor v)
        {
            //First, run it on this Node.
            if (this.Accept(v))
            {
                //Next, send it to all neighbors.
                foreach (Node n in AllNeighbors)
                    n.Broadcast(v);
            }
        }

        /// <summary>
        /// send visitor V to all nodes and return # of nodes visited
        /// 
        /// Precondition: none
        /// Postcondition: V has visited all nodes and the return value = # of nodes in the hypeerweb
        /// </summary>
        /// <param name="v">Visitor to be sent to all nodes</param>
        /// <param name="num">running count of nodes visited</param>
        /// <returns>number of nodes visited</returns>
        public virtual uint BroadcastWithAck(Visitor v, uint num)
        {
            //First, run it on this Node.
            if (this.Accept(v))
            {
                num++;
                //Next, send it to all neighbors.
                foreach (Node n in AllNeighbors)
                    num = n.BroadcastWithAck(v, num);
            }
            return num;
        }

        #region ISerializeMutator Members

        public virtual object ObjectToSerialize()
        {
            return Proxifier.GetProxy<Node>(EndPoint.Address.ToString(), (int)EndPoint.Port, "HypeerWeb", (int)Id);
        }

        #endregion
    }
}
