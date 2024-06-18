import React, { useState } from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, TextField, Button, FormControlLabel, Switch } from '@mui/material';
import { useSnackbar } from 'notistack';
import * as api from '../../../../api';

interface RenameServiceDialogProps {
  open: boolean
  service?: api.QueueServiceResponse
  cultures?: Array<api.AvailableCultureResponse>
  onClose: () => void
}

function RenameServiceDialog(props: RenameServiceDialogProps) {
  const { open, service, cultures, onClose } = props;
  const { enqueueSnackbar } = useSnackbar();
  const [names, setNames] = useState<api.CultureString[]>(cultures?.map(c => ({ culture: c.culture!, value: '' })) ?? []);

  React.useEffect(() => {
    if (open && service) {
      setNames(cultures?.map(c => {
        return service.names?.find(s => s.culture == c.culture) ?? { culture: c.culture, value: '' }
      }) ?? []);
    }
  }, [open, service])

  const handleRename = () => {
    if (!service)
      return handleClose();

    const serviceApi = new api.ServiceApi();

    serviceApi.apiServiceUpdateNamePut({ id: service.id, name: names })
      .then(() => {
        enqueueSnackbar('Услуга переименована', { variant: 'success' });
        handleClose();
      })
      .catch((err) => {
        enqueueSnackbar('Не удалось переименовать услугу' + err, { variant: 'error' });
      })

  };

  const handleNameChange = (index: number, value: string) => {
    const updatedNames = [...names];
    updatedNames[index].value = value;
    setNames(updatedNames);
  };

  const handleClose = () => {
    setNames(cultures?.map(c => ({ culture: c.culture!, value: '' })) ?? []);
    onClose();
  }

  return (
    <Dialog open={open} onClose={handleClose}>
      <DialogTitle>Переименовать услугу</DialogTitle>
      <DialogContent>
        {names?.map((n, index) => (
          <TextField
            key={n.culture}
            label={`Название (${n.culture})`}
            value={n.value}
            onChange={(e) => handleNameChange(index, e.target.value)}
            fullWidth
            margin="normal"
          />
        ))}
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose}>Отмена</Button>
        <Button onClick={handleRename} color="primary">Переименовать</Button>
      </DialogActions>
    </Dialog>
  );
}

export default RenameServiceDialog;
