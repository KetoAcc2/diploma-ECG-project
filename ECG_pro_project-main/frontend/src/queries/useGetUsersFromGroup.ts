import { useQuery } from "react-query";
import { getUsersFromGroupsAsync } from "../api/api";
import { useContext } from "react";
import { MyContext } from "../App";

const useGetUsersFromGroup = (groupId: number) => {
  const { refreshStatus } = useContext(MyContext);
  const {
    data: group,
    isError: groupIsError,
    isLoading: groupInfoLoading,
    error: groupError,
  } = useQuery(
    "usersFromGroup",
    async () => {
      return await getUsersFromGroupsAsync(groupId);
    },
    { enabled: !refreshStatus }
  );
  return { group, groupIsError, groupInfoLoading, groupError };
};

export default useGetUsersFromGroup;
