import { Runner } from 'api-client/generated';
import { Filter } from 'types/Filter';
import { StringBoolDict } from 'types/stringBoolDict';

const byTag = (r: Runner, tags: StringBoolDict) =>
  Object.entries(tags)
    .filter(([_, on]) => on)
    .every(([f, _]) => r.tagList.includes(f));
const byOnlineStatus = (r: Runner, online: boolean | null) => online === null || r.online === online;
const byActiveStatus = (r: Runner, active: boolean | null) => active === null || r.active === active;

export const byAll = (f: Filter) => (r: Runner) =>
  byTag(r, f.tags) && byOnlineStatus(r, f.online) && byActiveStatus(r, f.active);
