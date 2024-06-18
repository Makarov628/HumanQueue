import * as React from 'react';
import * as api from '../../../api';
import ServiceHeader from './ServiceHeader';
import ServiceList from './ServiceList';
import { MenuServiceFolderItemType } from './menus/MenuServiceFolder';
import { MenuServiceLiteralItemType } from './menus/MenuServiceLiteral';
import AddServiceDialog from './dialogs/AddServiceDialog';
import DeleteServiceDialog from './dialogs/DeleteServiceDialog';
import { act } from 'react-dom/test-utils';
import RenameServiceDialog from './dialogs/RenameServiceDialog';
import ChangeLiteralServiceDialog from './dialogs/ChangeLiteralServiceDialog';
import AttachWindowsServiceDialog from './dialogs/AttachWindowsServiceDialog';


interface ServiceProps {
  queueId: string,
  services?: Array<api.QueueServiceResponse>
  windows?: Array<api.QueueWindowResponse>,
  cultures?: Array<api.AvailableCultureResponse>
  onUpdate: () => void
}

function Service(props: ServiceProps) {
  const { queueId, services, windows, cultures, onUpdate } = props;
  const [selectedService, setSelectedService] = React.useState<api.QueueServiceResponse | null>(null)
  const [action, setAction] = React.useState<MenuServiceFolderItemType | MenuServiceLiteralItemType | ''>('')

  React.useEffect(() => {
    // console.log(services)
  }, [services])

  const handleServiceSelectedAction = (action: MenuServiceFolderItemType | MenuServiceLiteralItemType, service: api.QueueServiceResponse | null) => {
    setAction(action);
    setSelectedService(service)
  }

  const onAddAction = () => {
    setAction("add-service");
    setSelectedService(null);
  }

  const onClose = () => {
    setAction("");
    setSelectedService(null);
    onUpdate();
  }

  return (
    <>
      <ServiceHeader onAddAction={onAddAction} />
      <ServiceList services={services} windows={windows} onSelectedAction={handleServiceSelectedAction} />
      
      <AddServiceDialog 
        open={action == "add-service"} 
        parentService={selectedService ?? undefined}
        cultures={cultures} 
        queueId={queueId} 
        onClose={onClose}
      />

      <AttachWindowsServiceDialog
        open={action == "attach-windows"}
        serviceId={selectedService?.id}
        services={services}
        windows={windows}
        onChange={onUpdate}
        onClose={onClose}
      />

      <ChangeLiteralServiceDialog
        open={action == "change-literal"}
        service={selectedService ?? undefined}
        onClose={onClose}
      />

      <RenameServiceDialog
        open={action == "rename"}
        cultures={cultures}
        service={selectedService ?? undefined}
        onClose={onClose}
      />

      <DeleteServiceDialog
        open={action == "delete"}
        service={selectedService ?? undefined}
        onClose={onClose}
      />
    </>
  );
}

export default Service;