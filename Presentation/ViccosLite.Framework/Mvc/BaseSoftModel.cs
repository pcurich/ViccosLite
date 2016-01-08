using System.Collections.Generic;
using System.Web.Mvc;

namespace ViccosLite.Framework.Mvc
{
    [ModelBinder(typeof(SoftModelBinder))]
    public class BaseSoftModel
    {
        public BaseSoftModel()
        {
            CustomProperties = new Dictionary<string, object>();
            PostInitialize();
        }

        /// <summary>
        ///     Usa esta propiedad para almacenar cualquier valor del modelo
        /// </summary>
        public Dictionary<string, object> CustomProperties { get; set; }

        /// <summary>
        ///     Se puede sobreescribir este metodo en clases parciales
        ///     en orden para agregar algunos inicializadores personalizados en el constructor
        /// </summary>
        protected void PostInitialize()
        {
        }

        public virtual void BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
        }
    }

    /// <summary>
    ///     Base entity del modelo
    /// </summary>
    public class BaseSoftEntityModel : BaseSoftModel
    {
        public virtual int Id { get; set; }
    }
}