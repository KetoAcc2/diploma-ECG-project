import i18n from "i18next";
import LanguageDetector from "i18next-browser-languagedetector";
import { initReactI18next, useTranslation } from "react-i18next";
import { pageText } from "./pageText";

const DETECTION_OPTION = {
  order: ["localStorage", "navigator"],
  caches: ["localStorage"],
};

i18n
  .use(initReactI18next)
  .use(LanguageDetector)
  .init({
    resources: {
      en: {
        translation: pageText.en,
      },
      pl: {
        translation: pageText.pl,
      },
    },
    detection: DETECTION_OPTION,
    fallbackLng: "pl",
    interpolation: { escapeValue: false },
    keySeparator: ".",
  });

export default i18n;
