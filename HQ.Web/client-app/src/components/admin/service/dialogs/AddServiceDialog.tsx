import React, { useState } from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, TextField, Button, FormControlLabel, Switch } from '@mui/material';
import { useSnackbar } from 'notistack';
import * as api from '../../../../api';

interface AddServiceDialogProps {
  open: boolean
  queueId: string,
  parentService?: api.QueueServiceResponse
  cultures?: Array<api.AvailableCultureResponse>
  onClose: () => void
}

function AddServiceDialog(props: AddServiceDialogProps) {
  const { open, queueId, parentService, cultures, onClose } = props;
  const { enqueueSnackbar } = useSnackbar();
  const [isGroup, setIsGroup] = useState(true);

  const [literal, setLiteral] = useState('');
  const [names, setNames] = useState<api.CultureString[]>(cultures?.map(c => ({ culture: c.culture!, value: '' })) ?? [])

  const handleAdd = () => {
    if (!isGroup && !/^[A-Z0-9]+$/.test(literal)) {
      enqueueSnackbar('Неверное значение для литерала', { variant: 'error' });
      return;
    }

    const serviceApi = new api.ServiceApi();
    if (parentService) {
      serviceApi.apiServiceChildPost({ parentServiceId: parentService.id, literal, name: names })
        .then(() => {
          enqueueSnackbar('Услуга добавлена', { variant: 'success' });
          handleClose();
        })
        .catch((err) => {
          enqueueSnackbar('Не удалось добавить услугу' + err, { variant: 'error' });
        })
      return;
    }

    serviceApi.apiServicePost({ queueId, name: names, literal })
      .then(() => {
        enqueueSnackbar('Услуга добавлена', { variant: 'success' });
        handleClose();
      })
      .catch((err) => {
        enqueueSnackbar('Не удалось добавить услугу' + err, { variant: 'error' });
      })

  };

  const handleNameChange = (index: number, value: string) => {
    const updatedNames = [...names];
    updatedNames[index].value = value;
    setNames(updatedNames);
  };

  const handleLiteralChange = (value: string) => {
    if (!/^([A-Z0-9]+)*$/.test(value))
      return;

    setLiteral(value);
  }

  const handleClose = () => {
    setNames(cultures?.map(c => ({ culture: c.culture!, value: '' })) ?? []);
    setLiteral('');
    setIsGroup(true);
    onClose();
  }

  return (
    <Dialog open={open} onClose={handleClose}>
      <DialogTitle>Добавить услугу</DialogTitle>
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
        <FormControlLabel
          control={<Switch checked={isGroup} onChange={(e) => setIsGroup(e.target.checked)} />}
          label="Группа услуг"
        />
        {!isGroup && (
          <TextField
            label="Литерал (A-Z, 0-9)"
            value={literal}
            onChange={(e) => handleLiteralChange(e.target.value)}
            fullWidth
            margin="normal"
            inputProps={{ pattern: '^[A-Z0-9]+$' }}
          />
        )}
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose}>Отмена</Button>
        <Button onClick={handleAdd} color="primary">Добавить</Button>
      </DialogActions>
    </Dialog>
  );
}

export default AddServiceDialog;
