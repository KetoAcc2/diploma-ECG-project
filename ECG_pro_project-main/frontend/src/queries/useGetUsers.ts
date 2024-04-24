import { useQuery } from "react-query";
import { getUsersAsync } from "../api/api";
import { useContext } from "react";
import { MyContext } from "../App";

const useGetUsers = () => {
  const { refreshStatus } = useContext(MyContext);
  const {
    data: usersData,
    isError: usersIsError,
    isLoading: usersLoading,
    error: usersError,
    refetch: refetchUsers,
  } = useQuery("users", getUsersAsync, {
    refetchOnWindowFocus: false,
    enabled: !refreshStatus,
  });
  return { usersData, usersIsError, usersLoading, usersError };
};

export default useGetUsers;
