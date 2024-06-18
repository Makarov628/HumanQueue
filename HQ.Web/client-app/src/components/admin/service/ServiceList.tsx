import * as React from 'react';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import { QueueServiceResponse, QueueWindowResponse } from '../../../api';
import { Badge, Chip, Collapse, IconButton, List, ListItem, ListItemButton, ListItemIcon, ListItemText } from '@mui/material';
import { Add, Close, Edit, ExpandLess, ExpandMore, Folder, FolderOpen, MoreVert } from '@mui/icons-material';
import MenuServiceFolder, { MenuServiceFolderItemType } from './menus/MenuServiceFolder';
import MenuServiceLiteral, { MenuServiceLiteralItemType } from './menus/MenuServiceLiteral';
import ServiceItem from './ServiceItem';

interface ServiceListProps {
  services?: Array<QueueServiceResponse>,
  windows?: Array<QueueWindowResponse>,
  onSelectedAction: (action: MenuServiceFolderItemType | MenuServiceLiteralItemType, service: QueueServiceResponse | null) => void
}

type MenuType = '' | 'service-menu' | 'group-service-menu'

function ServiceList(props: ServiceListProps) {
  const { services, windows, onSelectedAction } = props;
  const [menuType, setMenuType] = React.useState<MenuType>('');
  const [menuAnchorEl, setMenuAnchorEl] = React.useState<null | HTMLElement>(null);
  const [selectedService, setSelectedService] = React.useState<null | QueueServiceResponse>(null);

  const handleMenuClose = () => {
    setMenuAnchorEl(null);
    setMenuType('')
  };

  const handleMenuOpen = (service: QueueServiceResponse) => (event: React.MouseEvent<HTMLElement>) => {
    setMenuAnchorEl(event.currentTarget);

    if (service.literal)
      setMenuType("service-menu");
    else
      setMenuType("group-service-menu")

    setSelectedService(service);
  };

  const handleMenuItemSelected = (itemType: MenuServiceFolderItemType | MenuServiceLiteralItemType) => {
    onSelectedAction(itemType, selectedService);
    handleMenuClose();
  }

  return (
    <>
      <List>
        {
          services?.map(service =>
            <ServiceItem service={service} windows={windows} onSelectedAction={onSelectedAction} onMenuOpen={handleMenuOpen(service)} />
          )
        }
      </List>
      <MenuServiceFolder open={menuType == 'group-service-menu'} anchorEl={menuAnchorEl} onClose={handleMenuClose} onItemClicked={(itemType) => { handleMenuItemSelected(itemType) }} />
      <MenuServiceLiteral open={menuType == 'service-menu'} anchorEl={menuAnchorEl} onClose={handleMenuClose} onItemClicked={(itemType) => { handleMenuItemSelected(itemType) }} />
    </>
  );
}


export default ServiceList