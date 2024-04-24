import { Button } from "@mui/material";
import { useTranslation } from "react-i18next";

const TaskDescription = (props: { ecgId: number | undefined; setPage: React.Dispatch<React.SetStateAction<number>> }) => {
    const { ecgId, setPage } = props;
    const { t } = useTranslation();
    const handleSubmit = () => {
        setPage(1);
    }
    if (!ecgId) {
         
        return (
            <Button color="primary" variant="contained" onClick={handleSubmit}>
                START
            </Button>
        );
    }



    return (
        <div style={{ margin:"auto",maxWidth:"90vh" }}>
            <div style={{ textAlign: "left", marginLeft: "50px", marginRight: "50px" }}>
                <span>{
                    t(`ecg_info.ecg_desc${ecgId}`).split('\n').map((line, index) => <span key={index} style={{ wordWrap: "break-word" }}>{line}<br/></span>)
                }</span>
            </div>
            <br />
            <Button color="primary" variant="contained" onClick={handleSubmit}>
                START
            </Button>
        </div>
    );
};


export default TaskDescription;