import * as React from 'react';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import { QueueServiceResponse, QueueWindowResponse } from '../../../api';
import { Badge, Chip, Collapse, IconButton, ListItem, ListItemIcon, ListItemText } from '@mui/material';
import { Add, Close, Edit, ExpandLess, ExpandMore, Folder, FolderOpen, MoreVert } from '@mui/icons-material';
import { MenuServiceFolderItemType } from './menus/MenuServiceFolder';
import { MenuServiceLiteralItemType } from './menus/MenuServiceLiteral';
import ServiceList from './ServiceList';


interface ServiceProps {
  service: QueueServiceResponse,
  windows?: Array<QueueWindowResponse>,
  onSelectedAction: (action: MenuServiceFolderItemType | MenuServiceLiteralItemType, service: QueueServiceResponse | null) => void
  onMenuOpen: (event: React.MouseEvent<HTMLElement>) => void
}

function ServiceItem(props: ServiceProps) {
  const { service, windows, onSelectedAction, onMenuOpen } = props;
  const [open, setOpen] = React.useState(false);

  const handleClick = () => {
    setOpen(!open);
  };

  return (
    <>
      <ListItem secondaryAction={
        <>
          <IconButton aria-label="Delete" color="primary" size='small' onClick={onMenuOpen}>
            <MoreVert />
          </IconButton>
        </>
      }>
        <ListItemIcon>
          <IconButton onClick={() => handleClick()}>
            {
              !service.literal
                ? open ? <FolderOpen color="primary" /> : <Folder color="primary" />
                : <Typography component="span" color="primary" style={{ fontWeight: "600" }}>{service.literal}</Typography>
            }
          </IconButton>
        </ListItemIcon>
        <ListItemText secondary={service.literal ? windows?.filter(window => service.linkedWindowsIds.includes(window.id!)).map(window =>
          <Chip label={`Окно № ${window.number!}`} color='primary' size='small' variant='outlined' sx={{ marginRight: "8px" }} />
        ) : null}>
          {service.name}
        </ListItemText>
      </ListItem>
      {
        service.childs!.length > 0 ?
          <Collapse in={open} timeout="auto" unmountOnExit sx={{ pl: 3 }}>
            <ServiceList services={service.childs!} windows={windows} onSelectedAction={onSelectedAction} />
          </Collapse>
          : null
      }
    </>
  );
}

export default ServiceItem;