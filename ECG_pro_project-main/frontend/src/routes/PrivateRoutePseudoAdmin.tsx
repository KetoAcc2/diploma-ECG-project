import { Navigate, Outlet } from "react-router-dom";
import { userRole } from "../constants/constants";
import { getUserData } from "../constants/reactTemplates";

const PrivateRoutePseudoAdmin = () => {
    const isPseudoAdmin = ()=>{
        const isPseudoAdminTmp = getUserData()
        if(isPseudoAdminTmp !== userRole.PSEUDO_ADMIN){
            return false;
        }
        return true
    }
    return isPseudoAdmin() ? (
        <>
        <Outlet/></>
    ):<Navigate to="/controller"/>
};

export default PrivateRoutePseudoAdmin;
