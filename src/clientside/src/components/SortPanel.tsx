import React, { useEffect } from 'react';
import { inject, observer } from 'mobx-react';

import ActionButton from 'elements/actionButton';
import { StoreProps } from 'mobXStore';
import { useSingleSearchParamReflector } from 'utils/useSearchParamReflector';

const SortPanel: React.FC<StoreProps> = ({ mobXStore }) => {
  const { setSort, sort, setOrder, order, loading } = mobXStore!;
  const [onSortInit, onSortUpdate] = useSingleSearchParamReflector('sort', setSort);
  const [onOrderInit, onOrderUpdate] = useSingleSearchParamReflector('order', (v) => {
    const order = Number.parseInt(v ?? '');
    if (order === 1 || order === -1) setOrder(order);
  });

  useEffect(() => {
    onSortInit();
    onOrderInit();
  }, []);

  useEffect(() => onSortUpdate(sort), [sort]);
  useEffect(() => onOrderUpdate(!sort ? null : order.toString()), [order]);

  const toggleSort = (toSort: string) => () => setSort(sort === toSort ? null : toSort);

  return (
    <section id="sort" className="mb-8">
      <div className="flex flex-row gap-4">
        <div id="sort-field">
          <h3 className="text-xl mb-2">Sort</h3>
          <div className="flex flex gap-1">
            <ActionButton action={toggleSort('name')} isOn={sort === 'name'} disabled={loading}>
              Name
            </ActionButton>
            <ActionButton action={toggleSort('id')} isOn={sort === 'id'} disabled={loading}>
              ID
            </ActionButton>
          </div>
        </div>
        <div id="sort-order">
          <h3 className="text-xl mb-2">Order</h3>
          <div className="flex gap-1">
            <ActionButton action={() => setOrder(1)} isOn={sort && order === 1} disabled={loading || !sort}>
              Asc
            </ActionButton>
            <ActionButton action={() => setOrder(-1)} isOn={sort && order === -1} disabled={loading || !sort}>
              Desc
            </ActionButton>
          </div>
        </div>
      </div>
    </section>
  );
};

export default inject('mobXStore')(observer(SortPanel));
