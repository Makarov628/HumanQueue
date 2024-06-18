import * as React from 'react';
import Typography from '@mui/material/Typography';
import * as api from '../../../api';
import { Button, Divider, IconButton } from '@mui/material';
import { MoreVert, OpenInNew } from '@mui/icons-material';
import WindowHeader from './WindowHeader';
import WindowList from './WindowList';
import { MenuWindowItemType } from './menus/MenuWindow';

interface WindowProps {
  windows?: Array<api.QueueWindowResponse>
  onUpdate: () => void
}

function Window(props: WindowProps) {
  const { windows, onUpdate } = props;


  const handleWindowSelectedAction = (action: MenuWindowItemType, Window: api.QueueWindowResponse | null) => {
    console.log(action, Window);
  }

  // Dialogs will be here

  return (
    <>
      <WindowHeader onAddAction={() => { }} />
      <WindowList windows={windows} onSelectedAction={handleWindowSelectedAction} />
    </>
  );
}

export default Window;