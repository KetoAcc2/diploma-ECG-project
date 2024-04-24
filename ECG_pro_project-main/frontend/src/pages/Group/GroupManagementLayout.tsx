import ConfirmContextProvider from "../../components/OnConfirmComponent/confirmContext";
import MainLayout from "../MainLayout/MainLayout";
import GroupManagement from "./GroupManagement";

const GroupManagementLayout = () => {
  return (
    <ConfirmContextProvider>
      <MainLayout>
        <GroupManagement />
      </MainLayout>
    </ConfirmContextProvider>
  );
};

export default GroupManagementLayout;
