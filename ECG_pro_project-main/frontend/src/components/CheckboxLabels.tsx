import Checkbox from '@mui/material/Checkbox';
import FormControlLabel from '@mui/material/FormControlLabel';
import FormGroup from '@mui/material/FormGroup';

export default function CheckboxLabels(props:string) {
  return (
    <FormGroup>
      <FormControlLabel control={<Checkbox defaultChecked />} label={props} />
    </FormGroup>
  );
}
