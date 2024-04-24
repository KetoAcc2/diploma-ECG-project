import FirstPageIcon from "@mui/icons-material/FirstPage";
import KeyboardArrowLeft from "@mui/icons-material/KeyboardArrowLeft";
import KeyboardArrowRight from "@mui/icons-material/KeyboardArrowRight";
import LastPageIcon from "@mui/icons-material/LastPage";
import { Button } from "@mui/material";
import Box from "@mui/material/Box";
import IconButton from "@mui/material/IconButton";
import Paper from "@mui/material/Paper";
import { useTheme } from "@mui/material/styles";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableFooter from "@mui/material/TableFooter";
import TablePagination from "@mui/material/TablePagination";
import TableRow from "@mui/material/TableRow";
import * as React from "react";
import { useTranslation } from "react-i18next";
import { useMutation, UseMutationResult, useQueryClient } from "react-query";
import { removeTask } from "../../api/api";
import { tableHeaderCellStyle } from "../../assets/muiStyles/styles";
import CircularIndeterminate from "../../components/CircularIndeterminate";
import ErrorMessage from "../../components/ErrorMessage";
import ConfirmModal from "../../components/OnConfirmComponent/ConfirmModal";
import useConfirm from "../../components/OnConfirmComponent/useConfirm";
import { ITaskDTO } from "../../interfaces/interface";

interface TablePaginationActionsProps {
  count: number;
  page: number;
  rowsPerPage: number;
  onPageChange: (
    event: React.MouseEvent<HTMLButtonElement>,
    newPage: number
  ) => void;
}

function TablePaginationActions(props: TablePaginationActionsProps) {
  const theme = useTheme();
  const { count, page, rowsPerPage, onPageChange } = props;

  const handleFirstPageButtonClick = (
    event: React.MouseEvent<HTMLButtonElement>
  ) => {
    onPageChange(event, 0);
  };

  const handleBackButtonClick = (
    event: React.MouseEvent<HTMLButtonElement>
  ) => {
    onPageChange(event, page - 1);
  };

  const handleNextButtonClick = (
    event: React.MouseEvent<HTMLButtonElement>
  ) => {
    onPageChange(event, page + 1);
  };

  const handleLastPageButtonClick = (
    event: React.MouseEvent<HTMLButtonElement>
  ) => {
    onPageChange(event, Math.max(0, Math.ceil(count / rowsPerPage) - 1));
  };

  return (
    <Box sx={{ flexShrink: 0, ml: 2.5 }}>
      <IconButton
        onClick={handleFirstPageButtonClick}
        disabled={page === 0}
        aria-label="first page">
        {theme.direction === "rtl" ? <LastPageIcon /> : <FirstPageIcon />}
      </IconButton>
      <IconButton
        onClick={handleBackButtonClick}
        disabled={page === 0}
        aria-label="previous page">
        {theme.direction === "rtl" ? (
          <KeyboardArrowRight />
        ) : (
          <KeyboardArrowLeft />
        )}
      </IconButton>
      <IconButton
        onClick={handleNextButtonClick}
        disabled={page >= Math.ceil(count / rowsPerPage) - 1}
        aria-label="next page">
        {theme.direction === "rtl" ? (
          <KeyboardArrowLeft />
        ) : (
          <KeyboardArrowRight />
        )}
      </IconButton>
      <IconButton
        onClick={handleLastPageButtonClick}
        disabled={page >= Math.ceil(count / rowsPerPage) - 1}
        aria-label="last page">
        {theme.direction === "rtl" ? <FirstPageIcon /> : <LastPageIcon />}
      </IconButton>
    </Box>
  );
}

type RemoveParamMutate = {
  taskId: number;
  groupId: number;
};

export type RemoveTasksProps = {
  tasksByGroup: ITaskDTO[] | undefined;
  tasksByGroupLoading: boolean;
  tasksByGroupIsError: boolean;
};

