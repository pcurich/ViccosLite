using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace ViccosLite.Core
{
    public class XmlHelper
    {
        #region Methods

        /// <summary>
        ///     XML Codificar
        /// </summary>
        /// <param name="str">Cadena</param>
        /// <returns>Cadena Codificada</returns>
        public static string XmlEncode(string str)
        {
            if (str == null)
                return null;
            str = Regex.Replace(str, @"[^\u0009\u000A\u000D\u0020-\uD7FF\uE000-\uFFFD]", "", RegexOptions.Compiled);
            return XmlEncodeAsIs(str);
        }

        /// <summary>
        ///     XML Codifica como es
        /// </summary>
        /// <param name="str">Cadena</param>
        /// <returns>Cadena Codificada</returns>
        public static string XmlEncodeAsIs(string str)
        {
            if (str == null)
                return null;

            using (var sw = new StringWriter())
            using (var xwr = new XmlTextWriter(sw))
            {
                xwr.WriteString(str);
                return sw.ToString();
            }
        }

        /// <summary>
        ///     Codifica un atributo
        /// </summary>
        /// <param name="str">Atributo</param>
        /// <returns>Atributo Codificado</returns>
        public static string XmlEncodeAttribute(string str)
        {
            if (str == null)
                return null;
            str = Regex.Replace(str, @"[^\u0009\u000A\u000D\u0020-\uD7FF\uE000-\uFFFD]", "", RegexOptions.Compiled);
            return XmlEncodeAttributeAsIs(str);
        }

        /// <summary>
        ///     Codifica un atributo como es
        /// </summary>
        /// <param name="str">Atributo</param>
        /// <returns>Atributo Codificado</returns>
        public static string XmlEncodeAttributeAsIs(string str)
        {
            return XmlEncodeAsIs(str).Replace("\"", "&quot;");
        }

        /// <summary>
        ///     Decodifica un atributo
        /// </summary>
        /// <param name="str">Atributo</param>
        /// <returns>Atributo Decodificado</returns>
        public static string XmlDecode(string str)
        {
            var sb = new StringBuilder(str);
            return sb.Replace("&quot;", "\"")
                .Replace("&apos;", "'")
                .Replace("&lt;", "<")
                .Replace("&gt;", ">")
                .Replace("&amp;", "&")
                .ToString();
        }

        /// <summary>
        ///     Serializa una fecha
        /// </summary>
        /// <param name="dateTime">Fecha</param>
        /// <returns>Fecha Serializada</returns>
        public static string SerializeDateTime(DateTime dateTime)
        {
            var xmlS = new XmlSerializer(typeof (DateTime));
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                xmlS.Serialize(sw, dateTime);
                return sb.ToString();
            }
        }

        /// <summary>
        ///     Deserializa una fecha
        /// </summary>
        /// <param name="dateTime">Fecha</param>
        /// <returns>Fecha Deserializada</returns>
        public static DateTime DeserializeDateTime(string dateTime)
        {
            var xmlS = new XmlSerializer(typeof (DateTime));
            using (var sr = new StringReader(dateTime))
            {
                var test = xmlS.Deserialize(sr);
                return (DateTime) test;
            }
        }

        public static void Serialize<T>(T o, string file, bool nameSpace = false) where T : class
        {
            var xmlSerializer = new XmlSerializer(typeof (T));
            Stream stream = new FileStream(file, FileMode.Append);
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            if (nameSpace)
                xmlSerializer.Serialize(stream, o, ns);

            xmlSerializer.Serialize(stream, o);
            stream.Close();
            stream.Dispose();
        }

        public static Stream SerializeWeb<T>(T o, bool nameSpace = false) where T : class
        {
            var xmlSerializer = new XmlSerializer(typeof (T));
            var stream = new MemoryStream();

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            if (nameSpace)
                xmlSerializer.Serialize(stream, o, ns);

            xmlSerializer.Serialize(stream, o);
            //stream.Close();
            return stream;
        }

        public static T Deserialize<T>(string file) where T : class
        {
            var xmlSerializer = new XmlSerializer(typeof (T));
            Stream stream = new FileStream(file, FileMode.Open);
            var result = xmlSerializer.Deserialize(stream) as T;
            stream.Close();
            stream.Dispose();
            return result;
        }

        #endregion
    }
}