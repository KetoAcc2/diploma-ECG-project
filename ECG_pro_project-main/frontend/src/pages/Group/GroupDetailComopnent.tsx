import { Button } from "@mui/material";
import { t } from "i18next";
import { IGroupCodeDTO, IUserDTO } from "../../interfaces/interface";
import GroupCode from "./GroupCode";
import GroupUsers from "./GroupUsers";

export const GroupDetailComponent = (props: {
  groupCode: IGroupCodeDTO | null | undefined;
  groupCodeInfoLoading: boolean;
  groupCodeIsError: boolean;
  id: number;
  setPage: React.Dispatch<React.SetStateAction<number>>;
  group: IUserDTO[] | undefined;
  groupInfoLoading: boolean;
  groupIsError: boolean;
}) => {
  const {
    groupCode,
    groupCodeIsError,
    groupCodeInfoLoading,
    id,
    setPage,
    group,
    groupInfoLoading,
    groupIsError,
  } = props;

  return (
    <div>
      <div
        style={{
          float: "right",
        }}
      >
        <div style={{ display: "flex" }}>
          <ul style={{ listStyleType: "none", textAlign: "right" }}>
            <li style={{ marginBottom: "5px" }}>
              <Button
                variant="outlined"
                onClick={() => {
                  setPage(1);
                }}
              >
                {t("check_assigned_tasks.text")}
              </Button>
            </li>
          </ul>
        </div>
      </div>
      <GroupCode
        style={{ marginBottom: "1%", textAlign: "left" }}
        groupId={id}
        groupCodeInfoLoading={groupCodeInfoLoading}
        groupCodeIsError={groupCodeIsError}
        groupCode={groupCode}
      />
      <div style={{ display: "flex", listStyleType: "none" }}>
        <div style={{ width: "100%" }}>
          <GroupUsers
            groupId={id}
            data={group}
            groupInfoLoading={groupInfoLoading}
            groupIsError={groupIsError}
          />
        </div>
        
      </div>
    </div>
  );
};
