import { Box, Button, TextField, Typography } from "@mui/material";
import { SetStateAction, useState } from "react";
import { useTranslation } from "react-i18next";
import { useQueryClient } from "react-query";
import { joinGroupAsync } from "../../api/api";
import ResponsiveDialog, {
  DialogType,
} from "../../components/ResponsiveDialog";

const JoinGroup = () => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();
  const [openDialog, setOpenDialog] = useState<boolean>(false);
  const [joinGroupInfo, setJoinGroupInfo] = useState<string>("");
  const [groupCode, setGroupCode] = useState("");
  const handleChangeGroupCode = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    setGroupCode(event.target.value);
  };
  const handleJoinGroup = async () => {
    const response = await joinGroupAsync(groupCode);
     
    if (!response) {
      setJoinGroupInfo("response.bad_response");
      return;
    }
    if (response.status === 200) {
      setOpenDialog(true);
      setJoinGroupInfo("joined_group.text");
    }
    if (response.status === 304) {
      setOpenDialog(true);
      setJoinGroupInfo("already_joined.text");
    }
    queryClient.refetchQueries("groupInfo");
  };
  return (
    <div>
      <ResponsiveDialog
        isOpen={openDialog}
        setIsOpen={setOpenDialog}
        centerText={joinGroupInfo}
        dialogType={DialogType.ALERT}
        submitText="ok.text"
      />
      <Box
        style={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
        }}>
        <Typography>{t("enter_group_code.text")}</Typography>
        <TextField
          style={{ marginTop: "10px" }}
          sx={{ width: "50%" }}
          id="filled-multiline-flexible"
          label={t("group_code.text")}
          multiline
          maxRows={4}
          value={groupCode}
          onChange={handleChangeGroupCode}
          variant="filled"
        />
        <br />
        <div>
          <Button
            variant="contained"
            style={{ marginBottom: "12px" }}
            onClick={handleJoinGroup}>
            {t("join_group.text")}
          </Button>
        </div>
      </Box>
    </div>
  );
};

export default JoinGroup;
