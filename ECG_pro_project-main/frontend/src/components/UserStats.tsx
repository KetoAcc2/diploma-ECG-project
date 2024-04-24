import { OverallScorePerGroupDTO } from "../interfaces/interface";
import CircularIndeterminate from "./CircularIndeterminate";
import ErrorMessage from "./ErrorMessage";

type UserStatsProps = {
  userStats: OverallScorePerGroupDTO | undefined;
  userStatsLoading: boolean;
  userStatsIsError: boolean;
};

const UserStats = ({
  userStats,
  userStatsLoading,
  userStatsIsError,
}: UserStatsProps) => {
  if (!userStats || userStatsLoading) {
    return <CircularIndeterminate />;
  }
  if (userStatsIsError) {
    return <ErrorMessage />;
  }
  return (
    <div>
      Hello stats cards{" "}
      {userStats.scorePerGroups.map((data,index) => (
        <div
          key={index}
          style={{
            padding:'10px',
            textAlign: "left",
            backgroundColor: "#F3F3F3",
            margin: "10px",
            borderRadius: "5px",
          }}
        >
          <div>Group: {data.groupId}</div>
          <div>Score: {data.scoreInPercentage}%</div>
        </div>
      ))}
    </div>
  );
};

export default UserStats;
