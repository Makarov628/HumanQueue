import * as React from 'react';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import { QueueServiceResponse, QueueWindowResponse } from '../../../api';
import { Chip, Collapse, IconButton, List, ListItem, ListItemButton, ListItemIcon, ListItemText } from '@mui/material';
import { Add, Close, Edit, ExpandLess, ExpandMore, Folder, FolderOpen, MoreVert } from '@mui/icons-material';

interface ServiceListProps {
  services: Array<QueueServiceResponse>,
  windows: Array<QueueWindowResponse>,
}

interface ServiceProps {
  service: QueueServiceResponse,
  windows: Array<QueueWindowResponse>
}

function ServiceList(props: ServiceListProps) {
  const { services, windows } = props;

  return (
    <List>
      {
        services.map(service =>
          <Service service={service} windows={windows} />
        )
      }
    </List>
  );
}

function Service(props: ServiceProps) {
  const { service, windows } = props;
  const [open, setOpen] = React.useState(false);

  const handleClick = () => {
    setOpen(!open);
  };

  return (
    <>
      <ListItem secondaryAction={
        <>
          <IconButton aria-label="Delete" color="primary" size='small'>
            <MoreVert />
          </IconButton>
        </>

      }>

        <ListItemIcon>
          <IconButton onClick={() => handleClick()}>
            {
              service.childs!.length > 0
                ? open ? <FolderOpen color="primary" /> : <Folder color="primary" />
                : <Typography component="span" color="primary" style={{ fontWeight: "600" }}>{service.literal}</Typography>
            }
          </IconButton>
        </ListItemIcon>
        <ListItemText secondary={service.literal ? windows.filter(window => service.linkedWindowsIds.includes(window.id!)).map(window =>
          <Chip label={`Окно № ${window.number!}`} color='primary' size='small' variant='outlined' sx={{ marginRight: "8px" }} />
        ) : null}>
          {service.name}
        </ListItemText>


      </ListItem>
      {
        service.childs!.length > 0 ?
          <Collapse in={open} timeout="auto" unmountOnExit sx={{ pl: 3 }}>
            <ServiceList services={service.childs!} windows={windows} />
          </Collapse>
          : null
      }
    </>
  );
}

export default ServiceList