import CustomPaginationActionsTableGroup from "../pages/Group/PaginationTableGroup";
import useGetCurrentGroups from "../queries/useGetCurrentGroups";
import { groupToArray } from "../util/utility";
import CircularIndeterminate from "./CircularIndeterminate";
import ErrorMessage from "./ErrorMessage";

function GroupInfoRender() {
  const { group, groupIsError, groupInfoLoading, groupIsFetching } =
    useGetCurrentGroups();
  if (!group || groupInfoLoading || groupIsFetching) {
    return <CircularIndeterminate />;
  }
  if (groupIsError) {
    return <ErrorMessage />;
  }
  return (
    <div>
      <CustomPaginationActionsTableGroup
        genericTable={groupToArray(group)}
        headers={{ columnOne: "", columnTwo: "" }}
      />
    </div>
  );
}

export default GroupInfoRender;
