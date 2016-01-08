using System;
using System.Collections.Generic;
using System.Linq;

namespace ViccosLite.Framework.Kendoui
{
    /// <summary>
    ///     Representa un filtro de kendo DataSource
    /// </summary>
    public class Filter
    {
        /// <summary>
        ///     Mapeo de los filtros de operaciones para Kendo DataSource para Dynamic Linq
        /// </summary>
        private static readonly IDictionary<string, string> Operators = new Dictionary<string, string>
        {
            {"eq", "="},
            {"neq", "!="},
            {"lt", "<"},
            {"lte", "<="},
            {"gt", ">"},
            {"gte", ">="},
            {"startswith", "StartsWith"},
            {"endswith", "EndsWith"},
            {"contains", "Contains"}
        };

        /// <summary>
        ///     Establece el nombre de un campo ordenado. Se establece <c>null</c> si los filtros son seteados
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        ///     Establece la operacion de un filtro. Se establece <c>null</c> si los filtros son seteados
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        ///     Establece el valor de lo filtrado. Se establece <c>null</c> si los filtros son seteados
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        ///     Establece la logica del filtro. Puede ser <c>or</c> or <c>and</c>. Se establece <c>null</c> si los filtros son
        ///     seteados
        /// </summary>
        public string Logic { get; set; }

        /// <summary>
        ///     Representa las expresiones hijas de los flltros. Se establece <c>null</c> Si no tiene exxpresiones hijas
        /// </summary>
        public IEnumerable<Filter> Filters { get; set; }

        /// <summary>
        ///     Lista de todas las exrpresiones de los filtros de los hijos
        /// </summary>
        /// <returns></returns>
        public IList<Filter> All()
        {
            var filters = new List<Filter>();
            Collect(filters);
            return filters;
        }

        private void Collect(ICollection<Filter> filters)
        {
            if (Filters != null && Filters.Any())
            {
                foreach (var filter in Filters)
                {
                    filters.Add(filter);
                    filter.Collect(filters);
                }
            }
            else
            {
                filters.Add(this);
            }
        }

        public string ToExpression(IList<Filter> filters)
        {
            if (Filters != null && Filters.Any())
            {
                return "(" +
                       String.Join(" " + Logic + " ", Filters.Select(filter => filter.ToExpression(filters)).ToArray()) +
                       ")";
            }

            var index = filters.IndexOf(this);
            var comparison = Operators[Operator];

            if (comparison == "Contains")
            {
                return String.Format("{0}.IndexOf(@{1}, System.StringComparison.InvariantCultureIgnoreCase) >= 0", Field,
                    index);
            }
            if (comparison == "=")
            {
                comparison = "Equals";
            }
            if (comparison == "StartsWith" || comparison == "EndsWith" || comparison == "Equals")
            {
                return String.Format("{0}.{1}(@{2}, System.StringComparison.InvariantCultureIgnoreCase)", Field,
                    comparison, index);
            }

            return String.Format("{0} {1} @{2}", Field, comparison, index);
        }
    }
}