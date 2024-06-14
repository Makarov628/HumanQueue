import React, { FC, useState } from 'react';
import { Menu, MenuItem, IconButton } from '@mui/material';
import { MoreVert, Delete } from '@mui/icons-material';

interface MenuWindowProps {
  onDelete: () => void;
}

const MenuWindow: FC<MenuWindowProps> = ({ onDelete }) => {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);

  const handleClick = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  return (
    <>
      <IconButton onClick={handleClick}>
        <MoreVert />
      </IconButton>
      <Menu anchorEl={anchorEl} open={Boolean(anchorEl)} onClose={handleClose}>
        <MenuItem onClick={onDelete}>
          <Delete /> Удалить
        </MenuItem>
      </Menu>
    </>
  );
};

export default MenuWindow;
