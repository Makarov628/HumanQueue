import React from 'react';
import Content from '../common/Content';
import { Box, Button, Container, CssBaseline, Divider, Stack } from '@mui/material';
import { default as dayjs } from 'dayjs';
import 'dayjs/locale/ru';
import useToken from '../common/Token';
import Head from '../common/Head';
import { useTranslation } from 'react-i18next';
import AlertDialog from '../common/AlertDialog';
import OperatorTable from './controls/OperatorTable';
import OperatorCard from './controls/OperatorCard';
import { useParams } from 'react-router-dom';
import { useSnackbar } from 'notistack';
import * as api from '../../api';
import { SignalRContext } from '../../signalr/signalr';

dayjs.locale('ru')

export interface IOperator {

}

function Operator({ ...props }: IOperator) {

    const { t, i18n } = useTranslation();

    const { windowId } = useParams()
    const [window, setWindow] = React.useState<api.WindowResponse | null>(null);
    const { enqueueSnackbar } = useSnackbar();

    const getWindow = () => {
        const windowApi = new api.WindowApi();
        windowApi.apiWindowIdGet(windowId!).then((window) => {
            setWindow(window)
        }).catch((err) => {
            enqueueSnackbar("Окно не найдено", { variant: 'error' });
        })
    };

    // const { token, removeToken, setToken } = useToken();
    // const [exitDialog, setExitDialog] = React.useState<boolean>(false);
    // const Logout = () => {
    //     removeToken();
    //     window.location.reload();
    // }s

   

    SignalRContext.useSignalREffect("request-status-changed", () => {
        if (window) {
            getWindow()
        }
    }, [window]);

    const updateWindow = () => {
        getWindow()
    }

    const getOneFromList = (): api.WaitingRequestResponse | undefined => {
        return window?.waitingRequests?.at(0);
    }

    React.useEffect(() => {
        if (!windowId)
            return

        getWindow()

        SignalRContext.connection?.onreconnected(() => {
            console.log("Window reconnected")
            getWindow()
        })

    }, [windowId])

    /*
      windowId?: string
    currentRequest?: api.CurrentRequestResponse
    buttonCallback: () => void
    getOneFromList: () => api.WaitingRequestResponse | undefined
    */

    return (
        <Content>
            <CssBaseline />
            <Stack spacing={0} sx={{ height: '100vh' }}>
                <Head>
                    {/* <Button variant='outlined' onClick={() => setExitDialog(true)}>{t('exit')}</Button> */}
                </Head>
                <Divider />
                <div style={{ height: '100vh', overflowY: 'auto' }}>
                    <Container maxWidth="xl">
                        <Box sx={{ minHeight: 'calc(100vh - 102px)', padding: '24px 0px', justifyContent: 'center', display: 'flex' }} >

                            <Stack direction="row" spacing={4}>
                                <OperatorTable waitingRequests={window?.waitingRequests} />
                                <OperatorCard
                                    windowId={window?.id}
                                    currentRequest={window?.currentRequest}
                                    buttonCallback={updateWindow}
                                    getOneFromList={getOneFromList}
                                />
                            </Stack>
                        </Box>
                    </Container>
                </div>
            </Stack>
            {/* <AlertDialog 
                No={setExitDialog} 
                Yes={Logout} 
                open={exitDialog} 
                title={t('exit')} 
                description={t('exitDescription')} 
                agreeText={t('exit')}/> */}
        </Content>
    );
}

export default Operator;
