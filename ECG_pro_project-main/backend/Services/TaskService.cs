using Microsoft.EntityFrameworkCore;

public class TaskService : BaseService, ITaskService
{
    private const int randomizedAnswersRange = 4;
    private readonly ApplicationDbContext _db;
    public TaskService(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }


    public async Task<List<TaskDTO>> GetTasks()
    {
        return await _db.Tasks.Select(x => new TaskDTO
        {
            TaskId = x.TaskId,
            TaskDescription = x.TaskDescription
        }).ToListAsync();
    }

    public async Task<Task?> GetTask(int taskId)
    {
        return await _db.Tasks.Where(x => x.TaskId == taskId).SingleOrDefaultAsync();
    }

    public async Task<List<TaskDTO>> GetTasksByUser(int userId)
    {
        return await _db.Users_Groups.Join(_db.Tasks_Groups, usr_grp => usr_grp.User_GroupId, tsk_grp => tsk_grp.AssignedUserGroupId, (usr_grp, tsk_grp) => new
        {
            TaskId = tsk_grp.TaskAssignedId,
            GroupId = usr_grp.GroupId,
            GroupName = usr_grp.IdGroupNavigation.GroupName,
            UserId = usr_grp.UserId,
            Submitted = tsk_grp.Submitted,
        })
        .Where(x => x.UserId == userId && !x.Submitted)
        .Join(_db.Tasks, tmp => tmp.TaskId, tsk => tsk.TaskId, (tmp, tsk) => new TaskDTO
        {
            TaskId = tsk.TaskId,
            TaskDescription = tsk.TaskDescription,
            GroupId = tmp.GroupId,
            GroupName = tmp.GroupName,
        }).ToListAsync();
    }

    public async Task<List<TaskDTO?>> GetTasksByGroup(int groupId)
    {
        return await _db.Users_Groups.Join(_db.Tasks_Groups, usr_grp => usr_grp.User_GroupId, tsk_grp => tsk_grp.AssignedUserGroupId, (usr_grp, tsk_grp) => new
        {
            TaskId = tsk_grp.TaskAssignedId,
            GroupId = usr_grp.GroupId,
            UserId = usr_grp.UserId,
            Submitted = tsk_grp.Submitted
        })
        .Where(x => x.GroupId == groupId)
        .Join(_db.Tasks, tmp => tmp.TaskId, tsk => tsk.TaskId, (tmp, tsk) => new TaskDTO
        {
            TaskId = tsk.TaskId,
            GroupId = tmp.GroupId,
            TaskDescription = tsk.TaskDescription
        })
        .GroupBy(x => x.TaskId)
        .Select(x => x.FirstOrDefault())
        .ToListAsync();
    }

    public async Task<List<QuestionTypeDTO>> GetQuestionTypes()
    {
        return await _db.Question_Types.Select(x => new QuestionTypeDTO
        {
            QuestionTypeId = x.Question_TypeId,
            QuestionTypeText = x.QuestionTypeText
        }).ToListAsync();
    }

    private async Task<List<ECGDiagram>> GetListOfECGDiagrams(int questionTypeId)
    {
        return await _db.ECGDiagrams.Where(x => x.QuestionTypeId == questionTypeId).ToListAsync();
    }

