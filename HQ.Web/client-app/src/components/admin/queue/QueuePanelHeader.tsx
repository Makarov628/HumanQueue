import * as React from 'react';
import Typography from '@mui/material/Typography';
import * as api from '../../../api';
import { Divider, IconButton } from '@mui/material';
import { MoreVert, OpenInNew } from '@mui/icons-material';

interface QueuePanelHeaderProps {
  queue?: api.QueueResponse
}

function QueuePanelHeader(props: QueuePanelHeaderProps) {
  const { queue } = props;
  return (
    <>
      <Typography variant="h4">
        {queue?.name}
        <IconButton aria-label="Open" color="primary" size='large' onClick={() => window.open(`/tablo/${queue?.id}`, "_blank")}>
          <OpenInNew />
        </IconButton>
        <IconButton aria-label="Delete" color="primary" size='small'>
          <MoreVert />
        </IconButton>
      </Typography>
      <br />
      <Divider />
      <br />
      <br />
    </>
  );
}

export default QueuePanelHeader;