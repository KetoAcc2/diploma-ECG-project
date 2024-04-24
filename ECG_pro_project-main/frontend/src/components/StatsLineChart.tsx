import { BarElement, CategoryScale, Chart, Legend, LineElement, LinearScale, PointElement, Title, Tooltip, BarController, registerables } from "chart.js";
import { useEffect } from "react";
import { Bar } from "react-chartjs-2";
import { useTranslation } from "react-i18next";
import { FinishedTasksByGroupDTO } from "../interfaces/interface";
import ErrorMessage from "./ErrorMessage";

const ToLineChart = (finishedTasks: FinishedTasksByGroupDTO, options: any, taskText: string, scoreText: string) => {
  if (finishedTasks.finishedTasks.length === 0) {
    return <></>;
  }
  return <Bar data={toData(finishedTasks, taskText, scoreText)} options={options} />;
};

const toData = (finishedTasks: FinishedTasksByGroupDTO, taskText: string, scoreText: string) => {
  let labels = [];
  let datasets = [];
  let data = [];
  let arr = finishedTasks.finishedTasks;
  for (let i = 0; i < arr.length; i++) {
    labels.push(`${taskText}: ${arr[i].taskId}`);
    data.push(arr[i].taskScore);
  }
  datasets.push({
    label: scoreText,
    fill: false,
    borderColor: "rgb(75, 192, 192)",
    borderWidth: 1,
    backgroundColor: "rgba(75, 192, 192, 0.2)",
    tension: 0.1,
    data: data,
  });
  return { labels, datasets };
};

const StatsLineChart = ({ finishedTasks, finishedTasksInfoLoading, finishedTasksIsError }: { finishedTasks: FinishedTasksByGroupDTO | undefined; finishedTasksInfoLoading: boolean; finishedTasksIsError: boolean }) => {
   
  const { t } = useTranslation();
  Chart.register(CategoryScale, LinearScale, PointElement, BarElement, BarController, Title, Tooltip, Legend, ...registerables);
  if (!finishedTasks || finishedTasksInfoLoading) {
    return <div>{t("no_tasks_no_group.text")}</div>;
  }
  if (finishedTasksIsError) {
    return <ErrorMessage />;
  }
  const options = {
    responsive: true,
    plugins: {
      title: {
        display: true,
        text: t("statistics.text"),
      },
    },
    scales: {
      y: {
        min: 0,
        max: 70,
        ticks: {
          stepSize: 5,
        },
      },
    },
  };
  return <div style={{ position: "relative", margin: "auto", width: "100%" }}>{ToLineChart(finishedTasks, options, t("task.text"), t("score.text"))}</div>;
};

export default StatsLineChart;
