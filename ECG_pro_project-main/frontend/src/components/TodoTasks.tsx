import { Tooltip } from "@mui/material";
import { t } from "i18next";
import CircularIndeterminate from "./CircularIndeterminate";
import ErrorMessage from "./ErrorMessage";
import { ITaskDTO } from "../interfaces/interface";
import DoneAllIcon from "@mui/icons-material/DoneAll";

type TodoTasksProp = {
  tasks: ITaskDTO[] | undefined;
  tasksLoading: boolean;
  tasksIsError: boolean;
};

const TodoTasks = ({ tasks, tasksLoading, tasksIsError }: TodoTasksProp) => {
  if (!tasks || tasksLoading) {
    return <CircularIndeterminate />;
  }
  if (tasksIsError) {
    return <ErrorMessage />;
  }
  return (
    <div style={{textAlign:'center'}}>
      {tasks.length === 0 ? (
        <div
          style={{
            backgroundColor: "#F3F3F3",
            padding: "5px",
            borderRadius: "5px",
            textAlign: "center",
          }}
        >
          <DoneAllIcon fontSize="small" style={{ marginRight: "10px" }} />
          {t("no_tasks.text")}
        </div>
      ) : (
        tasks.map((task, index) => {
          return (
            <Tooltip
              key={task.taskId}
              placement="right"
              title={`${task.taskDescription}`}
            >
              <div
                style={{
                  textOverflow: "ellipsis",
                  whiteSpace: "nowrap",
                  overflow: "hidden",
                  borderRadius: "5px",
                  backgroundColor: "#F3F3F3",
                  margin:"15px"
                }}
              >
                {task.taskDescription}
              </div>
            </Tooltip>
          );
        })
      )}
    </div>
  );
};


export default TodoTasks