const RemoveTasks = ({
  tasksByGroup: rows,
  tasksByGroupLoading,
  tasksByGroupIsError,
}: RemoveTasksProps) => {
  const queryClient = useQueryClient();
  const { t } = useTranslation();
  const { isConfirmed } = useConfirm();
  const tableHeader = tableHeaderCellStyle();
  const [page, setPage] = React.useState(0);
  const [rowsPerPage, setRowsPerPage] = React.useState(10);
  const rowsPerPageText = t("rows_per_page.text");
  if (!rows || tasksByGroupLoading) {
    return <CircularIndeterminate />;
  }
  if (tasksByGroupIsError) {
    return <ErrorMessage />;
  }
  const emptyRows =
    page > 0 ? Math.max(0, (1 + page) * rowsPerPage - rows.length) : 0;

  const handleChangePage = (
    event: React.MouseEvent<HTMLButtonElement> | null,
    newPage: number
  ) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (
    event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };
  const removeTaskGroupMutation: UseMutationResult<
    void,
    Error,
    RemoveParamMutate
  > = useMutation<void, Error, RemoveParamMutate>(
    async ({ taskId, groupId }) => {
      await removeTask(taskId, groupId);
      await queryClient.invalidateQueries("tasksInfoByGroup");
    }
  );
  const handleRemoveTask = async (
    event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
    obj: ITaskDTO
  ) => {
    const confirmed = await isConfirmed("questions_conf.text");
    if (confirmed) {
      await removeTaskGroupMutation.mutateAsync({
        taskId: obj.taskId,
        groupId: obj.groupId,
      });
      event.persist();
      await queryClient.refetchQueries("tasksInfoByGroup");
    }
  };
  return (
    <>
      <TableContainer sx={{ width: "100%" }} component={Paper}>
        {rows.length === 0 ? (
          <div style={{ padding: "20px 0 20px 0" }}>{t("no_tasks.text")}</div>
        ) : (
          <Table sx={{ minWidth: 500 }} aria-label="custom pagination table">
            <TableBody>
              <TableRow sx={{ backgroundColor: "#398CC7", color: "white" }}>
                <TableCell className={tableHeader.root}>
                  {t("task_number.text")}
                </TableCell>
                <TableCell className={tableHeader.root}>
                  {t("task_description.text")}
                </TableCell>
                <TableCell></TableCell>
              </TableRow>
              {(rowsPerPage > 0
                ? rows.slice(
                    page * rowsPerPage,
                    page * rowsPerPage + rowsPerPage
                  )
                : rows
              ).map((row, index) => (
                <TableRow key={index}>
                  <TableCell className={tableHeader.row}>
                    {row.taskId}
                  </TableCell>
                  <TableCell className={tableHeader.row}>
                    {row.taskDescription}
                  </TableCell>
                  <TableCell className={tableHeader.row}>
                    <Button
                      size="small"
                      variant="contained"
                      color="error"
                      onClick={async (e) => handleRemoveTask(e, row)}>
                      {t("remove.text")}
                    </Button>
                  </TableCell>
                </TableRow>
              ))}
              {emptyRows > 0 && (
                <TableRow style={{ height: 53 * emptyRows }}>
                  <TableCell colSpan={6} />
                </TableRow>
              )}
            </TableBody>
            <TableFooter>
              <TableRow>
                <TablePagination
                  labelRowsPerPage={rowsPerPageText}
                  rowsPerPageOptions={[10, 20, { label: "All", value: -1 }]}
                  colSpan={3}
                  count={rows.length}
                  rowsPerPage={rowsPerPage}
                  page={page}
                  SelectProps={{
                    inputProps: {
                      "aria-label": "rows per page",
                    },
                    native: true,
                  }}
                  onPageChange={handleChangePage}
                  onRowsPerPageChange={handleChangeRowsPerPage}
                  ActionsComponent={TablePaginationActions}
                />
              </TableRow>
            </TableFooter>
          </Table>
        )}
      </TableContainer>
      <ConfirmModal />
    </>
  );
};

export default RemoveTasks;
