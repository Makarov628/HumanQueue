import React, { FC, useState } from 'react';
import { Menu, MenuItem, IconButton } from '@mui/material';
import { MoreVert, Add, Edit, Delete } from '@mui/icons-material';

interface MenuServiceFolderProps {
  open: boolean,
  anchorEl: null | HTMLElement
  onItemClicked: MenuServiceFolderOnItemClicked;
  onClose: () => void;
}

export type MenuServiceFolderOnItemClicked = (itemType: 'add-service' | 'rename' | 'delete') => void

const MenuServiceFolder: FC<MenuServiceFolderProps> = ({ open, anchorEl, onItemClicked, onClose }) => {
  return (
    <Menu anchorEl={anchorEl} open={open && Boolean(anchorEl)} onClose={onClose}>
      <MenuItem onClick={() => onItemClicked('add-service')}>
        <Add /> Добавить услугу
      </MenuItem>
      <MenuItem onClick={() => onItemClicked('rename')}>
        <Edit /> Переименовать
      </MenuItem>
      <MenuItem onClick={() => onItemClicked('delete')}>
        <Delete /> Удалить
      </MenuItem>
    </Menu>
  );
};

export default MenuServiceFolder;
