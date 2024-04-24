import jwtDecode from "jwt-decode";
import {
  apiPath,
  fetchMethod,
  fetchUrl,
  localStorageKeys,
  tokenStatus,
} from "../constants/constants";
import {
  AnswerStructure,
  AssignTasksForm,
  ECGDiagramMsDTO,
  FinishedTasksByGroupDTO,
  ICreateGroupDTO,
  IGroup,
  IGroupCodeDTO,
  IGroupInfo,
  ITaskDTO,
  ITaskHistoryDTO,
  ITokenVerification,
  IUser,
  IUserDTO,
  OverallScorePerGroupDTO,
  QuestionTypeDTO,
  RegenGroupCodeDTO,
  RemoveTasksDTO,
  RemoveUserFromGroupDTO,
  TaskContentDTO,
  TaskHistoryResponseDTO,
  TaskSubmissionDTO,
  UpdateRoleDTO,
} from "../interfaces/interface";

const baseFetch = async (
  path: string,
  method: string,
  jwt: string
): Promise<Response> => {
  return await fetch(`${apiPath.BASE_URL}${path}`, {
    method: method,
    headers: {
      "Access-Control-Allow-Credentials": "true",
      Authorization: `Bearer ${jwt}`,
      "Content-Type": "application/json",
    },
  });
};
const baseFetchWtihPayload = async (
  path: string,
  method: string,
  jwt: string,
  payload: string
): Promise<Response> => {
  return await fetch(`${apiPath.BASE_URL}${path}`, {
    method: method,
    body: payload,
    headers: {
      "Access-Control-Allow-Credentials": "true",
      Authorization: `Bearer ${jwt}`,
      "Content-Type": "application/json",
    },
  });
};

export const getUsers = () => {
  return fetch(`${apiPath.BASE_URL}/Users`)
    .then((res) => res.json())
    .then((data) => {})
    .catch((error) => {});
};

//get all users
export async function getUsersAsync(): Promise<IUser[]> {
  const response = await fetch(`${apiPath.BASE_URL}/Users`);

  return await response.json();
}

export const verifyTokenExpiration = (): number => {
  const userData = localStorage.getItem(localStorageKeys.USER_PERSISTANCE);
  if (userData === undefined || userData === null) {
    return tokenStatus.UNFOUND;
  }
  const token = JSON.parse(userData!)["jwtToken"]["accessToken"];
  const decodedToken = jwtDecode<ITokenVerification>(token);
  const time = new Date().getTime() / 1000;
  if (decodedToken.exp < time) {
    return tokenStatus.INVALID;
  }
  return tokenStatus.VALID;
};

export const renewAccessToken = async (): Promise<number> => {
  const userData = localStorage.getItem(localStorageKeys.USER_PERSISTANCE);
  if (userData === undefined || userData === null) {
    return tokenStatus.UNFOUND;
  }
  try {
    const parsed = JSON.parse(userData);
    const response = await fetch(`${apiPath.BASE_URL}/Auth/Refresh`, {
      method: "POST",
      body: JSON.stringify(parsed["jwtToken"]),
      headers: {
        "Content-Type": "application/json",
      },
    });
    if (response.status === 401) {
      // localStorage.removeItem(localStorageKeys.USER_PERSISTANCE);
      return tokenStatus.RELOG;
    }
    if (response.status !== 200) {
      return tokenStatus.INVALID;
    }
    const res = await response.json();
     
     
    parsed["jwtToken"]["accessToken"] = res["accessToken"]["accessToken"];
    parsed["userData"] = res["userData"];
    localStorage.setItem(
      localStorageKeys.USER_PERSISTANCE,
      JSON.stringify(parsed)
    );
    return tokenStatus.VALID;
  } catch (error) {
    return tokenStatus.INVALID;
  }
};

export const getDashboard = async () => {
  const userData = localStorage.getItem(localStorageKeys.USER_PERSISTANCE);
  if (userData === null || userData === undefined) {
    return null;
  }
  const parsed = JSON.parse(userData);
  const jwt = parsed["jwtToken"]["accessToken"];
  const response = await fetch(`${apiPath.BASE_URL}/Users/Dashboard`, {
    method: "GET",
    headers: {
      "Access-Control-Allow-Credentials": "true",
      Authorization: `Bearer ${jwt}`,
    },
  });
  return await response.json();
};
const getLocalUserDataParsed = () => {
  const userData = localStorage.getItem(localStorageKeys.USER_PERSISTANCE);
  if (userData === null || userData === undefined) {
    return null;
  }

  return JSON.parse(userData);
};

