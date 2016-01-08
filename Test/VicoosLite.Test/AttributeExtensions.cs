using System;
using System.Reflection;

namespace Base.Test
{
    public static class AttributeExtensions
    {
        /// <summary>
        /// Retorna true si la etiqueta del atributo esta decorada con un atributo del tipo 
        /// TAttribute
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="attributeTarget"></param>
        /// <returns></returns>
        public static bool IsDecoratedWith<TAttribute>(this ICustomAttributeProvider attributeTarget) where TAttribute : Attribute
        {
            return attributeTarget.GetCustomAttributes(typeof(TAttribute), false).Length > 0;
        }

        /// <summary>
        /// Retornara true si el primer atributo es del tipo TAttribute
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="attributeTarget"></param>
        /// <returns></returns>
        public static TAttribute GetAttribute<TAttribute>(this ICustomAttributeProvider attributeTarget) where TAttribute : Attribute
        {
            return (TAttribute)attributeTarget.GetCustomAttributes(typeof(TAttribute), false)[0];
        }
    }
}