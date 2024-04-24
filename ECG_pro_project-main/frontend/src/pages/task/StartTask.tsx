import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { useNavigate, useParams } from "react-router-dom";
import ErrorMessage from "../../components/ErrorMessage";
import { pageInfo } from "../../constants/constants";
import useGetECGMs from "../../queries/useGetECGMs";
import useGetFinishedTasks from "../../queries/useGetFinishedTasks";
import TaskDescription from "./TaskDescription";
import TaskSolving from "./TaskSolving";
import TaskSummary from "./TaskSummary";

const StartTask = () => {
  const { taskId } = useParams();
  const { t } = useTranslation();
  if (!taskId) {
    return <div>{t("loading.text")}</div>;
  }
  const [page, setPage] = useState(0);

  const nav = useNavigate();

  const { finishedTasks, finishedTasksLoading, finishedTasksIsError } = useGetFinishedTasks();
  const { ecgMs, ecgMsIsError, ecgMsInfoLoading, ecgMsError, ecgMsIsFetching } = useGetECGMs(+taskId);

  if (finishedTasksIsError) {
    return <ErrorMessage />;
  }

  useEffect(() => {
    const checkTaskIsFinished = () => {
       
      const condition = finishedTasks?.find(ft => ft.taskId === +taskId)?.taskId === undefined;
      if (!condition) {
         
        nav(pageInfo.tasks.url);
      }
    }
    checkTaskIsFinished();
  }, [finishedTasks, nav]);



  const stepDisplay = () => {
     
    if (page === 0) {
      return (
        <TaskDescription ecgId={ecgMs?.ecgId} setPage={setPage}/>
      );
    } else if (page === 1) {
      return (
        <TaskSolving
          taskId={+taskId}
          setPage={setPage}
        />
      );
    } else if (page === 2) {
      return (
        <TaskSummary ecgMsId={ecgMs?.ecgId} />
      );
    }
  };

  return (
    <div>
      <div>{stepDisplay()}</div>
    </div>
  );
};
export default StartTask;
