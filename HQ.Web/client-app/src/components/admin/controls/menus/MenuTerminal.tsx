import React, { FC, useState } from 'react';
import { Menu, MenuItem, IconButton } from '@mui/material';
import { MoreVert, Print, Edit, Delete } from '@mui/icons-material';

interface MenuTerminalProps {
  onAssignPrinter: () => void;
  onRename: () => void;
  onDelete: () => void;
}

const MenuTerminal: FC<MenuTerminalProps> = ({ onAssignPrinter, onRename, onDelete }) => {
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
        <MenuItem onClick={onAssignPrinter}>
          <Print /> Назначить принтер
        </MenuItem>
        <MenuItem onClick={onRename}>
          <Edit /> Переименовать
        </MenuItem>
        <MenuItem onClick={onDelete}>
          <Delete /> Удалить
        </MenuItem>
      </Menu>
    </>
  );
};

export default MenuTerminal;
