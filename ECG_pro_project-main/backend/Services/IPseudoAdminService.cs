public interface IPseudoAdminService : IBaseService
{
    Task<bool> UpdateRole(string email, string role);
    Task<string> GetUserRole(string email);
}