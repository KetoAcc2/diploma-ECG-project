import { useQuery } from "react-query";
import { getUserAsync } from "../api/api";
import { IUserDTO } from "../interfaces/interface";
import { useContext } from "react";
import { MyContext } from "../App";

const useGetUserDetailQuery = (userId: number) => {
  const { refreshStatus } = useContext(MyContext);
  const {
    data: userData,
    isError: userIsError,
    isLoading: userLoading,
    error: userError,
  } = useQuery(
    "user",
    async (): Promise<IUserDTO | undefined> => {
      const user = await getUserAsync(userId);
      return user;
    },
    { enabled: !refreshStatus }
  );
  return { userData, userIsError, userLoading, userError };
};

export default useGetUserDetailQuery;
