import { useQuery } from "react-query";
import { getTodoTasks } from "../api/api";
import { useContext } from "react";
import { MyContext } from "../App";

export const useGetTasks = () => {
  const { refreshStatus } = useContext(MyContext);
  const {
    data: tasks,
    isError: tasksIsError,
    isLoading: tasksLoading,
    error: tasksError,
  } = useQuery("getTasks", async () => {
    return await getTodoTasks();
  },{enabled:!refreshStatus});
  return { tasks, tasksIsError, tasksLoading, tasksError };
};