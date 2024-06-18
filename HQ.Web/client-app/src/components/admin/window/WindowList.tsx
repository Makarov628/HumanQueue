import * as React from 'react';
import { QueueWindowResponse } from '../../../api';
import MenuWindow, { MenuWindowItemType } from './menus/MenuWindow';
import { IconButton, List, ListItem, ListItemText } from '@mui/material';
import { MoreVert, OpenInNew } from '@mui/icons-material';

interface WindowListProps {
  windows?: Array<QueueWindowResponse>
  onSelectedAction: (action: MenuWindowItemType, terminal: QueueWindowResponse | null) => void
}

function WindowList(props: WindowListProps) {
  const { windows, onSelectedAction } = props;
  const [menuAnchorEl, setMenuAnchorEl] = React.useState<null | HTMLElement>(null);

  const [selectedWindow, setSelectedWindow] = React.useState<null | QueueWindowResponse>(null);
  const handleMenuClose = () => {
    setMenuAnchorEl(null);
  };

  const handleMenuOpen = (window: QueueWindowResponse) => (event: React.MouseEvent<HTMLElement>) => {
    setMenuAnchorEl(event.currentTarget);
    setSelectedWindow(window);
  };

  const handleMenuItemSelected = (itemType: MenuWindowItemType) => {
    onSelectedAction(itemType, selectedWindow);
    handleMenuClose();
  }

  return (
    <>
      <List>
        {
          windows?.sort((a, b) => a.number! > b.number! ? 1 : -1).map((w) =>
            <ListItem secondaryAction={
              <>
                <IconButton aria-label="Open" color="primary" size='small' onClick={() => window.open(`/window/${w.id}`, "_blank")}>
                  <OpenInNew />
                </IconButton>
                <IconButton aria-label="Delete" color="primary" size='small' onClick={handleMenuOpen(w)}>
                  <MoreVert />
                </IconButton>
              </>
            }>
              <ListItemText >
                <strong>{w.number}</strong>
              </ListItemText>
            </ListItem>
          )
        }
      </List>
      <MenuWindow open={Boolean(menuAnchorEl)} anchorEl={menuAnchorEl} onClose={handleMenuClose} onItemClicked={(itemType) => { handleMenuItemSelected(itemType) }} />
    </>
  );
}

export default WindowList;