public static class ApiRoutes
{
    // public const string baseURL = "http://localhost:3000/";
    // public const string baseURL = "https://ecgapp.azurewebsites.net/";
    public const string baseURL = "https://main--lighthearted-lolly-f63c62.netlify.app/";

    public static class Auth
    {
        public const string MainRoute = "Auth";
        public const string Register = "Register";
        public const string Login = "Login";
        public const string Logout = "Logout";
        public const string Refresh = "Refresh";
        public const string ActivateEmail = "Activate/{activationToken}";
        public const string ResetPasswordSendEmail = "Reset";
        public const string ResetPassowrd = "Reset/{resetPasswordToken}";

    }
    public static class PseudoAdmin
    {
        public const string MainRoute = "PseudoAdmin";
        public const string UpdateRole = "UpdateRole";
        public const string GetUserRole = "GetUserRole/{email}";
        public const string GetITDoc = "GetITDoc";
    }
    public static class User
    {
        public const string MainRoute = "Users";
        public const string GetTeacherDoc = "GetTeacherDoc";
        public const string GetStudentDoc = "GetStudentDoc";
    }
    public static class Group
    {
        public const string MainRoute = "Groups";
        public const string RegenGroupCode = "RegenGroupCode";
        public const string RemoveUserFromGroup = "RemoveUserFromGroup";
        public const string DeleteGroup = "DeleteGroup/{groupId}";
        public const string JoinGroup = "JoinGroup";
        public const string JoinedGroup = "JoinedGroup";
        public const string CreateGroup = "CreateGroup";
        public const string CreatedGroup = "CreatedGroup";
        public const string GetUserJoinedGroupsById = "JoinedGroupByUserId/{userId}";
        public const string GetGroupCode = "GroupCode/{groupId}";
    }
    public static class Task
    {
        public const string MainRoute = "Task";
        public const string Pics = "Pics/{taskId}";
        public const string GetTaskScore = "TaskScore/{taskGroupId}";
        public const string RemoveTask = "RemoveTask";
        public const string UserStats = "UserStats";
        public const string SubmitTask = "SubmitTask";
        public const string AssignTasks = "AssignTasks";
        public const string TaskQuestions = "TaskQuestions";
        public const string QuestionType = "QuestionType";
        public const string OverallScore = "OverallScore";
        public const string GetTaskHistory = "TaskHistory/{taskId}";
        public const string GetUserTaskHistory = "TaskHistory/{taskId}/{userId}";
        public const string FinishedTasks = "FinishedTasks";
        public const string TasksByGroup = "TasksByGroup";
        public const string TasksByUser = "TasksByUser";
        public const string FinishedTasksByGroup = "FinishedTasksByGroup";
        public const string Tasks = "Tasks";
        public const string GetTask = "Tasks/{taskId}";
        public const string GetTaskSummary = "TaskSummary/{taskId}";
        public const string GetECGMs = "ECGMs/{taskId}";
        public const string GetFinishedTasksByGroup = "TargetFinishedTasksByGroup/{userId}";
    }
}