import { Checkbox, List, ListItem, ListItemButton, ListItemIcon, ListItemText } from "@mui/material";
import React from "react";
import { GroupsToAssignTaskProp } from "../../constants/reactTemplates";
import { AssignTasksForm } from "../../interfaces/interface";

const TaskGroupForm = (props: GroupsToAssignTaskProp) => {
    const { data, setForm } = props

    const [checked, setChecked] = React.useState<number[]>([]);

    const handleToggle = (value: number) => () => {
        const currentIndex = checked.indexOf(value);
        const newChecked = [...checked];

        if (currentIndex === -1) {
            newChecked.push(value);
        } else {
            newChecked.splice(currentIndex, 1);
        }
        setChecked(newChecked);
        setForm((prev: AssignTasksForm) => ({
            ...prev, groups: newChecked.reduce((acc: number[], curr: number) => {
                acc.push(data[curr].groupId)
                return acc;
            }, [])
        }))
    };

    return (
        <List sx={{ width: '100%', maxWidth: 360, bgcolor: 'background.paper' }}>
            {data.map((element, index) => {
                const labelId = `checkbox-list-label-${index}`;

                return (
                    <ListItem
                        key={index}
                        disablePadding
                    >
                        <ListItemButton role={undefined} onClick={handleToggle(index)} dense>
                            <ListItemIcon>
                                <Checkbox
                                    edge="start"
                                    checked={checked.indexOf(index) !== -1}
                                    tabIndex={-1}
                                    disableRipple
                                    inputProps={{ 'aria-labelledby': labelId }}
                                />
                            </ListItemIcon>
                            <ListItemText id={labelId} primary={element.groupName} />
                        </ListItemButton>
                    </ListItem>
                );
            })}
        </List>
    );
}

export default TaskGroupForm