import { useEffect, useState, useCallback } from 'react';
import api from '../../lib/api';
import CrudTable from '../../components/CrudTable';
import StatusTag from '../../components/StatusTag';
import type { UserResponse } from '../../lib/types';

export default function UsersList() {
  const [users, setUsers] = useState<UserResponse[]>([]);
  const [loading, setLoading] = useState(true);

  const fetch = useCallback(async () => {
    setLoading(true);
    try {
      const res = await api.get<UserResponse[]>('/admin/users');
      setUsers(res.data);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => { fetch(); }, [fetch]);

  const columns = [
    { title: 'Email', dataIndex: 'email', key: 'email' },
    {
      title: 'Nombre', key: 'name',
      render: (_: unknown, r: UserResponse) => `${r.firstName ?? ''} ${r.lastName ?? ''}`.trim() || '-',
      responsive: ['md' as const],
    },
    { title: 'Estado', dataIndex: 'isActive', key: 'isActive', render: (v: boolean) => <StatusTag active={v} /> },
  ];

  return (
    <CrudTable<UserResponse>
      title="Usuarios"
      subtitle="Usuarios del sistema"
      columns={columns}
      dataSource={users}
      loading={loading}
      rowKey="id"
      modalTitle=""
      modalOpen={false}
      onModalClose={() => {}}
    />
  );
}
