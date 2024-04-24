export const pageInfo = {
  search: {
    url: "/search",
  },
  user_history_task: {
    url: "/history/:taskId/:userId",
  },
  history_task: {
    url: "/history/:taskId",
  },
  history: {
    url: "/history",
  },
  user_detail: {
    url: "/user/:userId/:groupId",
  },
  group: {
    url: "/group",
  },
  group_detail: {
    url: "/group/:id",
  },
  create_group: {
    url: "/creategroup",
  },
  join_group: {
    url: "/joingroup",
  },
  login: {
    url: "/",
  },
  dashboard: {
    url: "/dashboard",
  },
  tasks: {
    url: "/tasks",
  },
  start_task: {
    url: "/starttask/:taskId",
  },
  assign_tasks: {
    url: "/assigntasks",
  },
  remove_tasks: {
    url: "/removetasks",
  },
  practice_ecg: {
    url: "/practice",
  },
  group_management: {
    url: "/groupmanagement",
  },
  register: {
    url: "/register",
  },
  reset_password_send_email: {
    url: "/resetpassordsendemail",
  },
  activation: {
    url: "/Auth/Activate/:activationToken",
  },
  reset_password: {
    url: "/resetpassword/:resetPasswordToken",
  },
  update_role: {
    url: "/updaterole",
  },
  it_instructions: {
    url: "/itinstruction",
  },
  teacher_instructions: {
    url: "/teacherinstruction",
  },
  student_instructions: {
    url: "/studentinstruction",
  },
};

export const apiPath = {
  BASE_URL: "https://ecgapp.azurewebsites.net",
  // BASE_URL: "https://localhost:7193",
};

export const localStorageKeys = {
  USER_PERSISTANCE: "userPersistance",
};

export const tokenStatus = {
  UNFOUND: 0,
  VALID: 1,
  INVALID: -1,
  RELOG: -2,
};

export const userRole = {
  UNKNOWN: 0,
  STUDENT: 1,
  TEACHER: 2,
  ADMIN: 3,
  PSEUDO_ADMIN: 4,
};

export const userRoleString = {
  UNKNOWN: "unknown",
  STUDENT: "Student",
  TEACHER: "Teacher",
  ADMIN: "Admin",
  PSEUDO_ADMIN: "PseudoAdmin",
};

export const fetchMethod = {
  GET: "GET",
  POST: "POST",
  PUT: "PUT",
  DELETE: "DELETE",
  PATCH: "PATCH",
};

export const fetchUrl = {
  RESET_SEND_EMAIL: "/Auth/Reset",
  ACTIVATE: "/Auth/Activate",
  REGISTER: "/Auth/Register",
  LOGIN: "/Auth/Login",
  CREATE_GROUP: "/Groups/CreateGroup",
  JOIN_GROUP: "/Groups/JoinGroup",
  GROUP_CODE: "/Groups/GroupCode",
  REMOVE_GROUP: "/Groups/DeleteGroup",
  GET_TASKS: "/Task/Tasks",
  GET_TASKS_BY_USER: "/Task/TasksByUser",
  GET_QUESTION_TYPES: "/Task/QuestionType",
  ASSIGN_TASKS: "/Task/AssignTasks",
  REMOVE_USER_FROM_GROUP: "/Groups/RemoveUserFromGroup",
  REGEN_GROUP_CODE: "/Groups/RegenGroupCode",
  TASK_CONTENT: "/Task/TaskContent",
  SUBMIT_TASK: "/Task/SubmitTask",
  OVERALLSCORE: "/Task/OverallScore",
  GET_TASKS_BY_GROUP: "/Task/TasksByGroup",
  REMOVE_TASK_FROM_GROUP: "/Task/RemoveTask",
  GET_PATIENTS: "Task/Patients",
  GET_PATIENT: "Task/Patient",
  GET_TASK_HISTORY: "/Task/TaskHistory",
  GET_FINISEHD_TASKS: "/Task/FinishedTasks",
  UPDATE_ROLE: "/PseudoAdmin/UpdateRole",
  GET_USER_ROLE: "/PseudoAdmin/GetUserRole",
  GET_FINISHED_TASKS_BY_GROUP: "/Task/FinishedTasksByGroup",
  GET_TARGET_FINISHED_TASKS_BY_GROUP: "/Task/TargetFinishedTasksByGroup",
  GET_PICS: "/Task/Pics",
  GET_TASK_SUMMARY: "/Task/TaskSummary",
  GET_IT_DOCS: "/PseudoAdmin/GetITDoc",
  GET_STUDENT_DOCS: "/Users/GetStudentDoc",
  GET_TEACHER_DOCS: "/Users/GetTeacherDoc",
  GET_ECGMS: "/Task/ECGMs",
};
