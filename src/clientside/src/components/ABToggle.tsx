import React from 'react';

import ActionButton from 'elements/actionButton';

interface ABToggleProps {
  value: boolean | null;
  setValue: (v: boolean | null) => void;
  title: string;
  onText: string;
  offText: string;
  disabled: boolean;
}

const ABToggle: React.FC<ABToggleProps> = ({ value, setValue, title, onText, offText, disabled }) => {
  return (
    <div className="flex mb-2 flex-wrap gap-1">
      {title}:
      <ActionButton action={() => setValue(value !== true ? true : null)} isOn={value === true} disabled={disabled}>
        {onText}
      </ActionButton>
      <ActionButton action={() => setValue(value !== false ? false : null)} isOn={value === false} disabled={disabled}>
        {offText}
      </ActionButton>
    </div>
  );
};

export default ABToggle;
