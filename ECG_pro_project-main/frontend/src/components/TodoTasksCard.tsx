import { Divider, Paper, Tooltip, Typography } from "@mui/material";
import { useTranslation } from "react-i18next";
import { CardDivStyle } from "../interfaces/interface";
import { useGetTasks } from "../queries/useGetTasks";
import TodoTasks from "./TodoTasks";

const TodoTasksCard = ({ style }: CardDivStyle) => {
  const { tasks, tasksLoading, tasksIsError } = useGetTasks();
  const { t } = useTranslation();
  if (!tasks || tasksLoading) {
    return (
      <div style={style}>
        <Paper style={{ width: "100%", padding: "10px" }}>
          <Divider />
          <div>{t("loading.text")}</div>
        </Paper>
      </div>
    );
  }
  return (
    <div style={style}>
      <Paper style={{ width: "100%", padding: "10px" }}>
        <Typography variant="h6" style={{ color: "#636363" }}>
          {t("tasks_to_do.text")}
        </Typography>
        <Divider />
        <div
          style={{
            textAlign: "left",
            padding: "5px 20px 5px 20px",
            marginTop: "5px",
            borderRadius: "5px",
            textOverflow: "ellipsis",
            maxHeight:'393px',
            minHeight:'393px',
            overflowY:'scroll'
          }}
        >
          <TodoTasks
            tasks={tasks}
            tasksLoading={tasksLoading}
            tasksIsError={tasksIsError}
          />
        </div>
      </Paper>
    </div>
  );
};

export default TodoTasksCard;
