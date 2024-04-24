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

const ResponsiveDialog = ({
  isOpen,
  setIsOpen,
  centerText,
  dialogType,
  onConfirm = () => {
    setIsOpen(false);
  },
  cancelText = "questions_conf.no",
  submitText = "questions_conf.yes",
}: {
  isOpen: boolean;
  setIsOpen: React.Dispatch<React.SetStateAction<boolean>>;
  centerText: string;
  dialogType: DialogType;
  onConfirm?: React.MouseEventHandler<HTMLButtonElement>;
  cancelText?: string;
  submitText?: string;
}) => {
  const { t } = useTranslation();
  const theme = useTheme();
  const fullScreen = useMediaQuery(theme.breakpoints.down("md"));

  const okayText = "OK";

  const handleClose = () => {
    setIsOpen(false);
  };

  return (
    <div>
      <Dialog
        fullScreen={fullScreen}
        open={isOpen}
        aria-labelledby="responsive-dialog-title">

        {dialogType === DialogType.CONFIRM ? (
          <>
            <DialogContent style={{ width: "300px" }}>
              <DialogContentText style={{ color: "black" }}>
                {t(centerText)}
              </DialogContentText>
            </DialogContent>
            <DialogActions>
              <Button autoFocus onClick={handleClose}>
                {t(cancelText)}
              </Button>
              <Button onClick={onConfirm} autoFocus>
                {t(submitText)}
              </Button>
            </DialogActions>
          </>
        ) : (
          <>
            <DialogContent style={{ width: "300px", minHeight: "50px" }}>
              <DialogContentText
                style={{ color: "black", textAlign: "center" }}>
                {t(centerText)}
              </DialogContentText>
            </DialogContent>
            <DialogActions style={{ margin: "auto" }}>
              <Button variant="contained" autoFocus onClick={onConfirm}>
                {t(okayText)}
              </Button>
            </DialogActions>
          </>
        )}
      </Dialog>
    </div>
  );
};

export default ResponsiveDialog;
