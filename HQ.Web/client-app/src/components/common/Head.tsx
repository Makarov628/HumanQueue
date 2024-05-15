import React, { ReactNode } from 'react';
import { Box, Container, Stack, Typography } from '@mui/material';
import { useTranslation } from 'react-i18next';

type IHead = {
    children: ReactNode
}

function Head({ children }: IHead) {
    const { t, i18n } = useTranslation();
    const [date, setDate] = React.useState<number>(Date.now());

    React.useEffect(() => {
        setTimeout(() => {
            setDate(Date.now());
        }, 1000);
    }, [date])

    return (
        <Container maxWidth="xl">
            <Box sx={{ height: '100px' }}>
                <Stack direction="row" spacing={1} sx={{ height: '100px', alignItems: 'center' }}>
                    <img src='/Logo.svg' style={{ width: '66px' }} />
                    <Box sx={{ display: 'grid' }}>
                        <Typography variant="h5" sx={{ color: 'rgba(0, 0, 0, 0.54)' }}>{t('title')}</Typography>
                    </Box>
                    <Box sx={{ marginLeft: 'auto !important' }}>
                        {children}
                    </Box>
                </Stack>
            </Box>
        </Container>
    );
}

export default Head;
