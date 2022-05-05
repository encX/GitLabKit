import React, { useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { inject, observer } from 'mobx-react';

import FilterPanel from 'components/FilterPanel';
import ControlPanel from 'components/ControlPanel';
import RunnerList from 'components/RunnerList';
import { StoreProps } from 'mobXStore';
import SortPanel from 'components/SortPanel';

const GroupPage: React.FC<StoreProps> = ({ mobXStore }) => {
  const { setGroupId, groupInfo } = mobXStore!;
  const { groupId: _groupId } = useParams<'groupId'>();
  const groupId = Number.parseInt(_groupId ?? '');

  if (!groupId || isNaN(groupId)) {
    return <div className="text-7xl text-center font-bold">What are you doing?</div>;
  }

  const pageTitle = () => {
    if (groupInfo === undefined) return 'Fetching...';
    if (groupInfo === null) return 'Group not found!';
    return `Runners in ${groupInfo?.name}`;
  };

  useEffect(() => {
    setGroupId(groupId);
  }, []);

  return (
    <section>
      <h1 className="text-3xl font-bold mb-8">{pageTitle()}</h1>
      <FilterPanel />
      <SortPanel />
      <RunnerList />
      <ControlPanel />
    </section>
  );
};

export default inject('mobXStore')(observer(GroupPage));
