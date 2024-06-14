import React, { useState } from 'react';
import Content from '../common/Content';
import { Box, Button, Container, CssBaseline, Divider, Fab, List, ListItem, ListItemButton, ListItemText, Stack, Toolbar, Typography } from '@mui/material';
import { default as dayjs } from 'dayjs';
import 'dayjs/locale/ru';
import useToken from '../common/Token';
import Head from '../common/Head';
import { useTranslation } from 'react-i18next';
import AlertDialog from '../common/AlertDialog';
import Tabs, { tabsClasses } from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
import TabPanel from './controls/TabPanel';
import * as api from '../../api';
import { useSnackbar } from 'notistack';
import { Home, KeyboardArrowRight } from '@mui/icons-material';
import QueuePanel from './controls/QueuePanel';

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
  const [selectedQueueId, setSelectedQueueId] = useState<string>("")



  const getQueueList = () => {
    const queueApi = new api.QueueApi();

    queueApi.apiQueueGet().then((queues) => {
      setQueues(queues)
    }).catch((err) => {
      enqueueSnackbar("Не удалось загрузить список очередей", { variant: 'error' });
    })
  };

  const selectQueue = (id: string) => {
    if (id == selectedQueueId)
      return setSelectedQueueId("");

    setSelectedQueueId(id);
  }

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
          <Box sx={{ width: '30%', height: "92vh", borderBottom: 'solid 1px #C1C1C1', justifyContent: 'center' }}>
            <Toolbar
              sx={{
                pl: { sm: 2 },
                pr: { xs: 1, sm: 1 },
                textAlign: 'center',
                display: "flex"
              }}
            >
              <Typography
                sx={{ flex: '1 1 100%', fontSize: '32px !important', fontWeight: '200' }}
                variant="h6"
                id="tableTitle"
                component="div"
              >

                {"Очереди"}
              </Typography>

              <Button variant='outlined' style={{ }}>+</Button>
            </Toolbar>


            <List>
              {
                queues.map((queue) =>
                  <ListItem selected={selectedQueueId == queue.id}>
                    <ListItemButton onClick={() => selectQueue(queue.id!)}>
                      <ListItemText primary={queue.name} />
                      <KeyboardArrowRight />
                    </ListItemButton>
                  </ListItem>
                )
              }
            </List>
          </Box>
          <Divider orientation="vertical" flexItem />
          <QueuePanel queueId={selectedQueueId}/>
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
