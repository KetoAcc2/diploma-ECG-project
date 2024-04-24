import { Button, Grid, Typography } from "@mui/material";
import { t } from "i18next";
import { useQueryClient } from "react-query";
import { regenGroupCode } from "../../api/api";
import CircularIndeterminate from "../../components/CircularIndeterminate";
import ErrorMessage from "../../components/ErrorMessage";
import { IGroupCodeDTO, RegenGroupCodeDTO } from "../../interfaces/interface";

type GroupCodeProps = {
  style?: React.CSSProperties;
  groupId: number;
  groupCodeInfoLoading: boolean;
  groupCodeIsError: boolean;
  groupCode?: IGroupCodeDTO | null | undefined;
};

const GroupCode = ({
  groupId,
  groupCodeInfoLoading,
  groupCodeIsError,
  groupCode,
  style,
}: GroupCodeProps) => {
  const queryClient = useQueryClient();
  const handleGroupCodeRegen = async () => {
    const success = await regenGroupCode({
      groupId: groupId,
    } as RegenGroupCodeDTO);
    if (success) {
       
    }
    queryClient.refetchQueries("groupCode");
  };

  if (!groupCode || groupCodeInfoLoading) {
    return <CircularIndeterminate />;
  }
  if (groupCodeIsError) {
    return <ErrorMessage />;
  }
  return (
    <Grid style={style}>
      <Grid item>
        <Typography variant="h4" style={{ margin: 0, display: "inline" }}>{`${t(
          "group_code.text"
        )}: ${groupCode.groupCode}`}</Typography>
      </Grid>
      <Grid item>
        <Button variant="outlined" onClick={handleGroupCodeRegen}>
          {t("regen_group_code.text")}
        </Button>
      </Grid>
    </Grid>
  );
};

export default GroupCode;
