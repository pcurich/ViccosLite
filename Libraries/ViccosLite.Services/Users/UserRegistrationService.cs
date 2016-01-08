using System;
using ViccosLite.Core;
using ViccosLite.Core.Domain.Users;
using ViccosLite.Services.Stores;

namespace ViccosLite.Services.Users
{
    public class UserRegistrationService : IUserRegistrationService
    {
        #region Ctr

        public UserRegistrationService(IUserService userService, IStoreService storeService)
        {
            _userService = userService;
            _storeService = storeService;
        }

        #endregion

        #region Campos

        private readonly IUserService _userService;
        private readonly IStoreService _storeService;

        #endregion

        #region Metodos

        public UserLoginResults ValidateUser(string usernameOrEmail, string password)
        {
            var user = _userService.GetUserByUserName(usernameOrEmail) ??
                _userService.GetUserByEmail(usernameOrEmail);

            if (user == null)
                return UserLoginResults.UserNotExist;
            if (user.Deleted)
                return UserLoginResults.Deleted;
            if (!user.Active)
                return UserLoginResults.NotActive;
            //only registered can login
            if (!user.IsRegistered())
                return UserLoginResults.NotRegistered;

            var isValid = password == user.Password;
            if (!isValid)
                return UserLoginResults.WrongPassword;

            //save last login date
            user.LastLoginDateUtc = DateTime.UtcNow;
            _userService.UpdateUser(user);
            return UserLoginResults.Successful;
        }

        public UserRegistrationResult RegisterUser(UserRegistrationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.User == null)
                throw new ArgumentException("No se ha cargado el usuario");

            var result = new UserRegistrationResult();
            if (request.User.IsSearchEngineAccount())
            {
                result.AddError("No se puede registrar un motor de busqueda");
                return result;
            }
            if (request.User.IsBackgroundTaskAccount())
            {
                result.AddError("Una tarea de Background no puede ser registrada");
                return result;
            }
            if (request.User.IsRegistered())
            {
                result.AddError("El usuario actual ya esta registrado");
                return result;
            }
            if (String.IsNullOrEmpty(request.Email))
            {
                result.AddError("El email no es valido");
                return result;
            }
            if (!CommonHelper.IsValidEmail(request.Email))
            {
                result.AddError("Email incorrecto");
                return result;
            }
            if (String.IsNullOrWhiteSpace(request.Password))
            {
                result.AddError("Ingrese la contraseña");
                return result;
            }

            if (String.IsNullOrEmpty(request.Username))
            {
                result.AddError("Se requiere nombre de usuario");
                return result;
            }
            //validate unique user
            if (_userService.GetUserByEmail(request.Email) != null)
            {
                result.AddError("Ya existe el email");
                return result;
            }
            if (_userService.GetUserByUserName(request.Username) != null)
            {
                result.AddError("Ya existe el nombre de usuario especificado");
                return result;
            }

            //at this point request is valid
            request.User.UserName = request.Username;
            request.User.Email = request.Email;
            request.User.Password = request.Password;
            request.User.Active = request.IsApproved;

            //add to 'Registered' role
            var registeredRole = _userService.GetUserRoleBySystemName(SystemUserRoleNames.Registered);
            if (registeredRole == null)
                throw new KsException("El rol 'Registered' no pudo establecerse");
            request.User.UserRoles.Add(registeredRole);

            return result;
        }

        public PasswordChangeResult ChangePassword(ChangePasswordRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var result = new PasswordChangeResult();
            if (String.IsNullOrWhiteSpace(request.Email))
            {
                result.AddError("No se ha ingreado el email");
                return result;
            }
            if (String.IsNullOrWhiteSpace(request.NewPassword))
            {
                result.AddError("Constraseña no ingresada");
                return result;
            }
            var user = _userService.GetUserByEmail(request.Email);
            if (user == null)
            {
                result.AddError("El email ingresado no se pudo encontrar");
                return result;
            }

            var requestIsValid = false;
            if (request.ValidateRequest)
            {

                var oldPasswordIsValid = request.OldPassword == user.Password;
                if (!oldPasswordIsValid)
                    result.AddError("La contraseña anterior no coincide");

                if (oldPasswordIsValid)
                    requestIsValid = true;
            }
            else
                requestIsValid = true;

            if (requestIsValid)
            {
                //at this point request is valid
                user.Password = request.NewPassword;
                _userService.UpdateUser(user);
            }
            
            return result;
        }

        public void SetEmail(User user, string newEmail)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            newEmail = newEmail.Trim();

            if (!CommonHelper.IsValidEmail(newEmail))

                if (newEmail.Length > 100)
                    throw new KsException("El email es demasiado largo");

            var user2 = _userService.GetUserByEmail(newEmail);
            if (user2 != null && user.Id != user2.Id)
                throw new KsException("El email ya está en uso");

            user.Email = newEmail;
            _userService.UpdateUser(user);

        }

        public void SetUsername(User user, string newUsername)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            newUsername = newUsername.Trim();

            if (newUsername.Length > 100)
                throw new KsException("Nombre de usuario es demasiado largo");

            var user2 = _userService.GetUserByUserName(newUsername);
            if (user2 != null && user.Id != user2.Id)
                throw new KsException("El nombre de usuario ya esta en uso");

            user.UserName = newUsername;
            _userService.UpdateUser(user);
        }

        #endregion
    }
}