import CircularIndeterminate from "../../components/CircularIndeterminate";
import ErrorMessage from "../../components/ErrorMessage";
import useGetFinishedTasks from "../../queries/useGetFinishedTasks";
import TaskHistoryCustomTable from "./TaskHistoryCustomTable";

const TaskHistoryTable = () => {
  const { finishedTasks, finishedTasksLoading, finishedTasksIsError } = useGetFinishedTasks();

  if (!finishedTasks || finishedTasksLoading) {
    return <CircularIndeterminate />;
  }
  if (finishedTasksIsError) {
    return <ErrorMessage />;
  }
  return (
    <div>
      <TaskHistoryCustomTable goToLink={"history"} data={finishedTasks} />
    </div>
  );
};

export default TaskHistoryTable;
