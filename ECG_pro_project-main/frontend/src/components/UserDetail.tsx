import { Tooltip } from "@mui/material";
import { useTranslation } from "react-i18next";
import { IUserDTO } from "../interfaces/interface";
import CircularIndeterminate from "./CircularIndeterminate";
import ErrorMessage from "./ErrorMessage";

type UserDetailProp = {
  userData: IUserDTO | undefined;
  userLoading: boolean;
  userIsError: boolean;
};

const UserDetail = ({ userData, userLoading, userIsError }: UserDetailProp) => {
  const { t } = useTranslation();

  if (!userData || userLoading) {
    return <CircularIndeterminate />;
  }
  if (userIsError) {
    return <ErrorMessage />;
  }
  return (
    <div
      style={{
        textAlign: "left",
        padding: "5px 20px 5px 20px",
        marginTop: "5px",
        borderRadius: "5px",
      }}>
      <div>ID: {userData?.userId}</div>
      <Tooltip placement="right" title={`${userData?.email}`}>
        <div
          style={{
            textOverflow: "ellipsis",
            whiteSpace: "nowrap",
            overflow: "hidden",
          }}>
          Email: {userData?.email}
        </div>
      </Tooltip>
      <div>
        {t("role.text")}: {userData?.role}
      </div>
    </div>
  );
};

export default UserDetail;
