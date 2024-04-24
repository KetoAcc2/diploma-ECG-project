import { useQuery } from "react-query";
import { getGroupsOfUser } from "../api/api";
import { useContext } from "react";
import { MyContext } from "../App";

const useGetUsersGroups = (userId: number) => {
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
      return await getGroupsOfUser(userId);
    },
    { enabled: !refreshStatus }
  );
  return { group, groupIsError, groupInfoLoading, groupError, groupIsFetching };
};

export default useGetUsersGroups;
