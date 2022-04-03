import React from 'react';

interface ActionButtonProps {
  action?: () => void;
  isOn?: boolean | null;
  disabled?: boolean;
}

const ActionButton: React.FC<React.PropsWithChildren<ActionButtonProps>> = ({ action, disabled, isOn, children }) => {
  const classes = [
    'transition',
    'border',
    'border-gray-700',
    'dark:border-gray-200',
    'disabled:opacity-50',
    'rounded',
    'px-2',
    'py-1',
  ];
  if (isOn)
    classes.push(
      'bg-gray-700',
      'dark:bg-gray-200',
      'hover:bg-black',
      'dark:hover:bg-white',
      'text-gray-200',
      'dark:text-gray-700'
    );
  else if (!disabled) classes.push('hover:bg-gray-200', 'dark:hover:bg-gray-800');
  return (
    <button className={classes.join(' ')} onClick={action} disabled={disabled}>
      {children}
    </button>
  );
};

export default ActionButton;
