using FluentValidation;

namespace ViccosLite.Framework.Validators
{
    public abstract class BaseSoftValidator<T> : AbstractValidator<T> where T : class
    {
        protected BaseSoftValidator()
        {
            PostInitialize();
        }

        /// <summary>
        /// Developers can override this method in custom partial classes
        /// in order to add some custom initialization code to constructors
        /// </summary>
        protected virtual void PostInitialize()
        {

        }
    }
}