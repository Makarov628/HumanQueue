import { Box, Card, CardActionArea, CardContent, Container, Divider, Grid, Stack, Typography } from '@mui/material';
import { useTheme } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import React, { useState } from 'react';
import Service from './controls/Service';
import Language from './controls/Language';
import { IServiceItem } from './interfaces/IServiceItem';
import PrintDialog from './controls/PrintDialog';
import Content from '../common/Content';
import Head from '../common/Head';
import { useTranslation } from 'react-i18next';
import { useParams } from 'react-router-dom';

import * as api from '../../api';
import { useSnackbar } from 'notistack';
import { SignalRContext } from '../../signalr/signalr';

export interface ITerminal {

}

function Terminal({ ...props }: ITerminal) {

    const { terminalId } = useParams()
    const [terminal, setTerminal] = useState<api.TerminalResponse | null>(null);
    const { enqueueSnackbar } = useSnackbar();

    const theme = useTheme();
    const { t, i18n } = useTranslation();

    const [selectedLanguage, setSelectedLanguage] = React.useState<string | undefined>("kk");
    const [selected, setSelected] = React.useState<api.TerminalServiceResponse | null>(null);
    const [selectedService, setSelectedService] = React.useState<api.TerminalServiceResponse | null>(null);
    const [printState, setPrintState] = React.useState<boolean>(false);

    const getTerminal = () => {
        const terminalApi = new api.TerminalApi();
        terminalApi.apiTerminalIdGet(terminalId!).then(terminal => {
            setTerminal(terminal);
        }).catch(err => {
            enqueueSnackbar(err?.detail ?? "Терминал не найден.", { variant: "error" });
        });
    }




    React.useEffect(() => {
        if (!terminalId)
            return;

        getTerminal()


        SignalRContext.connection?.onreconnected(() => {
            console.log("Terminal reconnected")
            getTerminal()
        })

    }, [terminalId])

    React.useEffect(() => {
        setPrintState(false);
    }, [selected])

    React.useEffect(() => {
        if (printState == false && selected !== null) {
            setSelectedService(null);
            setSelected(null);
        }
    }, [printState])

    const OnLanguageSelect = (lang: string) => {
        i18n.changeLanguage(lang);
        setSelectedLanguage(terminal?.availableCultures?.find(c => c.culture == lang)?.culture)
    }

    const OnServiceSelect = (service: api.TerminalServiceResponse) => {
        if (service.childs?.length == 0) {
            setSelectedService(service);
            setPrintState(true);
        }
        else {
            setSelected(service);
        }
    }

    return (
        <Content>
            <CssBaseline />
            <Stack spacing={0} sx={{ height: '100vh' }}>
                <Head>
                    <Language
                        availableLanguages={terminal?.availableCultures ?? []}
                        currentLang={selectedLanguage ?? "kk"}
                        select={OnLanguageSelect}
                    />
                </Head>
                <Divider />
                <div style={{ height: '100vh', overflowY: 'auto' }}>
                    <Container maxWidth="xl">
                        <Box sx={{ minHeight: 'calc(100vh - 102px)', padding: '24px 0px', alignItems: 'center', justifyContent: 'center', display: 'flex' }} >
                            <Grid container spacing={3} sx={{ justifyContent: 'center' }}>
                                {
                                    selected?.childs != null && (
                                        <Grid item sx={{ paddingBottom: '24px' }}>
                                            <Card sx={{ width: 'fit-content', border: `2px solid ${theme.palette.primary.main}`, borderRadius: '16px' }} elevation={0}>
                                                <CardActionArea
                                                    sx={{ height: '160px', width: '380px', color: theme.palette.primary.main }}
                                                    onClick={() => {
                                                        setSelected(null);
                                                    }}
                                                >
                                                    <CardContent sx={{ padding: '0px 24px' }}>
                                                        <Typography variant="h4" component="div" sx={{ textAlign: 'center' }}>
                                                            {t('back')}
                                                        </Typography>
                                                    </CardContent>
                                                </CardActionArea>
                                            </Card>
                                        </Grid>
                                    )
                                }
                                {
                                    (selected?.childs == null ? terminal?.services : terminal?.services?.filter(x => x.id == selected.id)[0]?.childs)
                                        ?.map((service: api.TerminalServiceResponse, index: number) => {
                                            return (
                                                <Grid item key={index} sx={{ paddingBottom: '24px' }}>
                                                    <Service
                                                        language={selectedLanguage ?? "kk"}
                                                        item={service}
                                                        select={OnServiceSelect}
                                                    />
                                                </Grid>
                                            )
                                        })
                                }
                            </Grid>
                        </Box>
                    </Container>
                </div>
            </Stack>
            <PrintDialog
                currentLanguage={selectedLanguage}
                selectedServiceId={selectedService?.id}
                terminalId={terminalId}
                state={printState}
                setState={setPrintState}
            />
        </Content>
    );
}

export default Terminal;
