using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace ViccosLite.Data.Extensions
{
    public static class DataReaderExtensions
    {
        /// <summary>
        /// Crea una lista que me devuelve el tipo de todas las filas de un DataReader
        /// 
        /// Nota Este metodo usa Reflection entonces no tiene buen performance
        /// Pero esto puede ser util para genericos dataReaders hacia la conversion de 
        /// un entity a un dato anonimo
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="reader">Un DataReader abierto en la posicion de leer</param>
        /// <param name="fieldsToSkip">
        /// Opcionalmente - comas que delimitan la lista de campos tu no deberias de actualizar esto 
        /// </param>
        /// <param name="piList">
        /// Opcionalmente - PropertyInfo en un cache de diccionario esto contiene la informacion de las propiedades del objeto
        /// Puede ser utilizado para el almacenamiento en caché de HTE estructura PropertyInfo para múltiples operaciones para acelerar la traduccion
        /// Si no pasa automaticamente 
        /// </param>
        /// <returns></returns>
        public static List<TType> DataReaderToObjectList<TType>(this IDataReader reader, string fieldsToSkip = null, Dictionary<string, PropertyInfo> piList = null)
            where TType : new()
        {
            if (reader == null)
                return null;

            var items = new List<TType>();

            // Crear lista de búsqueda de información de la propiedad de los objetos          
            if (piList == null)
            {
                piList = new Dictionary<string, PropertyInfo>();
                var props = typeof(TType).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (var prop in props)
                    piList.Add(prop.Name.ToLower(), prop);
            }

            while (reader.Read())
            {
                var inst = new TType();
                DataReaderToObject(reader, inst, fieldsToSkip, piList);
                items.Add(inst);
            }

            return items;
        }

        /// <summary>
        /// Rellena las propiedades de un objeto a partir de una sola fila DataReader usando 
        /// reflexión  para coincidir los campos DataReader a una propiedad pública sobre el objeto pasado 
        /// Si no coindicen las propiedades propiedades no se modifican 
        /// 
        /// Se necesita pasar un localizador en el dataReader de una fila activa que se quiere serializar
        /// </summary>
        /// <param name="reader">Instance of the DataReader to read data from. Should be located on the correct record (Read() should have been called on it before calling this method)</param>
        /// <param name="instance">Instance of the object to populate properties on</param>
        /// <param name="fieldsToSkip">Optional - A comma delimited list of object properties that should not be updated</param>
        /// <param name="piList">Optional - Cached PropertyInfo dictionary that holds property info data for this object</param>
        public static void DataReaderToObject(this IDataReader reader, object instance, string fieldsToSkip = null, Dictionary<string, PropertyInfo> piList = null)
        {
            if (reader.IsClosed)
                throw new InvalidOperationException("Data reader no se puede usar porque esta cerrado");

            if (string.IsNullOrEmpty(fieldsToSkip))
                fieldsToSkip = string.Empty;
            else
                fieldsToSkip = "," + fieldsToSkip + ",";

            fieldsToSkip = fieldsToSkip.ToLower();

            //Crea un diccionario de propiedades para buscar
            //podemos pasar esto en lo que podemos almacenar en una ista de caché de una vez 
            //para una lista de operaciones 
            if (piList == null)
            {
                piList = new Dictionary<string, PropertyInfo>();
                var props = instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (var prop in props)
                    piList.Add(prop.Name.ToLower(), prop);
            }

            for (var index = 0; index < reader.FieldCount; index++)
            {
                var name = reader.GetName(index).ToLower();
                if (piList.ContainsKey(name))
                {
                    var prop = piList[name];

                    if (fieldsToSkip.Contains("," + name + ","))
                        continue;

                    if ((prop != null) && prop.CanWrite)
                    {
                        var val = reader.GetValue(index);
                        prop.SetValue(instance, (val == DBNull.Value) ? null : val, null);
                    }
                }
            }
        }
         
    }
}