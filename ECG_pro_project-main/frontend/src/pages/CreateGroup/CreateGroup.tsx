import { Box, Button, Paper, TextField, Typography } from "@mui/material";
import React, { useState } from "react";
import { useTranslation } from "react-i18next";
import { useQueryClient } from "react-query";
import { createGroupAsync } from "../../api/api";
import ResponsiveDialog, {
  DialogType,
} from "../../components/ResponsiveDialog";

const CreateGroup = () => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();
  const [valueGroupName, setValueGroupName] = React.useState("");
  const [valueGroupDescription, setValueGroupDescription] = React.useState("");
  const [openDialog, setOpenDialog] = useState(false);
  const [centerText, setCenterText] = useState<string>("group_created.text");
  const handleChangeGroupName = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    setValueGroupName(event.target.value);
  };
  const handleChangeGroupDescription = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    setValueGroupDescription(event.target.value);
  };
  const handleCreateGroup = async () => {
    if (valueGroupName && valueGroupName.trim().length > 0) {
      const response = await createGroupAsync(valueGroupName.trim());
      if (!response) {
        return;
      }
      if (response.status === 200) {
        setCenterText("group_created.text");
        setOpenDialog(true);
      }
      queryClient.refetchQueries("groupManagementInfo");
    } else {
      setCenterText("group_created.error");
      setOpenDialog(true);
    }
  };

  return (
    <div>
      <ResponsiveDialog
        isOpen={openDialog}
        setIsOpen={setOpenDialog}
        centerText={centerText}
        dialogType={DialogType.ALERT}
      />
      <Paper>
        <Typography style={{ marginTop: "12px" }}>
          {t("enter_group_name.text")}
        </Typography>
        <TextField
          sx={{ width: "50%", margin: "10px" }}
          id="filled-multiline-flexible"
          label={t("group_name.text")}
          multiline
          maxRows={1}
          value={valueGroupName}
          onChange={handleChangeGroupName}
          variant="filled"
        />
        <br />
        <Button
          variant="contained"
          style={{ margin: "12px" }}
          onClick={handleCreateGroup}>
          {t("create_group.text")}
        </Button>
      </Paper>
    </div>
  );
};

export default CreateGroup;
