import { Divider, Paper, Typography } from "@mui/material";
import { useTranslation } from "react-i18next";
import { CardDivStyle } from "../interfaces/interface";
import { useUserDetailQuery } from "../queries/useUserDetailQuery";

import UserDetail from "./UserDetail";

const UserDetailCard = ({ style }: CardDivStyle) => {
  const { userData, userLoading, userIsError } = useUserDetailQuery();
  const { t } = useTranslation();
  return (
    <div style={style}>
      <Paper style={{ width: "100%", padding: "10px" }}>
        <div>
          <Typography variant="h6" style={{ color: "#636363" }}>
            {t("account_info.text")}
          </Typography>
        </div>
        <Divider />
        <UserDetail
          userData={userData}
          userLoading={userLoading}
          userIsError={userIsError}
        />
      </Paper>
    </div>
  );
};

export default UserDetailCard;
