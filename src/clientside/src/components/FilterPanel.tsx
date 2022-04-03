import React, { useEffect } from 'react';
import { inject, observer } from 'mobx-react';

import ABToggle from './ABToggle';
import ActionButton from 'elements/actionButton';
import { StoreProps } from 'mobXStore';
import { StringBoolDict } from 'types/stringBoolDict';
import { useSearchParamListReflector, useSingleSearchParamReflector } from 'utils/useSearchParamReflector';

const FilterPanel: React.FC<StoreProps> = ({ mobXStore }) => {
  const { setOnlineFilter, setActiveFilter, setTagFilter, filters, availableTags, loading, filteredRunners } =
    mobXStore!;
  const [onTagInit, onTagUpdate] = useSearchParamListReflector('tag', (rawQs: string[]) => {
    const qsTags = rawQs.reduce((obj, tag) => ({ ...obj, [tag]: true }), {} as StringBoolDict);
    setTagFilter(qsTags);
  });
  const [onOnlineInit, onOnlineUpdate] = useSingleSearchParamReflector('online', (v: string | null) =>
    setOnlineFilter(v === 'true' ? true : v === 'false' ? false : null)
  );
  const [onActiveInit, onActiveUpdate] = useSingleSearchParamReflector('active', (v: string | null) =>
    setActiveFilter(v === 'true' ? true : v === 'false' ? false : null)
  );

  useEffect(() => {
    // init: read from qs and preset in store
    onTagInit();
    onActiveInit();
    onOnlineInit();
  }, []);

  useEffect(() => {
    const onTags = Object.entries(filters.tags)
      .filter(([_, v]) => v)
      .map(([t]) => t);

    onTagUpdate(onTags);
  }, [filters.tags]);
  useEffect(() => onActiveUpdate(filters.active), [filters.active]);
  useEffect(() => onOnlineUpdate(filters.online), [filters.online]);

  const toggleTag = (tag: string, value: boolean): void => {
    setTagFilter({ ...filters.tags, [tag]: value });
  };

  const tagFilterButtons = availableTags.sort().map((t) => {
    const count = filteredRunners.filter((r) => r.tagList.includes(t)).length;
    return (
      <ActionButton key={t} action={() => toggleTag(t, !filters.tags[t])} isOn={filters.tags[t]} disabled={loading}>
        {t} ({count})
      </ActionButton>
    );
  });

  const offlineCount = filteredRunners.filter((r) => !r.online).length;
  const onlineCount = filteredRunners.filter((r) => r.online).length;
  const activeCount = filteredRunners.filter((r) => r.active).length;
  const pausedCount = filteredRunners.filter((r) => !r.active).length;

  return (
    <section id="filter-tag" className="mb-8">
      <h3 className="text-xl mb-2">Filters</h3>
      <div className="flex mb-2 flex-wrap gap-1 items-middle">Tag: {tagFilterButtons}</div>
      <ABToggle
        title="Online"
        value={filters.online}
        setValue={setOnlineFilter}
        disabled={loading}
        onText={`Online (${onlineCount})`}
        offText={`Offline (${offlineCount})`}
      />
      <ABToggle
        title="Active"
        value={filters.active}
        setValue={setActiveFilter}
        disabled={loading}
        onText={`Active (${activeCount})`}
        offText={`Paused (${pausedCount})`}
      />
    </section>
  );
};

export default inject('mobXStore')(observer(FilterPanel));
