import { useState, useCallback } from 'react';
import { Empty, Spin } from 'antd';

interface UseFetchResult<T> {
  data: T[];
  loading: boolean;
  fetch: () => Promise<void>;
}

export function useFetch<T>(fetcher: () => Promise<{ data: T[] }>): UseFetchResult<T> {
  const [data, setData] = useState<T[]>([]);
  const [loading, setLoading] = useState(true);

  const fetch = useCallback(async () => {
    setLoading(true);
    try {
      const res = await fetcher();
      setData(res.data);
    } finally {
      setLoading(false);
    }
  }, [fetcher]);

  return { data, loading, fetch };
}

interface Props {
  loading: boolean;
  data: unknown[];
  children: React.ReactNode;
}

export function DataState({ loading, data, children }: Props) {
  if (loading) return <Spin style={{ display: 'block', margin: '40px auto' }} />;
  if (!data.length) return <Empty description="Sin registros" style={{ margin: '40px 0' }} />;
  return <>{children}</>;
}
