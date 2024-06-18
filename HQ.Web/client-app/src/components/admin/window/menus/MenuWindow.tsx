import React, { FC, useState } from 'react';
import { Menu, MenuItem, IconButton } from '@mui/material';
import { MoreVert, Delete } from '@mui/icons-material';

interface MenuWindowProps {
  open: boolean,
  anchorEl: null | HTMLElement
  onItemClicked: MenuWindowOnItemClicked;
  onClose: () => void;
}

export type MenuWindowItemType = 'delete'
export type MenuWindowOnItemClicked = (itemType: MenuWindowItemType) => void

const MenuWindow: FC<MenuWindowProps> = ({ open, anchorEl, onItemClicked, onClose }) => {
  return (
    <>
      <Menu
        anchorEl={anchorEl}
        open={Boolean(anchorEl) && open}
        onClose={onClose}
        transformOrigin={{ horizontal: 'right', vertical: 'top' }}
        anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
      >
        <MenuItem onClick={() => onItemClicked("delete")}>
          <Delete color='error' style={{ marginRight: "8px" }} /> Удалить
        </MenuItem>
      </Menu>
    </>
  );
};

export default MenuWindow;
