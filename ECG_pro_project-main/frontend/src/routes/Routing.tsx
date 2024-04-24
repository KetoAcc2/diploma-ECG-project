import { useContext } from "react";
import { Route, Routes } from "react-router-dom";
import { MyContext } from "../App";
import { pageInfo } from "../constants/constants";
import AccountActivation from "../pages/AccountActivation/AccountActivation";
import DashboardLayout from "../pages/Dashboard/DashboardLayout";
import GroupDetailLayout from "../pages/Group/GroupDetailLayout";
import GroupLayout from "../pages/Group/GroupLayout";
import GroupManagementLayout from "../pages/Group/GroupManagementLayout";
import ITInstructionLayout from "../pages/Instructions/ITInstructionLayout";
import StudentInstructionLayout from "../pages/Instructions/StudentInstructionLayou";
import TeacherInstructionLayout from "../pages/Instructions/TeacherInstructionLayout";
import JoinGroup from "../pages/JoinGroup/JoinGroup";
import Login from "../pages/Login/Login";
import Register from "../pages/Register/Register";
import ResetPassword from "../pages/ResetPassword/ResetPassword";
import ResetPasswordSendEmail from "../pages/ResetPassword/ResetPasswordSendEmail";
import TaskHistory from "../pages/TaskHistory/TaskHistory";
import TaskHistoryTableLayout from "../pages/TaskHistory/TaskHistoryTableLayout";
import UpdateRoleLayout from "../pages/UpdateRole/UpdateRoleLayout";
import UserDetailFetchLayout from "../pages/UserDetail/UserDetailFetchLayout";
import StartTask from "../pages/task/StartTask";
import TasksLayout from "../pages/task/TasksLayout";
import PrivateRoute from "./PrivateRoute";
import PrivateRoutePseudoAdmin from "./PrivateRoutePseudoAdmin";
import PrivateRouteTeacher from "./PrivateRouteTeacher";

function Routing() {
  const {
    refreshStatus,
    setRefreshStatus,
    authenticated,
    setAuthenticated,
    shouldRefresh,
    setShouldRefresh,
  } = useContext(MyContext);
  return (
    <MyContext.Provider
      value={{
        refreshStatus,
        setRefreshStatus,
        authenticated,
        setAuthenticated,
        shouldRefresh,
        setShouldRefresh,
      }}>
      <Routes>
        <Route path={pageInfo.login.url} element={<Login />} />
        <Route path={pageInfo.register.url} element={<Register />} />
        <Route
          path={pageInfo.reset_password_send_email.url}
          element={<ResetPasswordSendEmail />}
        />
        <Route path={pageInfo.activation.url} element={<AccountActivation />} />
        <Route path={pageInfo.reset_password.url} element={<ResetPassword />} />
        <Route element={<PrivateRoute />}>
          <Route path={pageInfo.group.url} element={<GroupLayout />} />
          <Route path={pageInfo.dashboard.url} element={<DashboardLayout />} />
          <Route path={pageInfo.join_group.url} element={<JoinGroup />} />
          <Route path={pageInfo.tasks.url} element={<TasksLayout />} />
          <Route path={pageInfo.start_task.url} element={<StartTask />} />
          <Route path={pageInfo.history_task.url} element={<TaskHistory />} />
          <Route
            path={pageInfo.history.url}
            element={<TaskHistoryTableLayout />}
          />
          <Route
            path={pageInfo.student_instructions.url}
            element={<StudentInstructionLayout />}
          />
          <Route element={<PrivateRouteTeacher />}>
            <Route
              path={pageInfo.user_detail.url}
              element={<UserDetailFetchLayout />}
            />
            <Route
              path={pageInfo.group_management.url}
              element={<GroupManagementLayout />}
            />
            <Route
              path={pageInfo.group_detail.url}
              element={<GroupDetailLayout />}
            />
            <Route
              path={pageInfo.user_history_task.url}
              element={<TaskHistory />}
            />
            <Route
              path={pageInfo.teacher_instructions.url}
              element={<TeacherInstructionLayout />}
            />
          </Route>
          <Route element={<PrivateRoutePseudoAdmin />}>
            <Route
              path={pageInfo.update_role.url}
              element={<UpdateRoleLayout />}
            />
            <Route
              path={pageInfo.it_instructions.url}
              element={<ITInstructionLayout />}
            />
          </Route>
        </Route>
      </Routes>
    </MyContext.Provider>
  );
}

export default Routing;
