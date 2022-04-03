import { useSearchParams, createSearchParams, useLocation } from 'react-router-dom';
import { useEffect } from 'react';

type OnInit = () => void;
type OnUpdate<T> = (v: T | null) => void;

export function useSingleSearchParamReflector<T extends boolean | string>(
  queryName: string,
  setValue: (v: string | null) => void
): [OnInit, OnUpdate<T>] {
  const [searchParams, setSearchParams] = useSearchParams();
  const location = useLocation();

  const onInit = () => {
    const qsValue = searchParams.get(queryName);
    setValue(qsValue);
  };

  const onUpdate = (newValue: T | null) => {
    const searchObject = Object.fromEntries(createSearchParams(searchParams));
    const current = searchParams?.get(queryName);

    const isChanged = current !== (newValue?.toString() ?? null);
    if (!isChanged) return;

    if (newValue !== undefined && newValue !== null) {
      setSearchParams({ ...searchObject, [queryName]: `${newValue}` });
    } else {
      delete searchObject[queryName];
      setSearchParams(searchObject);
    }
  };

  useEffect(() => {
    onInit();
  }, [location]);

  return [onInit, onUpdate];
}

// Use csv instead of builtin feature from react-router-dom
// because it's using stupid iterator type and there's no functionality to just override single key
export function useSearchParamListReflector(
  queryName: string,
  setValue: (v: string[]) => void
): [OnInit, OnUpdate<string[]>] {
  const [searchParams, setSearchParams] = useSearchParams();
  const location = useLocation();

  const onInit = () => {
    const qsValue = searchParams.get(queryName);

    setValue(qsValue?.split(',') ?? []);
  };

  const onUpdate = (newValue: string[] | null) => {
    const searchObject = Object.fromEntries(createSearchParams(searchParams));
    const current = searchParams?.get(queryName)?.split(',')?.sort() ?? [];
    const newArr = newValue?.sort() ?? [];
    const isChanged = JSON.stringify(current) !== JSON.stringify(newArr);

    if (!isChanged) return;

    if (newValue !== undefined && newValue !== null && newValue.length) {
      setSearchParams({ ...searchObject, [queryName]: `${newValue.join(',')}` });
    } else {
      delete searchObject[queryName];
      setSearchParams(searchObject);
    }
  };

  useEffect(() => {
    onInit();
  }, [location]);

  return [onInit, onUpdate];
}
