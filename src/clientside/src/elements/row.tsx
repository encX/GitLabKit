import React from 'react';

interface RowProps {
  color?: 'red' | 'yellow' | 'green';
}

const Row: React.FC<React.PropsWithChildren<RowProps>> = ({ color, children }) => {
  const defaultClasses = [
    'flex',
    'flex-col',
    'sm:flex-row',
    'flex-wrap',
    'sm:items-center',
    'gap-4',
    'px-4',
    'py-2',
    'my-2',
    'border-solid',
    'border',
    'rounded',
  ];

  // it's tailwind
  if (color === 'red') {
    defaultClasses.push(`bg-red-200`, `border-red-300`, `dark:bg-red-900`, `dark:border-red-800`);
  } else if (color === 'yellow') {
    defaultClasses.push(`bg-yellow-200`, `border-yellow-300`, `dark:bg-yellow-900`, `dark:border-yellow-800`);
  } else if (color === 'green') {
    defaultClasses.push(`bg-green-100`, `border-green-200`, `dark:bg-green-900`, `dark:border-green-800`);
  } else {
    defaultClasses.push('bg-gray-100', 'border-gray-200', 'dark:bg-gray-800', 'dark:border-gray-700');
  }
  return <div className={defaultClasses.join(' ')}>{children}</div>;
};

export default Row;
