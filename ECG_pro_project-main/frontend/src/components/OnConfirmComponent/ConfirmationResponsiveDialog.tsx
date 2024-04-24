import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import { useTheme } from "@mui/material/styles";
import useMediaQuery from "@mui/material/useMediaQuery";
import * as React from "react";
import { useTranslation } from "react-i18next";

export enum DialogType {
  ALERT = "alert",
  CONFIRM = "confirm",
}

const ConfirmationResponsiveDialog = ({
  isOpen,
  centerText,
  onConfirm = () => {},
  onCancel = () => {},
  cancelText = "questions_conf.no",
  submitText = "questions_conf.yes",
}: {
  isOpen: boolean;
  centerText: string;
  onConfirm?: React.MouseEventHandler<HTMLButtonElement>;
  onCancel?: React.MouseEventHandler<HTMLButtonElement>;
  cancelText?: string;
  submitText?: string;
}) => {
  const { t } = useTranslation();
  const theme = useTheme();
  const fullScreen = useMediaQuery(theme.breakpoints.down("md"));

  return (
    <div>
      <Dialog
        fullScreen={fullScreen}
        open={isOpen}
        aria-labelledby="responsive-dialog-title">
        <>
          <DialogContent style={{ width: "300px" }}>
            <DialogContentText style={{ color: "black" }}>
              {t(centerText)}
            </DialogContentText>
          </DialogContent>
          <DialogActions>
            <Button
              onClick={onConfirm}
              variant="contained"
              color="error"
              autoFocus>
              {t(submitText)}
            </Button>
            <Button
              variant="contained"
              color="primary"
              autoFocus
              onClick={onCancel}>
              {t(cancelText)}
            </Button>
          </DialogActions>
        </>
      </Dialog>
    </div>
  );
};

export default ConfirmationResponsiveDialog;
