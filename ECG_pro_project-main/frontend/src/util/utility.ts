import { GenericTableProp, IGroup, ITaskDTO, IUserDTO } from "../interfaces/interface";

export const groupToArray = (data: IGroup[]): GenericTableProp => {
  let row = { tableRows: [] } as GenericTableProp;

  data.forEach((x) => {
    row.tableRows.push({
      columns: [`${x.groupId.toString()}`, x.groupName],
    });
  });
  row.detailsUrl = "/group";
  row.removeUrl = "/group";
  return row;
};

export const usersToArray = (data: IUserDTO[]): GenericTableProp => {
  let row = { tableRows: [] } as GenericTableProp;
  data.forEach((x) => {
    row.tableRows.push({
      columns: [`${x.userId.toString()}`, x.email],
    });
  });
  row.detailsUrl = "/user";
  row.removeUrl = "/removeuserfromgroup";
  return row;
};

export const tasksToArray = (data: ITaskDTO[] | null | undefined): GenericTableProp => {
  if (data === undefined || data == null) {
    return { tableRows: [] } as GenericTableProp;
  }
  let row = { tableRows: [] } as GenericTableProp;
  data.forEach((x) => {
    row.tableRows.push({
      columns: [x.taskId.toString(), x.taskDescription, x.groupId.toString(),x.groupName],
    });
  });
  //TODO: might want remove url at some point
  return row;
};
