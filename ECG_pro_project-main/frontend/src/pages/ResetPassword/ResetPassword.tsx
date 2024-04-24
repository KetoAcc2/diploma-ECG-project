import PasswordIcon from "@mui/icons-material/Password";
import {
  Avatar,
  Box,
  Button,
  Container,
  TextField,
  Typography,
} from "@mui/material";
import { SyntheticEvent, useState } from "react";
import { useTranslation } from "react-i18next";
import { useNavigate, useParams } from "react-router-dom";
import { resetPassword } from "../../api/authApi";
import CircularIndeterminate from "../../components/CircularIndeterminate";
import ErrorMessage from "../../components/ErrorMessage";
import { validatePassword } from "../Register/registrationValidations";
import ResponsiveDialog, {
  DialogType,
} from "../../components/ResponsiveDialog";

const ResetPassword = () => {
  const { resetPasswordToken } = useParams();
  const { t } = useTranslation();
  const nav = useNavigate();
  const [openDialog, setOpenDialog] = useState(false);
  const [dialogText, setDialogText] = useState("");
  const [resetPasswordLoading, setResetPasswordLoading] = useState(false);
  const [password, setPassword] = useState("");
  const [passwordRepeat, setPasswordRepeat] = useState("");
  const [showError, setShowError] = useState(false);
  if (!resetPasswordToken) {
    return <CircularIndeterminate />;
  }
  const resetPasswordHanlder = async (e: SyntheticEvent) => {
    e.preventDefault();
    setShowError(false);
    setResetPasswordLoading(true);
    if (password != passwordRepeat) {
      setShowError(true);
      setResetPasswordLoading(false);
      return;
    }
    if (!validatePassword(password)) {
      setShowError(true);
      setResetPasswordLoading(false);
      return;
    }
    const response = await resetPassword(resetPasswordToken, password);
    if (!response || response.status !== 200) {
      setResetPasswordLoading(false);
      setOpenDialog(true);
      setDialogText("errors.default");
      return;
    }
    setResetPasswordLoading(false);
    setOpenDialog(true);
    setDialogText("success.text");
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
          nav("/");
        }}
      />
      <Container maxWidth="xs">
        <form onSubmit={(e) => resetPasswordHanlder(e)}>
          <Box
            sx={{
              marginTop: 10,
              display: "flex",
              flexDirection: "column",
              alignItems: "center",
            }}>
            <Avatar sx={{ m: 1, bgcolor: "#398CC7" }}>
              <PasswordIcon />
            </Avatar>
            <h1>{t("reset_password.text")}</h1>
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
                onChange={(e) => setPassword(e.target.value)}
                id="demo-helper-text-aligned"
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
              onChange={(e) => setPasswordRepeat(e.target.value)}
              id="demo-helper-text-aligned"
              label={t("repeat_password.text")}
            />
            <Button
              type="submit"
              variant="contained"
              fullWidth
              sx={{ mt: 3, mb: 2 }}
              style={{
                borderRadius: 5,
                backgroundColor: "#398CC7",
                margin: "Calc(1%)",
                padding: "15px 90px",
              }}>
              {resetPasswordLoading ? (
                <CircularIndeterminate />
              ) : (
                t("reset_password.confirm")
              )}
            </Button>
          </Box>
        </form>
        {showError && <ErrorMessage errorMessage="Wrong " />}
      </Container>
    </div>
  );
};

export default ResetPassword;
