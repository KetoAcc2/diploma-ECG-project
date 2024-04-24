public interface IUserGroupService
{
    Task<UsersAndGroupsDTO> GetGroupInfoFromUser(int userId);
    Task<User?> GetUserByAccessToken(string token);
}