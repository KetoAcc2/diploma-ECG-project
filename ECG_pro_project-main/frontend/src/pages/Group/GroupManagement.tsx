import CircularIndeterminate from "../../components/CircularIndeterminate";
import ErrorMessage from "../../components/ErrorMessage";
import { HeaderNames } from "../../interfaces/interface";
import useGetGroupMangement from "../../queries/useGetGroupMangement";
import { groupToArray } from "../../util/utility";
import CreateGroup from "../CreateGroup/CreateGroup";
import PaginationGroupDetail from "./PaginationGroupDetail";

const GroupManagement = () => {
  const { group, groupInfoLoading, groupError } = useGetGroupMangement();

  if (!group || groupInfoLoading) {
    return <CircularIndeterminate />;
  }
  if (groupError) {
    return <ErrorMessage />;
  }
  return (
    <div>
      <div style={{ display: "flex", marginBottom: "25px" }}>
        <PaginationGroupDetail
          genericTable={groupToArray(group)}
          headers={
            {
              columnOne: "group_number.text",
              columnTwo: "group_name.text",
            } as HeaderNames
          }
        />
      </div>
      <div style={{ display: "block" }}>
        <CreateGroup />
      </div>
    </div>
  );
};
export default GroupManagement;
