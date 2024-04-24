import React, { createContext, useState } from "react";

interface ConfirmContextProp {
  prompt: string;
  isOpen: boolean;
  proceed: (value:any) => void;
  cancel: () => void;
}

interface ConfirmContextStateProp {
  confirm: ConfirmContextProp;
  setConfirm: React.Dispatch<React.SetStateAction<ConfirmContextProp>>;
}
export const ConfirmContext = createContext<ConfirmContextStateProp>({
  confirm: { prompt: "", isOpen: false, proceed: () => {}, cancel: () => {} },
  setConfirm: () => {},
});
const ConfirmContextProvider = ({ children }: { children: any }) => {
  const [confirm, setConfirm] = useState<ConfirmContextProp>({
    prompt: "",
    isOpen: false,
    proceed: () => {},
    cancel: () => {},
  });

  return (
    <ConfirmContext.Provider
      value={
        { confirm: confirm, setConfirm: setConfirm } as ConfirmContextStateProp
      }>
      {children}
    </ConfirmContext.Provider>
  );
};
export default ConfirmContextProvider;
