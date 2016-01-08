using System;
using System.Web;

namespace ViccosLite.Core.Fakes
{
    public static class FakeHttpExtensions
    {
        /// <summary>
        /// Indica si este contexto es una imitacion
        /// </summary>
        /// <param name="httpContext">Http Context</param>
        /// <returns></returns>
        public static bool IsFakeContext(this HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            return httpContext is FakeHttpContext;
        }
    }
}