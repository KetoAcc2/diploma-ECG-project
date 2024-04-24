import { useQuery } from "react-query";
import { getQuestionTypes } from "../api/api";
import { QuestionTypeDTO } from "../interfaces/interface";
import { useContext } from "react";
import { MyContext } from "../App";

const useGetTaskTypes = () => {
  const { refreshStatus } = useContext(MyContext);
  const {
    data: taskType,
    isError: taskTypeIsError,
    isLoading: taskTypeLoading,
    error: taskTypeError,
  } = useQuery(
    "getTaskTypes",
    async (): Promise<QuestionTypeDTO[]> => {
      return await getQuestionTypes();
    },
    { enabled: !refreshStatus }
  );
  return { taskType, taskTypeIsError, taskTypeLoading, taskTypeError };
};

export default useGetTaskTypes;
