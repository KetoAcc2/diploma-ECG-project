const MainLayout = (props: any) => {
  return (
    <div style={{ display: "flex" }}>
      <div style={{ minWidth: "235px", width: "235px" }}></div>
      <div style={{marginLeft:'2%', width: "100%",marginRight:'2%' }}>{props.children}</div>
    </div>
  );
};

export default MainLayout;
