import { useQuery } from "react-query";
import { getTargetFinishedTasksByGroup } from "../api/api";
import { useContext } from "react";
import { MyContext } from "../App";

const useGetTargetFinishedTasksByGroup = (groupId: number, userId: number) => {
  const { refreshStatus } = useContext(MyContext);
  const {
    data: finishedTasks,
    isError: finishedTasksIsError,
    isLoading: finishedTasksInfoLoading,
    error: finishedTasksError,
    isFetching: finishedTasksIsFetching,
    refetch: finishedTasksRefetch,
  } = useQuery(
    "getTargetFinishedTasksByGroup",
    async () => {
       
      return await getTargetFinishedTasksByGroup(groupId, userId);
    },
    { enabled: !refreshStatus }
  );
  return {
    finishedTasks,
    finishedTasksIsError,
    finishedTasksInfoLoading,
    finishedTasksError,
    finishedTasksIsFetching,
    finishedTasksRefetch,
  };
};

export default useGetTargetFinishedTasksByGroup;
