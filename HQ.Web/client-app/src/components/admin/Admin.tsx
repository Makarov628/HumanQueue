import React from 'react';
import Content from '../common/Content';
import { Box, Button, Container, CssBaseline, Divider, Stack } from '@mui/material';
import { default as dayjs } from 'dayjs';
import 'dayjs/locale/ru';
import useToken from '../common/Token';
import Head from '../common/Head';
import { useTranslation } from 'react-i18next';
import AlertDialog from '../common/AlertDialog';
import Tabs, { tabsClasses } from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
import TabPanel from './controls/TabPanel';

dayjs.locale('ru')

export interface IAdmin {

}

function Admin({ ...props }: IAdmin) {

    const { t, i18n } = useTranslation();
    const { token, removeToken, setToken } = useToken();

    const [exitDialog, setExitDialog] = React.useState<boolean>(false);
    const [tab, setTab] = React.useState<number>(0);

    React.useEffect(() => {

    }, [])

    const tabChange = (event: React.SyntheticEvent, newValue: number) => {
      setTab(newValue);
    };

    const Logout = () => {
        removeToken();
        window.location.reload();
    }

    return (
        <Content>
            <CssBaseline />
            <Stack spacing={0} sx={{ height: '100vh' }}>
                <Head>
                    <Button variant='outlined' onClick={() => setExitDialog(true)}>{t('exit')}</Button>
                </Head>
                <Divider />
                <Box sx={{ width: '100%', borderBottom: 'solid 1px #C1C1C1', display: 'flex', justifyContent: 'center' }}>
                        <Tabs
                          centered
                          value={tab}
                          onChange={tabChange}
                          variant="scrollable"
                          scrollButtons="auto"
                          sx={{
                            [`& .${tabsClasses.scrollButtons}`]: {
                              '&.Mui-disabled': { opacity: 0.3 },
                            },
                          }}
                        >
                          <Tab label="Услуги" />
                          <Tab label="Окна" />
                          <Tab label="Пользователи" />
                          <Tab label="Конфигурации" />
                        </Tabs>
                      </Box>
                <div style={{ height: '100vh', overflowY: 'auto' }}>
                    <Container maxWidth="xl">                    
                        <Box sx={{ minHeight: 'calc(100vh - 150px)', padding: '24px', justifyContent: 'center', display: 'flex' }} >
                          <TabPanel value={tab} index={0}>

                          </TabPanel>
                          <TabPanel value={tab} index={1}>

                          </TabPanel>
                          <TabPanel value={tab} index={2}>

                          </TabPanel>
                          <TabPanel value={tab} index={3}>

                          </TabPanel>
                        </Box>
                    </Container>
                </div>
            </Stack>
            <AlertDialog 
                No={setExitDialog} 
                Yes={Logout} 
                open={exitDialog} 
                title={t('exit')} 
                description={t('exitDescription')} 
                agreeText={t('exit')}/>
        </Content>
    );
}

export default Admin;
