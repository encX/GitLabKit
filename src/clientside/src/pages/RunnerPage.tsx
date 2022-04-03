import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';

import { runnerApi } from 'api-client';
import { Job, Runner } from 'api-client/generated';
import dayjs from 'utils/dayjs';
import JobRow from 'components/jobRow';
import Tag from 'elements/tag';

const RunnerPage: React.FC = () => {
  const { runnerId: _runnerId } = useParams<'runnerId'>();
  const runnerId = Number.parseInt(_runnerId ?? '');

  if (isNaN(runnerId)) {
    return <div className="text-7xl text-center font-bold">What are you doing?</div>;
  }

  const navigate = useNavigate();
  const [jobs, setJobs] = useState<Job[] | undefined | null>();
  const [runner, setRunner] = useState<Runner | undefined | null>();

  useEffect(() => {
    runnerApi
      .getSingleRunner(runnerId)
      .then((res) => setRunner(res.data))
      .catch(() => setRunner(null));
    runnerApi
      .getRunnerJobHistory(runnerId)
      .then((res) => setJobs(res.data))
      .catch(() => setJobs(null));
  }, []);

  const runnerDetails = () => {
    if (runner === undefined) return <div>fetching...</div>;
    if (runner === null) return <div>runner not found!</div>;

    return (
      <>
        <div>Name: {runner.description}</div>
        <div>IP: {runner.ipAddress}</div>
        <div>
          {runner.online ? 'Online' : 'Offline'}, {runner.active ? 'Enabled' : 'Disabled'}
        </div>
        <div>Last contact: {dayjs(runner.contactedAt).fromNow()}</div>
        <div className="flex gap-1">
          {runner.tagList.map((t) => (
            <Tag key={runner.id + t} text={t} />
          ))}
        </div>
      </>
    );
  };

  const goBack = () => navigate(-1);

  const jobRows = () => {
    if (jobs === undefined) return <div>fetching...</div>;
    if (jobs === null) return <div>jobs not found for runner!</div>;
    if (jobs.length === 0) return <div>this runner has not run anything yet</div>;
    return jobs.map((j) => <JobRow key={j.id} job={j} />);
  };

  return (
    <section>
      <a className="cursor-pointer" onClick={goBack}>
        &lt; back
      </a>
      <h1 className="text-3xl font-bold mb-8">Runner #{runnerId}</h1>
      <div className="mb-8">{runnerDetails()}</div>
      <h1 className="text-2xl font-bold mb-8">Jobs</h1>
      {jobRows()}
    </section>
  );
};

export default RunnerPage;
