import { SvgIconProps } from "@mui/material";

export interface IUser {
  userId: number;
  email: string;
  password: string;
  role: string;
}
export interface IGroup {
  groupId: number;
  groupName: string;
  groupCode: string;
}

export interface IUserDTO {
  userId: number;
  email: string;
  role: string;
}
export interface IGroupInfo {
  usersAndGroups: { [groupDescription: string]: IUserDTO[] };
}

export interface ICreateGroupDTO {
  groupName: string;
}
export interface IGroupCodeDTO {
  groupCode: string;
}

export interface ITaskDTO {
  taskId: number;
  groupId: number;
  taskDescription: string;
  groupName: string;
}

export interface IEcgWidthRatio {
  [key: string]: number;
}

export interface ILoginResponse {
  status: string;
  accessToken: string;
}

export interface ILoginDTO {
  email: string;
  password: string;
}

export interface IRegistrationDTO {
  email: string;
  password: string;
  passwordConfirmation: string;
}

export interface UserRegisterDTO {
  email: string;
  password: string;
}

export interface IJwtToken {
  accessToken: string;
  refreshToken: string;
}

export interface UserPersist {
  userData: IUserDTO;
  jwtToken: IJwtToken;
  loggedIn: boolean;
}

export interface ITokenVerification {
  exp: number;
}

export interface ISidebarMenu {
  sidebar: SidebarEntry[];
}

export interface SidebarDropmenuStatus {
  [key: string]: {
    opened: boolean;
  };
}

interface SidebarEntry {
  identifier: string;
  text: string;
  url?: string;
  icon: React.ReactElement<SvgIconProps>;
  dropDown?: DropDownEntry[];
}

interface DropDownEntry {
  text: string;
  url: string;
}

export interface TablePropWrapper {
  genericTable: GenericTableProp;
}

export interface GenericTableProp {
  tableRows: ColumnData[];
  detailsUrl?: string;
  removeUrl?: string;
}

export interface ColumnData {
  columns: string[];
}

export interface HeaderNames {
  columnOne: string;
  columnTwo: string;
  columnThree?: string;
}

export interface HeaderWrapper {
  header: HeaderNames;
}

export interface TablePropWrapper {
  headers: HeaderNames;
  genericTable: GenericTableProp;
}
export interface GroupUsersPropWrapper {
  headers: HeaderNames;
  genericTable: GenericTableProp;
  groupId: number;
}

export interface QuestionTypeDTO {
  questionTypeId: number;
  questionTypeText: string;
}

export interface AssignTasksForm {
  tasks: number[];
  groups: number[];
  taskDescription: string;
}

export interface RemoveUserFromGroupDTO {
  groupId: number;
  userId: number;
}

export interface RegenGroupCodeDTO {
  groupId: number;
}

export interface AnswerDTO {
  answerId: number;
  answerText: string;
}
export interface QuestionDTO {
  questionId: number;
  questionText: string;
  availableAnswers: AnswerDTO[];
}
export interface TaskContentDTO {
  taskQuestionId: number;
  questionsRelatedToThisTask: QuestionDTO[];
}

export interface TaskSubmissionDTO {
  taskId: number;
  answerStructures: AnswerStructure[];
}

export interface RemoveTasksDTO {
  taskId: number;
  groupId: number;
}

export type CardDivStyle = {
  style?: React.CSSProperties;
};

export interface AnswerStructure {
  parentQuestionNumber: number;
  questionNumber: number;
  answerNumber: number;
  ecgDiagramId: number;
  answer: string;
}

export interface OverallScorePerGroupDTO {
  scorePerGroups: { groupId: number; scoreInPercentage: number }[];
}

export interface AnswerCheckStructure {
  parentQuestionNumber: number;
  questionNumber: number;
  answerNumber: number;
  answer: string;
}

export interface TaskHistoryResponseDTO {
  cheatSheets: AnswerCheckStructure[];
  userAnswers: AnswerCheckStructure[];
}

export interface ITaskHistoryDTO {
  taskId: number;
  taskDescription: string;
  taskScore: number;
  groupId: number;
  groupName: string;
}

export interface UpdateRoleDTO {
  email: string;
  role: string;
}

export interface FinishedTasksByGroupDTO {
  groupId: number;
  finishedTasks: FinishedTasksByGroup[];
}

export interface FinishedTasksByGroup {
  taskId: number;
  taskDescription: string;
  taskScore: number;
  createdTime: Date;
}

export interface ECGDiagramMsDTO{
  ecgId: number;
  komor:number;
  pr:number;
  pq:number;
  qt:number;
  qtc:number;
}