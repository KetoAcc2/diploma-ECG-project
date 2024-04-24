public interface IUserService : IBaseService
{
    Task<List<UserDTO>> GetUsersAsync();
    Task<UserDTO> GetUserAsync(int userId);

    Task<List<UserDTO>> GetUsersFromGroupAsync(int groupId);
    Task<bool> ExistsUserAsync(int userId);
    Task<bool> ExistsGroupAsync(int groupId);
}