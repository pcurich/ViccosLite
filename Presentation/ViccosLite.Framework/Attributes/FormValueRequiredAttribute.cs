using System;
using System.Reflection;
using System.Web.Mvc;

namespace ViccosLite.Framework.Attributes
{
    public class FormValueRequiredAttribute : ActionMethodSelectorAttribute
    {
        private readonly FormValueRequirement _requirement;
        private readonly string[] _submitButtonNames;

        public FormValueRequiredAttribute(params string[] submitButtonNames) :
            this(FormValueRequirement.Equal, submitButtonNames)
        {
        }

        public FormValueRequiredAttribute(FormValueRequirement requirement, params string[] submitButtonNames)
        {
            //al menos un submit button bederia ser encontrado
            _submitButtonNames = submitButtonNames;
            _requirement = requirement;
        }

        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            foreach (var buttonName in _submitButtonNames)
            {
                var value = "";
                switch (_requirement)
                {
                    case FormValueRequirement.Equal:
                        {
                            value = controllerContext.HttpContext.Request.Form[buttonName];
                        }
                        break;
                    case FormValueRequirement.StartsWith:
                        {
                            foreach (var formValue in controllerContext.HttpContext.Request.Form.AllKeys)
                            {
                                if (formValue.StartsWith(buttonName, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    value = controllerContext.HttpContext.Request.Form[formValue];
                                    break;
                                }
                            }
                        }
                        break;
                }
                if (!String.IsNullOrEmpty(value))
                    return true;
            }
            return false;
        }
    }

    public enum FormValueRequirement
    {
        Equal,
        StartsWith
    }
}