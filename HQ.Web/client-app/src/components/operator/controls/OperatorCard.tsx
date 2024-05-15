import React from 'react';
import { Box, Button, Divider, Stack, Typography } from '@mui/material';
import { default as dayjs } from 'dayjs';
import 'dayjs/locale/ru';
import { useTranslation } from 'react-i18next';
import useToken from '../../common/Token';
import { useTheme } from '@mui/material/styles';

import * as api from '../../../api';
import { enqueueSnackbar } from 'notistack';

dayjs.locale('ru')

const scale = {
    '@keyframes scaleAnimation': {
        '0%': {
            transform: 'scale(1)',
            opacity: '1'
        },
        '25%': {
            transform: 'scale(0.5)',
            opacity: '0.5'
        },
        '50%': {
            transform: 'scale(1)',
            opacity: '1'
        },
        '75%': {
            transform: 'scale(1)',
            opacity: '1'
        },
        '100%': {
            transform: 'scale(1)',
            opacity: '1'
        }
    },
}

export interface IOperatorCard {
    windowId?: string
    currentRequest?: api.CurrentRequestResponse
    buttonCallback: () => void
    getOneFromList: () => api.WaitingRequestResponse | undefined
}

function OperatorCard({ ...props }: IOperatorCard) {
    const theme = useTheme();
    // const { t, i18n } = useTranslation();
    // const { token, removeToken, setToken } = useToken();
    const [buttonsAvailable, setButtonsAvailable] = React.useState(true);

    // React.useEffect(() => {

    // }, [props.currentRequest])


    const isAllowToAnimate = () => {
        return props.currentRequest?.status?.name == "Called"
    }

    const canCall = () => {
        return !props.currentRequest;
    }

    const canLost = () => {
        return props.currentRequest?.status?.name == "Called"
    }

    const canWorkStart = () => {
        return props.currentRequest?.status?.name == "Called"
    }

    const canWorkStop = () => {
        return props.currentRequest?.status?.name == "WorkStarted"
    }

    const call = () => {
        const requestFromList = props.getOneFromList()
        if (props.windowId && requestFromList) {

            setButtonsAvailable(false);

            const windowApi = new api.WindowApi();
            windowApi.apiWindowRequestStatusCalledPost({
                calledByWindowId: props.windowId,
                requestId: requestFromList.id
            }).then(() => {

                setButtonsAvailable(true)
                props.buttonCallback()

            }).catch(err => {

                setButtonsAvailable(true)
                props.buttonCallback()
                err.json().then((error: any) => {
                    enqueueSnackbar(error.detail ?? "Не удалось вызвать клиента", { variant: 'error' })
                })

            })
        }
    }

    const lost = () => {
        if (props.currentRequest && canLost()) {
            setButtonsAvailable(false);

            const windowApi = new api.WindowApi();
            windowApi.apiWindowRequestStatusLostedPost({
                requestId: props.currentRequest.id
            }).then(() => {

                setButtonsAvailable(true)
                props.buttonCallback()

            }).catch(err => {

                setButtonsAvailable(true)
                props.buttonCallback()
                err.json().then((error: any) => {
                    enqueueSnackbar(error.detail ?? "Не удалось присвоить статус 'Не пришел'", { variant: 'error' })
                })

            })
        }
    }

    const workStart = () => {
        if (props.currentRequest && canWorkStart()) {
            setButtonsAvailable(false);

            const windowApi = new api.WindowApi();
            windowApi.apiWindowRequestStatusWorkStartedPost({
                requestId: props.currentRequest.id
            }).then(() => {

                setButtonsAvailable(true)
                props.buttonCallback()

            }).catch(err => {

                setButtonsAvailable(true)
                props.buttonCallback()
                err.json().then((error: any) => {
                    enqueueSnackbar(error.detail ?? "Не удалось присвоить статус 'Начать работу'", { variant: 'error' })
                })

            })
        }
    }

    const workEnd = () => {
        if (props.currentRequest && canWorkStop()) {
            setButtonsAvailable(false);

            const windowApi = new api.WindowApi();
            windowApi.apiWindowRequestStatusWorkEndedPost({
                requestId: props.currentRequest.id
            }).then(() => {

                setButtonsAvailable(true)
                props.buttonCallback()

            }).catch(err => {

                setButtonsAvailable(true)
                props.buttonCallback()
                err.json().then((error: any) => {
                    enqueueSnackbar(error.detail ?? "Не удалось присвоить статус 'Работа завершена'", { variant: 'error' })
                })

            })
        }
    }

    return (
        <Box sx={{
            minWidth: '300px',
            p: 2,
            border: '1px dashed grey',
            justifyContent: 'center',
            display: 'flex',
            alignItems: 'flex-start'
        }}>
            <Stack spacing={2}>
                <Button variant='outlined' onClick={() => call()} disabled={!(buttonsAvailable && canCall())} sx={{ minWidth: '250px' }}>Вызвать</Button>
                <Button variant='outlined' onClick={() => lost()} disabled={!(buttonsAvailable && canLost())} sx={{ minWidth: '250px' }}>Не пришел</Button>
                <Button variant='outlined' onClick={() => workStart()} disabled={!(buttonsAvailable && canWorkStart())}>Начать</Button>
                <Button variant='outlined' onClick={() => workEnd()} disabled={!(buttonsAvailable && canWorkStop())}>Завершить</Button>
                <Divider />
                <Box sx={{ paddingTop: '66px', display: 'flex', justifyContent: 'center' }}>
                    {
                        props.currentRequest ?
                            <div>
                                <Typography variant="h2"
                                    sx={{
                                        fontWeight: 500,
                                        color: theme.palette.primary.main,
                                        scale: isAllowToAnimate() ? scale : null,
                                        animation: isAllowToAnimate() ? `scaleAnimation 2s 0.0s infinite` : null
                                    }}>
                                    {props.currentRequest.number}
                                </Typography>
                                <h3 style={{ fontSize: '2rem !important',  textAlign: 'center' }}>{props.currentRequest.serviceName}</h3>
                            </div>

                            : null
                    }

                </Box>
            </Stack>
        </Box>
    );
}

export default OperatorCard;
