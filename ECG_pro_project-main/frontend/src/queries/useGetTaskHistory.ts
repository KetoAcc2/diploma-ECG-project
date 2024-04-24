import { useQuery } from "react-query";
import { getTaskHistory } from "../api/api";
import { useContext } from "react";
import { MyContext } from "../App";

const useGetTaskHistory = (taskId: number) => {
  const { refreshStatus } = useContext(MyContext);
  const {
    data: taskHistory,
    isError: taskHistoryIsError,
    isLoading: taskHistoryLoading,
    error: taskHistoryError,
    isFetching: taskHistoryIsFetching,
  } = useQuery(
    "taskHistory",
    async () => {
      return await getTaskHistory(taskId);
    },
    { enabled: !refreshStatus }
  );
  return {
    taskHistory,
    taskHistoryIsError,
    taskHistoryLoading,
    taskHistoryError,
    taskHistoryIsFetching,
  };
};

export default useGetTaskHistory;
