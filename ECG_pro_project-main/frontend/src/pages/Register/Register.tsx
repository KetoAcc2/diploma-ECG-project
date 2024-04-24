import AppRegistrationIcon from "@mui/icons-material/AppRegistration";
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
import { register } from "../../api/authApi";
import CircularIndeterminate from "../../components/CircularIndeterminate";
import { pageInfo } from "../../constants/constants";
import { IRegistrationDTO } from "../../interfaces/interface";
import registrationValidation from "./registrationValidations";
import ResponsiveDialog, {
  DialogType,
} from "../../components/ResponsiveDialog";

type registerCredentialsError = {
  credentialNotFound: boolean;
  emailNotFound: boolean;
  passwordNotFound: boolean;
  passwordConfirmationNotFound: boolean;
  wrongPasswordConfirmation: boolean;
  invalidEmail: boolean;
  invalidPassword: boolean;
};

const Register = () => {
  const { t } = useTranslation();
  const navigte = useNavigate();
  const [openDialog, setOpenDialog] = useState(false);
  const [dialogText, setDialogText] = useState("");
  const [registerLoading, setRegisterLoading] = useState(false);
  const [credentials, setCredentials] = useState<IRegistrationDTO>({
    email: "",
    password: "",
    passwordConfirmation: "",
  });
  const [credentialError, setCredentialError] =
    useState<registerCredentialsError>({
      credentialNotFound: false,
      emailNotFound: false,
      passwordNotFound: false,
      passwordConfirmationNotFound: false,
      wrongPasswordConfirmation: false,
      invalidEmail: false,
      invalidPassword: false,
    });

  const registerHandler = async (e: SyntheticEvent) => {
    setRegisterLoading(true);
    e.preventDefault();
    let credentialNotFound: boolean = false;
    let emailNotFound: boolean = false;
    let passNotFound: boolean = false;
    let passwordConfirmationNotFound: boolean = false;
    let wrongPasswordConfirmation: boolean = false;
    if (!credentials) {
       
      credentialNotFound = true;
    }
    if (credentials?.email?.length < 1) {
      emailNotFound = true;
    }
    if (credentials?.password?.length < 1) {
      passNotFound = true;
    }
    if (credentials?.passwordConfirmation?.length < 1) {
      passwordConfirmationNotFound = true;
    }
    if (credentials.password != credentials.passwordConfirmation) {
      wrongPasswordConfirmation = true;
    }
    setCredentialError((prev) => ({
      ...prev,
      wrongPasswordConfirmation: wrongPasswordConfirmation,
    }));
    setCredentialError((prev) => ({
      ...prev,
      credentialNotFound: credentialNotFound,
    }));
    setCredentialError((prev) => ({ ...prev, passwordNotFound: passNotFound }));
    setCredentialError((prev) => ({ ...prev, emailNotFound: emailNotFound }));
    setCredentialError((prev) => ({
      ...prev,
      passwordConfirmationNotFound: passwordConfirmationNotFound,
    }));

    if (
      credentialNotFound ||
      emailNotFound ||
      passNotFound ||
      passwordConfirmationNotFound ||
      wrongPasswordConfirmation
    ) {
      setRegisterLoading(false);
      return;
    }
    const { invalidEmail, invalidPassword } = registrationValidation(
      credentials.email,
      credentials.password
    );
    setCredentialError((prev) => ({ ...prev, invalidEmail: invalidEmail }));
    setCredentialError((prev) => ({
      ...prev,
      invalidPassword: invalidPassword,
    }));
    if (invalidEmail || invalidPassword) {
      setRegisterLoading(false);
      return;
    }
    try {
      const response = await register(credentials);

      if (response?.status != 200) {
        setDialogText("registration_went_wrong.text");
        setOpenDialog(true);
        throw new Error("Registration failed");
      }
      //TODO: create success info
      setRegisterLoading(false);
      setDialogText("registration_successful.text");
      setOpenDialog(true);
    } catch (error) {
      setRegisterLoading(false);
    }
  };

  return (
    <div>
      <ResponsiveDialog
        isOpen={openDialog}
        setIsOpen={setOpenDialog}
        centerText={dialogText}
        dialogType={DialogType.ALERT}
        onConfirm={() => {
          setOpenDialog(false);
          navigte("/");
        }}
      />
      <Container maxWidth="xs">
        <form action="POST" onSubmit={registerHandler}>
          <Box
            sx={{
              marginTop: 5,
              display: "flex",
              flexDirection: "column",
              alignItems: "center",
            }}>
            <h1>{t("ecg.title")}</h1>
            <Avatar sx={{ m: 1, bgcolor: "#398CC7" }}>
              <AppRegistrationIcon />
            </Avatar>
            <h1>{t("register.register")}</h1>

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
              helperText={
                credentialError.emailNotFound
                  ? t("invalid.email")
                  : credentialError.invalidEmail && t("invalid_email.text")
              }
              id="demo-helper-text-aligned"
              label={t("email.text")}
              error={
                credentialError.emailNotFound
                  ? true
                  : credentialError.invalidEmail
                  ? true
                  : false
              }
            />
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
                onChange={(e) =>
                  setCredentials((prev) => ({
                    ...prev,
                    password: e.target.value,
                  }))
                }
                helperText={
                  credentialError.passwordNotFound
                    ? t("invalid.password")
                    : credentialError.invalidPassword &&
                      t("password_strength.text")
                }
                id="demo-helper-text-aligned-no-helper"
                label={t("password.text")}
                error={
                  credentialError.passwordNotFound
                    ? true
                    : credentialError.invalidPassword
                    ? true
                    : false
                }
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

            <TextField
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
                  passwordConfirmation: e.target.value,
                }))
              }
              helperText={
                credentialError.passwordConfirmationNotFound
                  ? t("invalid.password")
                  : credentialError.wrongPasswordConfirmation &&
                    t("password_not_same.text")
              }
              id="demo-helper-text-aligned-no-helper"
              label={t("repeat_password.text")}
              error={
                credentialError.passwordConfirmationNotFound
                  ? true
                  : credentialError.wrongPasswordConfirmation
                  ? true
                  : false
              }
            />

            <Button
              type="submit"
              variant="contained"
              disabled={registerLoading}
              fullWidth
              sx={{ mt: 3, mb: 2 }}
              style={{
                borderRadius: 5,
                backgroundColor: "#398CC7",
                margin: "Calc(1%)",
                padding: "15px 90px",
              }}>
              {registerLoading ? (
                <CircularIndeterminate />
              ) : (
                t("register.confirm")
              )}
            </Button>

            <Grid container>
              <Grid item xs>
                <Button
                  onClick={() => {
                    navigte(`${pageInfo.login.url}`);
                  }}>
                  {t("register.goback")}
                </Button>
              </Grid>
            </Grid>
          </Box>
        </form>
      </Container>
    </div>
  );
};

export default Register;
