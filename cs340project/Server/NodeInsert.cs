using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Server
{
    /// <summary>
    /// Common Extension Methods for the type uint
    /// 
    /// Domain: All positive 32bit integers
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
    /// Domain: all node of a Hypeerweb
    /// Invariant: Node must be a valid member of a Hypeerweb
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
        /// <returns>True if the web is empty, false otherwise</returns>
        public virtual bool emptyWeb()
        {
            return (Id == 0 && Neighbors.Count == 0);
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
        /// If the Node State is Up, returns the <see cref="searchRange"/> of the smallest neighbor
        /// and the largest Up node
        /// 
        /// if Down node, return the <see cref="searchRange"/> of parent and smallest Down node
        /// 
        /// if Largest, check for <see cref="perfectCube"/> and if it is, returns <see cref="searchRange()"/> 
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

                    ret.Add(n.SmallestUp);

                    break;
                case NodeState.Down:
                    ret.Add(n.Parent);

                    ret.Add(n.SmallestDown);

                    // My parent and Down min
                    break;
                case NodeState.Largest:
                    if (n.Id == 0 || perfectCube(n))
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
            if (n.Id == p)
                return n;

            var nearest = n.ClosestNeighbor(p);
            if (nearest.Id == p)
                return nearest;

            return getNode(p, nearest);
        }

        /// <summary>
        /// Gets if the the web is a Perfect cube.<<<<<<< .mine
        /// Gets if the the web is a Perfect cube.
        /// 
        /// PreCondition: The node is a member of a HypeerWeb
        /// who's <see cref="NodeState"/> is equal to Largest
        /// Domain: True of False
        /// PostCondtion: The answer to if the current HypeerWeb
        /// is a perfect cube
        /// 
        /// PreCondition: Node n is a valid member of the 
        /// Hypeerweb and their status is Largest
        /// Domain: Any Node in the Hypeerweb
        /// PostCondition: The status of the Hypeerweb with regards 
        /// to it being complete.
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
        /// PostCondtion: The HypeerWeb of the current node will increase by one node
        /// </summary>
        /// <returns>The newly inserted node.</returns>
        public virtual void InsertNode(Node n)
        {
            if (emptyWeb())
            {
                n.Id = 1;

                n.addNeighbor(this);
                addNeighbor(n);
                this.Fold = n;
                n.Fold = this;

                return;
            }

            var insert = insertionPoint(this);
            insert.insertChildNode(n);
        }

        /// <summary>
        /// Inserts the child node.
        /// 
        /// PreCondition: True
        /// PostCondtion: Following the logic of the HypeerWeb,
        /// a new node is inserted into the correct place and
        /// all the settings for the Folds, neighbors, and parent
        /// are modified accordingly.
        /// </summary>
        /// <returns>The newly inserted node</returns>
        public virtual void insertChildNode(Node n)
        {
            n.Id = NextChildId;
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

            addNeighbor(n);
            n.addNeighbor(this);
        }

        /// <summary>
        /// Adds the node as a neighbor.
        /// 
        /// PreCondition: The node is a member of a HypeerWeb
        /// PostCondtion: the node is added as a neighbor in the HypeerWeb
        /// </summary>
        /// <param name="n">Any node in the web</param>
        public virtual void addNeighbor(Node n)
        {
            if (Down.ContainsKey(n.Id))
                Down.Remove(n.Id);

            Neighbors.Add(n);
        }

        /// <summary>
        /// Adds the node as a surrogate.
        /// 
        /// PreCondition: The node is a member of a HypeerWeb
        /// PostCondtion: The node specified is turned into a
        /// surrogate node for the parameter node
        /// </summary>
        /// <param name="n">Any node in the web</param>
        /// <returns></returns>
        public virtual void addSurrogate(Node n)
        {
            Down.Add(n.SurrogateId(Id), n);
            n.AddUp(Id, this);
        }

        /// <summary>
        /// Removes the current node from the hypeerweb
        /// 
        /// PreCondition: Current Node is a member of the Hypeerweb
        /// PostCondtion: Current Node is no longer a member of the Hypeerweb
        /// </summary>
        public virtual Node Remove()
        {
            if (emptyWeb())
                return this;
            Node insert = insertionPoint(this);
            uint lastid = insert.NextChildId - 1;
            Node lastnode = getNode(lastid, this);


            // Me - tell all neighbors that I'm not here
            // last insert - All pointers and neighbors point elsewhere
            // Me's pointers put into last insert.
            // 

            disconnectDeletionPoint(lastnode);

            deleteNode(this, lastnode);

            if (this == lastnode)
                return insert;
            return lastnode;
        }

        private static void deleteNode(Node node, Node lastnode)
        {
            if (node != lastnode)
            {
                lastnode.Neighbors.Clear();
                lastnode.Up.Clear();
                lastnode.Down.Clear();

                foreach (var n in node.Neighbors)
                {
                    n.Neighbors.Remove(node);
                    n.Neighbors.Add(lastnode);
                    lastnode.Neighbors.Add(n);
                }

                lastnode.Down = new Dictionary<uint, Node>(node.Down);
                foreach (var n in node.Down.Values)
                {
                    n.Up.Remove(node.Id);
                    n.Up.Add(node.Id, lastnode);
                }

                lastnode.Up = new Dictionary<uint, Node>(node.Up);
                foreach (var n in node.Up.Values)
                {
                    n.Down.Remove(node.SurrogateId(n.Id));
                    n.Down.Add(node.SurrogateId(n.Id), lastnode);
                }

                lastnode.Fold = node.Fold;
                lastnode.OldFold = node.OldFold;

                if (node.Fold != null && node.Fold.Fold.Id == node.Id)
                    node.Fold.Fold = lastnode;

                if (node.OldFold != null && node.OldFold.Fold.Id == node.Id)
                    node.OldFold.Fold = lastnode;

                if (node.Fold != null && node.Fold.OldFold != null && node.Fold.OldFold.Id == node.Id)
                    node.Fold.OldFold = lastnode;

                lastnode.Id = node.Id;

            }
            node.Id = uint.MaxValue;
        }

        /// <summary>
        /// 	find pseudoparent => p
        ///find all neighbors but pseudoparent => np

        ///disconnect p (np, fold)
        ///    delete dp as neighbor from p
        ///    set up ointers from p to np
        ///    if (fold == p.fold)
        ///        do nothing
        ///    else
        ///        old fold = fold

        ///for all np
        ///    disconnect n (parent, dp)
        ///        remove dp from n's neighbors
        ///        add parent to n's neighbors

        ///disconnect from fold
        ///    disconnect fold (parent)
        ///        fold's fold = parent

        /// </summary>
        /// <param name="dp"></param>

        private static void disconnectDeletionPoint(Node dp)
        {
            Node p = getNode(dp.ParentId, dp);
            var np = new List<Node>(dp.Neighbors);
            np.Remove(p);

            p.Neighbors.Remove(dp);

            foreach (var npNode in np)
            {
                npNode.addSurrogate(p);
                npNode.Neighbors.Remove(dp);
            }
            foreach (var dpDown in dp.Down.Values)
            {
                dpDown.Up.Remove(dp.Id);
            }

            if (dp.Fold != p.Fold)
                p.OldFold = dp.Fold;

            dp.Fold.Fold = p;

            if (dp.Fold.Fold == dp.Fold.OldFold)
                dp.Fold.OldFold = null;

            if (p.Fold == p)
                p.Fold = null;
            if (p.OldFold == p)
                p.OldFold = null;
        }

        /// <summary>
        /// Effectively finishes the process of removing node from the hypeerweb
        /// 
        /// Precondition: lastnode and node are valid members of the hypeerweb
        ///                 lastnode and node have been preped for removal
        /// Postcondition: node is no longer a member of the hypeerweb and lastnode has taken its place.
        /// </summary>
        /// <param name="lastnode"> The last inserted Node in the hypeerweb</param>
        /// <param name="node">Node to be removed from the hypeerweb</param>

        /// <summary>
        /// Modifies the Folds as a result of removing Node from the hypeerweb
        /// 
        /// Precondition: lastnode and node are valid members of the hypeerweb.
        ///                 lastnode is the last node inserted in the hypeerweb.
        ///                  lastnode will hereafter no longer be a member of the hypeerweb.
        /// Postcondition: The folds are such that the removal of lastnode will result in
        ///                 a valid hypeerweb according to the specifications
        /// </summary>
        /// <param name="lastnode">Last Node inserted in the hypeerweb</param>
        /// <param name="node">Node to be removed</param>

        /// <summary>
        /// copy all neighbors from node into lastnode
        /// 
        /// Precondition: node and lastnode are both valid members of a hypeerweb
        ///                 node is the Node to be removed from the hypeerweb
        ///                  lastnode is the last inserted Node in the hypeerweb
        /// Postcondition: lastnode and node have effectively traded places
        ///                 with respect to their neighbors.
        /// </summary>
        /// <param name="lastnode">the last Node inserted in the hypeerweb</param>
        /// <param name="node">Node to be removed from the hypeerweb</param>
        private static void addNeighbors(Node lastnode, Node node)
        {
            foreach (Node n in node.Neighbors)
            {
                lastnode.addNeighbor(n);
                n.addNeighbor(lastnode);
            }
        }

        /// <summary>
        /// Updates All the neighbors of the last Node inserted such that upon
        /// removal of lastnode the hypeerweb is correct according to specification.
        /// 
        /// Precondition: lastnode is a valid member of a hypeerweb.
        ///                lastnode is the last inserted Node in the hypeerweb.
        /// Postcondition: lastnode will hereafter no longer be needed in the hypeerweb.
        /// </summary>
        /// <param name="lastnode">Last Node inserted in the hypeerweb</param>

        /// <summary>
        /// Removes node as a neighbor from its respective neighbors
        /// 
        /// Precondition: node is a valid member of a hypeerweb
        /// Postcondition: node is no longer associated with its respective neighbors and
        ///                 will hereafter be removed from the hypeerweb
        /// </summary>
        /// <param name="node">Node to be hereafter removed from the hypeerweb</param>

        /// <summary>
        /// remove node with give Id from the hypeerweb
        /// 
        /// PreCondition: Id is a valid Id for a Node in the HypeerWeb
        /// PostCondtion: The Node specified by said Id is no longer a member of the Hypeerweb
        /// </summary>
        /// <param name="Id">Id of node to be removed</param>
        public virtual Node RemoveById(uint Id)
        {
            Node rem = getNode(Id, this);
            if (rem != null)
                return rem.Remove();
            return null;
        }
    }
}
