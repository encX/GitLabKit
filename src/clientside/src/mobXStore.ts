import { action, computed, observable, makeObservable, runInAction, reaction } from 'mobx';
import { Group, Runner } from 'api-client/generated';
import { runnerApi, groupApi } from 'api-client';
import { byAll } from 'utils/filters';
import { StringBoolDict } from 'types/stringBoolDict';
import { Filter } from 'types/Filter';

export class MobXStore {
  @observable groupId?: number;
  @observable groupInfo?: Group | null;

  @observable runners: Runner[] = [];
  @computed get filteredRunners(): Runner[] {
    const filtered = this.runners.filter(byAll(this.filters));
    if (this.sort === 'id') return filtered.sort((a, b) => (a.id > b.id ? this.order : -this.order));
    if (this.sort === 'name')
      return filtered.sort((a, b) => (a.description > b.description ? this.order : -this.order));

    return filtered;
  }
  @computed get availableTags(): string[] {
    return Array.from(new Set(this.runners.flatMap((r) => r.tagList)));
  }

  @observable loading = false;
  @observable filters: Filter = { online: null, active: null, tags: {} };
  @observable sort: 'name' | 'id' | null = null;
  @observable order: 1 | -1 = 1;

  constructor() {
    makeObservable(this);

    reaction(
      () => this.groupId,
      () => {
        this.groupInfo = undefined;
        this.runners = [];
        this.fetchGroupRunners();
        this.fetchGroupInfo();
      }
    );
  }

  @action.bound
  setGroupId(groupId: number): void {
    this.groupId = groupId;
  }

  @action.bound
  async fetchGroupRunners(): Promise<void> {
    if (!this.groupId) {
      throw new Error('groupId is undefinded');
    }
    runInAction(() => (this.loading = true));

    const runners = (await runnerApi.getGroupRunners(this.groupId)).data;

    runInAction(() => {
      this.runners = runners;
      this.loading = false;
    });
  }

  @action.bound
  async fetchGroupInfo(): Promise<void> {
    if (!this.groupId) {
      throw new Error('groupId is undefinded');
    }

    try {
      const groupInfo = (await groupApi.getGroup(this.groupId)).data;

      runInAction(() => {
        this.groupInfo = groupInfo;
      });
    } catch {
      runInAction(() => {
        this.groupInfo = null;
      });
    }
  }

  @action.bound
  async setStatusForSingleRunner(runnerId: number, status: boolean): Promise<void> {
    const result = await runnerApi.setRunnerActiveStatus(runnerId, status);
    this.updateRunnerStatusInState(runnerId, result.data);
  }

  @action.bound
  async setStatusForFilteredRunners(status: boolean): Promise<void> {
    runInAction(() => (this.loading = true));
    const toUpdate: StringBoolDict = {};

    this.filteredRunners.forEach((r) => (toUpdate[r.id.toString()] = status));

    await runnerApi.bulkSetRunnerActiveStatus({ runners: toUpdate });
    this.filteredRunners.forEach((r) => this.updateRunnerStatusInState(r.id, status));
    runInAction(() => (this.loading = false));
  }

  @action.bound
  updateRunnerStatusInState(runnerId: number, status: boolean): void {
    const updateRunnerI = this.runners.findIndex((r) => r.id === runnerId);
    const updateRunner = Object.assign({}, this.runners[updateRunnerI]);
    updateRunner.active = status;

    this.runners = [...this.runners.slice(0, updateRunnerI), updateRunner, ...this.runners.slice(updateRunnerI + 1)];
  }

  @action.bound
  async deleteFilteredRunners(): Promise<void> {
    try {
      // warning popup?
      // await runnerApi.bulkDeleteRunner({ runnerIds: this.filteredRunners.map((r) => r.id) });
      const remaining = this.runners.filter((r) => !this.filteredRunners.includes(r));
      this.runners = remaining;
    } catch (error) {
      console.error(error);
      await this.fetchGroupRunners();
    }
  }

  @action.bound
  setOnlineFilter(online: boolean | null): void {
    this.filters.online = online;
  }

  @action.bound
  setActiveFilter(active: boolean | null): void {
    this.filters.active = active;
  }

  @action.bound
  setTagFilter(tagFilter: StringBoolDict): void {
    this.filters.tags = tagFilter;
  }

  @action.bound
  setSort(sort: string | null): void {
    if (sort === 'id' || sort === 'name' || sort === null) {
      this.sort = sort;
      return;
    }

    console.error(`Unknown sort type ${sort}`);
  }

  @action.bound
  setOrder(order: 1 | -1): void {
    this.order = order;
  }
}

export interface StoreProps {
  mobXStore?: MobXStore;
}

export const mobXStore = new MobXStore();
