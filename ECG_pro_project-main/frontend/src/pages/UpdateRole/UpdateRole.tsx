import {
  Button,
  FormControl,
  MenuItem,
  Select,
  TextField,
} from "@mui/material";
import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { getUserRole, updateRole } from "../../api/api";
import { userRole, userRoleString } from "../../constants/constants";
import ResponsiveDialog, {
  DialogType,
} from "../../components/ResponsiveDialog";

const UpdateRole = () => {
  const { t } = useTranslation();
  const [newRole, setNewRole] = useState(userRole.STUDENT);
  const [email, setEmail] = useState("");
  const [checkedUserRole, setCheckedUserRole] = useState("");
  const [hideCurrentRole, setHideCurrentRole] = useState(true);
  const [openDialog, setOpenDialog] = useState(false);
  const [dialogText, setDialogText] = useState("");
  const handleSubmit = async () => {
    let usrRole = "";
    switch (newRole) {
      case userRole.STUDENT:
        usrRole = userRoleString.STUDENT;
        break;
      case userRole.TEACHER:
        usrRole = userRoleString.TEACHER;
        break;
      default:
        setDialogText("errors.default");
        setOpenDialog(true);
        return;
    }
    const response = await updateRole(email, usrRole);
    if (response) {
      if (response.status === 200) {
        setCheckedUserRole(usrRole);
        setDialogText("success.text");
        setOpenDialog(true);
      } else if (response.status === 404) {
        setDialogText("no_such_user_email.text");
        setOpenDialog(true);
      } else {
        setDialogText("errors.default");
        setOpenDialog(true);
      }
    } else {
      setDialogText("errors.default");
      setOpenDialog(true);
    }

    return;
  };

  const getUserOldRole = async () => {
    if (email) {
      const response = await getUserRole(email);
      if (response) {
        if (response.status === 200) {
          setCheckedUserRole((await response.json())["result"]);
          setDialogText("success.text");
          setOpenDialog(true);
        } else if (response.status === 404) {
          setDialogText("no_such_user_email.text");
          setOpenDialog(true);
        } else {
          setDialogText("errors.default");
          setOpenDialog(true);
        }
      } else {
        setDialogText("errors.default");
        setOpenDialog(true);
      }
    }
  };

  useEffect(() => {
    setHideCurrentRole(checkedUserRole === "");
  }, [checkedUserRole]);

  return (
    <div>
      <ResponsiveDialog
        isOpen={openDialog}
        setIsOpen={setOpenDialog}
        centerText={dialogText}
        dialogType={DialogType.ALERT}
      />
      <div
        style={{
          flex: 1,
          display: "flex",
          flexDirection: "row",
          justifyContent: "center",
          alignItems: "center",
        }}>
        <div style={{ marginRight: "20px" }}>
          <TextField
            label={t("email.text")}
            id="filled-hidden-label-normal"
            variant="filled"
            onChange={(e) => {
              setEmail(e.target.value);
              setCheckedUserRole("");
            }}
          />
        </div>
        <div>
          <Button variant="contained" onClick={getUserOldRole}>
            {t("check_role.text")}
          </Button>
        </div>
      </div>
      <div
        style={{
          flex: 1,
          display: "flex",
          flexDirection: "row",
          justifyContent: "center",
          alignItems: "center",
        }}>
        <div style={{ marginRight: "20px" }} hidden={hideCurrentRole}>
          <h3>Current Role:</h3>
        </div>
        <div hidden={hideCurrentRole}>
          <h3>{checkedUserRole}</h3>
        </div>
      </div>
      <br />
      <div style={{ flex: 1 }}>
        <FormControl>
          <Select
            style={{ width: "200px" }}
            id="question2-select-label"
            value={newRole}
            inputProps={{ "aria-label": "Without label" }}
            onChange={(e) => setNewRole(+e.target.value)}>
            <MenuItem value={userRole.STUDENT}>{t("role.student")}</MenuItem>
            <MenuItem value={userRole.TEACHER}>{t("role.teacher")}</MenuItem>
          </Select>
        </FormControl>
      </div>
      <br />
      <div>
        <Button onClick={handleSubmit}>{t("submit.text")}</Button>
      </div>
    </div>
  );
};

export default UpdateRole;
