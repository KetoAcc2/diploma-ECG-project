import ArrowCircleRightIcon from '@mui/icons-material/ArrowCircleRight';
import DoneAllIcon from "@mui/icons-material/DoneAll";
import { Divider, FormControl, Grid, IconButton, InputLabel, NativeSelect, Paper, Tooltip, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { Link, useParams } from "react-router-dom";
import { CardDivStyle } from "../interfaces/interface";
import useGetTargetFinishedTasksByGroup from "../queries/useGetTargetFinishedTasksByGroup";
import useGetUserDetailQuery from "../queries/useGetUserDetailQuery";
import useGetUsersGroups from "../queries/useGetUsersGroups";
import CircularIndeterminate from "./CircularIndeterminate";
import ErrorMessage from "./ErrorMessage";
import StatsLineChart from "./StatsLineChart";
import UserDetail from "./UserDetail";

const GetUserDetailCard = ({ style }: CardDivStyle) => {
  const goToLink = "history";
  const { t } = useTranslation();
  const { userId, groupId } = useParams();
  if (!userId) {
    return <CircularIndeterminate />;
  }
  if (!groupId) {
    return <CircularIndeterminate />;
  }

  const [selectedGroup, setSelectedGroup] = useState(+groupId);

  const { userData, userLoading, userIsError } = useGetUserDetailQuery(+userId);

  const { finishedTasks, finishedTasksInfoLoading, finishedTasksIsError, finishedTasksRefetch } = useGetTargetFinishedTasksByGroup(selectedGroup, +userId);

  const { group, groupInfoLoading, groupIsError, groupIsFetching } = useGetUsersGroups(+userId);

  const { finishedTasks: taskHistory, finishedTasksIsError: taskHistoryIsError, finishedTasksInfoLoading: taskHistoryInfoLoading, finishedTasksError: taskHistoryError, finishedTasksIsFetching, finishedTasksRefetch: taskHistoryRefetch } = useGetTargetFinishedTasksByGroup(+groupId, +userId);

  useEffect(() => {
    finishedTasksRefetch();
  }, [selectedGroup]);

  if (!group || groupInfoLoading || groupIsFetching) {
    return <CircularIndeterminate />;
  }
  if (groupIsError) {
    return <ErrorMessage />;
  }

  if (!taskHistory || taskHistoryInfoLoading || finishedTasksIsFetching) {
    return <CircularIndeterminate />;
  }
  if (taskHistoryIsError) {
    return <ErrorMessage />;
  }

  return (
    <div style={style}>
      <Grid item xs={5} style={{ width: "28%" }}>
        <div style={{ width: "100%", height: "20%" }}>
          <div style={{ marginBottom: "20px" }}>
            <Paper style={{ width: "100%", padding: "10px" }}>
              <div>
                <Typography variant="h6" style={{ color: "#636363" }}>
                  {t("account_info.text")}
                </Typography>
              </div>
              <Divider />
              <UserDetail
                userData={userData}
                userLoading={userLoading}
                userIsError={userIsError}
              />
            </Paper>
          </div>
        </div>
        <div style={{ width: "100%", height: "70%" }}>
          <div>
            <Paper style={{ width: "100%", padding: "10px" }}>
              <Typography variant="h6" style={{ color: "#636363" }}>
                {t("task_history.text")}
              </Typography>
              <Divider />
              <div
                style={{
                  textAlign: "left",
                  padding: "5px 20px 5px 20px",
                  marginTop: "5px",
                  borderRadius: "5px",
                  textOverflow: "ellipsis",
                  maxHeight: '393px',
                  minHeight: '393px',
                  overflowY: 'scroll'
                }}
              >
                <div style={{ textAlign: 'center' }}>
                  {taskHistory.finishedTasks.length === 0 ? (
                    <div
                      style={{
                        backgroundColor: "#F3F3F3",
                        padding: "5px",
                        borderRadius: "5px",
                        textAlign: "center",
                      }}
                    >
                      <DoneAllIcon fontSize="small" style={{ marginRight: "10px" }} />
                      {t("no_tasks.text")}
                    </div>
                  ) : (
                    taskHistory.finishedTasks.map((task, index) => {
                      return (
                        <Tooltip
                          key={task.taskId}
                          placement="right"
                          title={`${task.taskDescription}`}
                        >
                          <div
                            style={{
                              textOverflow: "ellipsis",
                              whiteSpace: "nowrap",
                              overflow: "hidden",
                              borderRadius: "5px",
                              backgroundColor: "#F3F3F3",
                              margin: "15px",
                              padding: "6px",
                              display: "flex",
                              alignItems: "center",
                              justifyContent: "space-between"
                            }}
                          >
                            <span>{task.taskDescription}</span>
                            <IconButton size="small" component={Link} to={`/${goToLink}/${task.taskId}/${userId}`}>
                              <ArrowCircleRightIcon color="info" fontSize="large" />
                            </IconButton>
                          </div>
                        </Tooltip>
                      );
                    })
                  )}
                </div>
              </div>
            </Paper>
          </div>
        </div>
      </Grid>
      <div style={{ width: '69%', marginLeft: '10px' }}>
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
    </div>
  );
};

export default GetUserDetailCard;
