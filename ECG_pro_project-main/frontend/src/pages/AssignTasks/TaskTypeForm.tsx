import { Checkbox, FormControl, FormControlLabel, FormLabel, List, ListItem, ListItemButton, ListItemIcon, ListItemText, Paper, Radio, RadioGroup } from "@mui/material";
import React from "react";
import { AssignTaskFormProp } from "../../constants/reactTemplates";
import { AssignTasksForm } from "../../interfaces/interface";
import { useTranslation } from "react-i18next";

const TaskTypeForm = (props: AssignTaskFormProp) => {
  const { data, setForm } = props;
  const { t } = useTranslation();

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
     
    setForm((prev: AssignTasksForm) => ({ ...prev, tasks: [(event.target as HTMLInputElement).value] }));
  };

  return (
    <FormControl>
      <FormLabel id="demo-controlled-radio-buttons-group">{t("assign_tasks.task_types")}</FormLabel>
      <RadioGroup style={{ marginLeft: "20px" }} sx={{ width: "50vh", bgcolor: "background.paper", scrollBehavior: "smooth", justifyContent: "center" }} aria-labelledby="demo-controlled-radio-buttons-group" name="controlled-radio-buttons-group" onChange={handleChange}>
        {data.map((e, i) => {
          return <FormControlLabel key={i} value={e.questionTypeId} control={<Radio />} label={e.questionTypeText} />;
        })}
      </RadioGroup>
    </FormControl>
  );
};

export default TaskTypeForm;
