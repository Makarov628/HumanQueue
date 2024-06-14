import React, { FC, useState } from 'react';
import { Menu, MenuItem, IconButton } from '@mui/material';
import { MoreVert, Edit, Delete } from '@mui/icons-material';

interface MenuServiceLiteralProps {
  onChangeLiteral: () => void;
  onAttachWindows: () => void;
  onRename: () => void;
  onDelete: () => void;
}

const MenuServiceLiteral: FC<MenuServiceLiteralProps> = ({ onChangeLiteral, onAttachWindows, onRename, onDelete }) => {
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
        <MenuItem onClick={onChangeLiteral}>
          <Edit /> Изменить литерал
        </MenuItem>
        <MenuItem onClick={onAttachWindows}>
          <Edit /> Назначить окна
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

export default MenuServiceLiteral;
