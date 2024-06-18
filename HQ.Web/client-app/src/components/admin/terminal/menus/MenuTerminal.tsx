import React, { FC, useState } from 'react';
import { Menu, MenuItem, IconButton } from '@mui/material';
import { MoreVert, Print, Edit, Delete } from '@mui/icons-material';


interface MenuTerminalProps {
  open: boolean,
  anchorEl: null | HTMLElement
  onItemClicked: MenuTerminalOnItemClicked;
  onClose: () => void;
}

export type MenuTerminalItemType = 'assign-printer' | 'rename' | 'delete';
export type MenuTerminalOnItemClicked = (itemType: MenuTerminalItemType) => void

const MenuTerminal: FC<MenuTerminalProps> = ({ open, anchorEl, onItemClicked, onClose }) => {
  return (
    <>
      <Menu
        anchorEl={anchorEl}
        open={Boolean(anchorEl) && open}
        onClose={onClose}
        transformOrigin={{ horizontal: 'right', vertical: 'top' }}
        anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
      >
        <MenuItem onClick={() => onItemClicked("assign-printer")}>
          <Print color='primary' style={{ marginRight: "8px" }}/> Назначить принтер
        </MenuItem>
        <MenuItem onClick={() => onItemClicked("rename")}>
          <Edit color='primary' style={{ marginRight: "8px" }} /> Переименовать
        </MenuItem>
        <MenuItem onClick={() => onItemClicked("delete")}>
          <Delete color='error' style={{ marginRight: "8px" }} /> Удалить
        </MenuItem>
      </Menu>
    </>
  );
};

export default MenuTerminal;
