import React from 'react';

import { Job } from 'api-client/generated';
import Row from 'elements/row';
import Tag from 'elements/tag';
import JobDisplay from 'elements/jobDisplay';
import dayjs from 'utils/dayjs';

interface JobRowProps {
  job: Job;
}

const JobRow: React.FC<JobRowProps> = ({ job }) => {
  const rowColor =
    job.status === 'failed'
      ? 'red'
      : job.status === 'success'
      ? 'green'
      : job.status === 'running'
      ? 'yellow'
      : undefined;

  const jobDuration = dayjs.duration(job.duration, 'seconds').humanize();

  const timeStats =
    job.status === 'running'
      ? `running for ${jobDuration}`
      : `${job.status} ${dayjs(job.startedAt).fromNow()} (${jobDuration})`;

  return (
    <Row color={rowColor}>
      <JobDisplay jobs={job} />
      <div className="flex">
        <Tag text={job.ref} />
      </div>
      <div className="hidden sm:block flex-auto" />
      <div>{timeStats}</div>
    </Row>
  );
};

export default JobRow;
