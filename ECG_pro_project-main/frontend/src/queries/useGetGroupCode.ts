import { useQuery } from "react-query";
import { getGroupCode } from "../api/api";
import { useContext } from "react";
import { MyContext } from "../App";

const useGetGroupCode = (groupId: number) => {
  const { refreshStatus } = useContext(MyContext);
  const {
    data: groupCode,
    isError: groupCodeIsError,
    isLoading: groupCodeInfoLoading,
    error: groupCodeError,
  } = useQuery(
    "groupCode",
    async () => {
      return await getGroupCode(groupId);
    },
    { enabled: !refreshStatus }
  );
  return { groupCode, groupCodeIsError, groupCodeInfoLoading, groupCodeError };
};

export default useGetGroupCode;
