import React from 'react';
import { inject, observer } from 'mobx-react';

import ActionButton from 'elements/actionButton';
import { StoreProps } from 'mobXStore';

const ControlPanel: React.FC<StoreProps> = ({ mobXStore }) => {
  const { setStatusForFilteredRunners, deleteFilteredRunners, loading } = mobXStore!;

  return (
    <section id="all-action" className="mb-8">
      <h3 className="text-xl mb-2">Action to all visible runners (DANGER!)</h3>
      <div className="flex flex-wrap gap-1">
        <ActionButton action={() => setStatusForFilteredRunners(true)} disabled={loading}>
          Enable
        </ActionButton>
        <ActionButton action={() => setStatusForFilteredRunners(false)} disabled={loading}>
          Disable
        </ActionButton>
        <ActionButton action={() => deleteFilteredRunners()} disabled={loading}>
          Delete
        </ActionButton>
      </div>
    </section>
  );
};

export default inject('mobXStore')(observer(ControlPanel));
