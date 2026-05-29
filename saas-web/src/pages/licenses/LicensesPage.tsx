import { useEffect, useState, useCallback } from 'react';
import { message, Tag } from 'antd';
import { SafetyCertificateOutlined, CheckCircleOutlined } from '@ant-design/icons';
import api from '../../lib/api';
import DataTable from '../../components/DataTable';
import type { TenantResponse } from '../../lib/types';

export default function LicensesPage() {
  const [tenants, setTenants] = useState<TenantResponse[]>([]);
  const [loading, setLoading] = useState(true);

  const fetchTenants = useCallback(async () => {
    setLoading(true);
    try {
      const res = await api.get<TenantResponse[]>('/superadmin/tenants');
      setTenants(res.data);
    } catch {
      message.error('Error al cargar tenants');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => { fetchTenants(); }, [fetchTenants]);

  const columns = [
    {
      title: 'Tenant',
      key: 'name',
      render: (_: unknown, r: TenantResponse) => (
        <div style={{ display: 'flex', alignItems: 'center', gap: 10 }}>
          <div style={{
            width: 32, height: 32, borderRadius: 6,
            background: 'var(--color-primary-light)',
            display: 'flex', alignItems: 'center', justifyContent: 'center',
            color: 'var(--color-primary)', fontSize: 14, fontWeight: 700,
          }}>
            {r.razonSocial.charAt(0)}
          </div>
          <div>
            <div style={{ fontWeight: 600, fontSize: 13 }}>{r.razonSocial}</div>
            <div style={{ fontSize: 11, color: 'var(--color-text-light)' }}>{r.ruc}</div>
          </div>
        </div>
      ),
    },
    { title: 'Plan', dataIndex: 'subscriptionPlan', key: 'subscriptionPlan', render: (v: string | null) => v || '-' },
    {
      title: 'Estado',
      dataIndex: 'isActive',
      key: 'isActive',
      render: (v: boolean) => (
        <Tag color={v ? 'green' : 'default'}>{v ? 'Activo' : 'Inactivo'}</Tag>
      ),
    },
    {
      title: 'Sistemas',
      key: 'systems',
      render: () => <Tag icon={<CheckCircleOutlined />} color="blue">4 activos</Tag>,
    },
    {
      title: 'Módulos',
      key: 'modules',
      render: () => <Tag icon={<CheckCircleOutlined />} color="cyan">12 activos</Tag>,
    },
    {
      title: 'Submódulos',
      key: 'submodules',
      render: () => <Tag icon={<CheckCircleOutlined />} color="purple">28 activos</Tag>,
    },
    {
      title: 'Acciones',
      key: 'actions',
      render: () => (
        <div style={{ display: 'flex', gap: 4 }}>
          <button className="action-btn" title="Ver licencias">
            <SafetyCertificateOutlined />
          </button>
        </div>
      ),
    },
  ];

  return (
    <div className="licenses-page">
      <DataTable
        title="Licencias por Tenant"
        subtitle="Sistemas, módulos y submódulos habilitados"
        columns={columns}
        dataSource={tenants}
        loading={loading}
        rowKey="id"
        modalTitle="Licencias del Tenant"
        modalOpen={false}
        modalWidth={600}
        onModalClose={() => {}}
        modalFooter={null}
      />
    </div>
  );
}