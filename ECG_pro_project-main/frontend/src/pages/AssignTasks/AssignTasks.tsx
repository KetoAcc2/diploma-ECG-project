import { Button, Paper, Typography } from "@mui/material";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import { useQueryClient } from "react-query";
import { assignTask } from "../../api/api";
import CircularIndeterminate from "../../components/CircularIndeterminate";
import ErrorMessage from "../../components/ErrorMessage";
import ResponsiveDialog, { DialogType } from "../../components/ResponsiveDialog";
import { AssignTasksForm } from "../../interfaces/interface";
import useGetTaskTypes from "../../queries/useGetTaskTypes";
import TaskDescriptionForm from "./TaskDescriptionForm";
import TaskTypeForm from "./TaskTypeForm";

const AssignTasks = (props: { id: number }) => {
  const { id } = props;
  const { t } = useTranslation();
  const [submitLoading, setSubmitLoading] = useState(false);
  const [form, setForm] = useState({ groups: [id] } as AssignTasksForm);
  const { taskType, taskTypeIsError, taskTypeLoading } = useGetTaskTypes();
  const [openDialog, setOpenDialog] = useState(false);
  const [centerText, setCenterText] = useState<string>("");
  const queryClient = useQueryClient();

  const submitHandler = async () => {
    setSubmitLoading(true);
    console.log(form);
    if (form.taskDescription && form.taskDescription.trim().length > 0) {
      const success = await assignTask(form);
      if (success) {
        setCenterText("assign_tasks.success");
      } else {
        setCenterText("errors.default");
      }
    } else {
      setCenterText("assign_tasks.error");
    }
    setSubmitLoading(false);
    setOpenDialog(true);
    queryClient.refetchQueries("tasksInfoByGroup");

  };
  if (!taskType || taskTypeLoading) {
    return <CircularIndeterminate />;
  }
  if (taskTypeIsError) {
    return <ErrorMessage />;
  }
  return (
    <div>
      <ResponsiveDialog
        isOpen={openDialog}
        setIsOpen={setOpenDialog}
        centerText={centerText}
        dialogType={DialogType.ALERT}
      />
      <div style={{ marginBottom: "5%" }}>
        <Typography style={{ color: "#398BC7", fontSize: "20px", marginBottom: "5%" }}>{`${t("assign_tasks.text")}`}</Typography>
        <Paper
          style={{
            height: "50vh",
            maxHeight: "45.5vh",
            overflow: "auto",
            display: "inline-block",
            maxWidth: "900px",
          }}>
          <TaskTypeForm data={!taskType ? [] : taskType} setForm={setForm} />
        </Paper>
      </div>

      <Paper>
        <TaskDescriptionForm setForm={setForm} />
        <Button style={{ marginBottom: "12px" }} variant="contained" onClick={submitHandler}>
          {submitLoading ? <CircularIndeterminate /> : t("submit.text")}
        </Button>
      </Paper>
    </div>
  );
};

export default AssignTasks;
