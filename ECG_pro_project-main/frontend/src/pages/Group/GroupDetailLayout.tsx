import ConfirmContextProvider from "../../components/OnConfirmComponent/confirmContext";
import MainLayout from "../MainLayout/MainLayout";
import GroupDetail from "./GroupDetail";

const GroupDetailLayout = () => {
  return (
    <ConfirmContextProvider>
      <MainLayout>
        <GroupDetail />
      </MainLayout>
    </ConfirmContextProvider>
  );
};

export default GroupDetailLayout;
