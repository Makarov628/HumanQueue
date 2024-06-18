import React, { FC, useState } from 'react';
import { Menu, MenuItem, IconButton} from '@mui/material';
import WindowIcon from '@mui/icons-material/Window';
import { MoreVert, Edit, Delete, Window, Abc, TextFields } from '@mui/icons-material';

interface MenuServiceLiteralProps {
  open: boolean,
  anchorEl: null | HTMLElement
  onItemClicked: MenuServiceLiteralOnItemClicked;
  onClose: () => void;
}
export type MenuServiceLiteralItemType = 'change-literal' | 'attach-windows' | 'rename' | 'delete'
export type MenuServiceLiteralOnItemClicked = (itemType: MenuServiceLiteralItemType) => void

const MenuServiceLiteral: FC<MenuServiceLiteralProps> = ({ open, anchorEl, onItemClicked, onClose  }) => {
  return (
    <>
      <Menu 
        anchorEl={anchorEl} 
        open={Boolean(anchorEl) && open} 
        onClose={onClose} 
        transformOrigin={{ horizontal: 'right', vertical: 'top' }}
        anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
      >
        <MenuItem onClick={() => onItemClicked("change-literal")}>
          <TextFields color='primary' style={{ marginRight: "8px" }}/> Изменить литерал
        </MenuItem>
        <MenuItem onClick={() => onItemClicked("attach-windows")}>
          <Window color='primary' style={{ marginRight: "8px" }}/> Назначить окна
        </MenuItem>
        <MenuItem onClick={() => onItemClicked("rename")}>
          <Edit color='primary' style={{ marginRight: "8px" }}/> Переименовать
        </MenuItem>
        <MenuItem onClick={() => onItemClicked("delete")}>
          <Delete color='error' style={{ marginRight: "8px" }}/> Удалить
        </MenuItem>
      </Menu>
    </>
  );
};

export default MenuServiceLiteral;
