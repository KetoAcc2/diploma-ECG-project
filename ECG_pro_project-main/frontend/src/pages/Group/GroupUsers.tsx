import { Grid } from "@mui/material";
import CircularIndeterminate from "../../components/CircularIndeterminate";
import ErrorMessage from "../../components/ErrorMessage";
import { HeaderNames, IUserDTO } from "../../interfaces/interface";
import { usersToArray } from "../../util/utility";
import GroupUsersPagination from "./GroupUsersPagination";

type GroupUsersProp = {
  data?: IUserDTO[] | null | undefined;
  groupId: number;
  groupInfoLoading: boolean;
  groupIsError: boolean;
};

const GroupUsers = ({
  data,
  groupId,
  groupInfoLoading,
  groupIsError,
}: GroupUsersProp) => {
  if (!data || groupInfoLoading) {
    return (
      <div style={{ margin: "20% 0 0 45%" }}>
        <CircularIndeterminate />
      </div>
    );
  }
  if (groupIsError) {
    return (
      <div style={{ margin: "20% 0 0 0" }}>
        <ErrorMessage />
      </div>
    );
  }
  return (
    <Grid>
      <Grid item>
        <GroupUsersPagination
          genericTable={usersToArray(data)}
          headers={
            {
              columnOne: "user_number.text",
              columnTwo: "user_email.text",
            } as HeaderNames
          }
          groupId={groupId}
        />
      </Grid>
    </Grid>
  );
};

export default GroupUsers;
