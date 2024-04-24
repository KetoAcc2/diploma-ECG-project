import { useQuery } from "react-query";
import { getPics } from "../api/api";
import { useContext } from "react";
import { MyContext } from "../App";

const useGetPic = (taskId: number) => {
  const { refreshStatus } = useContext(MyContext);
  const {
    data: pic,
    isError: picIsError,
    isLoading: picInfoLoading,
    error: picError,
  } = useQuery(
    "getPics",
    async () => {
      return await getPics(taskId);
    },
    { enabled: !refreshStatus }
  );
  return { pic, picIsError, picInfoLoading, picError };
};

export default useGetPic;
