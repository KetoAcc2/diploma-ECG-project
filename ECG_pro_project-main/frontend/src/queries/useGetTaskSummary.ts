import { useQuery } from "react-query";
import { getTaskSummary } from "../api/api";
import { useContext } from "react";
import { MyContext } from "../App";

const useGetTaskSummary = (taskId: number) => {
  const { refreshStatus } = useContext(MyContext);
  const {
    data: taskSummary,
    isError: taskSummaryIsError,
    isLoading: taskSummaryInfoLoading,
    error: taskSummaryError,
  } = useQuery(
    "getTaskSummary",
    async () => {
      return await getTaskSummary(taskId);
    },
    { enabled: !refreshStatus }
  );
  return {
    taskSummary,
    taskSummaryIsError,
    taskSummaryInfoLoading,
    taskSummaryError,
  };
};

export default useGetTaskSummary;