    private int randomizedECGDiagram(int max)
    {
        Random r = new Random();
        return r.Next(0, max);
    }
    public async Task<bool> AssignTasks(AssignTasksDTO data)
    {
        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            foreach (var questionType in data.Tasks)
            {
                var temp = await GetListOfECGDiagrams(questionType);
                if (temp.Count < 1)
                {
                    //no question type found
                     
                    return false;
                }
                foreach (var groupId in data.Groups)
                {
                    Task newTask = new Task { TaskDescription = data.TaskDescription };
                    await _db.Tasks.AddAsync(newTask);
                    await _db.SaveChangesAsync();
                    var users = await GetUsersFromGroup(groupId);
                    foreach (var user in users)
                    {
                        var userGroup = await _db.Users_Groups.Where(x => x.UserId == user.Id && x.GroupId == groupId).FirstOrDefaultAsync();
                        if (userGroup == null)
                        {
                            return false;
                        }
                        int ecgId = temp[randomizedECGDiagram(temp.Count)].ECGDiagramId;
                        await _db.Tasks_Groups.AddAsync(new Task_Group { TaskAssignedId = newTask.TaskId, AssignedUserGroupId = userGroup.User_GroupId, ECGDiagramId = ecgId });
                        await _db.SaveChangesAsync();
                    }
                }
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
    private async Task<List<User>> GetUsersFromGroup(int groupId)
    {
        return await _db.Users.Join(_db.Users_Groups, usr => usr.Id, usr_grp => usr_grp.UserId, (usr, usr_grp) => new
        {
            UserObj = usr,
            GroupID = usr_grp.GroupId
        })
        .Where(x => x.GroupID == groupId)
        .Select(x => x.UserObj)
        .ToListAsync();
    }


    public async Task<Question_Type?> GetQuestionType(int questionTypeId)
    {
        return await _db.Question_Types.Where(x => x.Question_TypeId == questionTypeId).SingleOrDefaultAsync();
    }

    public async Task<Group?> GetGroup(int groupId)
    {
        return await _db.Groups.Where(x => x.GroupId == groupId).SingleOrDefaultAsync();
    }

    public async Task<List<Task_Question>> GetTaskQuestion()
    {
        return await _db.Tasks_Questions.ToListAsync();
    }

    public async Task<bool> HandleTaskSubmission(TaskSubmissionDTO taskSubmissionDTO, int userId)
    {
        var taskGroup = await _db.Tasks_Groups.Where(x => x.TaskAssignedId == taskSubmissionDTO.TaskId).Join(_db.Users_Groups, tsk_grp => tsk_grp.AssignedUserGroupId, usr_grp => usr_grp.User_GroupId, (tsk_grp, usr_grp) => new { UserId = usr_grp.UserId, TaskGroup = tsk_grp }).Where(x => x.UserId == userId).Select(x => x.TaskGroup).SingleOrDefaultAsync();
        if (taskGroup == null)
        {
            return false;
        }
        foreach (var element in taskSubmissionDTO.AnswerStructures)
        {
            await _db.Tasks_Questions.AddAsync(new Task_Question
            {
                UserId = userId,
                BelongedTaskGroupId = taskGroup.TaskGroupId,
                ParentQuestionNumber = element.ParentQuestionNumber,
                QuestionNumber = element.QuestionNumber,
                AnswerNumber = element.AnswerNumber,
                Answer = element.Answer
            });
        }
        taskGroup.Submitted = true;
        await _db.SaveChangesAsync();
        return true;
    }

    //TODO: fix user stats or delete if not used
    public async Task<double> GetUserStats(int userId)
    {
        return 1987;
    }

    private const double MAX_SCORE = 64;
    public async Task<double> ScorePerTask(int taskGroupId)
    {
        var allUserChoices = UserChoicesToDictionary(await _db.Tasks_Questions.Where(x => x.BelongedTaskGroupId == taskGroupId).ToListAsync());
        int ecgId = (await _db.Tasks_Groups.Where(x => x.TaskGroupId == taskGroupId).SingleAsync()).ECGDiagramId;
        if (allUserChoices == null || allUserChoices.Count == 0)
        {
            return 0; //every question will have at least one choice
        }

        var questionPath = QuestionFormat.questionFormat;

        var currScore = MAX_SCORE;

        for (int parentNumber = 1; parentNumber <= questionPath.Count; parentNumber++)
        {

            if (parentNumber == 4)
            {
                continue;
            }
            for (int subNumber = 1; subNumber <= questionPath[parentNumber].Count; subNumber++)
            {
                int difference = questionPath[parentNumber].Count;
                 
                if (parentNumber > 4)
                {
                    difference--;
                     
                }
                bool hasParentKey = allUserChoices.ContainsKey(parentNumber);
                bool shouldHaveParentKey = await ShouldHaveParentKey(parentNumber, ecgId);
                if ((!hasParentKey && shouldHaveParentKey) || (hasParentKey && !shouldHaveParentKey))
                {
                    currScore -= difference; //subtract the number of questions
                    break;
                }
                if (!hasParentKey && !shouldHaveParentKey)
                {
                    break;
                }
                bool hasKey = allUserChoices[parentNumber].ContainsKey(subNumber);
                bool shouldHaveKey = await ShouldHaveKey(parentNumber, subNumber);
                 
                if ((!hasKey && shouldHaveKey) || (hasKey && !shouldHaveKey))
                {
                    currScore--; //subtract the number of questions
                    continue;
                }
                if (!hasKey && !shouldHaveKey)
                {
                    continue;
                }
                bool isCorrect = await ScorePerQuestion(parentNumber, subNumber, allUserChoices[parentNumber][subNumber], ecgId);
                if (parentNumber > 4 && subNumber == 1 && parentNumber != 12)
                {
                    if (isCorrect)  //if user click "No answers correct" and indeed is the case
                    {
                        break; //next question
                    }
                    if (!isCorrect) //if user click "No answers correct"
                    {
                         
                        currScore -= difference; //subtract the number of questions
                        break;
                    }
                }
                if (!isCorrect)
                {
                     
                    currScore--;
                    break;
                }

            }
             
        }
        return currScore;
    }

    private async Task<bool> ShouldHaveParentKey(int parentNumber, int ecgId)
    {
        return await _db.CheatSheets.Where(x => x.ParentQuestionNumber == parentNumber && x.ECGDiagramId == ecgId).AnyAsync();
    }
    private async Task<bool> ShouldHaveKey(int parentNumber, int subNumber)
    {
        return await _db.CheatSheets.Where(x => x.ParentQuestionNumber == parentNumber && x.QuestionNumber == subNumber).AnyAsync();
    }
    private async Task<bool> ScorePerQuestion(int parentNumber, int subNumber, Dictionary<int, string> userChoices, int ecgId)
    {
        var cheatSheetQuestions = await GetCheatSheet(parentNumber, subNumber, ecgId);
        if (cheatSheetQuestions.Count != userChoices.Count)
        {
            return false;
        }
         
        foreach (var item in cheatSheetQuestions)
        {
             
        }
        foreach (var item in userChoices)
        {
             
        }
        foreach (var item in cheatSheetQuestions)
        {
             
            if (!userChoices.ContainsKey(item.AnswerNumber))
            {
                return false;
            }
            if (!userChoices[item.AnswerNumber].Trim().ToLower().Equals(item.Answer.Trim().ToLower()))
            {
                 
                return false;
            }
        }
        return true;
    }

    private async Task<List<CheatSheet>> GetCheatSheet(int parentNumber, int subNumber, int ecgId)
    {
        var res = new Dictionary<int, string>();
        return await _db.CheatSheets.Where(x => x.ParentQuestionNumber == parentNumber && x.QuestionNumber == subNumber && x.ECGDiagramId == ecgId).ToListAsync();
    }

    private Dictionary<int, Dictionary<int, Dictionary<int, string>>> UserChoicesToDictionary(List<Task_Question> taskQuestions)
    {
         
        var res = new Dictionary<int, Dictionary<int, Dictionary<int, string>>>();
        foreach (var item in taskQuestions)
        {
            if (!res.ContainsKey(item.ParentQuestionNumber))
            {
                res[item.ParentQuestionNumber] = new Dictionary<int, Dictionary<int, string>>();
            }
            if (!res[item.ParentQuestionNumber].ContainsKey(item.QuestionNumber))
            {
                res[item.ParentQuestionNumber][item.QuestionNumber] = new Dictionary<int, string>();
            }
            if (!res[item.ParentQuestionNumber][item.QuestionNumber].ContainsKey(item.AnswerNumber))
            {
                res[item.ParentQuestionNumber][item.QuestionNumber][item.AnswerNumber] = item.Answer;
            }
            res[item.ParentQuestionNumber][item.QuestionNumber][item.AnswerNumber] = item.Answer;

        }
        foreach (var item in res)
        {
            foreach (var item2 in item.Value)
            {
                foreach (var item3 in item2.Value)
                {
                     
                }
            }
        }
        return res;
    }
    public async Task<bool> RemoveTask(int taskId, int groupId)
    {
        var taskGroups = await _db.Tasks_Groups
        .Where(x => x.TaskAssignedId == taskId)
        .Join(_db.Users_Groups, tsk_grp => tsk_grp.AssignedUserGroupId, usr_grp => usr_grp.User_GroupId, (tsk_grp, usr_grp) => new
        {
            TaskGroup = tsk_grp,
            GroupId = usr_grp.GroupId
        })
        .Where(x => x.GroupId == groupId)
        .Select(x => x.TaskGroup)
        .ToListAsync();
        _db.Tasks_Groups.RemoveRange(taskGroups);
        await _db.SaveChangesAsync();
        if (!await _db.Tasks_Groups.Where(x => x.TaskAssignedId == taskId).AnyAsync())
        {
            Task? task = await _db.Tasks.Where(x => x.TaskId == taskId).SingleOrDefaultAsync();
            if (task != null)
            {
                _db.Tasks.Remove(task);
            }
        }
        await _db.SaveChangesAsync();
        return false;
    }

    public async Task<List<Patient>> GetPatients()
    {
        return await _db.Patients.ToListAsync();
    }

    public async Task<Patient> GetPatient(int patientId)
    {
        return await _db.Patients.Where(x => x.PatientId == patientId).SingleAsync();
    }
    public async Task<OverallScorePerGroupDTO> GetOverAllScoreForEachGroup(int userId)
    {

        var allGroups = await _db.Users_Groups.Where(x => x.UserId == userId).Select(x => x.GroupId).ToListAsync();
        List<ScorePerGroup> scores = new List<ScorePerGroup>();

        for (int i = 0; i < allGroups.Count; i++)
        {
            var allTaskGroups = await _db.Users_Groups.Where(x => x.UserId == userId && x.GroupId == allGroups[i]).Join(_db.Tasks_Groups, usr_grp => usr_grp.User_GroupId, tsk_grp => tsk_grp.AssignedUserGroupId, (usr_grp, tsk_grp) => tsk_grp).ToListAsync();
            foreach (var item in allTaskGroups)
            {
                if (!item.Submitted)
                {
                    break;
                }
                double percentage = await ScorePerTask(item.TaskGroupId) / MAX_SCORE * 100;
                scores.Add(new ScorePerGroup
                {
                    GroupId = allGroups[i],
                    ScoreInPercentage = percentage
                });
            }
        }

        return new OverallScorePerGroupDTO { ScorePerGroups = scores };
    }

    public async Task<TaskHistoryResponseDTO> GetTaskHistory(int userId, int taskId)
    {
        var taskGroup = await _db.Tasks_Groups.Where(x => x.TaskAssignedId == taskId).Join(_db.Users_Groups, tskGrp => tskGrp.AssignedUserGroupId, usrGrp => usrGrp.User_GroupId, (tskGrp, usrGrp) => new { TaskGroup = tskGrp, UserGroup = usrGrp }).Where(x => x.UserGroup.UserId == userId).Select(x => x.TaskGroup).SingleAsync();

        var cheatSheetsList = await _db.CheatSheets.Where(x => x.ECGDiagramId == taskGroup.ECGDiagramId).ToListAsync();

        var userAnswersList = await _db.Tasks_Questions.Where(x => x.BelongedTaskGroupId == taskGroup.TaskGroupId).ToListAsync();

        List<AnswerCheckStructure> cheatSheets = new List<AnswerCheckStructure>();

        List<AnswerCheckStructure> userAnswers = new List<AnswerCheckStructure>();

        foreach (var item in cheatSheetsList)
        {
            cheatSheets.Add(new AnswerCheckStructure
            {
                ParentQuestionNumber = item.ParentQuestionNumber,
                QuestionNumber = item.QuestionNumber,
                AnswerNumber = item.AnswerNumber,
                Answer = item.Answer
            });
        }
        foreach (var item in userAnswersList)
        {
            userAnswers.Add(new AnswerCheckStructure
            {
                ParentQuestionNumber = item.ParentQuestionNumber,
                QuestionNumber = item.QuestionNumber,
                AnswerNumber = item.AnswerNumber,
                Answer = item.Answer
            });
        }

        return new TaskHistoryResponseDTO
        {
            CheatSheets = cheatSheets,
            UserAnswers = userAnswers
        };
    }
    public async Task<List<TaskHistoryDTO>> GetFinishedTasksByUser(int userId)
    {
        var finishedTasks = await _db.Users_Groups.Join(_db.Tasks_Groups, usr_grp => usr_grp.User_GroupId, tsk_grp => tsk_grp.AssignedUserGroupId, (usr_grp, tsk_grp) => new
        {
            TaskGroupId = tsk_grp.TaskGroupId,
            TaskId = tsk_grp.TaskAssignedId,
            GroupId = usr_grp.GroupId,
            GroupName = usr_grp.IdGroupNavigation.GroupName,
            UserId = usr_grp.UserId,
            Submitted = tsk_grp.Submitted
        })
       .Where(x => x.UserId == userId && x.Submitted)
       .Join(_db.Tasks, tmp => tmp.TaskId, tsk => tsk.TaskId, (tmp, tsk) => new
       {
           CreatedTime = tsk.TimeCreated,
           TaskId = tsk.TaskId,
           TaskGroupId = tmp.TaskGroupId,
           GroupName = tmp.GroupName,
           GroupId = tmp.GroupId,
           TaskDescription = tsk.TaskDescription
       }).OrderByDescending(x => x.CreatedTime).ToListAsync();

        var res = new List<TaskHistoryDTO>();
        foreach (var item in finishedTasks)
        {
            var score = await ScorePerTask(item.TaskGroupId);
            res.Add(new TaskHistoryDTO
            {
                TaskId = item.TaskId,
                TaskDescription = item.TaskDescription,
                TaskScore = score,
                GroupId = item.GroupId,
                GroupName = item.GroupName,
            });
        }
        return res;

    }
    public async Task<bool> ExistsTaskGroup(int taskGroupId)
    {
        return await _db.Tasks_Groups.Where(x => x.TaskGroupId == taskGroupId).AnyAsync();
    }

    public async Task<FinishedTasksByGroupDTO> GetFinishedTasksByGroup(int groupId, int userId)
    {
        var userGroup = await _db.Users_Groups.Where(x => x.GroupId == groupId && x.UserId == userId).SingleOrDefaultAsync();
        if (userGroup == null)
        {
            return new FinishedTasksByGroupDTO();
        }
        var tasks = await _db.Tasks_Groups.Where(x => x.AssignedUserGroupId == userGroup.User_GroupId && x.Submitted).ToListAsync();
        if (tasks == null)
        {
            return new FinishedTasksByGroupDTO();
        }
        var res = new FinishedTasksByGroupDTO
        {
            GroupId = groupId,
            FinishedTasks = new List<FinishedTasksByGroup>()
        };
        foreach (var item in tasks)
        {
            var task = await _db.Tasks.Where(x => x.TaskId == item.TaskAssignedId).SingleAsync();
            res.FinishedTasks.Add(new FinishedTasksByGroup
            {
                TaskId = item.TaskAssignedId,
                TaskDescription = task.TaskDescription,
                TaskScore = await ScorePerTask(item.TaskGroupId),
                CreatedTime = task.TimeCreated
            });
        }

        return res;
    }

    public async Task<ECGDiagramDTO> GetDiagramForTask(int taskId, int userId)
    {
        var taskGroup = await _db.Tasks_Groups.Join(_db.Users_Groups, tskGrp => tskGrp.AssignedUserGroupId, usrGrp => usrGrp.User_GroupId, (tskGrp, usrGrp) => new { UserGroup = usrGrp, TaskGroup = tskGrp }).Where(x => x.UserGroup.UserId == userId && x.TaskGroup.TaskAssignedId == taskId).Select(x => x.TaskGroup).SingleAsync();

        var img = await _db.ECGDiagrams.Where(x => x.ECGDiagramId == taskGroup.ECGDiagramId).SingleAsync();
        return new ECGDiagramDTO { Image = await System.IO.File.ReadAllBytesAsync(img.Image) };
    }
    public async Task<ECGDiagramMsDTO> GetDiagramMsForTask(int taskId, int userId)
    {
        var taskGroup = await _db.Tasks_Groups.Join(_db.Users_Groups, tskGrp => tskGrp.AssignedUserGroupId, usrGrp => usrGrp.User_GroupId, (tskGrp, usrGrp) => new { UserGroup = usrGrp, TaskGroup = tskGrp }).Where(x => x.UserGroup.UserId == userId && x.TaskGroup.TaskAssignedId == taskId).Select(x => x.TaskGroup).SingleAsync();

        var ecg = await _db.ECGDiagrams.Where(x => x.ECGDiagramId == taskGroup.ECGDiagramId).SingleAsync();
         
         
        return new ECGDiagramMsDTO { EcgId = ecg.ECGDiagramId, Komor = ecg.Komor, PR = ecg.PR, PQ = ecg.PQ, QT = ecg.QT, QTC = ecg.QTC };
    }

    public async Task<ECGDiagramDTO> GetTaskSummaryForTask(int taskId, int userId)
    {
        var taskGroup = await _db.Tasks_Groups.Join(_db.Users_Groups, tskGrp => tskGrp.AssignedUserGroupId, usrGrp => usrGrp.User_GroupId, (tskGrp, usrGrp) => new { UserGroup = usrGrp, TaskGroup = tskGrp }).Where(x => x.UserGroup.UserId == userId && x.TaskGroup.TaskAssignedId == taskId).Select(x => x.TaskGroup).SingleAsync();

        var ecg = await _db.ECGDiagrams.Where(x => x.ECGDiagramId == taskGroup.ECGDiagramId).SingleAsync();
        return new ECGDiagramDTO { TaskSummary = "string" };
    }
}