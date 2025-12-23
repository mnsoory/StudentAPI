using StudentAPI.Domain.Entities;

namespace StudentAPI.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for user-related data operations.
    /// Provides methods to retrieve user account information from the persistence store.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves a user by their unique primary identifier.
        /// </summary>
        /// <param name="id">The unique integer identifier of the user.</param>
        /// <returns>A <see cref="User"/> entity if found; otherwise, null.</returns>
        Task<User?> GetByIdAsync(int id);

        /// <summary>
        /// Retrieves a user by their unique username. 
        /// Typically used during the authentication process.
        /// </summary>
        /// <param name="username">The unique username string.</param>
        /// <returns>A <see cref="User"/> entity if found; otherwise, null.</returns>
        Task<User?> GetByUsernameAsync(string username);
    }
}