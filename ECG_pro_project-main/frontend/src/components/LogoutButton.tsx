import { Button, Typography } from "@mui/material";
import { t } from "i18next";
import { useNavigate } from "react-router-dom";
import { logOut } from "../api/authApi";
import { localStorageKeys } from "../constants/constants";
import LogoutIcon from "@mui/icons-material/Logout";

const LogoutButton = () => {
  const navigate = useNavigate();
  const logoutHandler = async () => {
    await logOut();
    const userData = localStorage.getItem(localStorageKeys.USER_PERSISTANCE);
     
    navigate("/");
  };
  return (
    <Button
      onClick={logoutHandler}
      variant="outlined"
      size="medium"
      endIcon={<LogoutIcon />}
      style={{
        borderRadius: 30,
        backgroundColor: "#FFFFFF",
        margin: "Calc(1%)",
      }}
    >
      <Typography style={{ color: "#398BC7", fontSize: "12px" }}>
        {t("logout.text")}
      </Typography>
    </Button>
  );
};

export default LogoutButton;
