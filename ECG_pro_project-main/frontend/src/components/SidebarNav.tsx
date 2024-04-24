import ExpandLessIcon from "@mui/icons-material/ExpandLess";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import { Box, Collapse, CssBaseline, Divider, Drawer, List, ListItem, ListItemButton, ListItemIcon, ListItemText, Toolbar } from "@mui/material";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import { Link } from "react-router-dom";
import styles from "../assets/styles/sidebarNav.module.css";
import { localStorageKeys, userRole } from "../constants/constants";
import { getUserData, sidebarMenuPseudoAdmin, sidebarMenuStudent, sidebarMenuTeacher } from "../constants/reactTemplates";
import { ISidebarMenu, SidebarDropmenuStatus } from "../interfaces/interface";
import LogoutButton from "./LogoutButton";

const getSidebarUser = (): number => {
  try {
    return getUserData();
  } catch (error) {
    return userRole.UNKNOWN;
  }
};

const sidebar = [{} as ISidebarMenu, sidebarMenuStudent, sidebarMenuTeacher, {} as ISidebarMenu, sidebarMenuPseudoAdmin];

//page render starts here
const SidebarNav = () => {
  const { t } = useTranslation();
  const [sidebarMenu, setSidebarMenu] = useState(sidebar[getSidebarUser()]);
  const initializeMeunState = (): SidebarDropmenuStatus => {
    if (sidebarMenu.sidebar === null || sidebarMenu.sidebar === undefined) {
      return {};
    }
    return Object.assign(
      {} as SidebarDropmenuStatus,
      ...sidebarMenu.sidebar
        .filter((obj) => {
          if (obj.dropDown !== undefined) {
            return true;
          }
          return false;
        })
        .map((obj) => ({ [obj.identifier]: false }))
    );
  };

  const [opened, setOpened] = useState(initializeMeunState());

  const userData = localStorage.getItem(localStorageKeys.USER_PERSISTANCE);
  if (userData === undefined || userData === null) {
    return <div></div>;
  }

  const handleClick = (menuIndex: number) => {
    const identifier = sidebarMenu.sidebar[menuIndex].identifier;
    setOpened((prev) => ({
      ...prev,
      [identifier]: { opened: !opened[identifier].opened },
    }));
  };

  const conditionalDropdown = (menuIndex: number) => {
    const identifier = sidebarMenu.sidebar[menuIndex].identifier;
    if (sidebarMenu.sidebar[menuIndex].url !== undefined) {
      return (
        <ListItem key={menuIndex} disablePadding component={Link} to={sidebarMenu.sidebar[menuIndex].url!} color="inherit" style={{ textDecoration: "none", color: "black" }}>
          <ListItemButton>
            <ListItemIcon>{sidebarMenu.sidebar[menuIndex].icon}</ListItemIcon>
            <ListItemText primary={t(`${sidebarMenu.sidebar[menuIndex].text}`)} />
          </ListItemButton>
        </ListItem>
      );
    }
    return (
      <ListItem key={menuIndex} disablePadding onClick={() => handleClick(menuIndex)}>
        <ListItemButton>
          <ListItemIcon>{sidebarMenu.sidebar[menuIndex].icon}</ListItemIcon>
          <ListItemText primary={t(`${sidebarMenu.sidebar[menuIndex].text}`)} />
          <ListItemIcon>{opened[identifier].opened ? <ExpandLessIcon /> : <ExpandMoreIcon />}</ListItemIcon>
        </ListItemButton>
      </ListItem>
    );
  };
  return (
    <div>
      <Box sx={{ display: "flex" }}>
        <CssBaseline />
        <Drawer
          sx={{
            flexShrink: 0,
            "& .MuiDrawer-paper": {
              boxSizing: "border-box",
              backgroundColor: "#F3F3F3",
            },
          }}
          className={styles.siderbarDrawer}
          variant="permanent"
          anchor="left">
          <Toolbar />
          <Divider />
          <List style={{ minWidth: "235px" }}>
            {sidebarMenu.sidebar !== undefined &&
              sidebarMenu.sidebar.map((obj, index) => (
                <div key={index}>
                  {conditionalDropdown(index)}
                  {obj.dropDown !== undefined && (
                    <Collapse in={opened[obj.identifier].opened} timeout="auto" unmountOnExit>
                      <List key={index}>
                        {obj.dropDown!.map((nestedObj, nestedIndex) => (
                          <ListItem key={nestedIndex} disablePadding component={Link} to={nestedObj.url} color="inherit" style={{ textDecoration: "none", color: "black" }}>
                            <ListItemButton>
                              <ListItemIcon />
                              <ListItemText primary={t(`${nestedObj.text}`)} />
                            </ListItemButton>
                          </ListItem>
                        ))}
                      </List>
                    </Collapse>
                  )}
                </div>
              ))}
          </List>
          <Divider />
          <div style={{ marginTop: "auto", marginBottom: "5%" }}>
            <ListItem>
              <LogoutButton />
            </ListItem>
          </div>
        </Drawer>
      </Box>
    </div>
  );
};

export default SidebarNav;
