import React, { useState } from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, TextField, Button, FormControlLabel, Switch } from '@mui/material';
import { useSnackbar } from 'notistack';
import * as api from '../../../../api';

interface DeleteServiceDialogProps {
  open: boolean
  service?: api.QueueServiceResponse
  onClose: () => void
}

function DeleteServiceDialog(props: DeleteServiceDialogProps) {
  const { open, service, onClose } = props;
  const { enqueueSnackbar } = useSnackbar();

  const handleDelete = () => {
    if (!service)
      return handleClose();

    const serviceApi = new api.ServiceApi();

    serviceApi.apiServiceIdDelete(service.id!)
      .then(() => {
        enqueueSnackbar('Услуга удалена', { variant: 'success' });
        handleClose();
      })
      .catch((err) => {
        enqueueSnackbar('Не удалось удалить услугу' + err, { variant: 'error' });
      })

  };


  const handleClose = () => {
    onClose();
  }

  return (
    <Dialog open={open} onClose={handleClose}>
      <DialogTitle>Удалить услугу</DialogTitle>
      <DialogContent>
        Вы действительно хотите удалить услугу <b>{service?.name}</b>?
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose}>Отмена</Button>
        <Button onClick={handleDelete} color="error">Удалить</Button>
      </DialogActions>
    </Dialog>
  );
}

export default DeleteServiceDialog;
