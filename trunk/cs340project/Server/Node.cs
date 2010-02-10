using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Server
{
    public enum NodeState
    {
        Inner, Up, Down, Edge
    }
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

        /// <summary>
        /// Gets the node's parent Id.
        /// </summary>
        /// <value>The parent's Id.</value>
        public int ParentId
        {
            get
            {
                int par = 0;
                for (int i = 1; i < Id; )
                {
                    par = (int)Id - i;
                    i *= 2;
                }
                return par < 0 ? 0 : par;
            }
        }

        /// <summary>
        /// Gets or sets the state of the current node.
        /// </summary>
        /// <value>The state of the current.</value>
        public NodeState CurrentState { get; private set; }

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
            CurrentState = NodeState.Inner;
            Node.AllNodes[Id] = this;
        }

        #region Basic insertion algorithm

        #region static helper methods

        private static bool emptyWeb(Node n)
        {
            if (n.Id == 0 && n.Neighbors.Count == 0)
                return true;
            return false;
        }

        private static Node insertionPoint(Node n)
        {
            List<Node> right = new List<Node>();
            right.Add(n);
            while ((right = goRight(right[0])).Count == 1) ;

            return searchRange(right);
        }

        private static Node searchRange(List<Node> right)
        {
            throw new NotImplementedException();
        }

        private static List<Node> goRight(Node n)
        {
            List<Node> ret = new List<Node>();
            switch (n.CurrentState)
            {
                case NodeState.Up:
                    break;
                case NodeState.Down:
                    break;
                case NodeState.Edge:
                    if (perfectCube(n))
                    {
                        ret.Add(getNode(0, n));
                        ret.Add(ret[0]);
                    }
                    else
                    {
                        ret.Add(n.Fold);
                    }
                    break;
                default:

                    var node = from n1 in n.Neighbors
                               orderby n1.Id ascending
                               select n1;

                    ret.Add(node.Last());
                    break;
            }
            return ret;
        }

        private static Node getNode(uint p, Node n)
        {
            switch (n.CurrentState)
            {
                case NodeState.Down:
                    if (n.Down.ContainsKey(p))
                        return n.Down[p];
                    break;
                case NodeState.Up:
                    if (n.Up.ContainsKey(p))
                        return n.Up[p];
                    break;
            }
            var nearest = (from node in n.Neighbors
                           orderby node.Id & p ascending
                           select node).Last();
            if (nearest.Id == p)
                return nearest;

            return getNode(p, nearest);
        }

        private static bool perfectCube(Node n)
        {
            Node neighbor = (from node in n.Neighbors
                             orderby node.Id ascending
                             select node).Last();

            if (neighbor.CurrentState == NodeState.Up ||
                neighbor.CurrentState == NodeState.Down)
                return false;

            if (n.Id > neighbor.Id)
                return true;

            return perfectCube(neighbor);
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
        /// Inserts a new node in the hyperweb, and returns it.
        /// If this is the insertion point, insert it here, otherwise
        /// send it closer to the insertion point.
        /// </summary>
        /// <returns>The newly inserted node.</returns>
        public Node CreateNode()
        {
            if (emptyWeb(this))
            {
                Node n = new Node(1);

                n.Neighbors.Add(this);
                Neighbors.Add(n);
                this.Fold = n;
                n.Fold = n;

                return n;
            }

            var insert = insertionPoint(this);

            return insert.InsertChildNode();
        }

        private Node InsertChildNode()
        {
            uint i = 1;
            while (i < Id)
                i *= 2;
            uint childId = i + Id;
            
            Node n = new Node(childId);

            foreach (var upNode in Up)
            {
                Up.Remove(upNode.Key);
                upNode.Value.Down.Remove(childId);

            }

            return n;
        }

        #endregion
    }
}
