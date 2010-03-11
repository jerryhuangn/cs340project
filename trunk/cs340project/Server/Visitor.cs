using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{

    /// <summary>
    /// Base class used in the visitor pattern.  Only has one method, Visit.
    /// 
    /// Domain: Nodes in HypeerWeb (Specifically the Payload)
    /// </summary>
    public class Visitor
    {
        /// <summary>
        /// Visits the specified node. When visited, something happens
        /// and is recorded in the Payload. This is the base class
        /// so it currently does nothing.
        /// 
        /// Pre: The Node is valid
        /// Post: Whatever type of visitor the instance is, 
        /// it will correctly do its job
        /// </summary>
        /// <param name="obj">An object.</param>
        public void Visit(object obj)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MessageVisitor : Visitor
    {
        /// <summary>
        /// Visits the specified node. When visited, something happens
        /// and is recorded in the Payload. This is the base class
        /// so it currently does nothing.
        /// Pre: The Node is valid
        /// Post: Whatever type of visitor the instance is,
        /// it will correctly do its job
        /// </summary>
        /// <param name="obj">An object.</param>
        public void Visit(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
