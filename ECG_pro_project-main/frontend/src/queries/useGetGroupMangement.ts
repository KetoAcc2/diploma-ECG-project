import { useQuery } from "react-query";
import { getCreatedGroupAsync } from "../api/api";
import { useContext } from "react";
import { MyContext } from "../App";

const useGetGroupMangement = () => {
  const { refreshStatus } = useContext(MyContext);
  const {
    data: group,
    isError: groupIsError,
    isLoading: groupInfoLoading,
    error: groupError,
  } = useQuery(
    "groupManagementInfo",
    async () => {
      return await getCreatedGroupAsync();
    },
    { enabled: !refreshStatus }
  );
  return { group, groupIsError, groupInfoLoading, groupError };
};

export default useGetGroupMangement;
