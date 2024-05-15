import * as React from 'react';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import { useTranslation } from 'react-i18next';

export interface IAlertDialog {
  open: boolean
  title: string,
  description: string
  agreeText: string;
  No: (x: boolean) => void;
  Yes: () => void
}

export default function AlertDialog({ ...props }: IAlertDialog) {
  const { t, i18n } = useTranslation();

  const handleClose = () => {
    props.No(false);
  };

  return (
    <div>
      <Dialog
        open={props.open}
        BackdropProps={{
          style: {
            backdropFilter: "blur(3px)"
          }
        }}
        onClose={handleClose}
      >
        <DialogTitle id="alert-dialog-title">
          {props.title}
        </DialogTitle>
        <DialogContent>
          <DialogContentText id="alert-dialog-description">
            {props.description}
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => props.Yes()}>
            {props.agreeText}
          </Button>
          <Button onClick={() => props.No(false)} autoFocus>{t('cancel')}</Button>        
        </DialogActions>
      </Dialog>
    </div>
  );
}