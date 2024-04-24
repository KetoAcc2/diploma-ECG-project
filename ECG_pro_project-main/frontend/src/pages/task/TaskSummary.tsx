import { Button } from "@mui/material";
import { useTranslation } from "react-i18next";
import { useNavigate, useParams } from "react-router-dom";
import { pageInfo } from "../../constants/constants";

const TaskSummary = ({ ecgMsId=0 }: { ecgMsId?: number }) => {
  const { t } = useTranslation();
  const { taskId } = useParams();
  if (!taskId) {
    return <div></div>;
  }
  const nav = useNavigate();

  const handleSubmit = () => {
    nav(pageInfo.tasks.url);
  };

  return (
    <div style={{ margin: "auto", maxWidth: "90vh" }}>
      <div style={{ textAlign: "left" }}>
        <span style={{ hyphens: "auto", wordWrap: "break-word" }}>
          {t(`ecg_summary.ecg_summary${ecgMsId}`)
            .split("\n")
            .map((line, index) => (
              <span key={index} style={{ hyphens: "auto", wordWrap: "break-word" }}>
                {line}
                <br />
              </span>
            ))}
        </span>
      </div>
      <br />
      <Button color="primary" variant="contained" onClick={handleSubmit}>
        {t("task_submit_confirmation.close")}
      </Button>
    </div>
  );
};

export default TaskSummary;
