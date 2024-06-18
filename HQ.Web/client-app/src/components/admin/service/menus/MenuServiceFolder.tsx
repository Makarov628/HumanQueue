import React, { FC, useState } from 'react';
import { Menu, MenuItem, IconButton } from '@mui/material';
import { MoreVert, Add, Edit, Delete } from '@mui/icons-material';

interface MenuServiceFolderProps {
  open: boolean,
  anchorEl: null | HTMLElement
  onItemClicked: MenuServiceFolderOnItemClicked;
  onClose: () => void;
}

export type MenuServiceFolderItemType = 'add-service' | 'rename' | 'delete'
export type MenuServiceFolderOnItemClicked = (itemType: MenuServiceFolderItemType) => void

const MenuServiceFolder: FC<MenuServiceFolderProps> = ({ open, anchorEl, onItemClicked, onClose }) => {
  return (
    <Menu
      anchorEl={anchorEl}
      open={Boolean(anchorEl) && open}
      onClose={onClose}
      transformOrigin={{ horizontal: 'right', vertical: 'top' }}
      anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
    >
      <MenuItem onClick={() => onItemClicked('add-service')}>
        <Add color='primary' style={{ marginRight: "8px" }}/> Добавить услугу
      </MenuItem>
      <MenuItem onClick={() => onItemClicked('rename')}>
        <Edit color='primary' style={{ marginRight: "8px" }}/> Переименовать
      </MenuItem>
      <MenuItem onClick={() => onItemClicked('delete')}>
        <Delete color='error' style={{ marginRight: "8px" }}/> Удалить
      </MenuItem>
    </Menu>
  );
};

export default MenuServiceFolder;
