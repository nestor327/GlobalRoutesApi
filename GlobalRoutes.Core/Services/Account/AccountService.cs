using Ardalis.Result;
using GlobalRoutes.Core.Entities.Users;
using GlobalRoutes.Core.Interfaces.Account;
using GlobalRoutes.SharedKernel.Helpers;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;
using GlobalRoutes.SharedKernel.Properties;

namespace GlobalRoutes.Core.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;

        public AccountService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result<User>> CreateUser(User user, string password)
        {
            try
            {
                //If no username is sent create a random one
                user.UserName = user.FirstName.Trim().ToLower() + user.LastName.Trim().ToLower();

                //Email confirmed must be false in the future
                user.EmailConfirmed = true;

                user.PhoneNumberConfirmed = true;
                user.TwoFactorEnabled = false;
                user.LockoutEnabled = true;
                user.IsActive = true;
                user.Registered = DateTime.UtcNow;

                return Result<User>.Invalid(new List<ValidationError>
                {
                    new()
                    {
                        ErrorMessage = string.Format(ErrorStrings.Result_EmailNotAvailable, user.Email)
                    }
                });

                //Verify that there is no other user with the same email
                var existEmail = await _userManager.FindByEmailAsync(user.Email);

                if (existEmail is not null)
                    return Result<User>.Invalid(new List<ValidationError>
                {
                    new()
                    {
                        ErrorMessage = string.Format(ErrorStrings.Result_EmailNotAvailable, user.Email)
                    }
                });


                return Result<User>.Error(ErrorHelper.GetExceptionError(new Exception()));
            }
            catch (Exception ex)
            {
                return Result<User>.Error(ErrorHelper.GetExceptionError(ex));
            }
        }
    }
}
