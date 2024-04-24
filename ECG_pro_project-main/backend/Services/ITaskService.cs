public interface ITaskService : IBaseService
{
    Task<List<TaskDTO>> GetTasks();
    Task<Group?> GetGroup(int groupId);
    Task<Task?> GetTask(int taskId);
    Task<Question_Type?> GetQuestionType(int questionTypeId);
    Task<List<TaskDTO>> GetTasksByUser(int userId);
    Task<List<TaskHistoryDTO>> GetFinishedTasksByUser(int userId);
    Task<List<TaskDTO?>> GetTasksByGroup(int groupId);
    Task<List<QuestionTypeDTO>> GetQuestionTypes();
    Task<bool> AssignTasks(AssignTasksDTO data);
    Task<List<Task_Question>> GetTaskQuestion();
    Task<bool> HandleTaskSubmission(TaskSubmissionDTO taskSubmissionDTO, int userId);
    Task<double> GetUserStats(int userId);
    Task<bool> RemoveTask(int taskId, int groupId);
    Task<List<Patient>> GetPatients();
    Task<Patient> GetPatient(int patientId);
    Task<double> ScorePerTask(int taskGroupId);
    Task<OverallScorePerGroupDTO> GetOverAllScoreForEachGroup(int userId);
    Task<TaskHistoryResponseDTO> GetTaskHistory(int userId, int taskId);
    Task<bool> ExistsTaskGroup(int taskGroupId);
    Task<FinishedTasksByGroupDTO> GetFinishedTasksByGroup(int groupId, int userId);
    Task<ECGDiagramDTO> GetDiagramForTask(int taskId, int userId);
    Task<ECGDiagramDTO> GetTaskSummaryForTask(int taskId, int userId);
    Task<ECGDiagramMsDTO> GetDiagramMsForTask(int taskId, int userId);
}