import { Box, CircularProgress, CssBaseline, Divider, Grid, Typography, useTheme } from "@mui/material";
import Content from "../common/Content";
import { useParams } from "react-router-dom";
import React from "react";

import * as api from '../../api';
import { useSnackbar } from 'notistack';
import { useTranslation } from "react-i18next";
import { SignalRContext } from "../../signalr/signalr";




function Request({ ...props }) {

  const theme = useTheme();
  const { requestId } = useParams()
  const [request, setRequest] = React.useState<api.RequestResponse | null>(null);
  const { enqueueSnackbar } = useSnackbar();
  const { t, i18n } = useTranslation();

  const getRequest = () => {
    const serviceApi = new api.ServiceApi();
    serviceApi.apiServiceRequestIdGet(requestId!).then((req) => {
      setRequest(req);
      i18n.changeLanguage(req.culture);
      updateManifest(req);
    }).catch(err => {
      enqueueSnackbar("Электронный талон не найден.", { variant: "error" });
    });
  }

  const updateManifest = (req: api.RequestResponse) => {
    const manifestForRequest = {
      "name": `${t("ticket")} ${req.number}`,
      "short_name": `${req.number}`,
      "start_url": `./request/${req.id}`,
      "display": "standalone",
      "orientation": "portrait",
      "theme_color": "#009c7a",
      "background_color": "#ffffff",
      "icons": [
        {
          "src": "icons/manifest-icon-192.maskable.png",
          "sizes": "192x192",
          "type": "image/png",
          "purpose": "any"
        },
        {
          "src": "icons/manifest-icon-192.maskable.png",
          "sizes": "192x192",
          "type": "image/png",
          "purpose": "maskable"
        },
        {
          "src": "icons/manifest-icon-512.maskable.png",
          "sizes": "512x512",
          "type": "image/png",
          "purpose": "any"
        },
        {
          "src": "icons/manifest-icon-512.maskable.png",
          "sizes": "512x512",
          "type": "image/png",
          "purpose": "maskable"
        }
      ],
    }

    const stringManifest = JSON.stringify(manifestForRequest);
    const blob = new Blob([stringManifest], { type: "application/json; charset=utf-8" });
    const manifestURL = URL.createObjectURL(blob);
    // document.querySelector(`#manifest`)?.setAttribute("href", manifestURL);
  }

  SignalRContext.useSignalREffect("request-status-changed", (event) => {
    if (requestId == event.request.id.value) {
      getRequest()
    }
  }, [requestId]);

  React.useEffect(() => {
    if (!requestId) {
      return
    }
    getRequest()

    SignalRContext.connection?.onreconnected(() => {
      console.log("Request reconnected")
      getRequest()
    })

  }, [requestId]);

  return (
    <Content sx={{ display: 'flex', justifyContent: 'center', alignItems: !request ? 'center' : 'start' }}>
      <CssBaseline />
      {!request ? <CircularProgress size={100} /> : null}
      {request ?
        <Grid container spacing={3} sx={{ padding: "16px" }}>
          <Grid item xs={12} sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', marginTop: "16px" }}>
            <div>
              <Typography component={'h3'} sx={{ fontSize: '5rem !important', fontWeight: 600, color: theme.palette.primary.main, textAlign: 'center', width: "100%" }}>{request.number}</Typography>
              <h3 style={{ fontSize: '2rem !important', textAlign: 'center' }}>{request.serviceName}</h3>
            </div>
          </Grid>
          <Grid item xs={12} sx={{ marginTop: "8px" }}>
            <Divider />
          </Grid>
          <Grid item xs={12} sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
            <div>
              <h4 style={{ fontSize: '1.5rem', textAlign: 'center' }}>{t(request.status?.name ?? "")}</h4>
            </div>
          </Grid>
          <Grid item xs={12} sx={{}}>
            <Divider />
          </Grid>
          {
            request.windowNumber ?
              <Grid item xs={12} sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', marginTop: "-16px" }}>
                <div>
                  <h2 style={{ fontSize: '6.5rem', textAlign: 'center' }}>{request.windowNumber}</h2>
                  <h4 style={{ marginTop: "-32px", fontSize: '2.5rem', textAlign: 'center' }}>{t("window")}</h4>
                </div>
              </Grid>
              : null
          }
        </Grid>
        : null}
    </Content>
  );
}

export default Request;