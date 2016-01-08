using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ViccosLite.Framework.Kendoui
{
    public static class ModelStateExtensions
    {
        private static string GetErrorMessage(ModelError error, ModelState modelState)
        {
            if (!string.IsNullOrEmpty(error.ErrorMessage))
                return error.ErrorMessage;

            if (modelState.Value == null)
                return error.ErrorMessage;

            var args = new object[] {modelState.Value.AttemptedValue};
            return string.Format("ValueNotValidForProperty=El valor '{0}' es invalido ", args);
        }

        public static object SerializeErrors(this ModelStateDictionary modelState)
        {
            return modelState.Where(entry => entry.Value.Errors.Any())
                .ToDictionary(entry => entry.Key, entry => SerializeModelState(entry.Value));
        }

        private static Dictionary<string, object> SerializeModelState(ModelState modelState)
        {
            var dictionary = new Dictionary<string, object>();
            dictionary["errors"] = modelState.Errors.Select(x => GetErrorMessage(x, modelState)).ToArray();
            return dictionary;
        }

        public static object ToDataSourceResult(this ModelStateDictionary modelState)
        {
            return !modelState.IsValid ? modelState.SerializeErrors() : null;
        }
    }
}