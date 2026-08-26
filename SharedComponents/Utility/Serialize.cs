using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;


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
        /// Cache of the <see cref="XmlSerializer"/> instances, keyed by the serialized type and its known types.
        /// </summary>
        /// <remarks>
        /// Only the <see cref="XmlSerializer(Type)"/> and <see cref="XmlSerializer(Type, string)"/> constructors
        /// reuse the dynamically generated serialization assembly. Every other overload - including the ones
        /// taking known types - generates a fresh assembly per call, and such an assembly can never be unloaded.
        /// Caching the serializer instances here is what keeps repeated load/save rounds from leaking assemblies.
        /// </remarks>
        private static readonly ConcurrentDictionary<string, XmlSerializer> _serializerCache = new ConcurrentDictionary<string, XmlSerializer>();


        /// <summary>
        /// Gets a cached serializer for the specified type and known types, creating it if necessary.
        /// </summary>
        /// <param name="type">The type to serialize or deserialize.</param>
        /// <param name="knownTypes">The known types. May be null or empty.</param>
        /// <returns>A shared <see cref="XmlSerializer"/> instance.</returns>
        /// <exception cref="ArgumentNullException">type</exception>
        private static XmlSerializer GetSerializer(Type type, Type[] knownTypes)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var hasKnownTypes = (knownTypes != null) && (knownTypes.Length > 0);
            var key = hasKnownTypes
                ? type.AssemblyQualifiedName + "|" + string.Join("|", knownTypes.Select(t => t.AssemblyQualifiedName))
                : type.AssemblyQualifiedName;

            // An empty known type array would still hit the non-caching constructor overload, hence the branch.
            return _serializerCache.GetOrAdd(key, _ => hasKnownTypes
                ? new XmlSerializer(type, knownTypes)
                : new XmlSerializer(type));
        }


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
        /// <param name="obj">The object instance.</param>
        /// <param name="type">The type to serialize the object as.</param>
        /// <param name="knownTypes">The known types. May be null or empty.</param>
        /// <returns>Serialized XML string.</returns>
        private static string ToXmlWithNewlinesCore(object obj, Type type, Type[] knownTypes)
        {
            if (obj == null) return string.Empty;

            var serializer = GetSerializer(type, knownTypes);
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
        /// <returns>Serialized XML string.</returns>
        public static string ToXmlWithNewlines<T>(this T obj)
        {
            return ToXmlWithNewlinesCore(obj, typeof(T), null);
        }


        /// <summary>
        /// Serializes an object to XML string, preserving CR+LF.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="knownTypes"></param>
        /// <returns>Serialized XML string.</returns>
        /// <remarks>
        /// Note that <typeparamref name="T"/> is inferred from the static type of <paramref name="obj"/>, which is
        /// not necessarily its runtime type. Use <see cref="XmlSerializeToString(object, Type[])"/> when the
        /// concrete type is the one that should be serialized.
        /// </remarks>
        public static string ToXmlWithNewlines<T>(this T obj, params Type[] knownTypes)
        {
            return ToXmlWithNewlinesCore(obj, typeof(T), knownTypes);
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
            var serializer = GetSerializer(type, knownTypes);

            if (replaceDictionary != null)
            {
                foreach (var kvp in replaceDictionary)
                {
                    sb.Replace(kvp.Key, kvp.Value);
                }
            }

            var text = sb.ToString();

            if (xmlElementEventHandler == null)
                ret = Deserialize(serializer, text);
            else
            {
                // The serializer instance is shared through the cache, so the handler must be attached and
                // detached around this single call, and no other thread may use the instance meanwhile.
                lock (serializer)
                {
                    serializer.UnknownElement += xmlElementEventHandler;

                    try
                    {
                        ret = Deserialize(serializer, text);
                    }
                    finally
                    {
                        serializer.UnknownElement -= xmlElementEventHandler;
                    }
                }
            }

            return ret;
        }


        /// <summary>
        /// Deserializes an XML string with the specified serializer.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        /// <param name="xml">The XML string.</param>
        /// <returns>The deserialized object.</returns>
        private static object Deserialize(XmlSerializer serializer, string xml)
        {
            using var stringReader = new StringReader(xml);
            using var xmlReader = XmlReader.Create(stringReader);

            return serializer.Deserialize(xmlReader);
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

            GetSerializer(objectType, knownTypes).Serialize(xmlWriter, objectInstance);
        }


        /// <summary>
        /// Serializes to string.
        /// </summary>
        /// <param name="objectInstance">The object instance.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <returns>XML string.</returns>
        /// <exception cref="System.ArgumentNullException">objectInstance</exception>
        /// <remarks>
        /// The concrete runtime type of <paramref name="objectInstance"/> is serialized, so that this method and
        /// <see cref="XmlSerializeToFile(object, string, Type[])"/> use one and the same serializer.
        /// </remarks>
        public static string XmlSerializeToString(this object objectInstance, params Type[] knownTypes)
        {
            if (objectInstance == null) throw new ArgumentNullException(nameof(objectInstance));

            var ret = ToXmlWithNewlinesCore(objectInstance, objectInstance.GetType(), knownTypes);

            return ret;
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
            var serializer = GetSerializer(objectInstance.GetType(), knownTypes);

            using (var writer = new StringWriterUTF8(ret))
            {
                serializer.Serialize(writer, objectInstance, ns);
            }

            return ret.ToString();
        }

    }

}
