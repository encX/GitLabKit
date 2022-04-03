import React from 'react';

interface TagProps {
  text: string;
}

const Tag: React.FC<React.PropsWithChildren<TagProps>> = ({ text }) => {
  return <div className="px-1 bg-gray-200 dark:bg-gray-700 rounded text-sm align-middle">{text}</div>;
};

export default Tag;
