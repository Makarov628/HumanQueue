import * as React from 'react';
import Typography from '@mui/material/Typography';
import * as api from '../../../api';
import { Button, Divider, IconButton } from '@mui/material';
import { MoreVert, OpenInNew } from '@mui/icons-material';
import TerminalHeader from './TerminalHeader';
import TerminalList from './TerminalList';
import { MenuTerminalItemType } from './menus/MenuTerminal';

interface TerminalHeaderProps {
  terminals?: Array<api.QueueTerminalResponse>
  onUpdate: () => void
}

function Terminal(props: TerminalHeaderProps) {
  const { terminals, onUpdate } = props;


  const handleTerminalSelectedAction = (action: MenuTerminalItemType, terminal: api.QueueTerminalResponse | null) => {
    console.log(action, terminal);
  }

  // Dialogs will be here

  return (
    <>
      <TerminalHeader onAddAction={() => { }} />
      <TerminalList terminals={terminals} onSelectedAction={handleTerminalSelectedAction} />
    </>
  );
}

export default Terminal;