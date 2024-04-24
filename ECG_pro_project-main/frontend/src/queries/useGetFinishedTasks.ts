import { useQuery } from "react-query";
import { getFinishedTasks } from "../api/api";
import { useContext } from "react";
import { MyContext } from "../App";

const useGetFinishedTasks = () => {
  const { refreshStatus } = useContext(MyContext);
  const {
    data: finishedTasks,
    isError: finishedTasksInfoLoading,
    isLoading: finishedTasksLoading,
    error: finishedTasksIsError,
  } = useQuery(
    "finishedTasks",
    async () => {
      return getFinishedTasks();
    },
    { enabled: !refreshStatus }
  );
  return {
    finishedTasks,
    finishedTasksInfoLoading,
    finishedTasksLoading,
    finishedTasksIsError,
  };
};

export default useGetFinishedTasks;
