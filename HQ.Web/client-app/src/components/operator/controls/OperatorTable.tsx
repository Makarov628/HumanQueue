import { State } from "../../enums/State"
import React from 'react';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import ArrowForwardIcon from '@mui/icons-material/ArrowForward';
import { Toolbar, Typography } from "@mui/material";
import { useTheme } from '@mui/material/styles';
import dayjs from "dayjs";
import relativeTime from "dayjs/plugin/relativeTime";
import * as api from '../../../api';
import { useTranslation } from "react-i18next";


export interface IOperatorTable {
    waitingRequests?: api.WaitingRequestResponse[]
}

interface IRequest {
    id: number
    name: string
    service: string
    state: number,
    date: string
    elapsed: number
}

dayjs.extend(relativeTime);

export default function OperatorTable({ ...props }: IOperatorTable) {
    const theme = useTheme();
    const [date, setDate] = React.useState<number>(Date.now());

    React.useEffect(() => {
        const timer = setTimeout(() => {
            setDate(Date.now());
        }, 1000);

        return () => {
            clearTimeout(timer);
        }
    }, [date])



    const formattedMinutes = (dateString: string) => {
        return dayjs(dateString).to(date, true)
    }

    return (
        <TableContainer sx={{ maxHeight: 'calc(100vh - 150px)', border: '1px dashed grey' }}>
            <Table stickyHeader sx={{ minWidth: 650 }} aria-label="simple table">
                <TableHead>
                    <TableRow>
                        <TableCell align="center" sx={{ minWidth: '150px' }}>Номер</TableCell>
                        <TableCell align="left" sx={{ minWidth: '400px' }}>Услуга</TableCell>
                        <TableCell align="left" sx={{ minWidth: '150px' }}>Дата выдачи</TableCell>
                        <TableCell align="center" sx={{ minWidth: '150px' }}>Время ожидания</TableCell>
                        <TableCell align="center" sx={{ minWidth: '70px' }}>Язык</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {props.waitingRequests?.map((request: api.WaitingRequestResponse, index: number) => (
                        <TableRow
                            hover role="checkbox"
                            key={`request_${index}`}
                            sx={{
                                '&:last-child td, &:last-child th': { border: 0 }
                            }}
                        >
                            <TableCell align="center"
                                sx={{
                                    color: theme.palette.primary.main,
                                    fontWeight: 500,
                                    fontSize: "1.5rem"
                                }}>{request.number}</TableCell>
                            <TableCell align="left">{request.serviceName}</TableCell>
                            <TableCell align="left">{dayjs(Date.parse(request.createdDate?.toString() ?? "")).format('DD.MM.YYYY HH:mm:ss')}</TableCell>
                            <TableCell align="center">{formattedMinutes(request.createdDate?.toString() ?? "")}</TableCell>
                            <TableCell align="center">{request.culture}</TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    )
}