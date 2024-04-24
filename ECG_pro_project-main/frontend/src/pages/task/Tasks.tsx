import CircularIndeterminate from "../../components/CircularIndeterminate";
import ErrorMessage from "../../components/ErrorMessage";
import { useGetTasks } from "../../queries/useGetTasks";
import { tasksToArray } from "../../util/utility";
import CustomPaginationActionsTableTask from "./CustomPaginationActionsTableTask";

const Tasks = () => {
  const { tasks, tasksIsError, tasksLoading } = useGetTasks();

  if (!tasks || tasksLoading) {
    return <CircularIndeterminate />;
  }

  if (tasksIsError) {
    return <ErrorMessage />;
  }

  return (
    <div>
      <CustomPaginationActionsTableTask
        genericTable={{
          genericTable: tasksToArray(tasks),
          headers: {
            columnOne: "task_number.text",
            columnTwo: "task_description.text",
          },
        }}
        goToLink="starttask"
      />
    </div>
  );
};

export default Tasks;
