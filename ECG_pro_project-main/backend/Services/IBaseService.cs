public interface IBaseService
{
    Task<bool> HasRemoveTaskPrivilege(int userId, int groupId);
    Task<bool> TaskIsAssignedToGroup(int taskId, int groupId);
    Task<User?> GetUserByAccessToken(string token);
    Task<User?> GetUserById(int userId);
    Task<User?> GetUserByEmail(string email);
    Task<bool> ExistsGroup(int groupId);
    Task<bool> IsGroupOwner(int userId, int groupId);
    Task<UserDTO?> GetUserByEmailReturnDTO(string email);
    void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    Task<string> GetITDoc();
    Task<string> GetTeacherDoc();
    Task<string> GetStudentDoc();
}