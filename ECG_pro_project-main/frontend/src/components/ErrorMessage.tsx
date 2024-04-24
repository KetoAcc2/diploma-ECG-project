import { Block } from "@mui/icons-material";
import { Alert, AlertTitle } from "@mui/material";
import { useTranslation } from "react-i18next";

type ErrorMessageProp = {
  errorMessage?: string;
  errorTitle?: string;
  style?: React.CSSProperties;
};
const ErrorMessage = ({
  errorTitle,
  errorMessage,
  style = { width: "500px", marginLeft: "auto", marginRight: "auto" },
}: ErrorMessageProp) => {
  const { t } = useTranslation();
  return (
    <Alert severity="error" style={style}>
      <AlertTitle>{errorTitle ?? t("error.text")}</AlertTitle>
      {errorMessage ?? t("errors.default")}
    </Alert>
  );
};

export default ErrorMessage;
