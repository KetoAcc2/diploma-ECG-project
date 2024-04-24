import { Paper, Typography, Divider } from "@mui/material";
import { CardDivStyle } from "../interfaces/interface";
import useGetStats from "../queries/useGetStats";
import UserStats from "./UserStats";

const GetUserStatsCard = ({ style }: CardDivStyle) => {
  const { userStats, userStatsLoading, userStatsIsError } = useGetStats();

  return (
    <div style={style}>
      <Paper style={{ width: "100%", padding: "10px" }}>
        <Typography variant="h6" style={{ color: "#636363" }}>
          Stats
        </Typography>
        <Divider />
        <UserStats
          userStats={userStats}
          userStatsLoading={userStatsLoading}
          userStatsIsError={userStatsIsError}
        />
      </Paper>
    </div>
  );
};

export default GetUserStatsCard;
