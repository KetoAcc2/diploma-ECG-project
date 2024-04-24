import { Checkbox, FormControlLabel, Grid } from "@mui/material";
import { useTranslation } from "react-i18next";
import { useParams } from "react-router-dom";
import CircularIndeterminate from "../../components/CircularIndeterminate";
import ErrorMessage from "../../components/ErrorMessage";
import { checkboxRowStyle, headerContainerStyle } from "../../constants/reactTemplates";
import useGetTaskHistory from "../../queries/useGetTaskHistory";
import { correctCalculation, correctOrNot } from "./taskHistoryUtilities";
import { green, red, yellow } from "@mui/material/colors";
import useGetUserTaskHistory from "../../queries/useGetUserTaskHistory";
import { TaskHistoryResponseDTO } from "../../interfaces/interface";
import useGetECGMs from "../../queries/useGetECGMs";

const TaskHistory = () => {
  const { taskId, userId = null } = useParams();
  if (!taskId) {
    return <CircularIndeterminate />;
  }
  const { t } = useTranslation();
  let history: TaskHistoryResponseDTO;
  const { ecgMs, ecgMsIsError, ecgMsInfoLoading, ecgMsError, ecgMsIsFetching } = useGetECGMs(+taskId);
  if (userId == null) {
    const { taskHistory, taskHistoryIsError, taskHistoryLoading, taskHistoryIsFetching } = useGetTaskHistory(+taskId);
    if (!taskHistory || taskHistoryLoading || taskHistoryIsFetching) {
      return <CircularIndeterminate />;
    }
    if (taskHistoryIsError) {
      return <ErrorMessage />;
    }
    history = taskHistory;
  } else {
    const { userTaskHistory, userTaskIsError, userTaskLoading, userTaskError, userTaskIsFetching } = useGetUserTaskHistory(+taskId, +userId);
    if (!userTaskHistory || userTaskLoading || userTaskIsFetching) {
      return <CircularIndeterminate />;
    }
    if (userTaskIsError) {
      return <ErrorMessage />;
    }
    history = userTaskHistory;
  }

  
  //pq - parentQuestionNumber, qn - questionNumber
  const multiQuestions = (pq: number, qn: number) => {
    return (
      <Grid item>
        <Grid item style={checkboxRowStyle}>
          {correctOrNot(t("answerConstant.one"), pq, qn, 1, history)}
          {correctOrNot(t("answerConstant.two"), pq, qn, 2, history)}
          {correctOrNot(t("answerConstant.three"), pq, qn, 3, history)}
          {correctOrNot(t("answerConstant.aVR"), pq, qn, 4, history)}
        </Grid>
        <Grid item style={checkboxRowStyle}>
          {correctOrNot(t("answerConstant.aVL"), pq, qn, 5, history)}
          {correctOrNot(t("answerConstant.aVF"), pq, qn, 6, history)}
          {correctOrNot(t("answerConstant.V1"), pq, qn, 7, history)}
          {correctOrNot(t("answerConstant.V2"), pq, qn, 8, history)}
        </Grid>
        <Grid item style={checkboxRowStyle}>
          {correctOrNot(t("answerConstant.V3"), pq, qn, 9, history)}
          {correctOrNot(t("answerConstant.V4"), pq, qn, 10, history)}
          {correctOrNot(t("answerConstant.V5"), pq, qn, 11, history)}
          {correctOrNot(t("answerConstant.V6"), pq, qn, 12, history)}
        </Grid>
      </Grid>
    );
  };

  return (
    <div>
      <div style={{ marginLeft: "50px" }}>
        <div style={{ float: "left", textAlign: "left" }}>
          <div>
            <FormControlLabel
              control={
                <Checkbox
                  sx={{
                    "&.Mui-checked": {
                      color: green[400],
                    },
                  }}
                />
              }
              label={t("correct_choice.text")}
              checked
              labelPlacement="start"
            />
          </div>
          <div>
            <FormControlLabel
              control={
                <Checkbox
                  sx={{
                    "&.Mui-checked": {
                      color: red[400],
                    },
                  }}
                />
              }
              label={t("wrong_answer.text")}
              checked
              labelPlacement="start"
            />
          </div>
          <div>
            <FormControlLabel
              control={
                <Checkbox
                  sx={{
                    "&.Mui-checked": {
                      color: yellow[800],
                    },
                  }}
                />
              }
              label={t("didnt_choose.text")}
              checked
              labelPlacement="start"
            />
          </div>
        </div>
        <Grid container spacing={4} justifyContent="center" direction="column">
          <Grid item style={headerContainerStyle}>
            <div>1. {t("question1.text")}</div>
            {correctCalculation(1, 1, 1, history)}
          </Grid>
          <Grid item style={headerContainerStyle}>
            <div>2. {t("question2.text")}</div>
            <br />
            <Grid item>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question2.a1"), 2, 1, 1, history)}
                {correctOrNot(t("question2.a2"), 2, 1, 2, history)}
                {correctOrNot(t("question2.a3"), 2, 1, 3, history)}
                {correctOrNot(t("question2.a4"), 2, 1, 4, history)}
              </Grid>
              <br />
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question2.a5"), 2, 1, 5, history)}
                {correctOrNot(t("question2.a6"), 2, 1, 6, history)}
                {correctOrNot(t("question2.a7"), 2, 1, 7, history)}
                {correctOrNot(t("question2.a8"), 2, 1, 8, history)}
              </Grid>
              <br />
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question2.a9"), 2, 1, 9, history)}
                {correctOrNot(t("question2.a10"), 2, 1, 10, history)}
                {correctOrNot(t("question2.a11"), 2, 1, 11, history)}
                {correctOrNot(t("question2.a12"), 2, 1, 12, history)}
              </Grid>
              <br />
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question2.a13"), 2, 1, 13, history)}
                {correctOrNot(t("question2.a14"), 2, 1, 14, history)}
                {correctOrNot(t("question2.a15"), 2, 1, 15, history)}
                {correctOrNot(t("question2.a16"), 2, 1, 16, history)}
              </Grid>
              <br />
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question2.a17"), 2, 1, 17, history)}
                {correctOrNot(t("question2.a18"), 2, 1, 18, history)}
                {correctOrNot(t("question2.a19"), 2, 1, 19, history)}
                {correctOrNot(t("question2.a20"), 2, 1, 20, history)}
              </Grid>
            </Grid>
          </Grid>
          <Grid item style={headerContainerStyle}>
            <div>3. {t("question3.text")}</div>
            <br />
            <Grid item style={checkboxRowStyle}>
              {correctOrNot(t("question3.a1"), 3, 1, 1, history)}
              {correctOrNot(t("question3.a2"), 3, 2, 1, history)}
              {correctOrNot(t("question3.a3"), 3, 3, 1, history)}
              {correctOrNot(t("question3.a4"), 3, 4, 1, history)}
            </Grid>
          </Grid>
          <Grid item style={headerContainerStyle}>
            <div>4. {t("question4.text")}</div>
            <br />
            <Grid item>
              <div>
                {t("question4.v1")}
                {ecgMs && !ecgMsIsError && !ecgMsInfoLoading && !ecgMsIsFetching ? ecgMs.komor : "..."} ms
              </div>
              <div>
                {t("question4.v2")}
                {ecgMs && !ecgMsIsError && !ecgMsInfoLoading && !ecgMsIsFetching ? ecgMs.pr : "..."} ms
              </div>
              <div>
                {t("question4.v3")}
                {ecgMs && !ecgMsIsError && !ecgMsInfoLoading && !ecgMsIsFetching ? ecgMs.pq : "..."} ms
              </div>
              <div>
                {t("question4.v4")}
                {ecgMs && !ecgMsIsError && !ecgMsInfoLoading && !ecgMsIsFetching ? ecgMs.qt : "..."} ms
              </div>
              <div>
                {t("question4.v5")}
                {ecgMs && !ecgMsIsError && !ecgMsInfoLoading && !ecgMsIsFetching ? ecgMs.qtc : "..."} ms
              </div>
            </Grid>
          </Grid>

          <Grid item style={headerContainerStyle}>
            <div>5. {t("question5.text")}</div>
            <Grid item>{correctOrNot(t("question5.a1"), 5, 1, 1, history)}</Grid>
            <Grid item style={headerContainerStyle}>
              <div>5.1 {t("question5.a2")}</div>
              {multiQuestions(5, 2)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>5.2 {t("question5.a3")}</div>
              {multiQuestions(5, 3)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>5.3 {t("question5.a4")}</div>
              {multiQuestions(5, 4)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>5.4 {t("question5.a5")}</div>
              {multiQuestions(5, 5)}
            </Grid>
          </Grid>
          <Grid item style={headerContainerStyle}>
            <div>6. {t("question6.text")}</div>
            <Grid item>{correctOrNot(t("question6.a1"), 6, 1, 1, history)}</Grid>
            <Grid item style={headerContainerStyle}>
              <div>6.1 {t("question6.a2")}</div>
              {multiQuestions(6, 2)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>6.2 {t("question6.a3")}</div>
              {multiQuestions(6, 3)}
            </Grid>
          </Grid>
          <Grid item style={headerContainerStyle}>
            <div>7. {t("question7.text")}</div>
            <Grid item>{correctOrNot(t("question7.a1"), 7, 1, 1, history)}</Grid>
            <Grid item style={headerContainerStyle}>
              <div>7.1 {t("question7.a2")}</div>
              {multiQuestions(7, 2)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>7.2 {t("question7.a3")}</div>
              {multiQuestions(7, 3)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>7.3 {t("question7.a4")}</div>
              {multiQuestions(7, 4)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>7.4 {t("question7.a5")}</div>
              {multiQuestions(7, 5)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>7.5 {t("question7.a6")}</div>
              {multiQuestions(7, 6)}
            </Grid>
          </Grid>
          <Grid item style={headerContainerStyle}>
            <div>8. {t("question8.text")}</div>
            <Grid item>{correctOrNot(t("question8.a1"), 8, 1, 1, history)}</Grid>
            <Grid item style={headerContainerStyle}>
              <div>8.1 {t("question8.a2")}</div>
              {multiQuestions(8, 2)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>8.2 {t("question8.a3")}</div>
              {multiQuestions(8, 3)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>8.3 {t("question8.a4")}</div>
              {multiQuestions(8, 4)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>8.4 {t("question8.a5")}</div>
              {multiQuestions(8, 5)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>8.5 {t("question8.a6")}</div>
              {multiQuestions(8, 6)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>8.6 {t("question8.a7")}</div>
              {multiQuestions(8, 7)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>8.7 {t("question8.a8")}</div>
              {multiQuestions(8, 8)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>8.8 {t("question8.a9")}</div>
              {multiQuestions(8, 9)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>8.9 {t("question8.a10")}</div>
              {multiQuestions(8, 10)}
            </Grid>
          </Grid>
          <Grid item style={headerContainerStyle}>
            <div>9. {t("question9.text")}</div>
            <Grid item>{correctOrNot(t("question9.a1"), 9, 1, 1, history)}</Grid>
            <Grid item style={headerContainerStyle}>
              <div>9.1 {t("question9.a2")}</div>
              {multiQuestions(9, 2)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>9.2 {t("question9.a3")}</div>
              {multiQuestions(9, 3)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>9.3 {t("question9.a4")}</div>
              {multiQuestions(9, 4)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>9.4 {t("question9.a5")}</div>
              {multiQuestions(9, 5)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>9.5 {t("question9.a6")}</div>
              {multiQuestions(9, 6)}
            </Grid>
          </Grid>
          <Grid item style={headerContainerStyle}>
            <div>10. {t("question10.text")}</div>
            <Grid item>{correctOrNot(t("question10.a1"), 10, 1, 1, history)}</Grid>
            <Grid item style={headerContainerStyle}>
              <div>10.1 {t("question10.a2")}</div>
              {multiQuestions(10, 2)}
            </Grid>
          </Grid>
          <Grid item style={headerContainerStyle}>
            <div>11. {t("question11.text")}</div>
            <Grid item>{correctOrNot(t("question11.a1"), 11, 1, 1, history)}</Grid>
            <Grid item style={headerContainerStyle}>
              <div>11.1 {t("question11.a2")}</div>
              {correctOrNot(t("question11.a2"), 11, 2, 1, history)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>11.2 {t("question11.a2")}</div>
              {correctOrNot(t("question11.a3"), 11, 3, 1, history)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>11.3 {t("question11.a2")}</div>
              {correctOrNot(t("question11.a4"), 11, 4, 1, history)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>11.4 {t("question11.a2")}</div>
              {correctOrNot(t("question11.a5"), 11, 5, 1, history)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>11.5 {t("question11.a2")}</div>
              {correctOrNot(t("question11.a6"), 11, 6, 1, history)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>11.6 {t("question11.a2")}</div>
              {correctOrNot(t("question11.a7"), 11, 7, 1, history)}
            </Grid>
          </Grid>
          <Grid item style={headerContainerStyle}>
            <div>12. {t("question12.text")}</div>
            <Grid item>{correctOrNot(t("question11.a1"), 12, 1, 1, history)}</Grid>
            <Grid item style={headerContainerStyle}>
              <div>12.1 are ekstrasystolia komorowa and ekstrasystolia nadkomorowa choices?</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question12.a1"), 12, 2, 1, history)}
                {correctOrNot(t("question12.v1"), 12, 2, 2, history)}
                {correctOrNot(t("question12.v2"), 12, 2, 3, history)}
              </Grid>
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>12.2 are ekstrasystolia komorowa and ekstrasystolia nadkomorowa choices?</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question12.a2"), 12, 3, 1, history)}
                {correctOrNot(t("question12.v1"), 12, 3, 2, history)}
                {correctOrNot(t("question12.v2"), 12, 3, 3, history)}
              </Grid>
            </Grid>
          </Grid>
          <Grid item style={headerContainerStyle}>
            <div>13. {t("question13.text")}</div>
            <Grid item style={checkboxRowStyle}>
              {correctOrNot(t("question13.a1"), 13, 1, 1, history)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>13.1</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question13.a2"), 13, 2, 1, history)}
              </Grid>
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>13.2</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question13.a3"), 13, 3, 1, history)}
              </Grid>
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>13.3</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question13.a4"), 13, 4, 1, history)}
              </Grid>
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>13.4</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question13.a5"), 13, 5, 1, history)}
              </Grid>
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>13.5</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question13.a6"), 13, 6, 1, history)}
              </Grid>
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>13.6</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question13.a7"), 13, 7, 1, history)}
              </Grid>
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>13.7</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question13.a7"), 13, 8, 1, history)}
              </Grid>
            </Grid>
          </Grid>

          <Grid item style={headerContainerStyle}>
            <div>14. {t("question14.text")}</div>
            <Grid item style={checkboxRowStyle}>
              {correctOrNot(t("question14.a1"), 14, 1, 1, history)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>14.1 {t("question14.a2")}</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question14.v1"), 14, 2, 1, history)}
                {correctOrNot(t("question14.v2"), 14, 2, 2, history)}
                {correctOrNot(t("question14.v3"), 14, 2, 3, history)}
                {correctOrNot(t("question14.v4"), 14, 2, 4, history)}
              </Grid>
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>14.2 {t("question14.a3")}</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question14.v1"), 14, 3, 1, history)}
                {correctOrNot(t("question14.v2"), 14, 3, 2, history)}
                {correctOrNot(t("question14.v3"), 14, 3, 3, history)}
                {correctOrNot(t("question14.v4"), 14, 3, 4, history)}
              </Grid>
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>14.3 {t("question14.a4")}</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question14.v1"), 14, 4, 1, history)}
                {correctOrNot(t("question14.v2"), 14, 4, 2, history)}
                {correctOrNot(t("question14.v3"), 14, 4, 3, history)}
                {correctOrNot(t("question14.v4"), 14, 4, 4, history)}
              </Grid>
            </Grid>
          </Grid>

          <Grid item style={headerContainerStyle}>
            <div>15. {t("question15.text")}</div>
            <Grid item style={checkboxRowStyle}>
              {correctOrNot(t("question15.a1"), 15, 1, 1, history)}
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>15.1</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question15.a2"), 15, 2, 1, history)}
              </Grid>
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>15.2</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question15.a3"), 15, 3, 1, history)}
              </Grid>
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>15.3</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question15.a4"), 15, 4, 1, history)}
              </Grid>
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>15.4</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question15.a5"), 15, 5, 1, history)}
              </Grid>
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>15.5</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question15.a6"), 15, 6, 1, history)}
              </Grid>
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>15.6</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question15.a7"), 15, 7, 1, history)}
              </Grid>
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>15.7</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question15.a8"), 15, 8, 1, history)}
              </Grid>
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>15.8</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question15.a9"), 15, 9, 1, history)}
              </Grid>
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>15.9</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question15.a10"), 15, 10, 1, history)}
              </Grid>
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>15.10</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question15.a11"), 15, 11, 1, history)}
              </Grid>
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>15.11</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question15.a12"), 15, 12, 1, history)}
              </Grid>
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>15.12</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question15.a13"), 15, 13, 1, history)}
              </Grid>
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>15.13</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question15.a14"), 15, 14, 1, history)}
              </Grid>
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>15.14</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question15.a15"), 15, 15, 1, history)}
              </Grid>
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>15.15</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question15.a16"), 15, 16, 1, history)}
              </Grid>
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>15.16</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question15.a17"), 15, 17, 1, history)}
              </Grid>
            </Grid>
            <Grid item style={headerContainerStyle}>
              <div>15.17</div>
              <Grid item style={checkboxRowStyle}>
                {correctOrNot(t("question15.a18"), 15, 18, 1, history)}
              </Grid>
            </Grid>
          </Grid>
        </Grid>
      </div>
    </div>
  );
};

export default TaskHistory;
