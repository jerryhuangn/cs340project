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
    [Serializable]
    public class Visitor
    {
        /// <summary>
        /// A unique string identifying this message
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// When was this visitor created initially?
        /// </summary>
        public DateTime Created { get; set; }

        public Visitor()
        {
            Id = Guid.NewGuid().ToString();
            Created = DateTime.Now;
        }


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
        public virtual void Visit(Dictionary<string, object> Payload)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class MessageVisitor : Visitor
    {
        /// <summary>
        /// The message to be delivered by this visitor
        /// </summary>
        public string Message { get; set; }
        public MessageVisitor(string msg)
            : base()
        {
            Message = msg;
        }

        /// <summary>
        /// Visits the specified node. When visited, something happens
        /// and is recorded in the Payload. This is the base class
        /// so it currently does nothing.
        /// Pre: The Node is valid
        /// Post: Whatever type of visitor the instance is,
        /// it will correctly do its job
        /// </summary>
        /// <param name="Payload">The information this visitor acts upon.</param>
        public override void Visit(Dictionary<string, object> Payload)
        {
            List<string> Messages = (List<string>)Payload["Messages"];
            Messages.Add(this.Message);
        }
    }
}
