import { Button, Dialog, DialogActions, DialogTitle, useMediaQuery, useTheme } from "@mui/material";
import { useCallback, useEffect, useState } from "react";
import { useTranslation } from "react-i18next";

const ConfirmationButton = ({ buttonRef, submitMethod, methodParam }: { buttonRef: any, submitMethod: any, methodParam?: any | undefined }) => {
    const { t } = useTranslation();

    const theme = useTheme();
    const fullScreen = useMediaQuery(theme.breakpoints.down('md'));
    const [submitOpen, setSubmitOpen] = useState(false);
    const handleToggleSubmitOpen = useCallback((doOpen: boolean) => {
        setSubmitOpen(doOpen);
    }, []);

    useEffect(() => {
        if (buttonRef.current) {
            buttonRef.current.addEventListener('click', () => handleToggleSubmitOpen(true));
        }
    }, [buttonRef]);

    return (
        <div>
            <Dialog
                fullScreen={fullScreen}
                open={submitOpen}
                onClose={() => handleToggleSubmitOpen(false)}
                aria-labelledby="responsive-dialog-title"
            >
                <DialogTitle id="responsive-dialog-title">
                    {t("questions_conf.text")}
                </DialogTitle>
                <DialogActions>
                    <Button color="error" variant="contained" onClick={() => { handleToggleSubmitOpen(false); (methodParam == undefined) ? submitMethod() : submitMethod(methodParam)}} autoFocus>
                        {t("questions_conf.yes")}
                    </Button>
                    <Button variant="contained" autoFocus onClick={() => handleToggleSubmitOpen(false)}>
                        {t("questions_conf.no")}
                    </Button>
                </DialogActions>
            </Dialog>
        </div>
    );
};

export default ConfirmationButton;