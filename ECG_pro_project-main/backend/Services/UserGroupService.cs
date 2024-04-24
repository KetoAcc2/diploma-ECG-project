using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;

public class UserGroupService : IUserGroupService
{
    private readonly ApplicationDbContext _db;
    public UserGroupService(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task<User?> GetUserByAccessToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var tokens = jwtToken as JwtSecurityToken;
        var user = tokens.Claims.Select(c => c.Value).ToList();
        
        // var res = await _userManager.FindByEmailAsync(user[0]);
        return await _db.Users.Where(x => x.Email == user[0]).SingleOrDefaultAsync();
    }
    public async Task<UsersAndGroupsDTO> GetGroupInfoFromUser(int userId)
    {
        List<GroupDTO> groups = await GetGroupsFromUser(userId);
        Dictionary<String, List<UserDTO>> usersAndGroups = new Dictionary<String, List<UserDTO>>();
        foreach (GroupDTO group in groups)
        {
            List<UserDTO> users = await GetUsersFromGroup(group.GroupId);
            usersAndGroups.Add(group.GroupName, users);
            // TODO: check the model for group to see if Description is nullable, consider changing that or fix this snippet
        }
        return new UsersAndGroupsDTO
        {
            UsersAndGroups = usersAndGroups
        };
    }

    private async Task<List<GroupDTO>> GetGroupsFromUser(int userId)
    {
        return await _db.Groups.Join(_db.Users_Groups, grp => grp.GroupId, usr_grp => usr_grp.GroupId, (grp, usr_grp) => new
        {
            UserId = usr_grp.UserId,
            GroupId = grp.GroupId,
            Description = grp.GroupName
        }).Where(x => x.UserId == userId).Select(x => new GroupDTO
        {
            GroupId = x.GroupId,
            GroupName = x.Description
        }).ToListAsync();
    }
    private async Task<List<UserDTO>> GetUsersFromGroup(int groupId)
    {
        return await _db.Users.Join(_db.Users_Groups, usr => usr.Id, usr_grp => usr_grp.UserId, (usr, usr_grp) => new
        {
            UserID = usr.Id,
            Email = usr.Email,
            Role = usr.Role,
            GroupID = usr_grp.GroupId
        }).Where(x => x.GroupID == groupId).Select(user => new UserDTO
        {
            UserId = user.UserID,
            Email = user.Email,
            Role = user.Role
        }).ToListAsync();
    }
}