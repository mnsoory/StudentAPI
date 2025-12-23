using StudentAPI.Domain.Entities;

namespace StudentAPI.Domain.Interfaces
{
    /// <summary>
    /// Defines the abstraction for retrieving security permissions associated with users.
    /// This contract allows the application to verify access rights without depending on the underlying data store.
    /// </summary>
    public interface IUserPermissionRepository
    {
        /// <summary>
        /// Retrieves all permissions assigned to a specific user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A collection of <see cref="UserPermission"/> associated with the specified user.</returns>
        Task<IEnumerable<UserPermission>> GetByUserIdAsync(int userId);
    }
}