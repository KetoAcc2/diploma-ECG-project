import { Button } from "@mui/material";
import CircularIndeterminate from "../../components/CircularIndeterminate";
import ErrorMessage from "../../components/ErrorMessage";
import useGetITDocs from "../../queries/useGetITDocs";
import { apiPath } from "../../constants/constants";
import { useTranslation } from "react-i18next";

const StudentInstruction = () => {
  const { t } = useTranslation();
  let { ITDocs, getITDocsInfoLoading, getITDocsIsError } = useGetITDocs(3);
  if (!ITDocs || getITDocsInfoLoading) {
    return <CircularIndeterminate />;
  }
  if (getITDocsIsError) {
    return <ErrorMessage />;
  }
  return (
    <div>
      <div>{t("instruction_for_user.text")}</div>
      <a href={`${apiPath.BASE_URL}/${ITDocs}`} download={"InstructionDoc"} target={"_blank"}>
        <Button>{t("download.text")}</Button>
      </a>
    </div>
  );
};

export default StudentInstruction;
