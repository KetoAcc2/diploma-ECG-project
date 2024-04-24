import AccountCircleIcon from "@mui/icons-material/AccountCircle";
import AssignmentIcon from "@mui/icons-material/Assignment";
import DescriptionIcon from '@mui/icons-material/Description';
import GroupIcon from "@mui/icons-material/Group";
import { CSSProperties } from "react";
import { IGroup, ISidebarMenu, QuestionTypeDTO } from "../interfaces/interface";
import { localStorageKeys, pageInfo, userRole, userRoleString } from "./constants";

export const sidebarMenuStudent: ISidebarMenu = {
  sidebar: [
    {
      identifier: "Dashboard",
      text: "sidemenu.dashboard",
      url: pageInfo.dashboard.url,
      icon: <AccountCircleIcon />,
    },
    {
      identifier: "Task",
      text: "sidemenu.tasks",
      icon: <AssignmentIcon />,
      dropDown: [
        {
          url: pageInfo.tasks.url,
          text: "sidemenu.todo_tasks",
        },
        {
          url: pageInfo.history.url,
          text: "sidemenu.task_history",
        },
      ],
    },

    {
      identifier: "Group",
      text: "sidemenu.groups",
      url: pageInfo.group.url,
      icon: <GroupIcon />,
    },
    {
      identifier: "StudentInstruction",
      text: "sidemenu.instruction",
      url: pageInfo.student_instructions.url,
      icon: <DescriptionIcon />,
    },
  ],
};

export const sidebarMenuTeacher: ISidebarMenu = {
  sidebar: [
    {
      identifier: "Dashboard",
      text: "sidemenu.dashboard",
      url: pageInfo.dashboard.url,
      icon: <AccountCircleIcon />,
    },
    {
      identifier: "Task",
      text: "sidemenu.tasks",
      icon: <AssignmentIcon />,
      dropDown: [
        {
          url: pageInfo.tasks.url,
          text: "sidemenu.todo_tasks",
        },
        {
          url: pageInfo.history.url,
          text: "sidemenu.task_history",
        },
      ],
    },
    {
      identifier: "Group",
      text: "sidemenu.groups",
      icon: <GroupIcon />,
      dropDown: [
        {
          url: pageInfo.group.url,
          text: "sidemenu.my_groups",
        },
        {
          url: pageInfo.group_management.url,
          text: "sidemenu.group_management",
        },
      ],
    },
    {
      identifier: "TeacherInstruction",
      text: "sidemenu.instruction",
      url: pageInfo.teacher_instructions.url,
      icon: <DescriptionIcon />,
    },
  ],
};

export const sidebarMenuPseudoAdmin: ISidebarMenu = {
  sidebar:[
    {
      identifier: "UpdateRole",
      text: "sidemenu.updaterole",
      url: pageInfo.update_role.url,
      icon: <AccountCircleIcon />,
    },
    {
      identifier: "ITInstruction",
      text: "sidemenu.instruction",
      url: pageInfo.it_instructions.url,
      icon: <DescriptionIcon />,
    }
  ]
}

export const getUserData = (): number => {
  const userData = localStorage.getItem(localStorageKeys.USER_PERSISTANCE);
  if (userData === null || userData === null) {
    return userRole.UNKNOWN;
  }
  const parsed = JSON.parse(userData);
  if (parsed["userData"]["role"] === userRoleString.TEACHER) {
    return userRole.TEACHER;
  }
  if (parsed["userData"]["role"] === userRoleString.STUDENT) {
    return userRole.STUDENT;
  }
  if (parsed["userData"]["role"] === userRoleString.ADMIN) {
    return userRole.ADMIN;
  }
  if (parsed["userData"]["role"] === userRoleString.PSEUDO_ADMIN) {
    return userRole.PSEUDO_ADMIN;
  }
  return userRole.UNKNOWN;
};

export const isTeacher = (): boolean => {
  if (getUserData() === userRole.TEACHER) {
    return true;
  }
  return false;
};

export interface AssignTaskFormProp {
  setForm: React.Dispatch<React.SetStateAction<any>>;
  data: QuestionTypeDTO[];
}

export interface GroupsToAssignTaskProp {
  setForm: React.Dispatch<React.SetStateAction<any>>;
  data: IGroup[];
}

export const checkboxRowStyle = {
  display: "flex",
  textAlign: "right",
} as CSSProperties;
export const headerContainerStyle = { textAlign: "left" } as CSSProperties;
