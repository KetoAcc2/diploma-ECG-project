import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { activateAccount } from "../../api/authApi";
import CircularIndeterminate from "../../components/CircularIndeterminate";
import { useTranslation } from "react-i18next";

const AccountActivation = () => {
  const { t } = useTranslation();
  const { activationToken } = useParams();
  const nav = useNavigate();
  const [statusCode, setStatusCode] = useState(0);
  if (!activationToken) {
    return <CircularIndeterminate />;
  }
  useEffect(() => {
    const api = async () => {
      const response = await activateAccount(activationToken);
      if (!response) {
        return;
      }
      setStatusCode(response.status);
    };
    api();
  }, []);
  if (statusCode == 200) {
    setTimeout(() => {
      nav("/");
    }, 5000);
  }
  if (statusCode !== 200) {
    return <CircularIndeterminate />;
  }
  return (
    <div>
      {t("success_redirect.text")}
      <CircularIndeterminate />
    </div>
  );
};

export default AccountActivation;
