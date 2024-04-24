import { useQuery } from "react-query";
import { getUserStats } from "../api/api";
import { OverallScorePerGroupDTO } from "../interfaces/interface";
import { useContext } from "react";
import { MyContext } from "../App";

const useGetStats = () => {
  const { refreshStatus } = useContext(MyContext);
  const {
    data: userStats,
    isError: userStatsIsError,
    isLoading: userStatsLoading,
    error: userStatsError,
  } = useQuery(
    "userStats",
    async (): Promise<OverallScorePerGroupDTO | undefined> =>
      await getUserStats(),
    { enabled: !refreshStatus }
  );
  return { userStats, userStatsIsError, userStatsLoading, userStatsError };
};
export default useGetStats;
