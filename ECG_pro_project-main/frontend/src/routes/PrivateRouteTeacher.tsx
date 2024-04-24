import { Navigate, Outlet, useNavigate } from "react-router-dom";
import {
  userRole
} from "../constants/constants";
import { getUserData } from "../constants/reactTemplates";

function PrivateRouteTeacher() {
  const navigate = useNavigate();
  const isTeacher = () => {
    const isTeacherTmp = getUserData();
    if (isTeacherTmp !== userRole.TEACHER) {
      return false;
    }
    return true;
  };
  return isTeacher() ? (
    <>
      <Outlet />
    </>
  ) : (
    <Navigate to="/dashboard" />
  );
}

export default PrivateRouteTeacher;
