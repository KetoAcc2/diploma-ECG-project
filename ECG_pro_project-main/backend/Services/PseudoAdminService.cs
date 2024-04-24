
using Microsoft.EntityFrameworkCore;

public class PseudoAdminService : BaseService, IPseudoAdminService
{
    private readonly ApplicationDbContext _db;
    public PseudoAdminService(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task<bool> UpdateRole(string email, string role)
    {
        try
        {
            var user = await _db.Users.Where(x => x.Email == email).SingleAsync();
            user.Role = role;
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<string> GetUserRole(string email)
    {
        var user = await _db.Users.Where(x => x.Email == email).SingleAsync();
        return user.Role;
    }
}