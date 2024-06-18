import * as React from 'react';
import Typography from '@mui/material/Typography';
import * as api from '../../../api';
import { Box, Button, Divider, IconButton, List, ListItem, ListItemButton, ListItemText } from '@mui/material';
import { KeyboardArrowRight, MoreVert, OpenInNew } from '@mui/icons-material';
// import QueueHeader from './QueueHeader';
// import QueueList from './QueueList';
import QueuePanel from './QueuePanel';
import QueueHeader from './QueueHeader';
import QueueList from './QueueList';

interface QueueHeaderProps {
  queues?: Array<api.QueueResponse>
  onUpdate: () => void
}

function Queue(props: QueueHeaderProps) {
  const { queues, onUpdate } = props;
  const [selectedQueueId, setSelectedQueueId] = React.useState<string>("")

  const selectQueue = (id: string) => {
    if (id == selectedQueueId)
      return setSelectedQueueId("");

    setSelectedQueueId(id);
  }

  // const handleQueueSelectedAction = (action: MenuQueueItemType, queue: api.QueueResponse | null) => {
  //   console.log(action, queue);
  // }

  // Dialogs will be here

  return (
    <>
      <Box sx={{ width: '30%', height: "92vh", borderBottom: 'solid 1px #C1C1C1', justifyContent: 'center' }}>
        <QueueHeader onAddAction={() => {}}/>
        <QueueList queues={queues} selectedQueueId={selectedQueueId} onSelect={selectQueue}/>
      </Box>
      <Divider orientation="vertical" flexItem />
      <QueuePanel queueId={selectedQueueId} />
    </>
  );
}

export default Queue;