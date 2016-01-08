using System.Web.Mvc;

namespace ViccosLite.Framework.Mvc
{
    public class SoftModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = base.BindModel(controllerContext, bindingContext);
            var softModel = model as BaseSoftModel;
            if (softModel != null)
            {
                softModel.BindModel(controllerContext, bindingContext);
            }
            return model;
        }
    }
}