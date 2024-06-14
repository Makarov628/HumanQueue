import * as React from 'react';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import * as api from '../../../api';
import { useState } from 'react';
import { useSnackbar } from 'notistack';
import { Divider, Grid, Select, FormControl, InputLabel, MenuItem, SelectChangeEvent, List, ListItem, ListItemButton, ListItemText, Button, IconButton } from '@mui/material';
import { Add, Close, Edit, KeyboardArrowRight, MoreVert, OpenInNew } from '@mui/icons-material';
import ServiceList from './ServiceList';
import MenuServiceFolder from './menus/MenuServiceFolder';
import MenuServiceLiteral from './menus/MenuServiceLiteral';
import MenuWindow from './menus/MenuWindow';
import MenuTerminal from './menus/MenuTerminal';


interface QueuePanelProps {
  queueId: string;
}

function QueuePanel(props: QueuePanelProps) {
  const { queueId } = props;
  const { enqueueSnackbar } = useSnackbar();
  const [queue, setQueue] = useState<api.QueueResponse | null>(null);
  const [printers, setPrinters] = useState<api.TerminalResponse[]>([]);

  const [menuAnchorEl, setMenuAnchorEl] = useState<null | HTMLElement>(null);
  const [menuType, setMenuType] = useState<string>('');
  const [menuData, setMenuData] = useState<Service | Terminal | Window | null>(null);

  const handleMenuClick = (event: React.MouseEvent<HTMLElement>) => {
    setMenuAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setMenuAnchorEl(null);
  };

  const handleMenuOpen = (event: MouseEvent<HTMLElement>, type: string, data: Service | Terminal | Window) => {
    setMenuAnchorEl(event.currentTarget);
    setMenuType(type);
    setMenuData(data);
  };

  const handleMenuClose = () => {
    setMenuAnchorEl(null);
    setMenuType('');
    setMenuData(null);
  };

  const getQueue = () => {
    const queueApi = new api.QueueApi();
    queueApi.apiQueueIdGet(queueId!).then((queue) => {
      setQueue(queue)
    }).catch((err) => {
      enqueueSnackbar("Не удалось загрузить очередь", { variant: 'error' });
    })
  }

  const getTerminalPrinters = () => {
    const terminalApi = new api.TerminalApi();
    terminalApi.apiTerminalExternalPrintersGet().then((printers) => {
      setPrinters(printers as api.TerminalResponse[])
    }).catch((err) => {
      enqueueSnackbar("Не удалось загрузить список внешних принтеров для терминалов", { variant: 'error' });
    })
  }

  React.useEffect(() => {
    if (!queueId)
      return setQueue(null);

    getQueue();
    getTerminalPrinters()
  }, [queueId])

  if (!queueId || !queue)
    return (
      <Box sx={{ width: '100%', height: "92vh", padding: "32px", borderBottom: 'solid 1px #C1C1C1', justifyContent: 'center' }}>
      </Box>
    );

  return (
    <Box sx={{ width: '100%', height: "92vh", padding: "32px", borderBottom: 'solid 1px #C1C1C1', justifyContent: 'center' }}>
      <Typography variant="h4">
        {queue.name}
        <IconButton aria-label="Open" color="primary" size='large' onClick={() => window.open(`/tablo/${queue.id}`, "_blank")}>
          <OpenInNew />
        </IconButton>
        <IconButton aria-label="Delete" color="primary" size='small'>
          <MoreVert />
        </IconButton>
      </Typography>

      <br />
      <Divider />

      {/* <FormControl sx={{ m: 1, minWidth: 250 }}>
            <InputLabel id="demo-simple-select-disabled-label">Язык по умолчанию</InputLabel>
            <Select
              labelId="demo-simple-select-disabled-label"
              id="demo-simple-select-disabled"
              value={queue.defaultCulture}
              label="Язык по умолчанию"

            // onChange={handleChange}
            >
              <MenuItem value={"ru"}>Русский</MenuItem>
              <MenuItem value={"kk"}>Қазақ тілі</MenuItem>
              <MenuItem value={"en"}>English</MenuItem>
            </Select>
          </FormControl> */}

      <br />
      <br />

      <Grid container>
        <Grid xs={3.5}>
          <Typography variant="h5" style={{ fontWeight: "600" }}>
            {"Терминалы"}
            <Button variant='contained' size="small" sx={{ marginLeft: "16px", minWidth: "18px" }}>+</Button>
          </Typography>
          <br />
          <Divider />
          <List>
            {
              queue.terminals?.map((terminal) =>
                <ListItem secondaryAction={
                  <>
                    <IconButton aria-label="Open" color="primary" size='small' onClick={() => window.open(`/terminal/${terminal.id}`, "_blank")}>
                      <OpenInNew />
                    </IconButton>
                    <IconButton aria-label="Delete" color="primary" size='small'>
                      <MoreVert />
                    </IconButton>
                  </>
                }>
                  <ListItemText primary={terminal.name} />
                </ListItem>
              )
            }
          </List>
        </Grid>
        <Divider orientation="vertical" flexItem />
        <Grid xs={1}>
          <Typography variant="h5" style={{ fontWeight: "600", marginLeft: "16px" }}>
            {"Окна"}
            <Button variant='contained' size="small" sx={{ marginLeft: "16px", minWidth: "10px" }}>+</Button>
          </Typography>
          <br />
          <Divider />
          <List>
            {
              queue.windows?.sort((a, b) => a.number! > b.number! ? 1 : -1).map((w) =>
                <ListItem secondaryAction={
                  <>
                    <IconButton aria-label="Open" color="primary" size='small' onClick={() => window.open(`/window/${w.id}`, "_blank")}>
                      <OpenInNew />
                    </IconButton>
                    <IconButton aria-label="Delete" color="primary" size='small'>
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
        </Grid>
        <Divider orientation="vertical" flexItem />
        <Grid xs={7}>
          <Typography variant="h5" style={{ fontWeight: "600", marginLeft: "16px" }}>
            {"Услуги"}
            <Button variant='contained' size="small" sx={{ marginLeft: "16px", minWidth: "16px" }}>+</Button>
          </Typography>
          <br />
          <Divider />
          <ServiceList services={queue.services!} windows={queue.windows!} />
        </Grid>
      </Grid>
      <MenuServiceFolder anchorEl={menuAnchorEl} handleClose={handleMenuClose} handleAction={handleAction} />
      <MenuServiceLiteral anchorEl={menuAnchorEl} handleClose={handleMenuClose} handleAction={handleAction} />
      <MenuWindow anchorEl={menuAnchorEl} handleClose={handleMenuClose} handleAction={handleAction} />
      <MenuTerminal anchorEl={menuAnchorEl} handleClose={handleMenuClose} handleAction={handleAction} />

    </Box>
  );
}

export default QueuePanel

// import React, { useState } from 'react';
// import Typography from '@mui/material/Typography';
// import Box from '@mui/material/Box';
// import * as api from '../../../api';
// import { useSnackbar } from 'notistack';
// import { Divider, Grid, List, ListItem, ListItemText, Button, IconButton } from '@mui/material';
// import { OpenInNew, MoreVert } from '@mui/icons-material';
// import MenuServiceFolder from './MenuServiceFolder';
// import MenuServiceLiteral from './MenuServiceLiteral';
// import MenuWindow from './MenuWindow';
// import MenuTerminal from './MenuTerminal';
// import AddServiceDialog from './AddServiceDialog';
// import RenameDialog from './RenameDialog';
// import DeleteDialog from './DeleteDialog';
// import ServiceList from './ServiceList';

// function QueuePanel(props) {
//   const { queueId } = props;
//   const { enqueueSnackbar } = useSnackbar();
//   const [queue, setQueue] = useState<api.QueueResponse | null>(null);
//   const [printers, setPrinters] = useState<api.TerminalResponse[] | null>([]);

//   const [menuAnchorEl, setMenuAnchorEl] = useState(null);
//   const [menuType, setMenuType] = useState('');
//   const [menuData, setMenuData] = useState(null);

//   const [dialog, setDialog] = useState({ open: false, type: '', data: null });

//   const handleMenuOpen = (event, type, data) => {
//     setMenuAnchorEl(event.currentTarget);
//     setMenuType(type);
//     setMenuData(data);
//   };

//   const handleMenuClose = () => {
//     setMenuAnchorEl(null);
//     setMenuType('');
//     setMenuData(null);
//   };

//   const handleDialogOpen = (type, data) => {
//     setDialog({ open: true, type, data });
//   };

//   const handleDialogClose = () => {
//     setDialog({ open: false, type: '', data: null });
//   };

//   const getQueue = () => {
//     const queueApi = new api.QueueApi();
//     queueApi.apiQueueIdGet(queueId).then((queue) => {
//       setQueue(queue);
//     }).catch(() => {
//       enqueueSnackbar('Не удалось загрузить очередь', { variant: 'error' });
//     });
//   };

//   const getTerminalPrinters = () => {
//     const terminalApi = new api.TerminalApi();
//     terminalApi.apiTerminalExternalPrintersGet().then((printers) => {
//       setPrinters(printers);
//     }).catch(() => {
//       enqueueSnackbar('Не удалось загрузить список внешних принтеров для терминалов', { variant: 'error' });
//     });
//   };

//   React.useEffect(() => {
//     if (!queueId) return setQueue(null);

//     getQueue();
//     getTerminalPrinters();
//   }, [queueId]);

//   const handleAction = (action) => {
//     handleMenuClose();
//     switch (action) {
//       case 'add':
//         handleDialogOpen('addService', { parentServiceId: menuData.id });
//         break;
//       case 'rename':
//         handleDialogOpen('rename', { id: menuData.id, name: menuData.name });
//         break;
//       case 'delete':
//         handleDialogOpen('delete', { id: menuData.id, name: menuData.name });
//         break;
//       // Add other actions here
//       default:
//         break;
//     }
//   };

//   if (!queueId || !queue) return (
//     <Box sx={{ width: '100%', height: "92vh", padding: "32px", borderBottom: 'solid 1px #C1C1C1', justifyContent: 'center' }}>
//     </Box>
//   );

//   return (
//     <Box sx={{ width: '100%', height: "92vh", padding: "32px", borderBottom: 'solid 1px #C1C1C1', justifyContent: 'center' }}>
//       <Typography variant="h4">
//         {queue.name}
//         <IconButton aria-label="Open" color="primary" size='large' onClick={() => window.open(`/tablo/${queue.id}`, "_blank")}>
//           <OpenInNew />
//         </IconButton>
//         <IconButton aria-label="More" color="primary" size='small' onClick={(event) => handleMenuOpen(event, 'queue', queue)}>
//           <MoreVert />
//         </IconButton>
//       </Typography>
//       <br />
//       <Divider />
//       <br />
//       <br />
//       <Grid container>
//         <Grid item xs={3.5}>
//           <Typography variant="h5" style={{ fontWeight: "600" }}>
//             {"Терминалы"}
//             <Button variant='contained' size="small" sx={{ marginLeft: "16px", minWidth: "18px" }}>+</Button>
//           </Typography>
//           <br />
//           <Divider />
//           <List>
//             {
//               queue.terminals?.map((terminal) =>
//                 <ListItem key={terminal.id} secondaryAction={
//                   <>
//                     <IconButton aria-label="Open" color="primary" size='small' onClick={() => window.open(`/terminal/${terminal.id}`, "_blank")}>
//                       <OpenInNew />
//                     </IconButton>
//                     <IconButton aria-label="More" color="primary" size='small' onClick={(event) => handleMenuOpen(event, 'terminal', terminal)}>
//                       <MoreVert />
//                     </IconButton>
//                   </>
//                 }>
//                   <ListItemText primary={terminal.name} />
//                 </ListItem>
//               )
//             }
//           </List>
//         </Grid>
//         <Divider orientation="vertical" flexItem />
//         <Grid item xs={1}>
//           <Typography variant="h5" style={{ fontWeight: "600", marginLeft: "16px" }}>
//             {"Окна"}
//             <Button variant='contained' size="small" sx={{ marginLeft: "16px", minWidth: "10px" }}>+</Button>
//           </Typography>
//           <br />
//           <Divider />
//           <List>
//             {
//               queue.windows?.sort((a, b) => a.number > b.number ? 1 : -1).map((w) =>
//                 <ListItem key={w.id} secondaryAction={
//                   <>
//                     <IconButton aria-label="Open" color="primary" size='small' onClick={() => window.open(`/window/${w.id}`, "_blank")}>
//                       <OpenInNew />
//                     </IconButton>
//                     <IconButton aria-label="More" color="primary" size='small' onClick={(event) => handleMenuOpen(event, 'window', w)}>
//                       <MoreVert />
//                     </IconButton>
//                   </>
//                 }>
//                   <ListItemText >
//                     <strong>{w.number}</strong>
//                   </ListItemText>
//                 </ListItem>
//               )
//             }
//           </List>
//         </Grid>
//         <Divider orientation="vertical" flexItem />
//         <Grid item xs={7}>
//           <Typography variant="h5" style={{ fontWeight: "600", marginLeft: "16px" }}>
//             {"Услуги"}
//             <Button variant='contained' size="small" sx={{ marginLeft: "16px", minWidth: "16px" }}>+</Button>
//           </Typography>
//           <br />
//           <Divider />
//           <ServiceList services={queue.services} windows={queue.windows} onMoreVertClick={handleMenuOpen} />
//         </Grid>
//       </Grid>
//       <MenuServiceFolder anchorEl={menuAnchorEl} handleClose={handleMenuClose} handleAction={handleAction} />
//       <MenuServiceLiteral anchorEl={menuAnchorEl} handleClose={handleMenuClose} handleAction={handleAction} />
//       <MenuWindow anchorEl={menuAnchorEl} handleClose={handleMenuClose} handleAction={handleAction} />
//       <MenuTerminal anchorEl={menuAnchorEl} handleClose={handleMenuClose} handleAction={handleAction} />

//       {/* <AddServiceDialog open={dialog.open && dialog.type === 'addService'} handleClose={handleDialogClose} parentServiceId={dialog.data?.parentServiceId} />
//       <RenameDialog open={dialog.open && dialog.type === 'rename'} handleClose={handleDialogClose} id={dialog.data?.id} initialName={dialog.data?.name} />
//       <DeleteDialog open={dialog.open && dialog.type === 'delete'} handleClose={handleDialogClose} name={dialog.data?.name} onDelete={() => { /* Implement delete logic */ }} /> */}
//     </Box>
//   );
// }

// export default QueuePanel;
