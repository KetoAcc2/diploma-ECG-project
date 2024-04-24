import PasswordIcon from "@mui/icons-material/Password";
import { Avatar, Box, Button, Container, Grid, TextField } from "@mui/material";
import { SyntheticEvent, useState } from "react";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";
import { resetPasswordSendEmail } from "../../api/authApi";
import CircularIndeterminate from "../../components/CircularIndeterminate";
import { pageInfo } from "../../constants/constants";
import ResponsiveDialog, {
  DialogType,
} from "../../components/ResponsiveDialog";

const ResetPasswordSendEmail = () => {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const [openDialog, setOpenDialog] = useState(false);
  const [dialogText, setDialogText] = useState("");
  const [sendEmailLoading, setSendEmailLoading] = useState(false);
  const [email, setEmail] = useState("");
  const sendEmailHandler = async (e: SyntheticEvent) => {
    setSendEmailLoading(true);
    e.preventDefault();
    const response = await resetPasswordSendEmail(email);
    if (!response || response.status !== 200) {
      setSendEmailLoading(false);
      setOpenDialog(true);
      setDialogText("errors.default");
      return;
    }
    setSendEmailLoading(false);
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
          navigate("/");
        }}
      />
      <Container maxWidth="xs">
        <form onSubmit={(e) => sendEmailHandler(e)}>
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
            <h2>{t("reset_password.email")}</h2>

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
              onChange={(e) => setEmail(e.target.value)}
              id="demo-helper-text-aligned"
              label={t("email.text")}
            />

            <Button
              type="submit"
              variant="contained"
              disabled={sendEmailLoading}
              onClick={sendEmailHandler}
              fullWidth
              sx={{ mt: 3, mb: 2 }}
              style={{
                borderRadius: 5,
                backgroundColor: "#398CC7",
                margin: "Calc(1%)",
                padding: "15px 90px",
              }}>
              {sendEmailLoading ? (
                <CircularIndeterminate />
              ) : (
                t("reset_password.confirm")
              )}
            </Button>

            <Grid container>
              <Grid item xs>
                <Button
                  onClick={() => {
                    navigate(`${pageInfo.login.url}`);
                  }}>
                  {t("reset_password.goback")}
                </Button>
              </Grid>
            </Grid>
          </Box>
        </form>
      </Container>
    </div>
  );
};

export default ResetPasswordSendEmail;
