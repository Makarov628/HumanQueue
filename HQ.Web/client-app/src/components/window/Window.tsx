import React from 'react';
import Content from '../common/Content';
import { Box, Container, CssBaseline, Divider, Grid, Stack, Typography } from '@mui/material';
import { default as dayjs } from 'dayjs';
import * as isLeapYear from 'dayjs/plugin/isLeapYear';
import 'dayjs/locale/ru';
import TabloTable from './controls/Table';
import { State } from '../enums/State';
import Head from '../common/Head';
import { useParams } from 'react-router-dom';
import { useSnackbar } from 'notistack';
import * as api from '../../api';
import { SignalRContext } from '../../signalr/signalr';

dayjs.locale('ru')

export interface IWindow {

}

function Window({ ...props }: IWindow) {
  const [date, setDate] = React.useState<number>(Date.now());

  const { queueId } = useParams()
  const [tablo, setTablo] = React.useState<api.TabloResponse | null>(null);
  const { enqueueSnackbar } = useSnackbar();

  const getTablo = () => {
    if (queueId) {
      const tabloApi = new api.TabloApi()
      tabloApi.apiTabloQueueIdGet(queueId!).then((tablo) => {
        setTablo(tablo)
      }).catch(() => {
        enqueueSnackbar("Очередь не найдена.", { variant: "error" });
      })
    }
  }

  SignalRContext.useSignalREffect("request-status-changed", () => {
    if (tablo) {
      getTablo()
    }
  }, [tablo]);

  React.useEffect(() => {
    if (!queueId) {
      return
    }
    getTablo()

    SignalRContext.connection?.onreconnected(() => {
      console.log("Tablo reconnected")
      getTablo()
    })

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
        <div style={{ height: '100vh', overflow: 'hidden' }}>
          <Container maxWidth="xl">
            <Box sx={{ minHeight: 'calc(100vh - 102px)', padding: '24px 0px', justifyContent: 'center', display: 'flex' }} >
              <Stack direction="row" spacing={4}>
                <TabloTable key="table-waiting" waitingRequests={tablo?.waiting ?? []} title={'В ожидании'} />
                <Divider orientation="vertical" flexItem />
                <TabloTable key="table-called" calledRequests={tablo?.called ?? []} title={'В работе'} />
              </Stack>
            </Box>
          </Container>
        </div>
      </Stack>
    </Content>
  );
}

export default Window;
