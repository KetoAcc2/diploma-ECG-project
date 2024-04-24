import MonitorHeartIcon from "@mui/icons-material/MonitorHeart";
import {
  Avatar,
  Box,
  Button,
  Container,
  Grid,
  TextField,
  Typography,
} from "@mui/material";
import { SyntheticEvent, useState } from "react";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";
import { logIn } from "../../api/authApi";
import CircularIndeterminate from "../../components/CircularIndeterminate";
import ErrorMessage from "../../components/ErrorMessage";
import { localStorageKeys, pageInfo } from "../../constants/constants";
import { ILoginDTO } from "../../interfaces/interface";
import login_formcss from "./Login.module.css";

type credentialsError = {
  credentialNotFound: boolean;
  emailNotFound: boolean;
  passwordNotFound: boolean;
  wrongEmailOrPassword: boolean;
};

const Login = () => {
  const { t } = useTranslation();
  const navigte = useNavigate();
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
      //TODO: navigte(`/user/${json["userData"]["userId"]}`);

      navigte(`${pageInfo.dashboard.url}`);
    } catch (error) {
      setLoginLoading(false);

       
    }
  };
  return (
    <div className={login_formcss.login_form}>
      <Container maxWidth="xs">
        <form action="POST" onSubmit={loginHandler}>
          <Box
            sx={{
              display: "flex",
              flexDirection: "column",
              alignItems: "center",
            }}>
            <h1>{t("ecg.title")}</h1>
            <Avatar sx={{ m: 1, bgcolor: "#398CC7" }}>
              <MonitorHeartIcon />
            </Avatar>
            <h1>{t("welcome.welcome")}</h1>
            <h2>{t("welcome.plslog")}</h2>

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
                id="demo-helper-text-aligned"
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
                id="demo-helper-text-aligned-no-helper"
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
            <Button
              type="submit"
              variant="contained"
              fullWidth
              sx={{ mt: 3, mb: 2, maxHeight: "100px" }}
              style={{
                borderRadius: 5,
                backgroundColor: "#398CC7",
                margin: "Calc(1%)",
                padding: "15px 90px",
              }}>
              {loginLoading ? <CircularIndeterminate /> : t("login.verb")}
            </Button>

            <Grid container>
              <Grid item xs>
                <Button
                  onClick={() => {
                    navigte(`${pageInfo.register.url}`);
                  }}>
                  {t("welcome.register")}
                </Button>
              </Grid>
              <Grid item>
                <Button
                  onClick={() => {
                    navigte(`${pageInfo.reset_password_send_email.url}`);
                  }}>
                  {t("welcome.resetPassword")}
                </Button>
              </Grid>
            </Grid>
          </Box>

          {credentialError.wrongEmailOrPassword && (
            <ErrorMessage errorMessage={t("invalid.emailOrPassword")} />
          )}
        </form>
      </Container>
    </div>
  );
};

export default Login;
