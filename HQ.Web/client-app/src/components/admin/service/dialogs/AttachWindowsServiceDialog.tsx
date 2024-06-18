import React, { useState } from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, TextField, Button, FormControlLabel, Switch, Chip, Box, Divider, Stack, Typography } from '@mui/material';
import { useSnackbar } from 'notistack';
import * as api from '../../../../api';

interface AttachWindowsServiceDialogProps {
  open: boolean
  serviceId?: string
  services?: Array<api.QueueServiceResponse>
  windows?: Array<api.QueueWindowResponse>
  onChange: () => void
  onClose: () => void
}

function AttachWindowsServiceDialog(props: AttachWindowsServiceDialogProps) {
  const { open, serviceId, services, windows, onChange, onClose } = props;
  const { enqueueSnackbar } = useSnackbar();
  const [updatedService, setUpdatedService] = useState<api.QueueServiceResponse | undefined>();

  React.useEffect(() => {  
    if (serviceId)
      setUpdatedService(flatten(services ?? []).find(s => s.id == serviceId))
  }, [services, serviceId])

  const flatten: (services: Array<api.QueueServiceResponse>) => Array<api.QueueServiceResponse> = (services: Array<api.QueueServiceResponse>) => services.flatMap<api.QueueServiceResponse>((service) => [
    { ...service, childs: [] } as api.QueueServiceResponse,
    ...flatten(service.childs || [])
  ]);

  const attachWindow = (id: string) => {
    if (!serviceId)
      return handleClose();

    const serviceApi = new api.ServiceApi();
    serviceApi.apiServiceAttachWindowPost({ serviceId: serviceId, windowId: id })
      .then(() => {
        enqueueSnackbar('Окно назначено', { variant: 'success' });
        handleChange();
      })
      .catch((err) => {
        enqueueSnackbar('Не удалось назначить окно' + err, { variant: 'error' });
      })

  };

  const unattachWindow = (id: string) => {
    if (!serviceId)
      return handleClose();

    const serviceApi = new api.ServiceApi();
    serviceApi.apiServiceDeattachWindowPost({ serviceId: serviceId, windowId: id })
      .then(() => {
        enqueueSnackbar('Окно убрано', { variant: 'success' });
        handleChange();
      })
      .catch((err) => {
        enqueueSnackbar('Не удалось убрать окно' + err, { variant: 'error' });
      })


  }

  const handleChange = () => {
    onChange()
  }

  const handleClose = () => {
    onClose();
  }

  return (
    <Dialog open={open} onClose={handleClose}>
      <DialogTitle>Назначить окна услуге</DialogTitle>
      <DialogContent>
        <Stack columnGap={2} direction={"row"}>
          <Box sx={{ width: '50%', minWidth: "200px", height: "15vh", justifyContent: 'center' }}>
            <Typography variant="h6" style={{ fontWeight: "400", marginLeft: "8px", marginTop: "1px", textAlign: "center" }}>
              {"Доступно"}
            </Typography>
            <Divider />
            <br />

            <Stack useFlexGap flexWrap="wrap" columnGap={1} gap={1} direction={"row"}>
              {
                windows?.filter(w => !updatedService?.linkedWindowsIds?.includes(w.id!)).map(w =>
                  <Chip
                    label={`Окно № ${w.number}`}
                    color='primary'
                    size='small'
                    variant='filled'
                    onClick={() => attachWindow(w.id!)}
                  />
                )
              }

            </Stack>
          </Box>
          <Divider orientation="vertical" flexItem />
          <Box sx={{ width: '50%', minWidth: "200px", height: "15vh", justifyContent: 'center' }}>
            <Typography variant="h6" style={{ fontWeight: "400", marginLeft: "8px", marginTop: "1px", textAlign: "center" }}>
              {"Назначено"}
            </Typography>
            <Divider />
            <br />

            <Stack useFlexGap flexWrap="wrap" columnGap={1} gap={1} direction={"row"}>
              {
                updatedService?.linkedWindowsIds.map(id =>
                  <Chip
                    label={`Окно № ${windows?.find(w => w.id == id)?.number}`}
                    color='primary'
                    size='small'
                    variant='filled'
                    onDelete={() => { unattachWindow(id) }}
                  />
                )
              }

            </Stack>
          </Box>
        </Stack>
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} color="primary">OK</Button>
      </DialogActions>
    </Dialog>
  );
}

export default AttachWindowsServiceDialog;
