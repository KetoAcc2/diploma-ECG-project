import { useQuery } from "react-query";
import { getTaskHistory, getUserTaskHistory } from "../api/api";
import { useContext } from "react";
import { MyContext } from "../App";

const useGetUserTaskHistory = (taskId: number, userId: number) => {
  const { refreshStatus } = useContext(MyContext);
  const {
    data: userTaskHistory,
    isError: userTaskIsError,
    isLoading: userTaskLoading,
    error: userTaskError,
    isFetching: userTaskIsFetching,
  } = useQuery(
    "userTaskHistory",
    async () => {
      return await getUserTaskHistory(taskId, userId);
    },
    { enabled: !refreshStatus }
  );
  return {
    userTaskHistory,
    userTaskIsError,
    userTaskLoading,
    userTaskError,
    userTaskIsFetching,
  };
};

export default useGetUserTaskHistory;
