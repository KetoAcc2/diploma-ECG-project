import { Divider, FormControl, InputLabel, NativeSelect, Paper, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { CardDivStyle } from "../interfaces/interface";
import useGetCurrentGroups from "../queries/useGetCurrentGroups";
import useGetFinishedTasksByGroup from "../queries/useGetFinishedTasksByGroup";
import CircularIndeterminate from "./CircularIndeterminate";
import ErrorMessage from "./ErrorMessage";
import StatsLineChart from "./StatsLineChart";
import { useTranslation } from "react-i18next";

const UserStatsCard = ({ style }: CardDivStyle) => {
  const { t } = useTranslation();
  const [selectedGroup, setSelectedGroup] = useState(0);
  const { finishedTasks, finishedTasksInfoLoading, finishedTasksIsError, finishedTasksRefetch } = useGetFinishedTasksByGroup(selectedGroup);
  const { group, groupInfoLoading, groupIsError, groupIsFetching } = useGetCurrentGroups();

  useEffect(() => {
    finishedTasksRefetch();
  }, [selectedGroup]);

  if (!group || groupInfoLoading || groupIsFetching) {
    return <CircularIndeterminate />;
  }
  if (groupIsError) {
    return <ErrorMessage />;
  }

  return (
    <div style={style}>
      <FormControl fullWidth>
        <InputLabel variant="standard" htmlFor="uncontrolled-native">
          {t("group.text")}
        </InputLabel>
        <NativeSelect
          onChange={(e) => setSelectedGroup(+e.target.value)}
          defaultValue={selectedGroup}
          inputProps={{
            name: "group",
            id: "uncontrolled-native",
          }}>
          <option value="0">{t("not_selected.text")}</option>
          {group.map((x, index) => (
            <option key={index} value={x.groupId}>
              {x.groupId} - {x.groupName}
            </option>
          ))}
        </NativeSelect>
      </FormControl>
      <Paper style={{ width: "100%", padding: "10px", height: "80vh" }}>
        <Typography variant="h6" style={{ color: "#636363" }}>
          {t("statistics.text")}
        </Typography>
        <Divider />
        {finishedTasks?.finishedTasks.length === 0 && t('no_tasks_no_group.text')}
        <StatsLineChart finishedTasks={finishedTasks} finishedTasksInfoLoading={finishedTasksInfoLoading} finishedTasksIsError={finishedTasksIsError} />
      </Paper>
    </div>
  );
};

export default UserStatsCard;
