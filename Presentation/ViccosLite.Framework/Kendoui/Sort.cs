namespace ViccosLite.Framework.Kendoui
{
    /// <summary>
    ///     Representa una expresion de ordenamiento del Kendo DataSource.
    /// </summary>
    public class Sort
    {
        /// <summary>
        ///     Nombre del campo ordenado (propiedad).
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        ///     Direccion de ordenamiento. Debe ser "asc" o "desc".
        /// </summary>
        public string Dir { get; set; }

        /// <summary>
        ///     Convierte a un formulario requerido  por el Dynamic Linq
        ///     e.g. "Field1 desc"
        /// </summary>
        public string ToExpression()
        {
            return Field + " " + Dir;
        }
    }
}