import { useQuery } from "react-query";
import { getFinishedTasksByGroup } from "../api/api";
import { useContext } from "react";
import { MyContext } from "../App";

const useGetFinishedTasksByGroup = (groupId: number) => {
  const { refreshStatus } = useContext(MyContext);
  const {
    data: finishedTasks,
    isError: finishedTasksIsError,
    isLoading: finishedTasksInfoLoading,
    error: finishedTasksError,
    refetch: finishedTasksRefetch,
  } = useQuery(
    "getFinishedTasksByGroup",
    async () => {
       
      if (groupId <= 0) {
        return undefined;
      }
      return await getFinishedTasksByGroup(groupId);
    },
    { enabled: !refreshStatus }
  );
  return {
    finishedTasks,
    finishedTasksIsError,
    finishedTasksInfoLoading,
    finishedTasksError,
    finishedTasksRefetch,
  };
};

export default useGetFinishedTasksByGroup;
