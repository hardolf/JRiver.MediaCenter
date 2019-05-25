using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

/*
 * Not used in this solution version.
 */


namespace MediaCenter.SharedComponents
{

    /// <summary>
    /// Serializing helper class.
    /// </summary>
    /// <remarks>
    /// Source: https://stackoverflow.com/questions/2347642/deserialize-from-string-instead-textreader
    /// </remarks>
    public static class Serialize
    {

        /// <summary>
        /// Patches the namespace if missing from the root element.
        /// </summary>
        /// <param name="xmlText">The XML text.</param>
        /// <returns></returns>
        public static string PatchMissingNamespace(this string xmlText)
        {
            const string ns = "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"";

            var ret = new StringBuilder(xmlText);
            var idx1 = xmlText.IndexOf("?>", StringComparison.InvariantCultureIgnoreCase); // Document header element end
            var idx2 = xmlText.IndexOf(">", idx1 + 2, StringComparison.InvariantCultureIgnoreCase); // Root element end

            if ((idx1 > 0) && (idx2 > idx1))
                ret.Insert(idx2, $" {ns}");

            return ret.ToString();
        }


#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved
        /// <summary>
        /// Deserializes from file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath">The file path.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <returns>
        ///   <see cref="T" /> object.
        /// </returns>
#pragma warning restore CS1574 // XML comment has cref attribute that could not be resolved
        public static T XmlDeserializeFromFile<T>(this string filePath, params Type[] knownTypes)
        {
            return (T)XmlDeserializeFromFile(filePath, typeof(T), knownTypes);
        }


#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved
        /// <summary>
        /// Deserializes from file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="type">The type.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <returns>
        ///   <see cref="type" /> object.
        /// </returns>
#pragma warning restore CS1574 // XML comment has cref attribute that could not be resolved
        public static object XmlDeserializeFromFile(this string filePath, Type type, params Type[] knownTypes)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: \"{filePath}\"");

            object ret;
            XmlSerializer serializer;

            try
            {
                serializer = new XmlSerializer(type, knownTypes);
            }
            catch (Exception)
            {
                throw;
            }

            using (var reader = new StreamReader(filePath, true))
            {
                try
                {
                    ret = serializer.Deserialize(reader);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return ret;
        }


#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved
        /// <summary>
        /// Deserializes from string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml">The object data.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <returns>
        ///   <see cref="T" /> object.
        /// </returns>
#pragma warning restore CS1574 // XML comment has cref attribute that could not be resolved
        public static T XmlDeserializeFromString<T>(this string xml, params Type[] knownTypes)
        {
            return (T)XmlDeserializeFromString(xml, typeof(T), knownTypes);
        }


#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved
        /// <summary>
        /// Deserializes from string.
        /// </summary>
        /// <param name="xml">The object data.</param>
        /// <param name="type">The type.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <returns>
        ///   <see cref="type" /> object.
        /// </returns>
#pragma warning restore CS1574 // XML comment has cref attribute that could not be resolved
        public static object XmlDeserializeFromString(this string xml, Type type, params Type[] knownTypes)
        {
            if (xml.IsNullOrEmptyTrimmed())
                throw new ArgumentNullException(nameof(xml));

            object ret;
            XmlSerializer serializer;

            try
            {
                serializer = new XmlSerializer(type, knownTypes);
            }
            catch (Exception)
            {
                throw;
            }

            using (var reader = new StringReader(xml))
            {
                try
                {
                    ret = serializer.Deserialize(reader);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return ret;
        }


        /// <summary>
        /// Serializes to file.
        /// </summary>
        /// <param name="objectInstance">The object instance.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="knownTypes">The known types.</param>
        public static void XmlSerializeToFile(this object objectInstance, string filePath, params Type[] knownTypes)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: \"{filePath}\"");

            XmlSerializer serializer;

            try
            {
                serializer = new XmlSerializer(objectInstance.GetType(), knownTypes);
            }
            catch (Exception)
            {
                throw;
            }

            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                try
                {
                    serializer.Serialize(writer, objectInstance);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Serializes to string.
        /// </summary>
        /// <param name="objectInstance">The object instance.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <returns>XML string.</returns>
        public static string XmlSerializeToString(this object objectInstance, params Type[] knownTypes)
        {
            var ret = new StringBuilder();
            XmlSerializer serializer;

            try
            {
                serializer = new XmlSerializer(objectInstance.GetType(), knownTypes);
            }
            catch (Exception)
            {
                throw;
            }

            using (var writer = new StringWriter(ret))
            {
                try
                {
                    serializer.Serialize(writer, objectInstance);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return ret.ToString();
        }

    }

}
