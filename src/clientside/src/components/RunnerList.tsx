import React from 'react';
import { inject, observer } from 'mobx-react';

import RunnerRow from './runnerRow';
import { StoreProps } from 'mobXStore';

const RunnerList: React.FC<StoreProps> = ({ mobXStore }) => {
  const { filteredRunners } = mobXStore!;

  const runnerRows = () => filteredRunners.map((r) => <RunnerRow key={r.id + r.active.toString()} runner={r} />);

  return <section className="mb-8">{runnerRows()}</section>;
};

export default inject('mobXStore')(observer(RunnerList));
