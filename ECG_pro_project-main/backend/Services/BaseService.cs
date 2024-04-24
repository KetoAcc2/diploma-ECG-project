using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

public class BaseService : IBaseService
{
    private readonly ApplicationDbContext _db;
    public BaseService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<bool> HasRemoveTaskPrivilege(int userId, int groupId)
    {
        return await _db.Groups.Where(x => x.GroupOwner == userId && x.GroupId == groupId).AnyAsync();
    }
    public async Task<User?> GetUserByAccessToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var tokens = jwtToken as JwtSecurityToken;
        var user = tokens.Claims.Select(c => c.Value).ToList();
        return await _db.Users.Where(x => x.Email == user[0]).SingleOrDefaultAsync();
    }

    public async Task<bool> TaskIsAssignedToGroup(int taskId, int groupId)
    {
        return await _db.Tasks_Groups
        .Where(x => x.TaskAssignedId == taskId)
        .Join(_db.Users_Groups, tsk_grp => tsk_grp.AssignedUserGroupId, usr_grp => usr_grp.User_GroupId, (tsk_grp, usr_grp) => new
        {
            GroupId = usr_grp.GroupId
        })
        .Where(x => x.GroupId == groupId)
        .AnyAsync();
    }

    public async Task<bool> ExistsGroup(int groupId)
    {
        return await _db.Groups.Where(x => x.GroupId == groupId).AnyAsync();
    }

    public async Task<bool> IsGroupOwner(int userId, int groupId)
    {
        return await _db.Groups.Where(x => x.GroupOwner == userId && x.GroupId == groupId).AnyAsync();
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _db.Users.Where(x => x.Email == email).SingleOrDefaultAsync();
    }
    public async Task<UserDTO?> GetUserByEmailReturnDTO(string email)
    {
        UserDTO? user = await _db.Users.Where(x => x.Email == email && x.Role != null).Select(x => new UserDTO
        {
            UserId = x.Id,
            Email = x.Email,
            Role = x.Role!
        }).SingleOrDefaultAsync();
        return user;
    }
    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    public async Task<User?> GetUserById(int userId)
    {
        return await _db.Users.Where(x => x.Id == userId).SingleOrDefaultAsync();
    }

    public async Task<string> GetITDoc()
    {
        return await _db.Docs.Where(x => x.DocId == 1).Select(x => x.DocPath).SingleAsync();
    }

    public async Task<string> GetTeacherDoc()
    {
        return await _db.Docs.Where(x => x.DocId == 2).Select(x => x.DocPath).SingleAsync();
    }

    public async Task<string> GetStudentDoc()
    {
        return await _db.Docs.Where(x => x.DocId == 3).Select(x => x.DocPath).SingleAsync();
    }
}