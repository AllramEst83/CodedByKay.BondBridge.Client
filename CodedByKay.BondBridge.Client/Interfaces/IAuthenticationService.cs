using CodedByKay.BondBridge.Client.Models;

namespace CodedByKay.BondBridge.Client.Interfaces
{
    /// <summary>
    /// Defines authentication service operations.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Signs in a user asynchronously using their email and password.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="AuthModel"/> instance that represents the signed-in user.</returns>
        Task<AuthModel> SignInAsync(string email, string password);

        /// <summary>
        /// Adds a new user asynchronously using their email and password.
        /// </summary>
        /// <param name="email">The email for the new user.</param>
        /// <param name="password">The password for the new user.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddUser(string email, string password);
    }

}
