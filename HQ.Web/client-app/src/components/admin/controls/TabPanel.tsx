import * as React from 'react';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';

interface TabPanelProps {
    children?: React.ReactNode;
    index: number;
    value: number;
}

function TabPanel(props: TabPanelProps) {
    const { children, value, index, ...other } = props;

    return (
        <div
            role="tabpanel"
            hidden={value !== index}
            id={`vertical-tabpanel-${index}`}
            aria-labelledby={`vertical-tab-${index}`}
            {...other}
        >
            {value === index && (
                <Box sx = {{
                    minWidth: '800px',
                    minHeight: 'calc(100vh - 198px)',
                    p: 2,
                    border: '1px dashed grey',
                  }}>
                    {children}
                </Box>
            )}
        </div>
    );
}

export default TabPanel