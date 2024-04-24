import { useTheme } from "@mui/material/styles";
import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  TextField,
  Typography,
  useMediaQuery,
} from "@mui/material";
import { useTranslation } from "react-i18next";
import { DialogType } from "./ResponsiveDialog";
import { useState, SyntheticEvent, useContext } from "react";
import { logIn } from "../api/authApi";
import { localStorageKeys, pageInfo } from "../constants/constants";
import { ILoginDTO } from "../interfaces/interface";
import CircularIndeterminate from "./CircularIndeterminate";
import { MyContext } from "../App";

type credentialsError = {
  credentialNotFound: boolean;
  emailNotFound: boolean;
  passwordNotFound: boolean;
  wrongEmailOrPassword: boolean;
};

const ReauthenticateDialog = ({
  isOpen,
  setIsOpen,
  setIsAuthenticated,
}: {
  isOpen: boolean;
  setIsOpen: React.Dispatch<React.SetStateAction<boolean>>;
  setIsAuthenticated: React.Dispatch<React.SetStateAction<boolean>>;
}) => {
  const { t } = useTranslation();
  const theme = useTheme();
  const { setRefreshStatus,setShouldRefresh } = useContext(MyContext);
  const fullScreen = useMediaQuery(theme.breakpoints.down("md"));
  const [loginLoading, setLoginLoading] = useState(false);
  const [credentials, setCredentials] = useState<ILoginDTO>({
    email: "",
    password: "",
  });
  const [credentialError, setCredentialError] = useState<credentialsError>({
    credentialNotFound: false,
    emailNotFound: false,
    passwordNotFound: false,
    wrongEmailOrPassword: false,
  });

  const loginHandler = async (e: SyntheticEvent) => {
    e.preventDefault();
    setLoginLoading(true);
    let credentialNotFound: boolean = false;
    let emailNotFound: boolean = false;
    let passNotFound: boolean = false;
    if (credentials === undefined || credentials === null) {
       
      credentialNotFound = true;
    }
    if (
      credentials?.email === undefined ||
      credentials.email === null ||
      credentials.email.length < 1
    ) {
      emailNotFound = true;
    }
    if (
      credentials?.password === undefined ||
      credentials.password === null ||
      credentials.password.length < 1
    ) {
      passNotFound = true;
    }
    setCredentialError((prev) => ({
      ...prev,
      credentialNotFound: credentialNotFound,
    }));
    setCredentialError((prev) => ({ ...prev, passwordNotFound: passNotFound }));
    setCredentialError((prev) => ({ ...prev, emailNotFound: emailNotFound }));
    if (credentialNotFound || emailNotFound || passNotFound) {
       
       
       
       
      return;
    }

    try {
      const response = await logIn(credentials);

      if (response === null || response === undefined) {
        setCredentialError((prev) => ({ ...prev, wrongEmailOrPassword: true }));
        throw new Error("Oops, non-existing response");
        //TODO: please try again message on the screen perhaps?
      }
      if (response.status !== 200) {
        setCredentialError((prev) => ({ ...prev, wrongEmailOrPassword: true }));
        throw new Error("Unauthorized/not found user/faulty");
        //TODO: please try again messgage or something
      }
      setCredentialError((prev) => ({ ...prev, wrongEmailOrPassword: false }));
      const json = await response.json();
      localStorage.setItem(
        localStorageKeys.USER_PERSISTANCE,
        JSON.stringify(json)
      );
      setLoginLoading(false);
      setIsAuthenticated(true);
      setRefreshStatus(false)
      setIsOpen(false);
      //TODO: navigte(`/user/${json["userData"]["userId"]}`);
    } catch (error) {
       
    }
  };

  return (
    <div>
      <Dialog
        fullScreen={fullScreen}
        open={isOpen}
        aria-labelledby="responsive-dialog-title">
        <DialogTitle>
          <DialogContent>
            <DialogContentText>{t("reauthenticate.text")}</DialogContentText>
          </DialogContent>
        </DialogTitle>
        <DialogContent style={{ width: "300px" }}>
          <DialogContentText style={{ color: "black" }}>
            {t("reauthenticate_detail.text")}
          </DialogContentText>
        </DialogContent>
        <DialogContent style={{ width: "300px" }}>
          <div
            style={{
              width: "100%",
              display: "flex",
              alignItems: "start",
              flexDirection: "column",
            }}>
            <TextField
              style={{
                borderRadius: 5,
                backgroundColor: "#FFFFFFBB",
                margin: "Calc(1%)",
              }}
              margin="normal"
              required
              fullWidth
              autoFocus
              onChange={(e) =>
                setCredentials((prev) => ({ ...prev, email: e.target.value }))
              }
              helperText={credentialError.emailNotFound && t("invalid.email")}
              id="reauthenticate-login"
              label={t("email.text")}
              error={credentialError.emailNotFound ? true : false}
            />
          </div>
          <div
            style={{
              width: "100%",
              display: "flex",
              alignItems: "start",
              flexDirection: "column",
            }}>
            <TextField
              type="password"
              style={{
                borderRadius: 5,
                backgroundColor: "#FFFFFFBB",
                margin: "Calc(1%)",
              }}
              margin="normal"
              required
              fullWidth
              onChange={(e) =>
                setCredentials((prev) => ({
                  ...prev,
                  password: e.target.value,
                }))
              }
              helperText={
                credentialError.passwordNotFound && t("invalid.password")
              }
              id="reauthenticate-password"
              label={t("password.text")}
            />
            <Typography
              style={{
                fontSize: "12px",
                marginLeft: "12px",
                color: "#636363",
                textAlign: "left",
              }}>
              {t("password_strength.text")}
            </Typography>
          </div>
        </DialogContent>
        <DialogActions>
          <Button
            disabled={loginLoading}
            variant="contained"
            autoFocus
            onClick={loginHandler}>
            {loginLoading ? <CircularIndeterminate /> : t("login.verb")}
          </Button>
        </DialogActions>
      </Dialog>
    </div>
  );
};

export default ReauthenticateDialog;
