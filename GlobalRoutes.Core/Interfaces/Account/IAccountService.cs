using Ardalis.Result;
using GlobalRoutes.Core.Entities.Users;

namespace GlobalRoutes.Core.Interfaces.Account
{
    public interface IAccountService
    {
        /// <summary>
        ///     Create a new application user.
        /// </summary>
        /// <param name="applicationUser">The application user to create.</param>
        /// <param name="password">The password for the user.</param>
        /// <returns>Returns the user of the application created</returns>
        Task<Result<User>> CreateUser(User applicationUser, string password);

    }
}
