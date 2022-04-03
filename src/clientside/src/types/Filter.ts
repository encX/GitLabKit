import { StringBoolDict } from './stringBoolDict';

export type Filter = {
  online: boolean | null;
  active: boolean | null;
  tags: StringBoolDict;
};
