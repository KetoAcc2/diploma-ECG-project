using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
public class GroupService : BaseService, IGroupService
{
    private readonly ApplicationDbContext _db;
    public GroupService(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task<Group?> CreateGroup(string groupName, User groupOwner)
    {
        Group newGroup = new Group
        {
            GroupName = groupName,
            GroupOwner = groupOwner.Id
        };
        newGroup.GroupCode = randomCode();
        try
        {
            await _db.Groups.AddAsync(
                newGroup
            );
            await _db.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            return null;
        }
        return newGroup;
    }

    public async Task<List<Group>> GetGroupsAsync()
    {
        return await _db.Groups.ToListAsync();
    }

    public async Task<Group?> GetGroupAsync(int groupId)
    {
        return await _db.Groups.Where(x => x.GroupId == groupId).SingleOrDefaultAsync();
    }

    public async Task<List<Group>> GetJoinedGroupsAsync(int userId)
    {
        return await _db.Groups.Join(_db.Users_Groups, grp => grp.GroupId, usr_grp => usr_grp.GroupId, (grp, usr_grp) => new
        {
            UserId = usr_grp.UserId,
            GroupId = grp.GroupId,
            Description = grp.GroupName,
            CreatedTime = grp.CreatedTime
        }).Where(x => x.UserId == userId).Select(x => new Group
        {
            GroupId = x.GroupId,
            GroupName = x.Description,
            CreatedTime = x.CreatedTime
        }).OrderByDescending(x => x.CreatedTime).ToListAsync();

        // return await _db.Users_Groups.Where(x=>x.UserId == userId).ToListAsync();
    }

    public async Task<bool> JoinGroup(string groupCode, int userId)
    {
        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            if (!await existsUser(userId))
            {
                throw new Exception("User does not exist");
            }
            Group? group = await _db.Groups.Where(x => x.GroupCode == groupCode).SingleOrDefaultAsync();
            if (group == null)
            {
                throw new Exception("Group does not exist");
            }
            var newUserGroup = new User_Group { UserId = userId, GroupId = group.GroupId };
            await _db.Users_Groups.AddAsync(newUserGroup);
            await _db.SaveChangesAsync();

            var teacherUserGroup = await _db.Users_Groups.Where(x => x.GroupId == group.GroupId && x.UserId == group.GroupOwner).SingleAsync();

            var allTasks = await _db.Tasks_Groups.Where(x => x.AssignedUserGroupId == teacherUserGroup.User_GroupId).GroupBy(x => x.TaskAssignedId).Select(x => x.Single()).ToListAsync();

            foreach (var item in allTasks)
            {
                var newTask = new Task_Group
                {
                    AssignedUserGroupId = newUserGroup.User_GroupId,
                    TaskAssignedId = item.TaskAssignedId,
                    ECGDiagramId = item.ECGDiagramId
                };
                if (await ExistsTaskForUser(newTask))
                {
                    continue;
                }
                await _db.Tasks_Groups.AddAsync(newTask);
            }

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch (System.Exception)
        {
            await transaction.RollbackAsync();
            return false;
        }
    }
    private async Task<bool> ExistsTaskForUser(Task_Group task)
    {
        return await _db.Tasks_Groups.Where(x => x.AssignedUserGroupId == task.AssignedUserGroupId && x.TaskAssignedId == task.TaskAssignedId && x.ECGDiagramId == task.ECGDiagramId).AnyAsync();
    }
    private async Task<bool> existsUser(int userId)
    {
        return await _db.Users.AnyAsync(x => x.Id == userId);
    }
    private async Task<bool> existsGroup(int groupId)
    {
        return await _db.Groups.AnyAsync(x => x.GroupId == groupId);
    }
    private string randomCode()
    {
        Random random = new Random();
        int length = 6;
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    public async Task<List<Group>> GetCreatedGroupsAsync(int userId)
    {
        return await _db.Groups.Where(x => x.GroupOwner == userId).OrderByDescending(x => x.CreatedTime).ToListAsync();
    }
    public async Task<Group?> GetGroupByGroupCode(string groupCode)
    {
        return await _db.Groups.Where(x => x.GroupCode == groupCode).SingleOrDefaultAsync();
    }
    public async Task<bool> ExistsUserInGroup(int groupId, int userId)
    {
        return await _db.Users_Groups.AnyAsync(x => x.UserId == userId && x.GroupId == groupId);
    }

    public async Task<bool> DeleteGroup(int groupId)
    {
        Group? group = await _db.Groups.Where(x => x.GroupId == groupId).SingleOrDefaultAsync();
        if (group == null)
        {
            return false;
        }
        try
        {
            _db.Groups.Remove(group);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (System.Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteUserFromGroup(int groupId, int userId)
    {
        try
        {
            User_Group? usrGrp = await _db.Users_Groups.Where(x => x.GroupId == groupId && x.UserId == userId).SingleOrDefaultAsync();
            if (usrGrp == null)
            {
                return false;
            }
            _db.Users_Groups.Remove(usrGrp);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (System.Exception)
        {
            return false;
        }
    }

    public async Task<bool> RegenGroupCode(int groupId)
    {
        string newGroupCode = randomCode();
        var group = await _db.Groups.Where(x => x.GroupId == groupId).SingleOrDefaultAsync();
        if (group == null)
        {
            return false;
        }
        group.GroupCode = newGroupCode;
        await _db.SaveChangesAsync();
        return true;
    }
}
