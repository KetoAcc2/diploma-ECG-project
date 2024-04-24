import Button from "@mui/material/Button/Button";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import { useParams } from "react-router-dom";
import CircularIndeterminate from "../../components/CircularIndeterminate";
import useGetGroupCode from "../../queries/useGetGroupCode";
import useGetTasksFromGroup from "../../queries/useGetTasksFromGroup";
import useGetUsersFromGroup from "../../queries/useGetUsersFromGroup";
import AssignTasks from "../AssignTasks/AssignTasks";
import RemoveTasks from "../RemoveTasks/RemoveTasks";
import { GroupDetailComponent } from "./GroupDetailComopnent";

const GroupDetail = () => {
  const [page, setPage] = useState(0);
  const { t } = useTranslation();
  const { id } = useParams();
  if (!id) {
    return <div>{t("loading.text")}</div>;
  }

  const { group, groupInfoLoading, groupIsError } = useGetUsersFromGroup(+id);
  const { groupCode, groupCodeInfoLoading, groupCodeIsError } = useGetGroupCode(
    +id
  );

  const { tasksByGroup, tasksByGroupLoading, tasksByGroupIsError } =
    useGetTasksFromGroup(+id);

  if (!tasksByGroup || tasksByGroupLoading) {
    return <CircularIndeterminate />;
  }

  const stepDisplay = () => {
     
    if (page === 0) {
      return (
        <GroupDetailComponent
          groupCode={groupCode}
          id={+id}
          setPage={setPage}
          group={group}
          groupInfoLoading={groupInfoLoading}
          groupIsError={groupIsError}
          groupCodeInfoLoading={groupCodeInfoLoading}
          groupCodeIsError={groupCodeIsError}
        />
      );
    }
    if (page === 1) {
      return (
        <div>
          <div
            style={{
              float: "right",
              paddingTop: "3%",
            }}>
            <Button
              style={{ marginBottom: "10px" }}
              variant="outlined"
              onClick={() => {
                setPage(0);
              }}>
              {t("go_back.text")}
            </Button>
          </div>
          <div style={{ display: "flex", listStyleType: "none" }}>
            <div style={{ width: "100%", marginRight: "2%" }}>
              <RemoveTasks
                tasksByGroup={tasksByGroup}
                tasksByGroupLoading={tasksByGroupLoading}
                tasksByGroupIsError={tasksByGroupIsError}
              />
            </div>

            <div style={{ marginLeft: "auto", marginRight: "2%" }}>
              <AssignTasks id={+id} />
            </div>
          </div>
        </div>
      );
    }
  };
  return (
    <div>

        <div>{stepDisplay()}</div>

    </div>
  );
};
export default GroupDetail;