//get single user, currently unused consider deleting in the future
//most liekly going to be for professors if at all
export async function getUserAsync(
  userId: number
): Promise<IUserDTO | undefined> {
  const status = verifyTokenExpiration();
  if (status !== tokenStatus.VALID) {
    await renewAccessToken();
  }
  const userData = localStorage.getItem(localStorageKeys.USER_PERSISTANCE);
  if (userData === null || userData === undefined) {
    return undefined;
  }
  //TODO: use getLocalUserDataParsed here
  const parsed = JSON.parse(userData!);
  const jwt = parsed["jwtToken"]["accessToken"];
  const response = await fetch(`${apiPath.BASE_URL}/Users/${userId}`, {
    method: "GET",
    headers: {
      "Access-Control-Allow-Credentials": "true",
      Authorization: `Bearer ${jwt}`,
    },
  });
  return await response.json();
}

export const getGroupCode = async (
  groupId: number
): Promise<IGroupCodeDTO | null> => {
  const userData = getLocalUserDataParsed();
  if (userData === null) {
    return null;
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const response = await baseFetch(
    `${fetchUrl.GROUP_CODE}/${groupId}`,
    fetchMethod.GET,
    jwt
  );
  if (response.status !== 200) {
    return null;
  }
  return await response.json();
};

export async function getCurrentGroupsAsync(): Promise<IGroup[] | undefined> {
  const userData = getLocalUserDataParsed();
  if (userData === null) {
    return undefined;
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const response = await fetch(`${apiPath.BASE_URL}/Groups/JoinedGroup`, {
    method: "GET",
    headers: {
      Authorization: `Bearer ${jwt}`,
    },
  });
  if (response.status !== 200) {
    return undefined;
  }
  return await response.json();
}

export const createGroupAsync = async (
  groupName: string
): Promise<Response | null> => {
  const userData = getLocalUserDataParsed();
  if (userData === null) {
    return null;
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const payload = { groupName: groupName } as ICreateGroupDTO;
  const response = await baseFetchWtihPayload(
    fetchUrl.CREATE_GROUP,
    fetchMethod.POST,
    jwt,
    JSON.stringify(payload)
  );
  if (response.status !== 200) {
    return null;
  }
   
  return response;
};

export const joinGroupAsync = async (
  groupCode: string
): Promise<Response | null> => {
  const userData = getLocalUserDataParsed();
  if (userData === null) {
    return null;
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const payload = { groupCode: groupCode } as IGroupCodeDTO;
  const response = await baseFetchWtihPayload(
    fetchUrl.JOIN_GROUP,
    fetchMethod.POST,
    jwt,
    JSON.stringify(payload)
  );

  return response;
};

export const getCreatedGroupAsync = async (): Promise<IGroup[] | null> => {
  const userData = getLocalUserDataParsed();
  if (userData === null) {
    return null;
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const response = await baseFetch(
    "/Groups/CreatedGroup",
    fetchMethod.GET,
    jwt
  );
  if (response.status !== 200) {
    return null;
  }
  return await response.json();
};

export async function getUsersFromGroupsAsync(
  groupId: number
): Promise<IUserDTO[]> {
  const userData = getLocalUserDataParsed();
  if (userData === null || userData === undefined) {
     
    return [];
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const response = await fetch(
    `${apiPath.BASE_URL}/Users/UsersFromGroup/${groupId}`,
    {
      method: "GET",
      headers: {
        "Access-Control-Allow-Credentials": "true",
        Authorization: `Bearer ${jwt}`,
      },
    }
  );
  if (response.status !== 200) {
     
    return [];
  }
  return await response.json();
}
export async function getGroupInfoAsync(): Promise<IGroupInfo> {
  const userData = getLocalUserDataParsed();
  const defaultValue = {} as IGroupInfo;
  defaultValue.usersAndGroups = {};
  if (userData === null) {
    return defaultValue;
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const response = await fetch(`${apiPath.BASE_URL}/UsersGroups/GroupsInfo`, {
    method: "GET",
    headers: {
      "Access-Control-Allow-Credentials": "true",
      Authorization: `Bearer ${jwt}`,
    },
  });
  if (response.status !== 200) {
    return defaultValue;
  }
  return await response.json();
}

export const removeGroup = async (groupId: number) => {
  const userData = getLocalUserDataParsed();
  if (!userData) {
    return;
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const response = await baseFetch(
    `${fetchUrl.REMOVE_GROUP}/${groupId}`,
    fetchMethod.DELETE,
    jwt
  );
  return;
};

export const getTodoTasks = async (): Promise<ITaskDTO[]> => {
  const userData = getLocalUserDataParsed();
  if (!userData) {
    return [];
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const response = await baseFetch(
    `${fetchUrl.GET_TASKS_BY_USER}`,
    fetchMethod.GET,
    jwt
  );
  return await response.json();
};

export const getQuestionTypes = async (): Promise<QuestionTypeDTO[]> => {
  const userData = getLocalUserDataParsed();
  if (!userData) {
    return [];
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const response = await baseFetch(
    `${fetchUrl.GET_QUESTION_TYPES}`,
    fetchMethod.GET,
    jwt
  );
  return await response.json();
};

export const assignTask = async (form: AssignTasksForm) => {
  const userData = getLocalUserDataParsed();
  if (!userData) {
    return false;
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const payload = JSON.stringify(form);
  const response = await baseFetchWtihPayload(
    `${fetchUrl.ASSIGN_TASKS}`,
    fetchMethod.POST,
    jwt,
    payload
  );
  if (response.status === 200) {
    return true;
  }
  return false;
};

export const removeUserFromGroup = async (
  dto: RemoveUserFromGroupDTO
): Promise<boolean> => {
  if (dto.groupId < 0 || dto.userId < 0) {
    return false;
  }
  const userData = getLocalUserDataParsed();
  if (!userData) {
    return false;
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const payload = JSON.stringify(dto);
  const response = await baseFetchWtihPayload(
    `${fetchUrl.REMOVE_USER_FROM_GROUP}`,
    fetchMethod.DELETE,
    jwt,
    payload
  );

  if (response.status === 200) {
    return true;
  }
  return false;
};

export const regenGroupCode = async (
  dto: RegenGroupCodeDTO
): Promise<boolean> => {
  const userData = getLocalUserDataParsed();
  if (!userData) {
    return false;
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const payload = JSON.stringify(dto);
  const response = await baseFetchWtihPayload(
    `${fetchUrl.REGEN_GROUP_CODE}`,
    fetchMethod.PATCH,
    jwt,
    payload
  );
  if (response.status === 200) {
    return true;
  }
  return false;
};

export const getTaskContent = async (
  taskId: number
): Promise<TaskContentDTO | null> => {
  const userData = getLocalUserDataParsed();
  if (!userData) {
    return null;
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const response = await baseFetch(
    `${fetchUrl.TASK_CONTENT}/${taskId}`,
    fetchMethod.GET,
    jwt
  );
  if (response.status !== 200) {
    return null;
  }
  return await response.json();
};

export const submitTask = async (
  taskSubmission: AnswerStructure[],
  taskId: number
) => {
  const userData = getLocalUserDataParsed();
  if (!userData) {
    return null;
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const temp = {
    taskId: taskId,
    answerStructures: taskSubmission,
  } as TaskSubmissionDTO;
  const payload = JSON.stringify(temp);
  const response = await baseFetchWtihPayload(
    `${fetchUrl.SUBMIT_TASK}`,
    fetchMethod.POST,
    jwt,
    payload
  );
  if (response.status !== 200) {
    return null;
  }
  return response.status;
};

export const getUserStats = async (): Promise<OverallScorePerGroupDTO> => {
  const userData = getLocalUserDataParsed();
  const defaultRes = { scorePerGroups: [] };
  if (!userData) {
    return defaultRes;
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const response = await baseFetch(
    `${fetchUrl.OVERALLSCORE}`,
    fetchMethod.GET,
    jwt
  );
  if (response.status !== 200) {
    return defaultRes;
  }
  return await response.json();
};

export type TasksByGroupDTO = {
  groupId: number;
};
export const getTasksByGroup = async (groupId: number): Promise<ITaskDTO[]> => {
  const userData = getLocalUserDataParsed();
  if (!userData) {
    return [];
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const payload = JSON.stringify({ groupId: groupId } as TasksByGroupDTO);
  const response = await baseFetchWtihPayload(
    `${fetchUrl.GET_TASKS_BY_GROUP}`,
    fetchMethod.POST,
    jwt,
    payload
  );
  if (response.status !== 200) {
    return [];
  }
  return await response.json();
};

export const removeTask = async (taskId: number, groupId: number) => {
  const userData = getLocalUserDataParsed();
  if (!userData) {
    return [];
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const payload = JSON.stringify({
    taskId: taskId,
    groupId: groupId,
  } as RemoveTasksDTO);
  const response = await baseFetchWtihPayload(
    `${fetchUrl.REMOVE_TASK_FROM_GROUP}`,
    fetchMethod.DELETE,
    jwt,
    payload
  );
  if (response.status !== 200) {
    return [];
  }
  return response.status;
};

export const getTaskHistory = async (
  taskId: number
): Promise<TaskHistoryResponseDTO | undefined> => {
  const userData = getLocalUserDataParsed();
  const defaultRes = {
    cheatSheets: [],
    userAnswers: [],
  } as TaskHistoryResponseDTO;
  // return undefined;
  if (!userData) {
    return defaultRes;
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const response = await baseFetch(
    `${fetchUrl.GET_TASK_HISTORY}/${taskId}`,
    fetchMethod.GET,
    jwt
  );
  if (response.status !== 200) {
    return defaultRes;
  }
  return await response.json();
};

export const getUserTaskHistory = async (
  taskId: number,
  userId: number
): Promise<TaskHistoryResponseDTO | undefined> => {
  const userData = getLocalUserDataParsed();
  const defaultRes = {
    cheatSheets: [],
    userAnswers: [],
  } as TaskHistoryResponseDTO;
  // return undefined;
  if (!userData) {
    return defaultRes;
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const response = await baseFetch(
    `${fetchUrl.GET_TASK_HISTORY}/${taskId}/${userId}`,
    fetchMethod.GET,
    jwt
  );
  if (response.status !== 200) {
    return defaultRes;
  }
  return await response.json();
};

export const getFinishedTasks = async (): Promise<
  ITaskHistoryDTO[] | undefined
> => {
  const userData = getLocalUserDataParsed();
  if (!userData) {
    return undefined;
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const response = await baseFetch(
    `${fetchUrl.GET_FINISEHD_TASKS}`,
    fetchMethod.GET,
    jwt
  );
  if (response.status !== 200) {
    return undefined;
  }
  return await response.json();
};

export const updateRole = async (
  email: string,
  role: string
): Promise<Response | undefined> => {
  const userData = getLocalUserDataParsed();
  if (!userData) {
    return undefined;
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const payload = JSON.stringify({
    email: email,
    role: role,
  } as UpdateRoleDTO);
  const response = await baseFetchWtihPayload(
    `${fetchUrl.UPDATE_ROLE}`,
    fetchMethod.POST,
    jwt,
    payload
  );

  return response;
};

export const getUserRole = async (
  email: string
): Promise<Response | undefined> => {
  const userData = getLocalUserDataParsed();
  if (!userData) {
    return undefined;
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const response = await fetch(
    `${apiPath.BASE_URL}${fetchUrl.GET_USER_ROLE}/${email}`,
    {
      method: "GET",
      headers: {
        Authorization: `Bearer ${jwt}`,
      },
    }
  );

  return response;
};

export async function getGroupsOfUser(
  userId: number
): Promise<IGroup[] | undefined> {
  const userData = getLocalUserDataParsed();
  if (userData === null) {
    return undefined;
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const response = await fetch(
    `${apiPath.BASE_URL}/Groups/JoinedGroupByUserId/${userId}`,
    {
      method: "GET",
      headers: {
        Authorization: `Bearer ${jwt}`,
      },
    }
  );
  if (response.status !== 200) {
    return undefined;
  }
  return await response.json();
}

export const getFinishedTasksByGroup = async (
  groupId: number
): Promise<FinishedTasksByGroupDTO | undefined> => {
  const userData = getLocalUserDataParsed();
  if (!userData) {
    return undefined;
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const payload = JSON.stringify({
    groupId: groupId,
  } as TasksByGroupDTO);
  const response = await baseFetchWtihPayload(
    `${fetchUrl.GET_FINISHED_TASKS_BY_GROUP}`,
    fetchMethod.POST,
    jwt,
    payload
  );
  if (response.status !== 200) {
    return undefined;
  }
  return response.json();
};

export const getTargetFinishedTasksByGroup = async (
  groupId: number,
  userId: number
): Promise<FinishedTasksByGroupDTO | undefined> => {
  const userData = getLocalUserDataParsed();
  if (!userData) {
    return undefined;
  }
  const jwt = userData["jwtToken"]["accessToken"];
  const payload = JSON.stringify({
    groupId: groupId,
  } as TasksByGroupDTO);
  const response = await baseFetchWtihPayload(
    `${fetchUrl.GET_TARGET_FINISHED_TASKS_BY_GROUP}/${userId}`,
    fetchMethod.POST,
    jwt,
    payload
  );
  if (response.status !== 200) {
    return undefined;
  }
  return response.json();
};

type ECGDTO = {
  image: Uint8Array;
  taskSummary: string;
};

export const getPics = async (taskId: number) => {
  const userData = getLocalUserDataParsed();
  if (!userData) {
    return undefined;
  }
  const jwt = userData["jwtToken"]["accessToken"];

  const response = await fetch(
    `${apiPath.BASE_URL}${fetchUrl.GET_PICS}/${taskId}`,
    {
      method: "GET",
      headers: {
        "Access-Control-Allow-Credentials": "true",
        Authorization: `Bearer ${jwt}`,
        "Content-Type": "image/jpeg",
      },
    }
  );
  if (response.status !== 200) {
    return undefined;
  }
  const data: ECGDTO = await response.json();
  return data.image;
};

export const getTaskSummary = async (
  taskId: number
): Promise<string | undefined> => {
  const userData = getLocalUserDataParsed();
  if (!userData) {
    return "";
  }
  const jwt = userData["jwtToken"]["accessToken"];

  const response = await baseFetch(
    `${fetchUrl.GET_TASK_SUMMARY}/${taskId}`,
    fetchMethod.GET,
    jwt
  );
  if (response.status !== 200) {
    return "";
  }
  const data: ECGDTO = await response.json();
  return data.taskSummary;
};

export const getITDocs = async (): Promise<string | undefined> => {
  const userData = getLocalUserDataParsed();
  if (!userData) {
    return "";
  }
  const jwt = userData["jwtToken"]["accessToken"];

  const response = await baseFetch(
    `${fetchUrl.GET_IT_DOCS}`,
    fetchMethod.GET,
    jwt
  );
  if (response.status !== 200) {
    return "";
  }
  return await response.json();
};
export const getStudentDocs = async (): Promise<string | undefined> => {
  const userData = getLocalUserDataParsed();
  if (!userData) {
    return "";
  }
  const jwt = userData["jwtToken"]["accessToken"];

  const response = await baseFetch(
    `${fetchUrl.GET_STUDENT_DOCS}`,
    fetchMethod.GET,
    jwt
  );
  if (response.status !== 200) {
    return "";
  }
  return await response.json();
};

export const getTeacherDocs = async (): Promise<string | undefined> => {
  const userData = getLocalUserDataParsed();
  if (!userData) {
    return "";
  }
  const jwt = userData["jwtToken"]["accessToken"];

  const response = await baseFetch(
    `${fetchUrl.GET_TEACHER_DOCS}`,
    fetchMethod.GET,
    jwt
  );
  if (response.status !== 200) {
    return "";
  }
  return await response.json();
};

export const getECGMs = async (
  taskId: number
): Promise<ECGDiagramMsDTO | undefined> => {
  const userData = getLocalUserDataParsed();
  if (!userData) {
    return undefined;
  }
  const jwt = userData["jwtToken"]["accessToken"];

  const response = await baseFetch(
    `${fetchUrl.GET_ECGMS}/${taskId}`,
    fetchMethod.GET,
    jwt
  );
  if (response.status !== 200) {
    return undefined;
  }
  return await response.json();
};
