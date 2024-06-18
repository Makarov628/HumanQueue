import * as React from 'react';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import { Button, Divider, IconButton, List, ListItem, ListItemText } from '@mui/material';
import { QueueTerminalResponse } from '../../../api';
import { MoreVert, OpenInNew } from '@mui/icons-material';
import MenuTerminal, { MenuTerminalItemType } from './menus/MenuTerminal';

interface TerminalListProps {
  terminals?: Array<QueueTerminalResponse>
  onSelectedAction: (action: MenuTerminalItemType, terminal: QueueTerminalResponse | null) => void
}

function TerminalList(props: TerminalListProps) {
  const { terminals, onSelectedAction } = props;
  const [menuAnchorEl, setMenuAnchorEl] = React.useState<null | HTMLElement>(null);

  const [selectedTerminal, setSelectedTerminal] = React.useState<null | QueueTerminalResponse>(null);
  const handleMenuClose = () => {
    setMenuAnchorEl(null);
  };

  const handleMenuOpen = (terminal: QueueTerminalResponse) => (event: React.MouseEvent<HTMLElement>) => {
    setMenuAnchorEl(event.currentTarget);
    setSelectedTerminal(terminal);
  };

  const handleMenuItemSelected = (itemType: MenuTerminalItemType) => {
    onSelectedAction(itemType, selectedTerminal);
    handleMenuClose();
  }
  return (
    <>
      <List>
        {
          terminals?.map((terminal) =>
            <ListItem secondaryAction={
              <>
                <IconButton aria-label="Open" color="primary" size='small' onClick={() => window.open(`/terminal/${terminal.id}`, "_blank")}>
                  <OpenInNew />
                </IconButton>
                <IconButton aria-label="Delete" color="primary" size='small' onClick={handleMenuOpen(terminal)}>
                  <MoreVert />
                </IconButton>
              </>
            }>
              <ListItemText primary={terminal.name} />
            </ListItem>
          )
        }
      </List>
      <MenuTerminal open={Boolean(menuAnchorEl)} anchorEl={menuAnchorEl} onClose={handleMenuClose} onItemClicked={(itemType) => { handleMenuItemSelected(itemType) }}/>
    </>
  );
}

export default TerminalList