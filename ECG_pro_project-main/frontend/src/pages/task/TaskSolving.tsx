import ExpandLessIcon from "@mui/icons-material/ExpandLess";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import { Button, Checkbox, Collapse, FormControl, FormControlLabel, Grid, ListItemButton, MenuItem, Select, TextField } from "@mui/material";
import { useCallback, useEffect, useRef, useState } from "react";
import { useTranslation } from "react-i18next";
import { submitTask } from "../../api/api";
import ConfirmationButton from "../../components/ConfirmationButton";
import LoadPic from "../../components/LoadPic";
import ResponsiveDialog, { DialogType } from "../../components/ResponsiveDialog";
import { headerContainerStyle } from "../../constants/reactTemplates";
import { AnswerStructure } from "../../interfaces/interface";
import useGetECGMs from "../../queries/useGetECGMs";

const defaultShouldCollapse: { [collapseKey: number]: boolean } = {
  1: false,
  2: true,
  3: true,
  4: true,
  5: true,
  6: true,
  7: true,
  8: true,
  9: true,
  10: true,
  11: true,
  12: true,
  13: true,
  14: true,
  15: true,
};

const TaskSolving = (props: { taskId: number; setPage: React.Dispatch<React.SetStateAction<number>> }) => {
  const { taskId, setPage } = props;
  const { t } = useTranslation();
  const [shouldCollapse, setShouldCollapse] = useState(defaultShouldCollapse);
  const [taskAnswers, setTaskAnswers] = useState<AnswerStructure[]>([]);
  const [openDialog, setOpenDialog] = useState(false);
  const [dialogText, setDialogText] = useState("");
  const [question1, setQuestion1] = useState("");
  const [question2, setQuestion2] = useState("");
  const { ecgMs, ecgMsIsError, ecgMsInfoLoading, ecgMsError, ecgMsIsFetching } = useGetECGMs(+taskId);

  const handleSubmit = async () => {
    const ans1 = {
      parentQuestionNumber: 1,
      questionNumber: 1,
      answerNumber: 1,
      ecgDiagramId: +taskId,
      answer: question1,
    } as AnswerStructure;
    const ans2 = {
      parentQuestionNumber: 2,
      questionNumber: 1,
      answerNumber: +question2,
      ecgDiagramId: +taskId,
      answer: "true",
    } as AnswerStructure;

    const tmpMap = new Map<number, AnswerStructure[]>();
    for (let index = 0; index < taskAnswers.length; index++) {
      if (!tmpMap.has(taskAnswers[index].parentQuestionNumber)) {
        const tmpArr: Array<AnswerStructure> = [];
        for (let j = 0; j < taskAnswers.length; j++) {
          if (taskAnswers[j].parentQuestionNumber == taskAnswers[index].parentQuestionNumber) {
            tmpArr.push(taskAnswers[j]);
          }
        }

        tmpMap.set(taskAnswers[index].parentQuestionNumber, tmpArr);
      }
    }

    let newArr = [];
    for (const [key, value] of tmpMap) {
      if (key >= 5 && key <= 15) {
        let tmpAnswer: AnswerStructure | undefined = undefined;
        for (let index = 0; index < value.length; index++) {
          if (value[index].questionNumber == 1) {
            tmpAnswer = value[index];
            newArr.push(tmpAnswer);
            break;
          }
        }

        if (!tmpAnswer) {
          newArr.push(...value);
        }
      }

      if (key < 5) {
        newArr.push(...value);
      }
    }

    if (question1 !== "") {
      newArr.push(ans1);
    }
    if (question2 !== "") {
      newArr.push(ans2);
    }
     
    const response = await submitTask(newArr, +taskId);
     
    if (response === null) {
      setOpenDialog(true)
      setDialogText("please_try_again.text")
      return;
    }
    setPage(2);
  };
  const handleCollapse = (parentQuestionNumber: number) => {
    if (!shouldCollapse[parentQuestionNumber]) {
      let newDict = shouldCollapse;
      newDict[parentQuestionNumber] = !newDict[parentQuestionNumber];
      setShouldCollapse((prev) => ({ ...newDict }));
      return;
    }
    let prevCollapse = shouldCollapse[parentQuestionNumber];
    let newDict = shouldCollapse;
    for (let key in newDict) {
      if (+key !== parentQuestionNumber) {
        newDict[key] = prevCollapse;
      }
    }
    newDict[parentQuestionNumber] = !prevCollapse;
    setShouldCollapse((prev) => ({ ...newDict }));
  };
  const handleValue = (parentQuestionNumber: number, questionNumber: number, answerNumber: number) => {
    var result = false;
    for (let i = 0; i < taskAnswers.length; i++) {
      if (taskAnswers[i].parentQuestionNumber === parentQuestionNumber && taskAnswers[i].questionNumber === questionNumber && taskAnswers[i].answerNumber === answerNumber) {
        result = true;
        break;
      }
    }

    return result;
  };
  const handleSetTaskAnswers = (parentQuestionNumber: number, questionNumber: number, answerNumber: number, ecgDiagramId: number, answer: string) => {
    const tempAnswer = {
      parentQuestionNumber: parentQuestionNumber,
      questionNumber: questionNumber,
      answerNumber: answerNumber,
      ecgDiagramId: ecgDiagramId,
      answer: answer,
    } as AnswerStructure;
    let toRemove = -1;
    const falseBool = false;
    for (let i = 0; i < taskAnswers.length; i++) {
      if (taskAnswers[i].parentQuestionNumber === parentQuestionNumber && taskAnswers[i].questionNumber === questionNumber && taskAnswers[i].answerNumber === answerNumber && taskAnswers[i].ecgDiagramId === ecgDiagramId) {
        if (answer === falseBool.toString()) {
          toRemove = i;
          break;
        }
        let newArr = [...taskAnswers];
        newArr[i].answer = answer;
        setTaskAnswers(newArr);
        return;
      }
    }
    if (toRemove > -1) {
      let newArr = [];
      for (let i = 0; i < taskAnswers.length; i++) {
        if (i === toRemove) {
          continue;
        }
        newArr.push(taskAnswers[i]);
      }
      setTaskAnswers(newArr);
      return;
    }
    setTaskAnswers((prev) => [...prev, tempAnswer]);
  };
  const multiQuestions = (parentQuestionNumber: number, questionNumber: number) => {
    return (
      <Grid item>
        <FormControlLabel control={<Checkbox checked={handleValue(parentQuestionNumber, questionNumber, 1)} onChange={(e) => handleSetTaskAnswers(parentQuestionNumber, questionNumber, 1, +taskId, e.target.checked.toString())} />} label="I" labelPlacement="top" />
        <FormControlLabel control={<Checkbox checked={handleValue(parentQuestionNumber, questionNumber, 2)} onChange={(e) => handleSetTaskAnswers(parentQuestionNumber, questionNumber, 2, +taskId, e.target.checked.toString())} />} label="II" labelPlacement="top" />
        <FormControlLabel control={<Checkbox checked={handleValue(parentQuestionNumber, questionNumber, 3)} onChange={(e) => handleSetTaskAnswers(parentQuestionNumber, questionNumber, 3, +taskId, e.target.checked.toString())} />} label="III" labelPlacement="top" />
        <FormControlLabel control={<Checkbox checked={handleValue(parentQuestionNumber, questionNumber, 4)} onChange={(e) => handleSetTaskAnswers(parentQuestionNumber, questionNumber, 4, +taskId, e.target.checked.toString())} />} label="aVR" labelPlacement="top" />
        <FormControlLabel control={<Checkbox checked={handleValue(parentQuestionNumber, questionNumber, 5)} onChange={(e) => handleSetTaskAnswers(parentQuestionNumber, questionNumber, 5, +taskId, e.target.checked.toString())} />} label="aVL" labelPlacement="top" />
        <FormControlLabel control={<Checkbox checked={handleValue(parentQuestionNumber, questionNumber, 6)} onChange={(e) => handleSetTaskAnswers(parentQuestionNumber, questionNumber, 6, +taskId, e.target.checked.toString())} />} label="aVF" labelPlacement="top" />
        <FormControlLabel control={<Checkbox checked={handleValue(parentQuestionNumber, questionNumber, 7)} onChange={(e) => handleSetTaskAnswers(parentQuestionNumber, questionNumber, 7, +taskId, e.target.checked.toString())} />} label="V1" labelPlacement="top" />
        <FormControlLabel control={<Checkbox checked={handleValue(parentQuestionNumber, questionNumber, 8)} onChange={(e) => handleSetTaskAnswers(parentQuestionNumber, questionNumber, 8, +taskId, e.target.checked.toString())} />} label="V3" labelPlacement="top" />
        <FormControlLabel control={<Checkbox checked={handleValue(parentQuestionNumber, questionNumber, 9)} onChange={(e) => handleSetTaskAnswers(parentQuestionNumber, questionNumber, 9, +taskId, e.target.checked.toString())} />} label="V3" labelPlacement="top" />
        <FormControlLabel control={<Checkbox checked={handleValue(parentQuestionNumber, questionNumber, 10)} onChange={(e) => handleSetTaskAnswers(parentQuestionNumber, questionNumber, 10, +taskId, e.target.checked.toString())} />} label="V4" labelPlacement="top" />
        <FormControlLabel control={<Checkbox checked={handleValue(parentQuestionNumber, questionNumber, 11)} onChange={(e) => handleSetTaskAnswers(parentQuestionNumber, questionNumber, 11, +taskId, e.target.checked.toString())} />} label="V5" labelPlacement="top" />
        <FormControlLabel control={<Checkbox checked={handleValue(parentQuestionNumber, questionNumber, 12)} onChange={(e) => handleSetTaskAnswers(parentQuestionNumber, questionNumber, 12, +taskId, e.target.checked.toString())} />} label="V6" labelPlacement="top" />
      </Grid>
    );
  };

  const imageRef = useRef<HTMLDivElement>(null);

  const handleScroll = useCallback(() => {
    imageRef.current && (imageRef.current.style.top = window.pageYOffset + "px");
  }, []);

  useEffect(() => {
    window.addEventListener("scroll", handleScroll);
    return () => window.removeEventListener("scroll", handleScroll);
  }, [taskId]);

  const [imageOpen, setImageOpen] = useState(false);
  const handleToggleImage = useCallback((doOpen: boolean) => {
    setImageOpen(doOpen);
  }, []);

  const submitRef = useRef(null);

  return (
    <div>
      <ResponsiveDialog
        isOpen={openDialog}
        setIsOpen={setOpenDialog}
        centerText={dialogText}
        dialogType={DialogType.ALERT}
      />
      <div style={{ display: "grid", gridTemplateColumns: "3fr 1fr", gridTemplateRows: "1fr" }}>
        <Grid container spacing={4} justifyContent="center" direction="column" style={{ marginLeft: "50px", maxWidth: "87%", marginRight: "auto", overflow: "auto", gridRow: "1/2", gridColumn: "1/2" }}>
          <div>
            <div style={{ textAlign: "left", marginLeft: "50px", maxWidth: "450px" }}>
              <span>
                {t(`ecg_info.ecg_desc${ecgMs?.ecgId}`)
                  .split("\n")
                  .map((line, index) => (
                    <span key={index} style={{ wordWrap: "break-word" }}>
                      {line}
                      <br />
                    </span>
                  ))}
              </span>
            </div>
            <br />
          </div>
          <Grid item style={headerContainerStyle}>
            <ListItemButton style={{ backgroundColor: "#F3F3F3" }} onClick={() => handleCollapse(1)}>
              <div>
                {!shouldCollapse[1] ? <ExpandLessIcon /> : <ExpandMoreIcon />} 1. {t("question1.text")}
              </div>
            </ListItemButton>
            <br />
            <Collapse in={!shouldCollapse[1]} timeout="auto" unmountOnExit>
              <TextField id="answer1" value={question1} label={t("question1.text2")} onChange={(e) => setQuestion1(e.target.value)} />
            </Collapse>
          </Grid>

          <Grid item style={headerContainerStyle}>
            <ListItemButton style={{ backgroundColor: "#F3F3F3" }} onClick={() => handleCollapse(2)}>
              <div>
                {!shouldCollapse[2] ? <ExpandLessIcon /> : <ExpandMoreIcon />} 2. {t("question2.text")}
              </div>
            </ListItemButton>
            <br />
            <Collapse in={!shouldCollapse[2]} timeout="auto" unmountOnExit>
              <FormControl>
                <Select style={{ width: "500px" }} id="question2-select-label" value={question2} inputProps={{ "aria-label": "Without label" }} onChange={(e) => setQuestion2(e.target.value)}>
                  <MenuItem value={1}>{t("question2.a1")}</MenuItem>
                  <MenuItem value={2}>{t("question2.a2")}</MenuItem>
                  <MenuItem value={3}>{t("question2.a3")}</MenuItem>
                  <MenuItem value={4}>{t("question2.a4")}</MenuItem>
                  <MenuItem value={5}>{t("question2.a5")}</MenuItem>
                  <MenuItem value={6}>{t("question2.a6")}</MenuItem>
                  <MenuItem value={7}>{t("question2.a7")}</MenuItem>
                  <MenuItem value={8}>{t("question2.a8")}</MenuItem>
                  <MenuItem value={9}>{t("question2.a9")}</MenuItem>
                  <MenuItem value={10}>{t("question2.a10")}</MenuItem>
                  <MenuItem value={11}>{t("question2.a11")}</MenuItem>
                  <MenuItem value={12}>{t("question2.a12")}</MenuItem>
                  <MenuItem value={13}>{t("question2.a13")}</MenuItem>
                  <MenuItem value={14}>{t("question2.a14")}</MenuItem>
                  <MenuItem value={15}>{t("question2.a15")}</MenuItem>
                  <MenuItem value={16}>{t("question2.a16")}</MenuItem>
                  <MenuItem value={17}>{t("question2.a17")}</MenuItem>
                  <MenuItem value={18}>{t("question2.a18")}</MenuItem>
                  <MenuItem value={19}>{t("question2.a19")}</MenuItem>
                  <MenuItem value={29}>{t("question2.a20")}</MenuItem>
                </Select>
              </FormControl>
            </Collapse>
          </Grid>
          <Grid item style={headerContainerStyle}>
            <ListItemButton style={{ backgroundColor: "#F3F3F3" }} onClick={() => handleCollapse(3)}>
              <div>
                {!shouldCollapse[3] ? <ExpandLessIcon /> : <ExpandMoreIcon />} 3. {t("question3.text")}
              </div>
            </ListItemButton>

            <br />
            <Collapse in={!shouldCollapse[3]} timeout="auto" unmountOnExit>
              <Grid item>
                <FormControlLabel control={<Checkbox checked={handleValue(3, 1, 1)} onChange={(e) => handleSetTaskAnswers(3, 1, 1, +taskId, e.target.checked.toString())} />} label={t("question3.a1")} labelPlacement="top" />
                <FormControlLabel control={<Checkbox checked={handleValue(3, 1, 2)} onChange={(e) => handleSetTaskAnswers(3, 1, 2, +taskId, e.target.checked.toString())} />} label={t("question3.a2")} labelPlacement="top" />
                <FormControlLabel control={<Checkbox checked={handleValue(3, 1, 3)} onChange={(e) => handleSetTaskAnswers(3, 1, 3, +taskId, e.target.checked.toString())} />} label={t("question3.a3")} labelPlacement="top" />
                <FormControlLabel control={<Checkbox checked={handleValue(3, 1, 4)} onChange={(e) => handleSetTaskAnswers(3, 1, 4, +taskId, e.target.checked.toString())} />} label={t("question3.a4")} labelPlacement="top" />
              </Grid>
            </Collapse>
          </Grid>
          <Grid item style={headerContainerStyle}>
            <ListItemButton style={{ backgroundColor: "#F3F3F3" }} onClick={() => handleCollapse(4)}>
              <div>
                {!shouldCollapse[4] ? <ExpandLessIcon /> : <ExpandMoreIcon />} 4. {t("question4.text")}
              </div>
            </ListItemButton>
            <br />
            <Collapse in={!shouldCollapse[4]} timeout="auto" unmountOnExit>
              <Grid item>
                <div>
                  {t("question4.v1")} {ecgMs && !ecgMsIsError && !ecgMsInfoLoading && !ecgMsIsFetching ? ecgMs.komor : "..."} ms
                </div>
                <div>
                  {t("question4.v2")} {ecgMs && !ecgMsIsError && !ecgMsInfoLoading && !ecgMsIsFetching ? ecgMs.pr : "..."} ms
                </div>
                <div>
                  {t("question4.v3")} {ecgMs && !ecgMsIsError && !ecgMsInfoLoading && !ecgMsIsFetching ? ecgMs.pq : "..."} ms
                </div>
                <div>
                  {t("question4.v4")} {ecgMs && !ecgMsIsError && !ecgMsInfoLoading && !ecgMsIsFetching ? ecgMs.qt : "..."} ms
                </div>
                <div>
                  {t("question4.v5")} {ecgMs && !ecgMsIsError && !ecgMsInfoLoading && !ecgMsIsFetching ? ecgMs.qtc : "..."} ms
                </div>
              </Grid>
            </Collapse>
          </Grid>

          <Grid item style={headerContainerStyle}>
            <ListItemButton style={{ backgroundColor: "#F3F3F3" }} onClick={() => handleCollapse(5)}>
              <div>
                {!shouldCollapse[5] ? <ExpandLessIcon /> : <ExpandMoreIcon />} 5. {t("question5.text")}
              </div>
            </ListItemButton>
            <br />
            <Collapse in={!shouldCollapse[5]} timeout="auto" unmountOnExit>
              <Grid item>
                <Grid item>
                  <FormControlLabel control={<Checkbox checked={handleValue(5, 1, 1)} onChange={(e) => handleSetTaskAnswers(5, 1, 1, +taskId, e.target.checked.toString())} />} label={t("question5.a1")} labelPlacement="start" />
                </Grid>
              </Grid>
              <br />

              <Grid item>
                <div>5.1 {t("question5.a2")}</div>
                <br />
                {multiQuestions(5, 2)}
              </Grid>
              <br />
              <Grid item>
                <div>5.2 {t("question5.a3")}</div>
                <br />
                {multiQuestions(5, 3)}
              </Grid>
              <Grid item>
                <div>5.3 {t("question5.a4")}</div>
                <br />
                {multiQuestions(5, 4)}
              </Grid>
              <Grid item>
                <div>5.4 {t("question5.a5")}</div>
                <br />
                {multiQuestions(5, 5)}
              </Grid>
            </Collapse>
          </Grid>
          <Grid item style={headerContainerStyle}>
            <ListItemButton style={{ backgroundColor: "#F3F3F3" }} onClick={() => handleCollapse(6)}>
              <div>
                {!shouldCollapse[6] ? <ExpandLessIcon /> : <ExpandMoreIcon />} 6. {t("question6.text")}
              </div>
            </ListItemButton>
            <br />
            <Collapse in={!shouldCollapse[6]} timeout="auto" unmountOnExit>
              <Grid item>
                <Grid item>
                  <FormControlLabel control={<Checkbox checked={handleValue(6, 1, 1)} onChange={(e) => handleSetTaskAnswers(6, 1, 1, +taskId, e.target.checked.toString())} />} label={t("question6.a1")} labelPlacement="start" />
                </Grid>
              </Grid>
              <Grid item>
                <div>6.1 {t("question6.a2")}</div>
                <br />
                {multiQuestions(6, 2)}
              </Grid>
              <Grid item>
                <div>6.2 {t("question6.a3")}</div>
                <br />
                {multiQuestions(6, 3)}
              </Grid>
            </Collapse>
          </Grid>
          <Grid item style={headerContainerStyle}>
            <ListItemButton style={{ backgroundColor: "#F3F3F3" }} onClick={() => handleCollapse(7)}>
              <div>
                {!shouldCollapse[7] ? <ExpandLessIcon /> : <ExpandMoreIcon />} 7. {t("question7.text")}
              </div>
            </ListItemButton>
            <br />
            <Collapse in={!shouldCollapse[7]} timeout="auto" unmountOnExit>
              <Grid item>
                <Grid item>
                  <FormControlLabel control={<Checkbox checked={handleValue(7, 1, 1)} onChange={(e) => handleSetTaskAnswers(7, 1, 1, +taskId, e.target.checked.toString())} />} label={t("question7.a1")} labelPlacement="start" />
                </Grid>
              </Grid>
              <Grid item>
                <div>7.1 {t("question7.a2")}</div>
                <br />
                {multiQuestions(7, 2)}
              </Grid>
              <Grid item>
                <div>7.2 {t("question7.a3")}</div>
                <br />
                {multiQuestions(7, 3)}
              </Grid>
              <Grid item>
                <div>7.3 {t("question7.a4")}</div>
                <br />
                {multiQuestions(7, 4)}
              </Grid>
              <Grid item>
                <div>7.4 {t("question7.a5")}</div>
                <br />
                {multiQuestions(7, 5)}
              </Grid>
              <Grid item>
                <div>7.5 {t("question7.a6")}</div>
                <br />
                {multiQuestions(7, 6)}
              </Grid>
            </Collapse>
          </Grid>
          <Grid item style={headerContainerStyle}>
            <ListItemButton style={{ backgroundColor: "#F3F3F3" }} onClick={() => handleCollapse(8)}>
              <div>
                {!shouldCollapse[8] ? <ExpandLessIcon /> : <ExpandMoreIcon />} 8. {t("question8.text")}
              </div>
            </ListItemButton>

            <br />
            <Collapse in={!shouldCollapse[8]} timeout="auto" unmountOnExit>
              <Grid item>
                <Grid item>
                  <FormControlLabel control={<Checkbox onChange={(e) => handleSetTaskAnswers(8, 1, 1, +taskId, e.target.checked.toString())} />} label={t("question8.a1")} labelPlacement="start" />
                </Grid>
              </Grid>
              <Grid item>
                <div>8.1 {t("question8.a2")}</div>
                <br />
                {multiQuestions(8, 2)}
              </Grid>
              <Grid item>
                <div>8.2 {t("question8.a3")}</div>
                <br />
                {multiQuestions(8, 3)}
              </Grid>
              <Grid item>
                <div>8.3 {t("question8.a4")}</div>
                <br />
                {multiQuestions(8, 4)}
              </Grid>
              <Grid item>
                <div>8.4 {t("question8.a5")}</div>
                <br />
                {multiQuestions(8, 5)}
              </Grid>
              <Grid item>
                <div>8.5 {t("question8.a6")}</div>
                <br />
                {multiQuestions(8, 6)}
              </Grid>
              <Grid item>
                <div>8.6 {t("question8.a7")}</div>
                <br />
                {multiQuestions(8, 7)}
              </Grid>
              <Grid item>
                <div>8.7 {t("question8.a8")}</div>
                <br />
                {multiQuestions(8, 8)}
              </Grid>
              <Grid item>
                <div>8.8 {t("question8.a9")}</div>
                <br />
                {multiQuestions(8, 9)}
              </Grid>
              <Grid item>
                <div>8.9 {t("question8.a10")}</div>
                <br />
                {multiQuestions(8, 10)}
              </Grid>
            </Collapse>
          </Grid>
          <Grid item style={headerContainerStyle}>
            <ListItemButton style={{ backgroundColor: "#F3F3F3" }} onClick={() => handleCollapse(9)}>
              <div>
                {!shouldCollapse[9] ? <ExpandLessIcon /> : <ExpandMoreIcon />} 9. {t("question9.text")}
              </div>
            </ListItemButton>

            <br />
            <Collapse in={!shouldCollapse[9]} timeout="auto" unmountOnExit>
              <Grid item>
                <Grid item>
                  <FormControlLabel control={<Checkbox onChange={(e) => handleSetTaskAnswers(9, 1, 1, +taskId, e.target.checked.toString())} />} label={t("question9.a1")} labelPlacement="start" />
                </Grid>
              </Grid>
              <Grid item>
                <div>9.1 {t("question9.a2")}</div>
                <br />
                {multiQuestions(9, 2)}
              </Grid>
              <Grid item>
                <div>9.2 {t("question9.a3")}</div>
                <br />
                {multiQuestions(9, 3)}
              </Grid>
              <Grid item>
                <div>9.3 {t("question9.a4")}</div>
                <br />
                {multiQuestions(9, 4)}
              </Grid>
              <Grid item>
                <div>9.4 {t("question9.a5")}</div>
                <br />
                {multiQuestions(9, 5)}
              </Grid>
              <Grid item>
                <div>9.5 {t("question9.a6")}</div>
                <br />
                {multiQuestions(9, 6)}
              </Grid>
            </Collapse>
          </Grid>

          <Grid item style={headerContainerStyle}>
            <ListItemButton style={{ backgroundColor: "#F3F3F3" }} onClick={() => handleCollapse(10)}>
              <div>
                {!shouldCollapse[10] ? <ExpandLessIcon /> : <ExpandMoreIcon />} 10. {t("question10.text")}
              </div>
            </ListItemButton>

            <br />
            <Collapse in={!shouldCollapse[10]} timeout="auto" unmountOnExit>
              <Grid item>
                <Grid item>
                  <FormControlLabel control={<Checkbox onChange={(e) => handleSetTaskAnswers(10, 1, 1, +taskId, e.target.checked.toString())} />} label={t("question10.a1")} labelPlacement="start" />
                </Grid>
              </Grid>
              <Grid item>
                <div>10.1 {t("question10.a2")}</div>
                <br />
                {multiQuestions(10, 2)}
              </Grid>
            </Collapse>
          </Grid>
          <Grid item style={headerContainerStyle}>
            <ListItemButton style={{ backgroundColor: "#F3F3F3" }} onClick={() => handleCollapse(11)}>
              <div>
                {!shouldCollapse[11] ? <ExpandLessIcon /> : <ExpandMoreIcon />} 11. {t("question11.text")}
              </div>
              <br />
            </ListItemButton>
            <br />
            <Collapse in={!shouldCollapse[11]} timeout="auto" unmountOnExit>
              <Grid item>
                <Grid item>
                  <FormControlLabel control={<Checkbox checked={handleValue(11, 1, 1)} onChange={(e) => handleSetTaskAnswers(11, 1, 1, +taskId, e.target.checked.toString())} />} label={t("question11.a1")} labelPlacement="start" />
                </Grid>
              </Grid>
              <Grid item>
                <div>11.1 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(11, 2, 1)} onChange={(e) => handleSetTaskAnswers(11, 2, 1, +taskId, e.target.checked.toString())} />} label={t("question11.a2")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>11.2 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(11, 3, 1)} onChange={(e) => handleSetTaskAnswers(11, 3, 1, +taskId, e.target.checked.toString())} />} label={t("question11.a3")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>11.3 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(11, 4, 1)} onChange={(e) => handleSetTaskAnswers(11, 4, 1, +taskId, e.target.checked.toString())} />} label={t("question11.a4")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>11.4 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(11, 5, 1)} onChange={(e) => handleSetTaskAnswers(11, 5, 1, +taskId, e.target.checked.toString())} />} label={t("question11.a5")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>11.5 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(11, 6, 1)} onChange={(e) => handleSetTaskAnswers(11, 6, 1, +taskId, e.target.checked.toString())} />} label={t("question11.a6")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>11.6 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(11, 7, 1)} onChange={(e) => handleSetTaskAnswers(11, 7, 1, +taskId, e.target.checked.toString())} />} label={t("question11.a7")} labelPlacement="start" />
              </Grid>
            </Collapse>
          </Grid>
          <Grid item style={headerContainerStyle}>
            <ListItemButton style={{ backgroundColor: "#F3F3F3" }} onClick={() => handleCollapse(12)}>
              <div>
                {!shouldCollapse[12] ? <ExpandLessIcon /> : <ExpandMoreIcon />} 12. {t("question12.text")}
              </div>
            </ListItemButton>

            <br />
            <Collapse in={!shouldCollapse[12]} timeout="auto" unmountOnExit>
              <Grid item>
                <Grid item>
                  <FormControlLabel control={<Checkbox checked={handleValue(12, 1, 1)} onChange={(e) => handleSetTaskAnswers(12, 1, 1, +taskId, e.target.checked.toString())} />} label={t("question12.z1")} labelPlacement="start" />
                </Grid>
              </Grid>
              <Grid item>
                <div>12.1 {t("question12.a2")}</div>
                <br />
                <Grid item>
                  <FormControlLabel control={<Checkbox checked={handleValue(12, 2, 1)} onChange={(e) => handleSetTaskAnswers(12, 2, 1, +taskId, e.target.checked.toString())} />} label={t("question12.v1")} labelPlacement="start" />
                  <FormControlLabel control={<Checkbox checked={handleValue(12, 2, 2)} onChange={(e) => handleSetTaskAnswers(12, 2, 2, +taskId, e.target.checked.toString())} />} label={t("question12.v2")} labelPlacement="start" />
                  <FormControlLabel control={<Checkbox checked={handleValue(12, 2, 3)} onChange={(e) => handleSetTaskAnswers(12, 2, 3, +taskId, e.target.checked.toString())} />} label={t("question12.v3")} labelPlacement="start" />
                </Grid>
              </Grid>
              <Grid item>
                <div>12.2 {t("question12.a2")}</div>
                <br />
                <Grid item>
                  <FormControlLabel control={<Checkbox checked={handleValue(12, 3, 1)} onChange={(e) => handleSetTaskAnswers(12, 3, 1, +taskId, e.target.checked.toString())} />} label={t("question12.v1")} labelPlacement="start" />
                  <FormControlLabel control={<Checkbox checked={handleValue(12, 3, 2)} onChange={(e) => handleSetTaskAnswers(12, 3, 2, +taskId, e.target.checked.toString())} />} label={t("question12.v2")} labelPlacement="start" />
                  <FormControlLabel control={<Checkbox checked={handleValue(12, 3, 3)} onChange={(e) => handleSetTaskAnswers(12, 3, 3, +taskId, e.target.checked.toString())} />} label={t("question12.v3")} labelPlacement="start" />
                </Grid>
              </Grid>
            </Collapse>
          </Grid>
          <Grid item style={headerContainerStyle}>
            <ListItemButton style={{ backgroundColor: "#F3F3F3" }} onClick={() => handleCollapse(13)}>
              <div>
                {!shouldCollapse[13] ? <ExpandLessIcon /> : <ExpandMoreIcon />} 13. {t("question13.text")}
              </div>
            </ListItemButton>
            <br />
            <Collapse in={!shouldCollapse[13]} timeout="auto" unmountOnExit>
              <Grid item>
                <Grid item>
                  <FormControlLabel control={<Checkbox checked={handleValue(13, 1, 1)} onChange={(e) => handleSetTaskAnswers(13, 1, 1, +taskId, e.target.checked.toString())} />} label={t("question13.a1")} labelPlacement="start" />
                </Grid>
              </Grid>
              <Grid item>
                <div>13.1 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(13, 2, 1)} onChange={(e) => handleSetTaskAnswers(13, 2, 1, +taskId, e.target.checked.toString())} />} label={t("question13.a2")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>13.2 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(13, 3, 1)} onChange={(e) => handleSetTaskAnswers(13, 3, 1, +taskId, e.target.checked.toString())} />} label={t("question13.a3")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>13.3 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(13, 4, 1)} onChange={(e) => handleSetTaskAnswers(13, 4, 1, +taskId, e.target.checked.toString())} />} label={t("question13.a4")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>13.4 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(13, 5, 1)} onChange={(e) => handleSetTaskAnswers(13, 5, 1, +taskId, e.target.checked.toString())} />} label={t("question13.a5")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>13.5 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(13, 6, 1)} onChange={(e) => handleSetTaskAnswers(13, 6, 1, +taskId, e.target.checked.toString())} />} label={t("question13.a6")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>13.6 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(13, 7, 1)} onChange={(e) => handleSetTaskAnswers(13, 7, 1, +taskId, e.target.checked.toString())} />} label={t("question13.a7")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>13.7 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(13, 8, 1)} onChange={(e) => handleSetTaskAnswers(13, 8, 1, +taskId, e.target.checked.toString())} />} label={t("question13.a8")} labelPlacement="start" />
              </Grid>
            </Collapse>
          </Grid>
          <Grid item style={headerContainerStyle}>
            <ListItemButton style={{ backgroundColor: "#F3F3F3" }} onClick={() => handleCollapse(14)}>
              <div>
                {!shouldCollapse[14] ? <ExpandLessIcon /> : <ExpandMoreIcon />} 14. {t("question14.text")}
              </div>
            </ListItemButton>

            <br />
            <Collapse in={!shouldCollapse[14]} timeout="auto" unmountOnExit>
              <Grid item>
                <Grid item>
                  <FormControlLabel control={<Checkbox checked={handleValue(14, 1, 1)} onChange={(e) => handleSetTaskAnswers(14, 1, 1, +taskId, e.target.checked.toString())} />} label={t("question14.a1")} labelPlacement="start" />
                </Grid>
              </Grid>
              <Grid item>
                <div>14.1 {t("question14.a2")}</div>
                <br />
                <Grid item>
                  <FormControlLabel control={<Checkbox checked={handleValue(14, 2, 1)} onChange={(e) => handleSetTaskAnswers(14, 2, 1, +taskId, e.target.checked.toString())} />} label={t("question14.v1")} labelPlacement="start" />
                  <FormControlLabel control={<Checkbox checked={handleValue(14, 2, 2)} onChange={(e) => handleSetTaskAnswers(14, 2, 2, +taskId, e.target.checked.toString())} />} label={t("question14.v2")} labelPlacement="start" />
                  <FormControlLabel control={<Checkbox checked={handleValue(14, 2, 3)} onChange={(e) => handleSetTaskAnswers(14, 2, 3, +taskId, e.target.checked.toString())} />} label={t("question14.v3")} labelPlacement="start" />
                  <FormControlLabel control={<Checkbox checked={handleValue(14, 2, 4)} onChange={(e) => handleSetTaskAnswers(14, 2, 4, +taskId, e.target.checked.toString())} />} label={t("question14.v4")} labelPlacement="start" />
                </Grid>
              </Grid>
              <Grid item>
                <div>14.2 {t("question14.a3")}</div>
                <br />
                <Grid item>
                  <FormControlLabel control={<Checkbox checked={handleValue(14, 3, 1)} onChange={(e) => handleSetTaskAnswers(14, 3, 1, +taskId, e.target.checked.toString())} />} label={t("question14.v1")} labelPlacement="start" />
                  <FormControlLabel control={<Checkbox checked={handleValue(14, 3, 2)} onChange={(e) => handleSetTaskAnswers(14, 3, 2, +taskId, e.target.checked.toString())} />} label={t("question14.v2")} labelPlacement="start" />
                  <FormControlLabel control={<Checkbox checked={handleValue(14, 3, 3)} onChange={(e) => handleSetTaskAnswers(14, 3, 3, +taskId, e.target.checked.toString())} />} label={t("question14.v3")} labelPlacement="start" />
                  <FormControlLabel control={<Checkbox checked={handleValue(14, 3, 4)} onChange={(e) => handleSetTaskAnswers(14, 3, 4, +taskId, e.target.checked.toString())} />} label={t("question14.v4")} labelPlacement="start" />
                </Grid>
              </Grid>
              <Grid item>
                <div>14.3 {t("question14.a4")}</div>
                <br />
                <Grid item>
                  <FormControlLabel control={<Checkbox checked={handleValue(14, 4, 1)} onChange={(e) => handleSetTaskAnswers(14, 4, 1, +taskId, e.target.checked.toString())} />} label={t("question14.v1")} labelPlacement="start" />
                  <FormControlLabel control={<Checkbox checked={handleValue(14, 4, 2)} onChange={(e) => handleSetTaskAnswers(14, 4, 2, +taskId, e.target.checked.toString())} />} label={t("question14.v2")} labelPlacement="start" />
                  <FormControlLabel control={<Checkbox checked={handleValue(14, 4, 3)} onChange={(e) => handleSetTaskAnswers(14, 4, 3, +taskId, e.target.checked.toString())} />} label={t("question14.v3")} labelPlacement="start" />
                  <FormControlLabel control={<Checkbox checked={handleValue(14, 4, 4)} onChange={(e) => handleSetTaskAnswers(14, 4, 4, +taskId, e.target.checked.toString())} />} label={t("question14.v4")} labelPlacement="start" />
                </Grid>
              </Grid>
              <Grid item>
                <div>14.4 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(14, 5, 1)} onChange={(e) => handleSetTaskAnswers(14, 5, 1, +taskId, e.target.checked.toString())} />} label={t("question14.a5")} labelPlacement="start" />
              </Grid>
            </Collapse>
          </Grid>
          <Grid item style={headerContainerStyle}>
            <ListItemButton style={{ backgroundColor: "#F3F3F3" }} onClick={() => handleCollapse(15)}>
              <div>
                {!shouldCollapse[15] ? <ExpandLessIcon /> : <ExpandMoreIcon />} 15. {t("question15.text")}
              </div>
            </ListItemButton>
            <br />
            <Collapse in={!shouldCollapse[15]} timeout="auto" unmountOnExit>
              <Grid item>
                <Grid item>
                  <FormControlLabel control={<Checkbox checked={handleValue(15, 1, 1)} onChange={(e) => handleSetTaskAnswers(15, 1, 1, +taskId, e.target.checked.toString())} />} label={t("question15.a1")} labelPlacement="start" />
                </Grid>
              </Grid>
              <Grid item>
                <div>15.1 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(15, 2, 1)} onChange={(e) => handleSetTaskAnswers(15, 2, 1, +taskId, e.target.checked.toString())} />} label={t("question15.a2")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>15.2 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(15, 3, 1)} onChange={(e) => handleSetTaskAnswers(15, 3, 1, +taskId, e.target.checked.toString())} />} label={t("question15.a3")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>15.3 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(15, 4, 1)} onChange={(e) => handleSetTaskAnswers(15, 4, 1, +taskId, e.target.checked.toString())} />} label={t("question15.a4")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>15.4 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(15, 5, 1)} onChange={(e) => handleSetTaskAnswers(15, 5, 1, +taskId, e.target.checked.toString())} />} label={t("question15.a5")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>15.5 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(15, 6, 1)} onChange={(e) => handleSetTaskAnswers(15, 6, 1, +taskId, e.target.checked.toString())} />} label={t("question15.a6")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>15.6 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(15, 7, 1)} onChange={(e) => handleSetTaskAnswers(15, 7, 1, +taskId, e.target.checked.toString())} />} label={t("question15.a7")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>15.7 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(15, 8, 1)} onChange={(e) => handleSetTaskAnswers(15, 8, 1, +taskId, e.target.checked.toString())} />} label={t("question15.a8")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>15.8 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(15, 9, 1)} onChange={(e) => handleSetTaskAnswers(15, 9, 1, +taskId, e.target.checked.toString())} />} label={t("question15.a9")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>15.9 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(15, 10, 1)} onChange={(e) => handleSetTaskAnswers(15, 10, 1, +taskId, e.target.checked.toString())} />} label={t("question15.a10")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>15.10 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(15, 11, 1)} onChange={(e) => handleSetTaskAnswers(15, 11, 1, +taskId, e.target.checked.toString())} />} label={t("question15.a11")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>15.11</div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(15, 12, 1)} onChange={(e) => handleSetTaskAnswers(15, 12, 1, +taskId, e.target.checked.toString())} />} label={t("question15.a12")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>15.12 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(15, 13, 1)} onChange={(e) => handleSetTaskAnswers(15, 13, 1, +taskId, e.target.checked.toString())} />} label={t("question15.a13")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>15.13 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(15, 14, 1)} onChange={(e) => handleSetTaskAnswers(15, 14, 1, +taskId, e.target.checked.toString())} />} label={t("question15.a14")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>15.14 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(15, 15, 1)} onChange={(e) => handleSetTaskAnswers(15, 15, 1, +taskId, e.target.checked.toString())} />} label={t("question15.a15")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>15.15 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(15, 16, 1)} onChange={(e) => handleSetTaskAnswers(15, 16, 1, +taskId, e.target.checked.toString())} />} label={t("question15.a16")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>15.16 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(15, 17, 1)} onChange={(e) => handleSetTaskAnswers(15, 17, 1, +taskId, e.target.checked.toString())} />} label={t("question15.a17")} labelPlacement="start" />
              </Grid>
              <Grid item>
                <div>15.17 </div>
                <br />
                <FormControlLabel control={<Checkbox checked={handleValue(15, 18, 1)} onChange={(e) => handleSetTaskAnswers(15, 18, 1, +taskId, e.target.checked.toString())} />} label={t("question15.a18")} labelPlacement="top" />
              </Grid>
            </Collapse>
          </Grid>
        </Grid>
        <div style={{ height: "100%", overflow: "auto", top: 0, gridRow: "1/2", gridColumn: "2/3" }}>
          <div ref={imageRef} style={{ position: "sticky", top: 0, right: 0 }}>
            <LoadPic taskId={+taskId} open={imageOpen} handleToggleImage={handleToggleImage} />
          </div>
        </div>
      </div>
      <br />
      <Button color="error" variant="contained" style={{ marginBottom: "15px" }} ref={submitRef}>
        {t("submit.text")}
      </Button>
      <ConfirmationButton buttonRef={submitRef} submitMethod={handleSubmit} />
    </div>
  );
};

export default TaskSolving;
