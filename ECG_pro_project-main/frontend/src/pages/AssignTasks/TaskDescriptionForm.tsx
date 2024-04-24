import { TextField, Typography } from "@mui/material";
import React from "react";
import { useTranslation } from "react-i18next";
import { AssignTasksForm } from "../../interfaces/interface";

const TaskDescriptionForm = (props: {
  setForm: React.Dispatch<React.SetStateAction<AssignTasksForm>>;
}) => {
  const { setForm } = props;
  const { t } = useTranslation();

  const handleTaskDescription = (
    event: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>
  ) => {
     
    setForm((prev: AssignTasksForm) => ({
      ...prev,
      taskDescription: event.target.value,
    }));
  };

  return (
    <div>
      <Typography style={{margin:"12px 12px 0px 12px", textAlign:"center"}}>{t("enter_task_name.text")}</Typography>
      <TextField
        margin="normal"
        variant="filled"
        label={t("assign_tasks.task_name")}
        onChange={handleTaskDescription}
      ></TextField>
    </div>
  );
};

export default TaskDescriptionForm;
