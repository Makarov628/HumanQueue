import { Box, Card, CardActionArea, CardContent, Divider, Grid, Typography } from '@mui/material';
import { useTheme } from '@mui/material/styles';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import CircularProgress from '@mui/material/CircularProgress';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import React, { useState } from 'react';
import { useSnackbar } from 'notistack';
import QRCode from "react-qr-code";

import * as api from '../../../api';
import { useTranslation } from 'react-i18next';
import { SignalRContext } from '../../../signalr/signalr';

export interface IPrintDialog {
    terminalId?: string
    selectedServiceId?: string
    currentLanguage?: string
    state: boolean
    setState: (x: boolean) => void
}

function PrintDialog({ ...props }: IPrintDialog) {
    const theme = useTheme();

    const { t } = useTranslation();
    const { enqueueSnackbar } = useSnackbar();
    const [request, setRequest] = React.useState<api.TerminalRequestResponse | null>(null);
    const [printEnabled, setPrintEnabled] = React.useState<boolean>(false);

    SignalRContext.useSignalREffect("request-status-changed", (event) => {
        closeIfQrCodeScanned(event.request.id.value)
    }, [request]);

    const closeIfQrCodeScanned = (requestIdFromEvent: string) => {
        if (requestIdFromEvent == request?.id) {
            handleClose()
        }
    }

    React.useEffect(() => {
        // const timer = setTimeout(() => {
        //     props.setState(false);
        // }, 3000);

        // return () => {
        //     clearTimeout(timer);
        // };

        if (props.state && props.terminalId && props.selectedServiceId && props.currentLanguage) {
            const terminalApi = new api.TerminalApi();
            terminalApi.apiTerminalCreateRequestPost({
                fromTerminalId: props.terminalId,
                toServiceId: props.selectedServiceId,
                culture: props.currentLanguage,
            }).then(newRequest => {
                setRequest(newRequest);
                setPrintEnabled(true);
                props.setState(true)
            }).catch(err => {
                if (err) {
                    err?.json()?.then((error: any) => {
                        handleClose()
                        enqueueSnackbar(error.detail ?? "Не удалось создать талон.", { variant: 'error' })
                    })
                    return;
                }

                handleClose()
                enqueueSnackbar("Не удалось создать талон.", { variant: 'error' })
            });
        }

    }, [props.state])

    const printRequest = () => {
        setPrintEnabled(false);
        const terminalApi = new api.TerminalApi();
        terminalApi.apiTerminalPrintRequestPost({
            terminalId: props.terminalId,
            requestId: request?.id
        }).then(() => {
            handleClose()
        }).catch(err => {
            if (err) {
                err?.json().then((error: any) => {
                    handleClose()
                    enqueueSnackbar(error.detail ?? "Не удалось распечатать талон.", { variant: 'error' })
                })
                return
            }
            handleClose()
            enqueueSnackbar("Не удалось распечатать талон.", { variant: 'error' })
        });
    }

    const handleClose = () => {
        props.setState(false);
        setRequest(null);
    };

    return (
        <Dialog
            open={props.state}
            BackdropProps={{
                style: {
                    backdropFilter: "blur(3px)"
                }
            }}
        >
            <DialogContent>
                <Box sx={{ minHeight: '550px', minWidth: '550px' }}>
                    {
                        request != null
                            ? <>
                                <Grid container spacing={3}>
                                    <Grid item xs={12} sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
                                        <div>
                                            <Typography component={'h2'} sx={{ fontSize: '8rem !important', color: theme.palette.primary.main, textAlign: 'center', width: "100%", height: "160px" }}>{request.number}</Typography>
                                            <h3 style={{ fontSize: '2rem !important', textAlign: 'center' }}>{request.serviceName}</h3>
                                        </div>
                                    </Grid>
                                    <Grid item xs={12} sx={{ marginTop: "16px", marginBottom: "16px" }}>
                                        <Divider />
                                    </Grid>
                                    <Grid item xs={6} sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
                                        <div>
                                            <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', marginBottom: "16px", marginTop: "16px" }}>
                                                <QRCode
                                                    value={`${window.location.origin}/request/${request.id}`}
                                                    size={200}
                                                />
                                            </div>
                                            {/* <Typography component={'div'} sx={{ fontSize: '1rem !important', textAlign: 'center', color: theme.palette.primary.main }}>Отсканировать QR-код</Typography> */}
                                        </div>
                                    </Grid>
                                    <Grid item xs={6} sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
                                        <div>
                                            <Card sx={{ width: 'fit-content', borderRadius: '16px', backgroundColor: "lightgray"/*theme.palette.primary.main*/, marginBottom: "16px", marginTop: "16px" }} elevation={0}>
                                                <CardActionArea
                                                    disabled={!printEnabled}
                                                    onClick={() => printRequest()}
                                                    sx={{ height: '200px', width: "200px", backgroundColor: printEnabled ? theme.palette.primary.main : "lightgray", color: 'white' }}
                                                >
                                                    <CardContent sx={{}}>
                                                        <Typography variant="h5" component="div" sx={{ textAlign: 'center' }}>
                                                            {t("print")}
                                                        </Typography>
                                                    </CardContent>
                                                </CardActionArea>
                                            </Card>
                                        </div>

                                    </Grid>
                                </Grid>

                            </>
                            :
                            <Grid container spacing={3}>
                                    <Grid item xs={12} sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
                                        <div>
                                            <CircularProgress size={150} />
                                        </div>
                                    </Grid>
                            </Grid> 
                            
                            
                    }

                </Box>
            </DialogContent>
        </Dialog>
    );
}

export default PrintDialog;
