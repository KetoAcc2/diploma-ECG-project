public interface IGroupService:IBaseService
{
    Task<List<Group>> GetJoinedGroupsAsync(int userId);
    Task<List<Group>> GetCreatedGroupsAsync(int userId);
    Task<List<Group>> GetGroupsAsync();
    Task<Group?> GetGroupAsync(int groupId);
    Task<Group?> CreateGroup(string groupName, User grouOwner);
    Task<bool> JoinGroup(string groupCode, int userId);
    Task<Group?>GetGroupByGroupCode(string groupCode);
    Task<bool> ExistsUserInGroup(int groupId, int userId);
    Task<bool> DeleteGroup(int groupId);
    Task<bool> DeleteUserFromGroup(int groupId, int userId);
    Task<bool> RegenGroupCode(int groupId);
}