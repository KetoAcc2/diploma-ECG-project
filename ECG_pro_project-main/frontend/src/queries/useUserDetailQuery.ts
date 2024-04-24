import { useQuery } from "react-query";
import { getDashboard } from "../api/api";
import { IUserDTO } from "../interfaces/interface";
import { useContext } from "react";
import { MyContext } from "../App";

export const useUserDetailQuery = () => {
  const { refreshStatus } = useContext(MyContext);
  const {
    data: userData,
    isError: userIsError,
    isLoading: userLoading,
    error: userError,
  } = useQuery(
    "dashboard",
    async (): Promise<IUserDTO> => {
      const user = await getDashboard();
      return user;
    },
    { enabled: !refreshStatus }
  );
  return { userData, userIsError, userLoading, userError };
};
