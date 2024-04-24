import { useQuery } from "react-query";
import { getITDocs, getStudentDocs, getTeacherDocs } from "../api/api";
import { useContext } from "react";
import { MyContext } from "../App";

const useGetITDocs = (docType: number) => {
  const { refreshStatus } = useContext(MyContext);
  const {
    data: ITDocs,
    isError: getITDocsIsError,
    isLoading: getITDocsInfoLoading,
    error: getITDocsError,
    isFetching: getITDocsIsFetching,
  } = useQuery(
    "getITDocsInfo",
    async () => {
      switch (docType) {
        case 1:
          return await getITDocs();
        case 2:
          return await getTeacherDocs();
        case 3:
          return await getStudentDocs();
        default:
          return undefined;
      }
    },
    { enabled: !refreshStatus }
  );
  return {
    ITDocs,
    getITDocsIsError,
    getITDocsInfoLoading,
    getITDocsError,
    getITDocsIsFetching,
  };
};

export default useGetITDocs;
