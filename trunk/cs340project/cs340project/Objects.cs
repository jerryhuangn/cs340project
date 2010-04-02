using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;

namespace cs340project
{
    /// <summary>
    /// Person Name Class.  Holds different names for a Person
    /// </summary>
    [Serializable]
    public class PersonName
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public string First { get; private set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public string Last { get; private set; }
        /// <summary>
        /// Gets or sets the nick name.
        /// </summary>
        /// <value>The nick name.</value>
        public string Nick { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonName"/> class.
        /// </summary>
        public PersonName(string first, string last, string nick)
        {
            First = first;
            Last = last;
            Nick = nick;
        }
    }

    /// <summary>
    /// A Person.  Serializable Data
    /// </summary>
    [Serializable]
    public class Person
    {
        /// <summary>
        /// Gets or sets my name.
        /// </summary>
        /// <value>My name.</value>
        public virtual PersonName Name { get; set; }
        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        /// <value>The age.</value>
        public virtual uint Age { get; set; }

        /// <summary>
        /// Test for an array.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <returns></returns>
        public virtual int[] DoubleArray(int[] v)
        {
            for (var i = 0; i < v.Length; i++)
                v[i] *= 2;
            return v;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class.
        /// </summary>
        public Person()
        {
        }
    }
}
