import { useQuery } from "react-query";
import { getGroupsOfUser } from "../api/api";
import { useContext, useEffect } from "react";
import { MyContext } from "../App";

const useGetGroups = (userId: number) => {
  const { refreshStatus } = useContext(MyContext);
  const {
    data: group,
    isError: groupIsError,
    isLoading: groupInfoLoading,
    error: groupError,
  } = useQuery("getJoinedGroupsOfUser", async () => {
    useEffect(()=>{},[])
    return await getGroupsOfUser(userId);
  },{enabled:!refreshStatus});
  return { group, groupIsError, groupInfoLoading, groupError };
};

export default useGetGroups;
