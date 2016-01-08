using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using ViccosLite.Core.ComponentModel;

namespace ViccosLite.Core
{
    /// <summary>
    ///     Representa ayudas comunes
    /// </summary>
    public class CommonHelper
    {
        private static AspNetHostingPermissionLevel? _trustLevel;

        /// <summary>
        ///     Asegura que el email este correcto o lanza una excepcion
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns></returns>
        public static string EnsureSubscriberEmailOrThrow(string email)
        {
            var output = EnsureNotNull(email);
            output = output.Trim();
            output = EnsureMaximumLength(output, 255);

            if (!IsValidEmail(output))
            {
                throw new KsException("Email no valido.");
            }

            return output;
        }

        /// <summary>
        ///     Encuentra el nivel de confianza de una aplicacion corriendo
        ///     (http://blogs.msdn.com/dmitryr/archive/2007/01/23/finding-out-the-current-trust-level-in-asp-net.aspx)
        /// </summary>
        /// <returns>Nivel de confianza</returns>
        public static AspNetHostingPermissionLevel GetTrustLevel()
        {
            if (_trustLevel.HasValue)
                return _trustLevel.Value;

            //set minimum
            _trustLevel = AspNetHostingPermissionLevel.None;

            //determine maximum
            foreach (var trustLevel in new[]
            {
                AspNetHostingPermissionLevel.Unrestricted,
                AspNetHostingPermissionLevel.High,
                AspNetHostingPermissionLevel.Medium,
                AspNetHostingPermissionLevel.Low,
                AspNetHostingPermissionLevel.Minimal
            })
            {
                try
                {
                    new AspNetHostingPermission(trustLevel).Demand();
                    _trustLevel = trustLevel;
                    break; //we've set the highest permission we can
                }
                catch (SecurityException)
                {
                }
            }
            return _trustLevel.Value;
        }

        /// <summary>
        ///     Setea una propiedad de un objeto a un valor
        /// </summary>
        /// <param name="instance">El objeto cuya propiedad sera establecida</param>
        /// <param name="propertyName">Nombre de la propiedad a establecer</param>
        /// <param name="value">El valor a establecer en la propiedad</param>
        public static void SetProperty(object instance, string propertyName, object value)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            if (propertyName == null) throw new ArgumentNullException("propertyName");

            var instanceType = instance.GetType();
            var pi = instanceType.GetProperty(propertyName);

            if (pi == null)
                throw new KsException("No propiedad '{0}' encontrada en la instancia de tipo '{1}'.", propertyName,
                    instanceType);

            if (!pi.CanWrite)
                throw new KsException("La propiedad '{0}' en la instancia de tipo '{1}' no tiene setters.",
                    propertyName, instanceType);

            if (value != null && !value.GetType().IsAssignableFrom(pi.PropertyType))
                value = To(value, pi.PropertyType);
            pi.SetValue(instance, value, new object[0]);
        }

        /// <summary>
        /// Dado un tipo devuelve un Convertidor generico para los tipos de datos dados
        /// </summary>
        /// <param name="type">Lista de tipo de dato (List[int], List[decimal], List[double], List[string]).</param>
        /// <returns></returns>
        public static TypeConverter GetSoftCustomTypeConverter(Type type)
        {
            //No se puede usar el codigo  para registrar nuestro descriptor personalizado
            //TypeDescriptor.AddAttributes(typeof(List<int>), new TypeConverterAttribute(typeof(GenericListTypeConverter<int>)));
            //entonces se debe hacer manualmente

            if (type == typeof(List<int>))
                return new GenericListTypeConverter<int>();
            if (type == typeof(List<decimal>))
                return new GenericListTypeConverter<decimal>();
            if (type == typeof(List<double>))
                return new GenericListTypeConverter<double>();
            if (type == typeof(List<string>))
                return new GenericListTypeConverter<string>();
            return TypeDescriptor.GetConverter(type);
        }

