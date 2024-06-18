import * as React from 'react';
import Typography from '@mui/material/Typography';
import * as api from '../../../api';
import { Button, Divider, IconButton } from '@mui/material';
import { MoreVert, OpenInNew } from '@mui/icons-material';

interface WindowHeaderProps {
  onAddAction?: () => void
}

function WindowHeader(props: WindowHeaderProps) {
  const { onAddAction } = props;

  // Dialogs will be here

  return (
    <>
      <Typography variant="h5" style={{ fontWeight: "600", marginLeft: "16px" }}>
        {"Окна"}
        <Button variant='contained' size="small" sx={{ marginLeft: "16px", minWidth: "10px" }}>+</Button>
      </Typography>
      <br />
      <Divider />
    </>
  );
}

export default WindowHeader;