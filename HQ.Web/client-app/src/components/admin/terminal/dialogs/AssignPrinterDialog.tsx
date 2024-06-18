import * as React from 'react';
import Box from '@mui/material/Box';
import { useSnackbar } from 'notistack';
import * as api from '../../../../api';

interface AssignPrinterDialogProps {
  open: boolean
}

function AssignPrinterDialog(props: AssignPrinterDialogProps) {
  const { open } = props;
  const { enqueueSnackbar } = useSnackbar();
  const [printers, setPrinters] = React.useState<api.TerminalResponse[]>([]);

  const getTerminalPrinters = () => {
    const terminalApi = new api.TerminalApi();
    terminalApi.apiTerminalExternalPrintersGet().then((printers) => {
      setPrinters([...printers as api.TerminalResponse[]])
    }).catch((err) => {
      enqueueSnackbar("Не удалось загрузить список внешних принтеров для терминалов", { variant: 'error' });
    })
  }

  React.useEffect(() => {
    if (open) getTerminalPrinters()
  }, [open])

  
  return (
   <>
   </>
  );
}

export default AssignPrinterDialog