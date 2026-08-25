using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
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
        internal static string PatchMissingNamespace(this string xmlText)
        {
            if (xmlText.IsNullOrEmptyTrimmed()) throw new ArgumentNullException(nameof(xmlText));

            const string ns = "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"";

            var ret = new StringBuilder(xmlText);
            var idx1 = xmlText.IndexOf("?>", StringComparison.InvariantCultureIgnoreCase); // Document header element end
            var idx2 = xmlText.IndexOf(">", idx1 + 2, StringComparison.InvariantCultureIgnoreCase); // Root element end

            if ((idx1 > 0) && (idx2 > idx1))
                ret.Insert(idx2, $" {ns}");

            return ret.ToString();
        }


        /// <summary>
        /// Serializes an object to XML string, preserving CR+LF.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns>Serialized XML string.</returns>
        public static string ToXmlWithNewlines<T>(this T obj)
        {
            if (obj == null) return string.Empty;

            var serializer = new XmlSerializer(typeof(T));
            var settings = new XmlWriterSettings
            {
                Indent = true,
                NewLineHandling = NewLineHandling.Entitize, // Bevar CR+LF i strenge
                Encoding = Encoding.UTF8
            };

            var sb = new StringBuilder();
            using (var stringWriter = new StringWriter(sb))
            using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
            {
                serializer.Serialize(xmlWriter, obj);
            }
            return sb.ToString();
        }


        /// <summary>
        /// Serializes an object to XML string, preserving CR+LF.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="knownTypes"></param>
        /// <returns>Serialized XML string.</returns>
        public static string ToXmlWithNewlines<T>(this T obj, params Type[] knownTypes)
        {
            if (obj == null) return string.Empty;

            var serializer = new XmlSerializer(typeof(T), knownTypes);  // ← Tilføjet knownTypes
            var settings = new XmlWriterSettings
            {
                Indent = true,
                NewLineHandling = NewLineHandling.Entitize,
                Encoding = Encoding.UTF8
            };

            var sb = new StringBuilder();
            using (var stringWriter = new StringWriter(sb))
            using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
            {
                serializer.Serialize(xmlWriter, obj);
            }
            return sb.ToString();
        }


#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved
        /// <summary>
        /// Deserializes from file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath">The file path.</param>
        /// <param name="xmlElementEventHandler">The XML element event handler.</param>
        /// <param name="replaceDictionary">The replace dictionary.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <returns>
        ///   <see cref="T" /> object.
        /// </returns>
#pragma warning restore CS1574 // XML comment has cref attribute that could not be resolved
        public static T XmlDeserializeFromFile<T>(this string filePath, XmlElementEventHandler xmlElementEventHandler = null, IDictionary<string, string> replaceDictionary = null, params Type[] knownTypes)
        {
            return (T)XmlDeserializeFromFile(filePath, typeof(T), xmlElementEventHandler, replaceDictionary, knownTypes);
        }


#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved
        /// <summary>
        /// Deserializes from file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="type">The type.</param>
        /// <param name="xmlElementEventHandler">The XML element event handler.</param>
        /// <param name="replaceDictionary">The replace dictionary.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <returns>
        ///   <see cref="type" /> object.
        /// </returns>
        /// <exception cref="ArgumentNullException">filePath</exception>
        /// <exception cref="FileNotFoundException">File not found: \"{filePath}\"</exception>
