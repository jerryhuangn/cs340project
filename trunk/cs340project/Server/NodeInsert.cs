using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    /// <summary>
    /// Common Extension Methods
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Distance from the current uint to the specified other uint.
        /// Distance is the number of 1 bits in the XOR of the two numbers
        /// </summary>
        /// <param name="numb">The current uint</param>
        /// <param name="otherNumb">The other uint</param>
        /// <returns></returns>
        public static uint Distance(this uint numb, uint otherNumb)
        {
            uint xor = numb ^ otherNumb;
            uint count = 0;
            while (xor != 0)
            {
                count++;
                xor &= (xor - 1);
            }
            return count;
        }
    }
    public partial class Node
    {
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
            // first node id + 1
            // second node id
            // half way between them => ID
            // Get that ID's node
            //
            // Does it have a child? all lower not it
            // else all upper not it

            uint min = right[0].Id;
            uint max = right[1].Id;

            if (min == max)
                return right[0];

            uint mid = (max + min) / 2;

            var midNode = getNode(mid, right[0]);

            if (!midNode.HasChild)
            {
                right[1] = midNode;
                return searchRange(right);
            }

            if ((mid + 1) == right[1].Id)
                return right[1];

            right[0] = midNode;

            return searchRange(right);
        }

        private static List<Node> goRight(Node n)
        {
            List<Node> ret = new List<Node>();
            switch (n.CurrentState)
            {
                case NodeState.Up:
                    ret.Add((from n1 in n.Up.Values
                             orderby n1.Id ascending
                             select n1).Last());

                    ret.Add(n);
                    // largest up and Me
                    break;
                case NodeState.Down:
                    ret.Add(n.Neighbors.First(n1 => n1.Id == n.ParentId));

                    ret.Add((from n1 in n.Down.Values
                             orderby n1.Id ascending
                             select n1).First());

                    // My parent and Down min
                    break;
                case NodeState.Largest:
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
            List<Node> nodes = new List<Node>();
            nodes.AddRange(n.Neighbors);
            nodes.AddRange(n.Up.Values);
            nodes.AddRange(n.Down.Values);

            nodes.Add(n.Fold);
            nodes.Add(n.OldFold);

            var nearest = (from node in nodes
                           orderby node.Id.Distance(p) ascending
                           select node).First();
            if (nearest.Id == p)
                return nearest;

            return getNode(p, nearest);
        }

        private static bool perfectCube(Node n)
        {
            if (n.CurrentState == NodeState.Largest)
                return n.Fold.Id == 0;

            return perfectCube((from n1 in n.Neighbors
                                orderby n1.Id ascending
                                select n1).Last());
        }

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

            return insert.insertChildNode();
        }

        private Node insertChildNode()
        {
            Node n = new Node(ChildId);

            foreach (var upNode in Up)
            {
                Up.Remove(upNode.Key);
                upNode.Value.addNeighbor(n);
                n.addNeighbor(upNode.Value);
            }
            foreach (var neighbor in Neighbors)
            {
                if (!neighbor.HasChild)
                {
                    n.addSurrogate(neighbor);
                }
            }

            if (OldFold == null)
            {
                Fold.OldFold = Fold.Fold;
                Fold.Fold = n;
            }
            else
            {
                OldFold.Fold = n;
                OldFold.OldFold = null;
            }

            OldFold = null;

            Neighbors.Add(n);
            n.Neighbors.Add(this);

            return n;
        }

        private bool setAsFold(Node n)
        {
            if ((n.Id ^ Id) == (n.Id | Id))
            {
                OldFold = Fold;
                Fold = n;

                return true;
            }
            return false;
        }

        private void addNeighbor(Node n)
        {
            if (Up[n.Id] != null)
                Up.Remove(n.Id);

            if (Down[n.Id] != null)
                Down.Remove(n.Id);

            Neighbors.Add(n);
        }

        private bool addSurrogate(Node n)
        {
            if (Id > n.Id)
                return false;

            Down.Add(n.SurrogateId, n);
            n.Up.Add(Id, this);

            return true;
        }
    }
}
