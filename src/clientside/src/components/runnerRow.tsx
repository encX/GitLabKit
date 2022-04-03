import React, { useState } from 'react';
import { inject, observer } from 'mobx-react';
import { Link } from 'react-router-dom';

import { Runner } from 'api-client/generated';
import { StoreProps } from 'mobXStore';
import Row from 'elements/row';
import Tag from 'elements/tag';
import Toggle from 'elements/toggle';
import JobDisplay from 'elements/jobDisplay';

interface RunnerRowProps extends StoreProps {
  runner: Runner;
}

const RunnerRow: React.FC<RunnerRowProps> = ({ runner: r, mobXStore }) => {
  const [updating, setUpdating] = useState(false);
  const { setStatusForSingleRunner, loading } = mobXStore!;

  const tags = () => r.tagList.map((t) => <Tag key={r.id + t} text={t} />);

  const rowColor = !r.online ? 'red' : r.currentJob.length > 0 ? 'yellow' : undefined;

  const updateStatus = async () => {
    setUpdating(true);
    await setStatusForSingleRunner(r.id, !r.active);
    setUpdating(false);
  };

  const copyIp = () => {
    navigator.clipboard.writeText(r.ipAddress);
  };

  return (
    <Row color={rowColor}>
      <div>
        <Toggle checked={r.active} disabled={loading || updating} onChange={updateStatus} />
        &nbsp;&nbsp;
        <Link to={`/r/${r.id}`} title="See job run history">
          {r.description} (#{r.id})
        </Link>
      </div>
      <div className="flex flex-wrap gap-1">{tags()}</div>
      <div className="hidden md:block">
        <a title="Click to copy" onClick={copyIp} className="cursor-pointer">
          {r.ipAddress}
        </a>
      </div>
      <JobDisplay className="sm:flex-auto sm:text-right" jobs={r.currentJob} />
    </Row>
  );
};

export default inject('mobXStore')(observer(RunnerRow));
