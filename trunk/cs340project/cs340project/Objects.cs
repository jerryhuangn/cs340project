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
    /// Contains Static Serialization Methods for both XML and Binary
    /// </summary>
    public class Serializer
    {
        public static bool BinarySerializeObject(Stream stream, object obj)
        {
            BinaryFormatter bformatter = new BinaryFormatter();

            try
            {
                bformatter.Serialize(stream, obj);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool BinarySerialize<T>(Stream stream, T obj)
        {
            BinaryFormatter bformatter = new BinaryFormatter();

            try
            {
                bformatter.Serialize(stream, obj);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool XMLSerializeObject(Stream stream, object obj)
        {
            XmlSerializer x = new XmlSerializer(obj.GetType());

            try
            {
                x.Serialize(stream, obj);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool XMLSerialize<T>(Stream stream, T obj)
        {
            XmlSerializer x = new XmlSerializer(typeof(T));

            try
            {
                x.Serialize(stream, obj);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// Person Name Class.  Holds different names for a Person
    /// </summary>
    [Serializable()]
    public class PersonName : ISerializable
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public string First { get; set; }
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public string Last { get; set; }
        /// <summary>
        /// Gets or sets the nick name.
        /// </summary>
        /// <value>The nick name.</value>
        public string Nick { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonName"/> class.
        /// </summary>
        public PersonName()
        {
            First = string.Empty;
            Last = string.Empty;
            Nick = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonName"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        public PersonName(SerializationInfo info, StreamingContext context)
        {
            First = (string)info.GetValue("First", typeof(string));
            Last = (string)info.GetValue("Last", typeof(string));
            Nick = (string)info.GetValue("Nick", typeof(string));
        }

        #region ISerializable Members

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization.</param>
        /// <exception cref="T:System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("First", First);
            info.AddValue("Last", Last);
            info.AddValue("Nick", Nick);
        }

        #endregion
    }

    /// <summary>
    /// A Person.  Serializable Data
    /// </summary>
    [Serializable()]
    public class Person : ISerializable
    {
        /// <summary>
        /// Gets or sets my name.
        /// </summary>
        /// <value>My name.</value>
        public virtual PersonName MyName { get; set; }
        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        /// <value>The age.</value>
        public uint Age { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class.
        /// </summary>
        public Person()
        {
            MyName = new PersonName();
            Age = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        public Person(SerializationInfo info, StreamingContext context)
        {
            MyName = info.GetValue("MyName", typeof(PersonName)) as PersonName;
            Age = (uint)info.GetValue("Age", typeof(uint));
        }

        #region ISerializable Members

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization.</param>
        /// <exception cref="T:System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("MyName", MyName);
            info.AddValue("Age", Age);
        }

        #endregion
    }
}
