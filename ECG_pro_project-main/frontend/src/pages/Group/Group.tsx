import { Paper } from "@mui/material";
import GroupInfoRender from "../../components/GroupInfoRender";
import JoinGroup from "../JoinGroup/JoinGroup";

function Group() {
  return (
    <div>
      <div
        style={{
          display: "inline-block",
          marginBottom: "25px",
          width: "100%",
        }}
      >
        <div
          style={{
            display: "inline-block",
            marginBottom: "25px",
            width: "100%",
          }}
        >
          <GroupInfoRender />
        </div>
        <Paper>
          <JoinGroup />
        </Paper>
      </div>
    </div>
  );
}

export default Group;
