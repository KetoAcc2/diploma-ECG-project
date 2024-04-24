import { Box, Button, Dialog, DialogActions, Paper } from "@mui/material";
import { useTranslation } from "react-i18next";
import useGetPic from "../queries/useGetPic";
import CircularIndeterminate from "./CircularIndeterminate";
import ErrorMessage from "./ErrorMessage";

const LoadPic = ({ taskId, open, handleToggleImage }: { taskId: number; open: boolean; handleToggleImage: any }) => {
  const { t } = useTranslation();
  const { pic, picIsError, picInfoLoading } = useGetPic(taskId);
  if (!pic || picInfoLoading) {
    return <CircularIndeterminate />;
  }
  if (picIsError) {
    return <ErrorMessage />;
  }

  const imageSrc = `data:image/png;base64,${pic}`;

  return (
    <>
      <Box display="flex" justifyContent="center" alignItems="center">
        <Button variant="contained" onClick={() => handleToggleImage(true)}>
          {t("ecg.open_fullscreen")}
        </Button>
      </Box>
      <div style={{ width: "100%", height: "100%" }}>
        <img src={imageSrc} alt="ECG Diagram" onClick={() => handleToggleImage(true)} style={{ maxWidth: "70%", maxHeight: "70%", objectFit: "contain" }} />
        <Dialog open={open} onClose={() => handleToggleImage(false)} fullScreen maxWidth="xl" style={{ maxWidth: "80%", maxHeight: "80%", margin: "auto" }}>
          <DialogActions style={{ maxHeight: "10%" }}>
            <Button variant="contained" onClick={() => handleToggleImage(false)}>
              {t("ecg.close")}
            </Button>
          </DialogActions>

          <img src={imageSrc} style={{ width: "90%", height: "auto", margin: "auto", objectFit: "contain" }} />
        </Dialog>
      </div>
    </>
  );
};

export default LoadPic;
