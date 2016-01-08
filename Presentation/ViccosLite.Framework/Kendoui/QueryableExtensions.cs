using System.Collections.Generic;
using System.Linq;

namespace ViccosLite.Framework.Kendoui
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, Filter filter)
        {
            if (filter == null || filter.Logic == null)
                return queryable;

            //Coleccion de una  lista plana de todos los filtros
            var filters = filter.All();

            //Obtiene todos los valores de los filtros como un arreglo
            //(Necesitado por el metodo Where del Dynamic Linq)
            var values = filters.Select(f => f.Value).ToArray();

            // Crea una expresion de predicado 
            // Field1 = @0 And Field2 > @1
            var predicate = filter.ToExpression(filters);

            // Usa el metodo Where del Dynamic Linq a un filtro de data
            queryable = queryable.Where(predicate, values);

            return queryable;
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> queryable, IEnumerable<Sort> sort)
        {
            if (sort != null && sort.Any())
            {
                //Crea una expresion ordenada por ejemplo campo1 asc, campo2 desc
                var ordering = string.Join(",", sort.Select(s => s.ToExpression()));

                //Usar el metodo OrderBy del linq Dinamico para ordenar la data 
                return queryable.OrderBy(ordering);
            }

            return queryable;
        }
    }
}