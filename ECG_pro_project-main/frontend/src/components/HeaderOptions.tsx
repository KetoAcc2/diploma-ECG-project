import TranslateIcon from "@mui/icons-material/Translate";
import { Box, Button, Typography } from "@mui/material";
import { useTranslation } from "react-i18next";
import { useLocation, useNavigate } from "react-router-dom";
import ArrowBackIosNewIcon from '@mui/icons-material/ArrowBackIosNew';
import { apiPath, pageInfo } from "../constants/constants";


const HeaderOptions = () => {
  const { t, i18n } = useTranslation();
  const navigate = useNavigate();
  const location = useLocation();
  const languageHandler = () => {
    let preferredLng: String | null = localStorage.getItem("i18nextLng");
    if (preferredLng != "en" && preferredLng != "pl") {
      i18n.changeLanguage("pl");
    } else if (preferredLng == "en") {
      i18n.changeLanguage("pl");
    } else {
      i18n.changeLanguage("en");
    }
  };

  const isHidden = location.pathname === pageInfo.login.url || location.pathname === pageInfo.register.url;

  return (
    <div>
      <Box m={1} display="flex" justifyContent="flex-end" alignItems="flex-end">
        <Button
          onClick={() => navigate(-1)}
          variant="outlined"
          size="medium"
          startIcon={<ArrowBackIosNewIcon />}
          style={{
            borderRadius: 30,
            backgroundColor: "#FFFFFF",
            margin: "Calc(1%)",
            display: `${(isHidden) ? "none" : "inline-flex"}`
          }}>
          <Typography style={{ color: "#398BC7", fontSize: "12px" }}>
            {t("go_back.text")}
          </Typography>
        </Button>
        <Button
          onClick={languageHandler}
          variant="outlined"
          size="medium"
          startIcon={<TranslateIcon />}
          style={{
            borderRadius: 30,
            backgroundColor: "#FFFFFF",
            margin: "Calc(1%)",
          }}
        >
          <Typography style={{ color: "#398BC7", fontSize: "12px" }}>
            {t("switch_lng.text")}
          </Typography>
        </Button>
      </Box>
    </div>
  );
};

export default HeaderOptions;
