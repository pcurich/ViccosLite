using FluentValidation;
using ViccosLite.Framework.Validators;
using ViccosLite.Web.Models.Install;

namespace ViccosLite.Web.Validators.Install
{
    public class InstallValidator : BaseSoftValidator<InstallModel>
    {
        public InstallValidator()
        {
            RuleFor(x => x.AdminEmail)
                .NotEmpty()
                .WithMessage("Introduce el email de administrador");
            
            RuleFor(x => x.AdminEmail).EmailAddress();

            RuleFor(x => x.AdminPassword)
                .NotEmpty()
                .WithMessage("Introduzca la contraseña de administrador");
            
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithMessage("Introduzca confirmar la contraseña");

            RuleFor(x => x.AdminPassword)
                .Equal(x => x.ConfirmPassword).
                WithMessage("Las contraseñas no coinciden");

            RuleFor(x => x.DataProvider)
                .NotEmpty()
                .WithMessage("Seleccione el proveedor de datos");
        }
    }
}