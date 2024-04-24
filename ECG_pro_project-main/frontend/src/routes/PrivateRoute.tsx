import { useContext, useEffect, useState } from "react";
import { Navigate, Outlet, useNavigate } from "react-router-dom";
import { renewAccessToken, verifyTokenExpiration } from "../api/api";
import SidebarNav from "../components/SidebarNav";
import { localStorageKeys, tokenStatus } from "../constants/constants";
import { MyContext } from "../App";
import ReauthenticateDialog from "../components/ReauthenticateDialog";

function PrivateRoute() {
  const isStartTaskOrHistoryTask = (str: string): boolean => {
    const regexP = new RegExp(
      "^/(starttask|history/[1-9]+[0-9]*)|(/history/[1-9]+[0-9]*/[1-9]+[0-9]*)$"
    );
    return regexP.test(str);
  };
  const navigate = useNavigate();
  const { setRefreshStatus,shouldRefresh } = useContext(MyContext);
  const [isAuthenticated, setIsAuthenticated] = useState(true);

  const [openDialog, setOpenDialog] = useState(false);

  const dialogHandler = (res: number) => {
    switch (res) {
      case tokenStatus.VALID: {
        setRefreshStatus(false);
         
        setIsAuthenticated(true);
        break;
      }
      case tokenStatus.RELOG: {
        setOpenDialog(true);
         
        setIsAuthenticated(true);
        break;
      }
      case tokenStatus.INVALID: {
        setRefreshStatus(false);
         
        setIsAuthenticated(false);
        break;
      }
      default: {
        setIsAuthenticated(false);
        break;
      }
    }
  };

  useEffect(() => {
    const innerAsync = async () => {
      setRefreshStatus(true);
      const res = await renewAccessToken();
      setRefreshStatus(false);
      dialogHandler(res);
    };
    innerAsync();
    const intervalId = setInterval(async () => {
      setRefreshStatus(true);
      const res = await renewAccessToken();
      dialogHandler(res);
    }, 60000*120); // refresh token every 2 hours
    const userData = localStorage.getItem(localStorageKeys.USER_PERSISTANCE);
    if (!userData) {
       
      return () => {
        clearInterval(intervalId);
        navigate("/");
      };
    }
    return () => {
      clearInterval(intervalId);
      navigate("/");
    };
  }, []);
  return (
    <>
      {isAuthenticated ? (
        <>
          <ReauthenticateDialog
            isOpen={openDialog}
            setIsOpen={setOpenDialog}
            setIsAuthenticated={setIsAuthenticated}
          />
          {!isStartTaskOrHistoryTask(window.location.pathname) ? (
            <SidebarNav />
          ) : (
            <></>
          )}

          <Outlet />
        </>
      ) : (
        <Navigate to="/" />
      )}
    </>
  );
}

export default PrivateRoute;
