import { useQuery } from "react-query";
import { getTaskContent } from "../api/api";
import { useContext } from "react";
import { MyContext } from "../App";

const useGetTaskContent = (taskId: number) => {
  const { refreshStatus } = useContext(MyContext);
  const {
    data: taskContent,
    isError: taskContentIsError,
    isLoading: taskContentLoading,
    error: taskConentError,
  } = useQuery(
    "getTaskContent",
    async () => {
      const res = await getTaskContent(taskId);
      return res;
    },
    { enabled: !refreshStatus }
  );
  return {
    taskContent,
    taskContentIsError,
    taskContentLoading,
    taskConentError,
  };
};

export default useGetTaskContent;