#pragma warning restore CS1574 // XML comment has cref attribute that could not be resolved
        internal static object XmlDeserializeFromFile(this string filePath, Type type, XmlElementEventHandler xmlElementEventHandler = null, IDictionary<string, string> replaceDictionary = null, params Type[] knownTypes)
        {
            if (filePath.IsNullOrEmptyTrimmed()) throw new ArgumentNullException(nameof(filePath));
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: \"{filePath}\"");

            object ret;

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(stream, true))
                {
                    var s = reader.ReadToEnd();

                    ret = s.XmlDeserializeFromString(type, xmlElementEventHandler, replaceDictionary, knownTypes);
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
        public static T XmlDeserializeFromString<T>(this string xml, XmlElementEventHandler xmlElementEventHandler = null, IDictionary<string, string> replaceDictionary = null, params Type[] knownTypes)
        {
            return (T)XmlDeserializeFromString(xml, typeof(T), xmlElementEventHandler, replaceDictionary, knownTypes);
        }


#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved
        /// <summary>
        /// Deserializes from string.
        /// </summary>
        /// <param name="xml">The object data.</param>
        /// <param name="type">The type.</param>
        /// <param name="xmlElementEventHandler">The XML element event handler.</param>
        /// <param name="replaceDictionary">The replace dictionary.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <returns>
        ///   <see cref="type" /> object.
        /// </returns>
        /// <exception cref="ArgumentNullException">xml</exception>
#pragma warning restore CS1574 // XML comment has cref attribute that could not be resolved
        internal static object XmlDeserializeFromString(this string xml, Type type, XmlElementEventHandler xmlElementEventHandler = null, IDictionary<string, string> replaceDictionary = null, params Type[] knownTypes)
        {
            if (xml.IsNullOrEmptyTrimmed()) throw new ArgumentNullException(nameof(xml));

            object ret;
            //var sb = new StringBuilder(xml.LfToCrLf());
            var sb = new StringBuilder(xml);
            var serializer = new XmlSerializer(type, knownTypes);

            if (xmlElementEventHandler != null)
                serializer.UnknownElement += xmlElementEventHandler;

            if (replaceDictionary != null)
            {
                foreach (var kvp in replaceDictionary)
                {
                    sb.Replace(kvp.Key, kvp.Value);
                }
            }

            StringReader sr = null;
            try
            {
                sr = new StringReader(sb.ToString());
                using (var xr = XmlReader.Create(sr))
                {
                    ret = serializer.Deserialize(xr);
                }

            }
            finally
            {
                if (sr != null)
                    sr.Dispose();
            }

            return ret;
        }


        /// <summary>
        /// Serializes an object to a file with UTF-8 encoding.
        /// </summary>
        /// <param name="objectInstance">The object instance.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <exception cref="ArgumentNullException">filePath</exception>
        /// <exception cref="DirectoryNotFoundException">Directory not found for the file to write: \"{filePath}\"</exception>
        public static void XmlSerializeToFile(this object objectInstance, string filePath, params Type[] knownTypes)
        {
            if (objectInstance == null) throw new ArgumentNullException(nameof(objectInstance));
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
            if (!Directory.Exists(Path.GetDirectoryName(filePath))) throw new DirectoryNotFoundException($"Directory not found for the file to write: \"{filePath}\"");

            using var fileStream = File.Create(filePath);
            using var streamWriter = fileStream.CreateUTF8NoBOM();
            using var xmlWriter = XmlWriter.Create(streamWriter, new XmlWriterSettings
            {
                Indent = true,
                NewLineHandling = NewLineHandling.Entitize,
                Encoding = Encoding.UTF8
            });

            var objectType = objectInstance.GetType();
            new XmlSerializer(objectType, knownTypes).Serialize(xmlWriter, objectInstance);
        }


        /// <summary>
        /// Serializes to string.
        /// </summary>
        /// <param name="objectInstance">The object instance.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <returns>XML string.</returns>
        public static string XmlSerializeToString(this object objectInstance, params Type[] knownTypes)
        {
            if (objectInstance == null) throw new ArgumentNullException(nameof(objectInstance));

            var ret = ToXmlWithNewlines(objectInstance, knownTypes);

            return ret.ToString();
        }


        /// <summary>
        /// Serializes to string.
        /// </summary>
        /// <param name="objectInstance">The object instance.</param>
        /// <param name="ns">The ns.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <returns>
        /// XML string.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">objectInstance</exception>
        public static string XmlSerializeToString(this object objectInstance, XmlSerializerNamespaces ns, params Type[] knownTypes)
        {
            if (objectInstance == null) throw new ArgumentNullException(nameof(objectInstance));

            var ret = new StringBuilder();
            var serializer = new XmlSerializer(objectInstance.GetType(), knownTypes);

            using (var writer = new StringWriterUTF8(ret))
            {
                serializer.Serialize(writer, objectInstance, ns);
            }

            return ret.ToString();
        }

    }

}
