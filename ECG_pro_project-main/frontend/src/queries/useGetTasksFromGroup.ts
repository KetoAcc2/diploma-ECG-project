import { useQuery } from "react-query";
import { getTasksByGroup } from "../api/api";
import { ITaskDTO } from "../interfaces/interface";
import { useContext } from "react";
import { MyContext } from "../App";

const useGetTasksFromGroup = (groupid: number) => {
  const { refreshStatus } = useContext(MyContext);
  const {
    data: tasksByGroup,
    isError: tasksByGroupIsError,
    isLoading: tasksByGroupLoading,
    error: tasksByGroupError,
  } = useQuery(
    "tasksInfoByGroup",
    async (): Promise<ITaskDTO[]> => {
      return await getTasksByGroup(groupid);
    },
    { enabled: !refreshStatus }
  );
  return {
    tasksByGroup,
    tasksByGroupIsError,
    tasksByGroupLoading,
    tasksByGroupError,
  };
};

export default useGetTasksFromGroup;
