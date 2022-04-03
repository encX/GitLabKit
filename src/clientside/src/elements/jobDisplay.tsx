import React from 'react';

import { Job } from 'api-client/generated';

interface JobDisplayProps {
  jobs: Job | Job[];
  className?: string;
}

const JobDisplay: React.FC<JobDisplayProps> = ({ jobs, className }) => {
  if (Array.isArray(jobs) && jobs.length === 0) return <div className={className}>idle</div>;
  if (Array.isArray(jobs) && jobs.length > 1) return <div className={className}>running {jobs.length} jobs</div>;

  const job = Array.isArray(jobs) ? jobs[0] : jobs;
  return (
    <div className={className}>
      {job.project.name} =&gt; {job.name}
      &nbsp;&nbsp;
      <a href={job.webUrl} target="_blank" rel="noreferrer" title="To job page on GitLab">
        (#{job.id})
      </a>
    </div>
  );
};

export default JobDisplay;
