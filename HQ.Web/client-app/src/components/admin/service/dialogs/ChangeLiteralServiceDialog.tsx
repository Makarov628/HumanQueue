import React, { useState } from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, TextField, Button, FormControlLabel, Switch } from '@mui/material';
import { useSnackbar } from 'notistack';
import * as api from '../../../../api';

interface ChangeLiteralServiceDialogProps {
  open: boolean
  service?: api.QueueServiceResponse
  onClose: () => void
}

function ChangeLiteralServiceDialog(props: ChangeLiteralServiceDialogProps) {
  const { open, service, onClose } = props;
  const { enqueueSnackbar } = useSnackbar();
  const [literal, setLiteral] = useState('');

  React.useEffect(() => {
    if (open && service) {
      setLiteral(service.literal ?? '');
    }
  }, [open, service])

  const handleChange = () => {
    if (!service)
      return handleClose();

    if (!/^[A-Z0-9]+$/.test(literal)) {
      enqueueSnackbar('Неверное значение для литерала', { variant: 'error' });
      return;
    }

    const serviceApi = new api.ServiceApi();

    serviceApi.apiServiceUpdateLiteralPut({ id: service.id, litetal: literal })
      .then(() => {
        enqueueSnackbar('Литерал услуги изменён', { variant: 'success' });
        handleClose();
      })
      .catch((err) => {
        enqueueSnackbar('Не удалось изменить литерал услуги' + err, { variant: 'error' });
      })

  };

  const handleLiteralChange = (value: string) => {
    if (!/^([A-Z0-9]+)*$/.test(value))
      return;

    setLiteral(value);
  }

  const handleClose = () => {
    setLiteral('');
    onClose();
  }

  return (
    <Dialog open={open} onClose={handleClose}>
      <DialogTitle>Изменить литерал услуги</DialogTitle>
      <DialogContent>
        <TextField
          label="Литерал (A-Z, 0-9)"
          value={literal}
          onChange={(e) => handleLiteralChange(e.target.value)}
          fullWidth
          margin="normal"
          inputProps={{ pattern: '^[A-Z0-9]+$' }}
        />
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose}>Отмена</Button>
        <Button onClick={handleChange} color="primary">Изменить</Button>
      </DialogActions>
    </Dialog>
  );
}

export default ChangeLiteralServiceDialog;
