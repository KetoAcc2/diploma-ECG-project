using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
public class UserService : BaseService, IUserService
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<User> _userManager;

    public UserService(ApplicationDbContext db, UserManager<User> userManager) : base(db)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<UserDTO> GetUserAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        return new UserDTO
        {
            UserId = user.Id,
            Email = user.Email,
            Role = user.Role
        };
        // return 
        // return await _db.Users.Where(x => x.UserId == userId).Select(user => new UserDTO
        // {
        //     UserId = user.UserId,
        //     Email = user.Email,
        //     Role = user.Role
        // }).SingleOrDefaultAsync();
    }
    public async Task<bool> ExistsUserAsync(int userId)
    {
        return (await _userManager.FindByIdAsync(userId.ToString())) == null ? false : true;
        // return await _db.Users.AnyAsync(user => user.UserId == userId);
    }

    public async Task<List<UserDTO>> GetUsersAsync()
    {

        return await _db.Users.Select(user => new UserDTO
        {
            UserId = user.Id,
            Email = user.Email,
            Role = user.Role
        }).ToListAsync();
    }

    public async Task<List<UserDTO>> GetUsersFromGroupAsync(int groupId)
    {
        return await _db.Users.Join(_db.Users_Groups, usr => usr.Id, usr_grp => usr_grp.UserId, (usr, usr_grp) => new
        {
            UserID = usr.Id,
            Email = usr.Email,
            Role = usr.Role,
            GroupID = usr_grp.GroupId,
            GroupOwnerId = usr_grp.IdGroupNavigation.GroupOwner,
        }).Where(x => x.GroupID == groupId && x.UserID != x.GroupOwnerId).Select(user => new UserDTO
        {
            UserId = user.UserID,
            Email = user.Email,
            Role = user.Role
        }).ToListAsync();
    }

    public async Task<bool> ExistsGroupAsync(int groupId)
    {
        return await _db.Groups.AnyAsync(group => group.GroupId == groupId);
    }
}