import ConfirmationResponsiveDialog from "./ConfirmationResponsiveDialog";
import useConfirm from "./useConfirm";
const ConfirmModal = () => {
  const { prompt = "", isOpen = false, proceed, cancel } = useConfirm();
  return (
    <ConfirmationResponsiveDialog
      isOpen={isOpen}
      centerText={prompt}
      onConfirm={proceed}
      onCancel={cancel}
    />
  );
};
export default ConfirmModal;
