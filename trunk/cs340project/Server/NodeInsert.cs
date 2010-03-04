﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    /// <summary>
    /// Common Extension Methods for the type uint
    /// 
    /// Author: Joel Day
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Distance from the current uint to the specified other uint.
        /// Distance is the number of 1 bits in the XOR of the two numbers
        /// 
        /// PreCondition: The current uint has been assigned a value
        /// Domain: 0-31
        /// PostCondition: The return will be the "Distance" of the 
        /// current Id to the supplied Id in the HypeerWeb.
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

        /// <summary>
        /// Gets the n in the 2^n for the leading 1
        /// 
        /// EX: 100101001 = 9 or 100 = 3
        /// 
        /// PreCondition: The current instance uint has been assigned
        /// a value
        /// Domain: 0-31
        /// PostCondition: The return will be the number of digits including
        /// the leading 1 till the end of the binary number
        /// 
        /// PreCondition: uint
        /// Domain: All positive 32bit integers
        /// PostCondition: Will return the n for 2^n for the leading 1
        /// </summary>
        /// <param name="numb">The number.</param>
        /// <returns>The dimension</returns>
        public static uint Dimension(this uint numb)
        {
            uint count = 0;
            uint temp = numb;
            while (temp != 0)
            {
                count++;
                temp >>= 1;
            }
            return count;
        }
    }

    /// <summary>
    /// Part of the Node class that has the static methods for inserts
    /// 
    /// Author: Joel Day
    /// </summary>
    public partial class Node
    {
        #region static helper methods

        /// <summary>
        /// Determines if the web is empty
        /// 
        /// PreCondition: The node is not null
        /// Domain: True, False
        /// PostCondtion: The return will answer if the current
        /// HypeerWeb that the node belongs to is empty
        /// </summary>
        /// <param name="n">Any node in the web</param>
        /// <returns>True if the web is empty, false otherwise</returns>
        private static bool emptyWeb(Node n)
        {
            if (n.Id == 0 && n.Neighbors.Count == 0)
                return true;
            return false;
        }

        /// <summary>
        /// Gets the Insertion point.  A node who's child is the 
        /// next node to be inserted
        /// 
        /// PreCondition: The node is a member of a HypeerWeb
        /// Domain: All the nodes of the HypeerWeb
        /// PostCondtion: The node who is defined by the HypeerWeb
        /// structure to be the parent of the next node to be inserted
        /// </summary>
        /// <param name="n">Any node in the web</param>
        /// <returns>
        /// The father of the next node to be inserted (insertion point)
        /// </returns>
        private static Node insertionPoint(Node n)
        {
            List<Node> right = new List<Node>();
            right.Add(n);
            while ((right = goRight(right[0])).Count == 1) ;

            return searchRange(right);
        }

        /// <summary>
        /// Searches the range of nodes.
        /// 
        /// PreCondition: The list only contains 1 or 2 nodes.
        /// Both nodes must be members of the same HypeerWeb.
        /// Domain: All nodes in the HypeerWeb
        /// PostCondtion: The node that is to be returned for the 
        /// <see cref="insertionPoint"/> method
        /// </summary>
        /// <param name="right">
        /// A list of nodes.  Should only have 2 nodes, and if
        /// node 1 and 2 are the same, one node is returned, 
        /// else the function is recursively called with the 
        /// the new proper range.
        /// </param>
        /// <returns></returns>
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

        /// <summary>
        /// Goes from any node in the web and "goes right" based on node state.
        /// 
        /// If the Node State is Up, returns the <see cref="searchRange()"/> of the smallest neighbor
        /// and the largest Up node
        /// 
        /// if Down node, return the <see cref="searchRange()"/> of parent and smallest Down node
        /// 
        /// if Largest, check for <see cref="perfectCube()"/> and if it is, returns <see cref="searchRange()"/> 
        /// with 1 and 2 being the root node.  Else, returns the fold.
        /// 
        /// Else returns the Largest Neighbor
        /// 
        /// PreCondition: The node is a member of a HypeerWeb
        /// Domain: One or Two nodes in the HypeerWeb
        /// PostCondtion: The final return will have a list with two
        /// nodes from the HypeerWeb, representing the upper and lower
        /// bounds of the <see cref="searchRange"/> method
        /// </summary>
        /// <param name="n">Any node in the web</param>
        /// <returns>A list of 1 or 2 nodes</returns>
        private static List<Node> goRight(Node n)
        {
            List<Node> ret = new List<Node>();
            switch (n.CurrentState)
            {
                case NodeState.Up:
                     // largest up and Me*/

                    ret.Add(n.SmallestNeighbor);

                    ret.Add((from n1 in n.Up.Values
                             orderby n1.Id ascending
                             select n1).Last());

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

                    ret.Add(n.LargestNeighbor);
                    break;
            }
            return ret;
        }

        /// <summary>
        /// Gets the node defined by p.
        /// 
        /// PreCondition: The node is a member of a HypeerWeb, and the id is 
        /// an id of a node that is in the HypeerWeb
        /// Domain: All nodes in the HypeerWeb
        /// PostCondtion: The node who's Id is equal to the parameter P.
        /// </summary>
        /// <param name="p">The web id of the node.</param>
        /// <param name="n">Any node in the web</param>
        /// <returns>The node with web id == p</returns>
        private static Node getNode(uint p, Node n)
        {
            var nearest = (from node in n.AllNeighbors
                           orderby node.Id.Distance(p) ascending
                           select node).First();
            if (nearest.Id == p)
                return nearest;

            return getNode(p, nearest);
        }

        /// <summary>
        /// Gets if the the web is a Perfect cube.
<<<<<<< .mine
        /// 
        /// PreCondition: The node is a member of a HypeerWeb
        /// who's <see cref="NodeState"/> is equal to Largest
        /// Domain: True of False
        /// PostCondtion: The answer to if the current HypeerWeb
        /// is a perfect cube
=======
        /// 
        /// PreCondition: Node n is a valid member of the 
        /// Hypeerweb and their status is Largest
        /// Domain: Any Node in the Hypeerweb
        /// PostCondition: The status of the Hypeerweb with regards 
        /// to it being complete.
>>>>>>> .r104
        /// </summary>
        /// <param name="n">Any node in the web</param>
        /// <returns></returns>
        private static bool perfectCube(Node n)
        {
            return n.Fold.Id == 0;
        }

        #endregion

        /// <summary>
        /// Inserts a new node in the hyperweb, and returns it.
        /// If this is the insertion point, insert it here, otherwise
        /// send it closer to the insertion point.
        /// 
        /// PreCondition: True
        /// Domain: none
        /// PostCondtion: The HypeerWeb of the current node will increase by one node
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
                n.Fold = this;

                return n;
            }

            var insert = insertionPoint(this);

            return insert.insertChildNode();
        }

        /// <summary>
        /// Inserts the child node.
        /// 
        /// PreCondition: True
        /// Domain: none
        /// PostCondtion: Following the logic of the HypeerWeb,
        /// a new node is inserted into the correct place and
        /// all the settings for the Folds, neighbors, and parent
        /// are modified accordingly.
        /// </summary>
        /// <returns>The newly inserted node</returns>
        private Node insertChildNode()
        {
            Node n = null;
            if (Id == 0)
            {
                n = new Node(Fold.Id + 1);
            }
            else
            {
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

                n = new Node(newId);
            }

            foreach (var neighbor in Neighbors)
            {
                if (!neighbor.HasChild)
                {
                    n.addSurrogate(neighbor);
                }
            }

            var oldUp = new List<Node>(Up.Values);
            foreach (var upNode in oldUp)
            {
                Up.Remove(upNode.Id);
                upNode.addNeighbor(n);
                n.addNeighbor(upNode);
            }

            if (OldFold == null)
            {
                Fold.OldFold = Fold.Fold;
                Fold.Fold = n;
                n.Fold = Fold;
            }
            else
            {
                OldFold.Fold = n;
                n.Fold = OldFold;
                OldFold.OldFold = null;
            }

            OldFold = null;

            Neighbors.Add(n);
            n.Neighbors.Add(this);

            return n;
        }

        /// <summary>
        /// Adds the node as a neighbor.
        /// 
        /// PreCondition: The node is a member of a HypeerWeb
        /// Domain: none
        /// PostCondtion: the node is added as a neighbor in the HypeerWeb
        /// </summary>
        /// <param name="n">Any node in the web</param>
        private void addNeighbor(Node n)
        {
            if (Down.ContainsKey(n.Id))
                Down.Remove(n.Id);

            Neighbors.Add(n);
        }

        /// <summary>
        /// Adds the node as a surrogate.
        /// 
        /// PreCondition: The node is a member of a HypeerWeb
        /// Domain: none
        /// PostCondtion: The node specified is turned into a
        /// surrogate node for the parameter node
        /// </summary>
        /// <param name="n">Any node in the web</param>
        /// <returns></returns>
        private void addSurrogate(Node n)
        {
            Down.Add(n.SurrogateId(Id), n);
            n.Up.Add(Id, this);
        }

        /// <summary>
        /// Removes the current node from the hypeerweb
        /// 
        /// PreCondition:
        /// Domain:
        /// PostCondtion:
        /// </summary>
        public void Remove()
        {
            var insert = insertionPoint(this);
            uint lastid = insert.ChildId(AbsoluteLargestNeighbor.Id) - 1;
            var lastnode = getNode(lastid, this);

            //For testing purposes, take the last one out of AllNodes.
            Node.AllNodes.Remove(lastid);
            Node.AllNodes.Remove(Id);

            // tell all of lastnodes neighborst to remove him as a neighbor
            List<Node> tmpneighbors = lastnode.AllNeighbors;
            for (int i = 0; i < tmpneighbors.Count; i++)
            {
                tmpneighbors[i].RemoveNeighbor(lastnode);
            }

            //tell the parent of lastnode to remove the child
            //Node lastparent = getNode(lastnode.ParentId, this);
            //lastparent.RemoveChild();

            //copy remove node info into lastnode
            lastnode.Id = this.Id;
            lastnode.Neighbors = this.Neighbors;
            lastnode.Up = this.Up;
            lastnode.Down = this.Down;
            lastnode.Fold = this.Fold;
            lastnode.OldFold = this.OldFold;

            //tell all remove nodes neighbors to switch to lastnode
            for (int i = 0; i < this.AllNeighbors.Count; i++)
            {
                this.AllNeighbors[i].SwitchNeighbor(this, lastnode);
            }

            if (lastid != Id)
                Node.AllNodes.Add(Id, lastnode);
        }

        /// <summary>
        /// remove node n from the hypeerweb
        /// 
        /// PreCondition: The node is a member of a HypeerWeb
        /// Domain:
        /// PostCondtion:
        /// </summary>
        /// <param name="n">node to be removed</param>
        public void Remove(Node n)
        {
            n.Remove();
        }

        /// <summary>
        /// remove node with give Id from the hypeerweb
        /// 
        /// PreCondition:
        /// Domain:
        /// PostCondtion:
        /// </summary>
        /// <param name="Id">Id of node to be removed</param>
        public void Remove(uint Id)
        {
            Node rem = getNode(Id, this);
            if (rem != null)
                rem.Remove();
        }

        /// <summary>
        /// Remove neighbor node and replace with lastnode
        /// 
        /// PreCondition: The node is a member of a HypeerWeb
        /// Domain:
        /// PostCondtion:
        /// </summary>
        /// <param name="node">Current neighbor of this</param>
        /// <param name="lastnode">Replacement node</param>
        private void SwitchNeighbor(Node node, Node lastnode)
        {
            if (Neighbors.Contains(node)) //if it is a neighbor remove the nieghbor and add lastnode as a neighbor
            {
                Neighbors.Remove(node);
                this.addNeighbor(lastnode);
            }
            else if (Up.ContainsValue(node)) //if it is an up pointer remove from dictionary and add lastnode with same key
            {
                Up.Remove(node.Id);
                Up.Add(node.Id, lastnode);
            }
            else if (Down.ContainsValue(node)) //same as if its an up pointer
            {
                Down.Remove(node.Id);
                Down.Add(node.Id, lastnode);
            }
        }

        /// <summary>
        /// Remove nieghbore if it exists
        /// 
        /// PreCondition: The node is a member of a HypeerWeb
        /// Domain:
        /// PostCondtion:
        /// </summary>
        /// <param name="n">neighbor of this</param>
        public void RemoveNeighbor(Node n)
        {
            if (Neighbors.Contains(n))
            {
                Neighbors.Remove(n);
                if (Id != n.ParentId)
                    this.addSurrogate(getNode(n.ParentId, this));
                else
                {
                    if (n.Fold.OldFold == null)
                    {
                        OldFold = n.Fold;
                        OldFold.Fold = this;

                    }
                    else
                    {
                        Fold = n.Fold;
                        Fold.Fold = Fold.OldFold;
                        Fold.OldFold = null;
                    }

                    if (Fold.Id == Id)
                        Fold = null;
                    if (OldFold.Id == Id)
                        OldFold = null;
                }
            }
            else if (Up.ContainsKey(n.Id))
            {
                Up.Remove(n.Id);
            }
            else if (Down.ContainsKey(n.Id))
            {
                Down.Remove(n.Id);
            }
        }
    }
}
