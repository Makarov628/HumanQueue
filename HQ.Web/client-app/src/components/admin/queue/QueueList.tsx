import * as React from 'react';
import Typography from '@mui/material/Typography';
import * as api from '../../../api';
import { Button, Divider, IconButton, List, ListItem, ListItemButton, ListItemText } from '@mui/material';
import { KeyboardArrowRight, MoreVert, OpenInNew } from '@mui/icons-material';

interface QueueListProps {
  queues?: Array<api.QueueResponse>
  selectedQueueId?: string,
  onSelect: (id: string) => void
}

function QueueList(props: QueueListProps) {
  const { queues, selectedQueueId, onSelect } = props;

  return (
    <List>
      {
        queues?.map((queue) =>
          <ListItem selected={selectedQueueId == queue.id}>
            <ListItemButton onClick={() => onSelect(queue.id!)}>
              <ListItemText primary={queue.name} />
              <KeyboardArrowRight />
            </ListItemButton>
          </ListItem>
        )
      }
    </List>
  );
}

export default QueueList;