import { Card, CardActionArea, CardContent, Typography } from '@mui/material';
import { useTheme } from '@mui/material/styles';
import { IServiceItem } from '../interfaces/IServiceItem';

import * as api from '../../../api';

export interface ISerivice {
    language: string
    item: api.TerminalServiceResponse
    select: (item: api.TerminalServiceResponse) => void
}

function Service({ ...props }: ISerivice) {
    const theme = useTheme();

    return (
            <Card sx={{ width: 'fit-content', border: `2px solid ${theme.palette.primary.main}`, borderRadius: '16px', backgroundColor: theme.palette.primary.main }} elevation={0}>
                <CardActionArea
                    sx={{ height: '160px', width: '380px', backgroundColor: theme.palette.primary.main, color: 'white' }}
                    onClick={() => props.select(props.item)}
                    >
                    <CardContent sx={{ padding: '0px 24px'}}>
                        <Typography variant="h5" component="div" sx={{ textAlign: 'left'}}>
                            { props.item.name?.find(value => value.culture == props.language)?.value }
                        </Typography>
                    </CardContent>
                </CardActionArea>
            </Card>
    );
}

export default Service;
