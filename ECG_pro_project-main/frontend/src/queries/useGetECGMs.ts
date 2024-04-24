import { useQuery } from "react-query";
import { getECGMs } from "../api/api";
import { useContext } from "react";
import { MyContext } from "../App";

const useGetECGMs = (taskId: number) => {
  const { refreshStatus } = useContext(MyContext);
  const {
    data: ecgMs,
    isError: ecgMsIsError,
    isLoading: ecgMsInfoLoading,
    error: ecgMsError,
    isFetching: ecgMsIsFetching,
  } = useQuery(
    "ecgMsInfo",
    async () => {
      return await getECGMs(taskId);
    },
    { enabled: !refreshStatus }
  );
  return { ecgMs, ecgMsIsError, ecgMsInfoLoading, ecgMsError, ecgMsIsFetching };
};

export default useGetECGMs;
