import { Checkbox, FormControlLabel } from "@mui/material";
import { green, red, yellow } from "@mui/material/colors";
import { TaskHistoryResponseDTO } from "../../interfaces/interface";

const checkboxStyle = { width: "35vh", height: "10vh" };
export const checkedOrNot = (
    parentQuestionNumber: number,
    questionNumber: number,
    answerNumber: number,
    taskHistory: TaskHistoryResponseDTO
  ) => {
    let ans = false;
    for (let i = 0; i < taskHistory.cheatSheets.length; i++) {
      if (
        taskHistory.cheatSheets[i].parentQuestionNumber ===
          parentQuestionNumber &&
        taskHistory.cheatSheets[i].questionNumber === questionNumber &&
        taskHistory.cheatSheets[i].answerNumber === answerNumber
      ) {
        ans = true;
        break;
      }
    }
    let usrAns = false;
    for (let i = 0; i < taskHistory.userAnswers.length; i++) {
      if (
        taskHistory.userAnswers[i].parentQuestionNumber ===
          parentQuestionNumber &&
        taskHistory.userAnswers[i].questionNumber === questionNumber &&
        taskHistory.userAnswers[i].answerNumber === answerNumber
      ) {
        usrAns = true;
        break;
      }
    }
    return { ans, usrAns };
  };
  
  const hasCorrectHeader = (
    parentQuestionNumber: number,
    questionNumber: number,
    answerNumber: number,
    taskHistory: TaskHistoryResponseDTO
  ) => {
    let headerAns = false;
    let headerUsrAns = false;
    if (
      parentQuestionNumber <= 4 ||
      (questionNumber === 1 && answerNumber === 1)
    ) {
      return { headerAns, headerUsrAns };
    }
  
    for (let i = 0; i < taskHistory.cheatSheets.length; i++) {
      if (
        taskHistory.cheatSheets[i].parentQuestionNumber ===
          parentQuestionNumber &&
        taskHistory.cheatSheets[i].questionNumber === 1 &&
        taskHistory.cheatSheets[i].answerNumber === 1
      ) {
        headerAns = true;
        break;
      }
    }
  
    for (let i = 0; i < taskHistory.userAnswers.length; i++) {
      if (
        taskHistory.userAnswers[i].parentQuestionNumber ===
          parentQuestionNumber &&
        taskHistory.userAnswers[i].questionNumber === 1 &&
        taskHistory.userAnswers[i].answerNumber === 1
      ) {
        headerUsrAns = true;
        break;
      }
    }
    return { headerAns, headerUsrAns };
  };
  
  export const correctOrNot = (
    answerText: string,
    parentQuestionNumber: number,
    questionNumber: number,
    answerNumber: number,
    taskHistory: TaskHistoryResponseDTO
  ) => {
    const { ans, usrAns } = checkedOrNot(
      parentQuestionNumber,
      questionNumber,
      answerNumber,
      taskHistory
    );
    const { headerAns, headerUsrAns } = hasCorrectHeader(
      parentQuestionNumber,
      questionNumber,
      answerNumber,
      taskHistory
    );
  
  
    if (headerAns && headerUsrAns) {
      <div style={checkboxStyle}>
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
          label={answerText}
          checked
          labelPlacement="start"
        />
      </div>;
    }
    if (usrAns && ans) {
      return (
        <div style={checkboxStyle}>
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
            label={answerText}
            checked
            labelPlacement="start"
          />
        </div>
      );
    }
    if (ans && !usrAns) {
      return (
        <div style={checkboxStyle}>
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
            label={answerText}
            checked
            labelPlacement="start"
          />
        </div>
      );
    }
    if (!ans && usrAns) {
      return (
        <div style={checkboxStyle}>
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
            label={answerText}
            checked
            labelPlacement="start"
          />
        </div>
      );
    }
    if (!ans && !usrAns) {
      return (
        <div style={checkboxStyle}>
          <FormControlLabel
            control={<Checkbox />}
            label={answerText}
            disabled
            labelPlacement="start"
          />
        </div>
      );
    }
  };
  
  export const correctCalculation = (
    parentQuestionNumber: number,
    questionNumber: number,
    answerNumber: number,
    taskHistory: TaskHistoryResponseDTO
  ) => {
    let cheatIndex = 0;
    for (let i = 0; i < taskHistory.cheatSheets.length; i++) {
      if (
        taskHistory.cheatSheets[i].parentQuestionNumber ===
          parentQuestionNumber &&
        taskHistory.cheatSheets[i].questionNumber === questionNumber &&
        taskHistory.cheatSheets[i].answerNumber === answerNumber
      ) {
        cheatIndex = i;
        break;
      }
    }
    let userIndex = 0;
    for (let i = 0; i < taskHistory.userAnswers.length; i++) {
      if (
        taskHistory.userAnswers[i].parentQuestionNumber ===
          parentQuestionNumber &&
        taskHistory.userAnswers[i].questionNumber === questionNumber &&
        taskHistory.userAnswers[i].answerNumber === answerNumber
      ) {
        userIndex = i;
        break;
      }
    }
    if (
      taskHistory?.cheatSheets?.[cheatIndex]?.answer.trim().toLowerCase() ===
      taskHistory?.userAnswers?.[userIndex]?.answer.trim().toLowerCase()
    ) {
      return (
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
            label={taskHistory?.userAnswers?.[userIndex]?.answer}
            checked
            labelPlacement="start"
          />
        </div>
      );
    }
    return (
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
          label={taskHistory?.userAnswers?.[userIndex]?.answer ? taskHistory?.userAnswers?.[userIndex]?.answer : '--'}
          checked
          labelPlacement="start"
        />
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
          label={taskHistory?.cheatSheets?.[cheatIndex]?.answer}
          checked
          labelPlacement="start"
        />
      </div>
    );
  };
  