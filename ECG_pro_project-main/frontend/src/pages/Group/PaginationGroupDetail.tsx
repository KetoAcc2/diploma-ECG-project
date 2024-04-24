import FirstPageIcon from "@mui/icons-material/FirstPage";
import KeyboardArrowLeft from "@mui/icons-material/KeyboardArrowLeft";
import KeyboardArrowRight from "@mui/icons-material/KeyboardArrowRight";
import LastPageIcon from "@mui/icons-material/LastPage";
import { Button } from "@mui/material";
import Box from "@mui/material/Box";
import IconButton from "@mui/material/IconButton";
import Paper from "@mui/material/Paper";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableFooter from "@mui/material/TableFooter";
import TablePagination from "@mui/material/TablePagination";
import TableRow from "@mui/material/TableRow";
import { useTheme } from "@mui/material/styles";
import * as React from "react";
import { FC, useState } from "react";
import { useTranslation } from "react-i18next";
import { UseMutationResult, useMutation, useQueryClient } from "react-query";
import { Link } from "react-router-dom";
import { removeGroup } from "../../api/api";
import { tableHeaderCellStyle } from "../../assets/muiStyles/styles";
import ConfirmModal from "../../components/OnConfirmComponent/ConfirmModal";
import useConfirm from "../../components/OnConfirmComponent/useConfirm";
import ResponsiveDialog, {
  DialogType,
} from "../../components/ResponsiveDialog";
import { isTeacher } from "../../constants/reactTemplates";
import { TablePropWrapper } from "../../interfaces/interface";

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

export type RemoveParamMutateGroupManagement = {
  groupId: number;
};

const PaginationGroupDetail: FC<TablePropWrapper> = ({
  genericTable,
  headers,
}: TablePropWrapper) => {
  const queryClient = useQueryClient();
  const { t } = useTranslation();
  const { isConfirmed } = useConfirm();
  const [openDialog, setOpenDialog] = useState(false);
  const [dialogText, setDialogText] = useState("");
  const tableHeader = tableHeaderCellStyle();
  const tableRows = genericTable.tableRows;
  if (tableRows === null || tableRows === undefined) {
    return <div>{t("no_data.text")}</div>;
  }
  const rows = tableRows;
  const [page, setPage] = React.useState(0);
  const [rowsPerPage, setRowsPerPage] = React.useState(5);
  const rowsPerPageText = t("rows_per_page.text");

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
  const removeGroupMutation: UseMutationResult<
    void,
    Error,
    RemoveParamMutateGroupManagement
  > = useMutation<void, Error, RemoveParamMutateGroupManagement>(
    async ({ groupId }) => {
      await removeGroup(groupId);
      await queryClient.invalidateQueries("groupManagementInfo");
    },
    {
      onMutate: () => {},
      onError: () => {
        setOpenDialog(true);
        setDialogText("erros.default");
      },
      onSettled: () => {},
      onSuccess: () => {
        setOpenDialog(true);
        setDialogText("success.text");
      },
    }
  );

  return (
    <>
      <ResponsiveDialog
        isOpen={openDialog}
        setIsOpen={setOpenDialog}
        centerText={dialogText}
        dialogType={DialogType.ALERT}
      />
      <TableContainer sx={{ width: "100%", float: "right" }} component={Paper}>
        {rows.length === 0 ? (
          <div style={{ padding: "20px 0 20px 0" }}>{t("no_groups.text")}</div>
        ) : (
          <Table sx={{ minWidth: 500 }} aria-label="custom pagination table">
            <TableBody>
              <TableRow sx={{ backgroundColor: "#398CC7", color: "white" }}>
                <TableCell className={tableHeader.root}>
                  {t(headers.columnOne)}
                </TableCell>
                <TableCell className={tableHeader.root}>
                  {t(headers.columnTwo)}
                </TableCell>
                <TableCell></TableCell>
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
                  {row.columns.map((column, index) => (
                    <TableCell
                      className={tableHeader.row}
                      key={column}
                      component="th"
                      scope="row">
                      {column}
                    </TableCell>
                  ))}
                  {isTeacher() &&
                    genericTable.detailsUrl !== undefined &&
                    genericTable.removeUrl !== undefined && (
                      <>
                        <TableCell
                          className={tableHeader.row}
                          component="th"
                          scope="row">
                          <Button
                            size="small"
                            component={Link}
                            variant="contained"
                            to={`${genericTable.detailsUrl}/${row.columns[0]}`}>
                            {t("detail.text")}
                          </Button>
                        </TableCell>
                        <TableCell
                          className={tableHeader.row}
                          component="th"
                          scope="row">
                          <Button
                            size="small"
                            variant="contained"
                            color="error"
                            onClick={async (e) => {
                              const confirmed = await isConfirmed(
                                "questions_conf.text"
                              );
                              if (confirmed) {
                                await removeGroupMutation.mutateAsync({
                                  groupId: +row.columns[0],
                                });
                                e.persist();
                                queryClient.refetchQueries(
                                  "groupManagementInfo"
                                );
                              }
                            }}>
                            {t("remove.text")}
                          </Button>
                        </TableCell>
                      </>
                    )}
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
                  rowsPerPageOptions={[5, 10, { label: "All", value: -1 }]}
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

export default PaginationGroupDetail;
