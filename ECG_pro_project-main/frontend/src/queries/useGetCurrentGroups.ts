import { useQuery } from "react-query";
import { getCurrentGroupsAsync } from "../api/api";
import { useContext } from "react";
import { MyContext } from "../App";

const useGetCurrentGroups = () => {
  const { refreshStatus } = useContext(MyContext);
  const {
    data: group,
    isError: groupIsError,
    isLoading: groupInfoLoading,
    error: groupError,
    isFetching: groupIsFetching,
  } = useQuery(
    "groupInfo",
    async () => {
      return await getCurrentGroupsAsync();
    },
    { enabled: !refreshStatus }
  );
  return { group, groupIsError, groupInfoLoading, groupError, groupIsFetching };
};

export default useGetCurrentGroups;
