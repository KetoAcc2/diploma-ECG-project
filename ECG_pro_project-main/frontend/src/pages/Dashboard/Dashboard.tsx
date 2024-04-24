import { Grid } from "@mui/material";
import TodoTasksCard from "../../components/TodoTasksCard";
import UserDetailCard from "../../components/UserDetailCard";
import UserStatsCard from "../../components/UserStatsCard";

const Dashboard = () => {
  return (
    <div style={{ display: "flex" }}>
      <Grid item xs={5} style={{ width: "28%" }}>
        <div style={{ width: "100%", height: "20%" }}>
          <UserDetailCard style={{ marginBottom: "20px" }} />
        </div>
        <div style={{ width: "100%", height: "70%" }}>
          <TodoTasksCard style={{ marginBottom: "20px" }} />
        </div>
      </Grid>
      <Grid item xs={10} style={{ width: "69%", marginLeft:'10px' }}>
        <div style={{ width: "100%", height: "100%" }}>
          <UserStatsCard />
        </div>
      </Grid>
    </div>
  );
};

export default Dashboard;
