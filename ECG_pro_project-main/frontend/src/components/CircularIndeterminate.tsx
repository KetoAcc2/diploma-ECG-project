import Box from "@mui/material/Box";
import CircularProgress from "@mui/material/CircularProgress";

export default function CircularIndeterminate({
  style = { marginLeft: "auto", marginRight: "auto" },
  size = 24,
}: {
  style?: React.CSSProperties;
  size?: string | number;
}) {
  return (
    <Box style={style}>
      <CircularProgress color="inherit" size={size} />
    </Box>
  );
}