        /// <summary>
        ///     Convierte un valor a un tipo destinatario
        /// </summary>
        /// <param name="value">Valor a convertir</param>
        /// <param name="destinationType">El tipo al que sera convertido el valor</param>
        /// <returns>Valor convertido</returns>
        public static object To(object value, Type destinationType)
        {
            return To(value, destinationType, CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Convierte un valor a un tipo destinatario
        /// </summary>
        /// <param name="value">Valor a convertir</param>
        /// <param name="destinationType">El tipo al que sera convertido el valor</param>
        /// <param name="culture">cultura</param>
        /// <returns>valor convertido</returns>
        public static object To(object value, Type destinationType, CultureInfo culture)
        {
            if (value == null)
                return null;

            var sourceType = value.GetType();

            var destinationConverter = GetSoftCustomTypeConverter(destinationType);
            var sourceConverter = GetSoftCustomTypeConverter(sourceType);

            if (destinationConverter != null && destinationConverter.CanConvertFrom(value.GetType()))
                return destinationConverter.ConvertFrom(null, culture, value);

            if (sourceConverter != null && sourceConverter.CanConvertTo(destinationType))
                return sourceConverter.ConvertTo(null, culture, value, destinationType);

            if (destinationType.IsEnum && value is int)
                return Enum.ToObject(destinationType, (int)value);

            if (!destinationType.IsInstanceOfType(value))
                return Convert.ChangeType(value, destinationType, culture);
            return value;
        }

        /// <summary>
        ///     Convierte un valor a un tipo destinatario
        /// </summary>
        /// <param name="value">Valor a convertir</param>
        /// <returns>Valor convertido</returns>
        public static T To<T>(object value)
        {
            //return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            return (T)To(value, typeof(T));
        }

        /// <summary>
        ///     Convierte Enums para el from-end
        /// </summary>
        /// <param name="str">Cadena a convertir</param>
        /// <returns>Valor convertido</returns>
        public static string ConvertEnum(string str)
        {
            var result = string.Empty;
            var letters = str.ToCharArray();
            foreach (var c in letters)
                if (c.ToString(CultureInfo.InvariantCulture) != c.ToString(CultureInfo.InvariantCulture).ToLower())
                    result += " " + c;
                else
                    result += c;
            return result;
        }

        /// <summary>
        ///     Establece a telerik a una cultura (Kendo ui)
        /// </summary>
        public static void SetTelerikCulture()
        {
            //pequeño hack here
            //siempre establece culture a 'en-US' (Kendo UI has a bug related to editing decimal values in other cultures). Like currently it's done for admin area in Global.asax.cs

            var culture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        #region Method

        /// <summary>
        ///     Verifica si una cadena es un email valido
        /// </summary>
        /// <param name="email">Email a verificar</param>
        /// <returns>Boolean</returns>
        public static bool IsValidEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
                return false;

            email = email.Trim();
            var result = Regex.IsMatch(email,
                "^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$",
                RegexOptions.IgnoreCase);
            return result;
        }

        /// <summary>
        ///     Genera numeros aleatorios
        /// </summary>
        /// <param name="length">Longitud</param>
        /// <returns>Cadena de resultado</returns>
        public static string GenerateRandomDigitCode(int length)
        {
            var random = new Random();
            var str = string.Empty;
            for (var i = 0; i < length; i++)
                str = String.Concat(str, random.Next(10).ToString(CultureInfo.InvariantCulture));
            return str;
        }

        /// <summary>
        ///     Retorna numeros enteros aleatorios con un especifico rango
        /// </summary>
        /// <param name="min">Numero minimo</param>
        /// <param name="max">Numero maximo</param>
        /// <returns>Resultado</returns>
        public static int GenerateRandomInteger(int min = 0, int max = int.MaxValue)
        {
            var randomNumberBuffer = new byte[10];
            new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
            return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
        }

        /// <summary>
        ///     Asegura que la cadena no exceda el maximo permitido
        /// </summary>
        /// <param name="str">cadena de entrada</param>
        /// <param name="maxLength">Longitud Maxima</param>
        /// <param name="postfix">Cadena Si la longitud es OK de lo contrario se trunca y se agrega al final</param>
        /// <returns></returns>
        public static string EnsureMaximumLength(string str, int maxLength, string postfix = null)
        {
            if (String.IsNullOrEmpty(str))
                return str;

            if (str.Length <= maxLength)
                return str;

            var result = str.Substring(0, maxLength);
            if (!String.IsNullOrEmpty(postfix))
            {
                result += postfix;
            }
            return result;
        }

        /// <summary>
        ///     Asegura que la cadena no sea nula
        /// </summary>
        /// <param name="str">Cadena de entrada</param>
        /// <returns>resultado</returns>
        public static string EnsureNotNull(string str)
        {
            return str ?? string.Empty;
        }

        /// <summary>
        ///     Asegura que la cadena solo contenga valores numericos
        /// </summary>
        /// <param name="str">Cadena de entrada</param>
        /// <returns>Cadena vacia si no contiene solo numeros, de lo contrario la cadena ingresada</returns>
        public static string EnsureNumericOnly(string str)
        {
            if (String.IsNullOrEmpty(str))
                return string.Empty;

            var result = new StringBuilder();
            foreach (var c in str)
            {
                if (Char.IsDigit(c))
                    result.Append(c);
            }
            return result.ToString();
        }

        /// <summary>
        ///     Indica si un elemento del arreglo es nulo o vacio
        /// </summary>
        /// <param name="stringsToValidate">Arreglo de cadena a validar</param>
        /// <returns>Boolean</returns>
        public static bool AreNullOrEmpty(params string[] stringsToValidate)
        {
            var result = false;
            Array.ForEach(stringsToValidate, str => { if (string.IsNullOrEmpty(str)) result = true; });
            return result;
        }

        /// <summary>
        ///     Compara 2 arreglos para saber si son iguales
        /// </summary>
        /// <typeparam name="T">Tipo</typeparam>
        /// <param name="a1">Arreglo 1</param>
        /// <param name="a2">Arreglo 2</param>
        /// <returns>Resultado</returns>
        public static bool ArraysEqual<T>(T[] a1, T[] a2)
        {
            //also see Enumerable.SequenceEqual(a1, a2);
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            var comparer = EqualityComparer<T>.Default;
            for (var i = 0; i < a1.Length; i++)
            {
                if (!comparer.Equals(a1[i], a2[i]))
                    return false;
            }
            return true;
        }

        #endregion
    }
}