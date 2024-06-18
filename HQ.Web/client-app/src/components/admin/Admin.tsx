import 'dayjs/locale/ru';
import React, { useState } from 'react';
import Content from '../common/Content';
import { Box, Button, CssBaseline, Divider, Fab, List, ListItem, ListItemButton, ListItemText, Stack, Toolbar, Typography } from '@mui/material';
import { default as dayjs } from 'dayjs';
import useToken from '../common/Token';
import Head from '../common/Head';
import { useTranslation } from 'react-i18next';
import * as api from '../../api';
import { useSnackbar } from 'notistack';
import { Home, KeyboardArrowRight } from '@mui/icons-material';
import QueuePanel from './queue/QueuePanel';
import Queue from './queue/Queue';

dayjs.locale('ru')

export interface IAdmin {

}

function Admin({ ...props }: IAdmin) {

  const { t, i18n } = useTranslation();
  const { token, removeToken, setToken } = useToken();

  const [exitDialog, setExitDialog] = React.useState<boolean>(false);
  const [tab, setTab] = React.useState<number>(0);
  const [date, setDate] = React.useState<number>(Date.now());
  const { enqueueSnackbar } = useSnackbar();
  const [queues, setQueues] = useState<api.QueuesResponse[]>([])




  const getQueueList = () => {
    const queueApi = new api.QueueApi();

    queueApi.apiQueueGet().then((queues) => {
      setQueues([...queues])
    }).catch((err) => {
      enqueueSnackbar("Не удалось загрузить список очередей", { variant: 'error' });
    })
  };


  React.useEffect(() => {
    getQueueList()
  }, [])


  React.useEffect(() => {
    setTimeout(() => {
      setDate(Date.now());
    }, 1000);
  }, [date])

  return (
    <Content>
      <CssBaseline />
      <Stack spacing={0} sx={{ height: '100vh' }}>
        <Head>
          <Typography variant="h5" sx={{ color: 'rgba(0, 0, 0, 0.54)' }}>
            {dayjs(date).format('DD.MM.YYYY HH:mm:ss')}
          </Typography>
        </Head>
        <Divider />
        <Stack direction="row">
          <Queue queues={queues} onUpdate={() => { getQueueList() }}/>
        </Stack>
      </Stack>
      {/* <AlertDialog
        No={setExitDialog}
        Yes={Logout}
        open={exitDialog}
        title={t('exit')}
        description={t('exitDescription')}
        agreeText={t('exit')} /> */}
    </Content>
  );
}

export default Admin;
