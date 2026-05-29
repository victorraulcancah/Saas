import { useEffect, useState, useCallback } from 'react';
import { message } from 'antd';
import { SafetyOutlined, CheckCircleOutlined } from '@ant-design/icons';
import api from '../../lib/api';
import DataTable from '../../components/DataTable';
import StatCard from '../../components/StatCard';
import type { SaasPlanResponse } from '../../lib/types';
import { DollarOutlined } from '@ant-design/icons';

export default function PlansPage() {
  const [plans, setPlans] = useState<SaasPlanResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);

  const fetchPlans = useCallback(async () => {
    setLoading(true);
    try {
      const res = await api.get<SaasPlanResponse[]>('/superadmin/saas-plans');
      setPlans(res.data);
    } catch {
      message.error('Error al cargar planes');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => { fetchPlans(); }, [fetchPlans]);

  const columns = [
    {
      title: 'Plan',
      key: 'name',
      render: (_: unknown, r: SaasPlanResponse) => (
        <div style={{ display: 'flex', alignItems: 'center', gap: 10 }}>
          <div style={{
            width: 36, height: 36, borderRadius: 8,
            background: 'var(--color-primary-light)',
            display: 'flex', alignItems: 'center', justifyContent: 'center',
            color: 'var(--color-primary)', fontSize: 16,
          }}>
            <SafetyOutlined />
          </div>
          <div>
            <div style={{ fontWeight: 600, fontSize: 13 }}>{r.name}</div>
            <div style={{ fontSize: 11, color: 'var(--color-text-light)' }}>{r.key}</div>
          </div>
        </div>
      ),
    },
    { title: 'Precio', dataIndex: 'price', key: 'price', render: (v: number) => `$${v}` },
    { title: 'Moneda', dataIndex: 'currency', key: 'currency' },
    { title: 'Ciclo', dataIndex: 'billingCycle', key: 'billingCycle' },
    { title: 'Max Usuarios', dataIndex: 'maxUsers', key: 'maxUsers' },
    { title: 'Max Sucursales', dataIndex: 'maxBranches', key: 'maxBranches' },
    { title: 'Max Docs/mes', dataIndex: 'maxDocumentsPerMonth', key: 'maxDocumentsPerMonth' },
    {
      title: 'Descripción',
      dataIndex: 'description',
      key: 'description',
      render: (v: string | null) => v || '-',
    },
  ];

  const totalRevenue = plans.reduce((acc, p) => acc + p.price, 0);
  const avgPrice = plans.length ? Math.round(totalRevenue / plans.length) : 0;

  return (
    <div className="plans-page">
      <div className="stats-grid">
        <StatCard
          title="Total Planes"
          value={plans.length}
          icon={<SafetyOutlined />}
          color="var(--color-primary)"
        />
        <StatCard
          title="Precio Promedio"
          value={`$${avgPrice}`}
          icon={<DollarOutlined />}
          color="#12B76A"
        />
        <StatCard
          title="Planes Activos"
          value={plans.length}
          icon={<CheckCircleOutlined />}
          color="#1570EF"
        />
      </div>

      <DataTable
        title="Planes de Suscripción"
        subtitle="Planes disponibles para tenants"
        columns={columns}
        dataSource={plans}
        loading={loading}
        rowKey="id"
        modalTitle="Detalle del Plan"
        modalOpen={modalOpen}
        modalWidth={500}
        onModalClose={() => setModalOpen(false)}
        modalFooter={null}
      />
    </div>
  );
}