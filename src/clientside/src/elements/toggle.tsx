import React from 'react';

import './toggle.css';

interface ToggleProps {
  checked: boolean;
  disabled: boolean;
  onChange: () => void;
}

const Toggle: React.FC<ToggleProps> = ({ checked, disabled, onChange }) => {
  return (
    <label className="switch">
      <input type="checkbox" checked={checked} disabled={disabled} onChange={onChange} />
      <span className="slider round" title={checked ? 'Disable' : 'Enable'}></span>
    </label>
  );
};

export default Toggle;
