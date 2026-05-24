import { useEffect, useState, useCallback } from 'react';
import { message, Popconfirm } from 'antd';
import { EditOutlined, DeleteOutlined, EyeOutlined } from '@ant-design/icons';
import api from '../../lib/api';
import CrudTable from '../../components/CrudTable';
import StatusTag from '../../components/StatusTag';
import TenantForm from './TenantForm';
import type { TenantResponse } from '../../lib/types';

const AVATAR_COLORS = ['#F97316', '#1570EF', '#7F56D9', '#12B76A', '#F04438', '#F59E0B'];

function getAvatarColor(name: string) {
  let hash = 0;
  for (let i = 0; i < name.length; i++) hash = name.charCodeAt(i) + ((hash << 5) - hash);
  return AVATAR_COLORS[Math.abs(hash) % AVATAR_COLORS.length];
}

export default function TenantsList() {
  const [tenants, setTenants] = useState<TenantResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<TenantResponse | null>(null);

  const fetch = useCallback(async () => {
    setLoading(true);
    try {
      const res = await api.get<TenantResponse[]>('/superadmin/tenants');
      setTenants(res.data);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => { fetch(); }, [fetch]);

  const handleDelete = async (id: string) => {
    await api.delete(`/superadmin/tenants/${id}`);
    message.success('Tenant desactivado');
    fetch();
  };

  const openCreate = () => { setEditing(null); setModalOpen(true); };
  const openEdit = (t: TenantResponse) => { setEditing(t); setModalOpen(true); };
  const closeModal = () => setModalOpen(false);

  const columns = [
    {
      title: 'RUC', dataIndex: 'ruc', key: 'ruc', width: 140,
      render: (ruc: string) => <span className="ruc-tag">{ruc}</span>,
      responsive: ['md' as const],
    },
    {
      title: 'Razón Social', key: 'name', width: 250,
      render: (_: unknown, record: TenantResponse) => (
        <div className="company-cell">
          <div className="company-avatar" style={{ background: getAvatarColor(record.razonSocial) }}>
            {record.razonSocial.charAt(0)}
          </div>
          <div>
            <div className="company-name">{record.razonSocial}</div>
            <div className="company-type">{record.nombreComercial ?? record.name}</div>
          </div>
        </div>
      ),
    },
    {
      title: 'Plan', dataIndex: 'subscriptionPlan', key: 'subscriptionPlan', width: 120,
      render: (plan: string | null) => plan ? <span className="plan-badge">{plan}</span> : '-',
      responsive: ['lg' as const],
    },
    {
      title: 'Estado', dataIndex: 'isActive', key: 'isActive', width: 110,
      render: (v: boolean) => <StatusTag active={v} />,
    },
    {
      title: 'Acciones', key: 'actions', width: 120,
      render: (_: unknown, record: TenantResponse) => (
        <div className="acts">
          <button className="action-btn" title="Ver"><EyeOutlined /></button>
          <button className="action-btn" title="Editar" onClick={() => openEdit(record)}><EditOutlined /></button>
          <Popconfirm title="¿Desactivar tenant?" onConfirm={() => handleDelete(record.id)}>
            <button className="action-btn danger" title="Desactivar"><DeleteOutlined /></button>
          </Popconfirm>
        </div>
      ),
    },
  ];

  const renderCard = (t: TenantResponse) => (
    <>
      <div style={{ display: 'flex', alignItems: 'center', gap: 10, marginBottom: 12 }}>
        <div className="company-avatar" style={{ background: getAvatarColor(t.razonSocial) }}>
          {t.razonSocial.charAt(0)}
        </div>
        <div>
          <div className="company-name">{t.razonSocial}</div>
          <div className="company-type">{t.nombreComercial ?? t.name}</div>
        </div>
      </div>
      <div className="crud-card-field">
        <span className="crud-card-label">RUC</span>
        <span className="crud-card-value"><span className="ruc-tag">{t.ruc}</span></span>
      </div>
      <div className="crud-card-field">
        <span className="crud-card-label">Plan</span>
        <span className="crud-card-value">{t.subscriptionPlan ? <span className="plan-badge">{t.subscriptionPlan}</span> : '-'}</span>
      </div>
      <div className="crud-card-field">
        <span className="crud-card-label">Estado</span>
        <span className="crud-card-value"><StatusTag active={t.isActive} /></span>
      </div>
      <div className="crud-card-actions">
        <button className="action-btn" title="Ver"><EyeOutlined /></button>
        <button className="action-btn" title="Editar" onClick={() => openEdit(t)}><EditOutlined /></button>
        <Popconfirm title="¿Desactivar tenant?" onConfirm={() => handleDelete(t.id)}>
          <button className="action-btn danger" title="Desactivar"><DeleteOutlined /></button>
        </Popconfirm>
      </div>
    </>
  );

  return (
    <CrudTable<TenantResponse>
      title="Tenants"
      subtitle="Empresas Inquilinas"
      actionLabel="Nuevo Tenant"
      onAction={openCreate}
      columns={columns}
      dataSource={tenants}
      loading={loading}
      rowKey="id"
      renderCard={renderCard}
      modalTitle={editing ? 'Editar Tenant' : 'Nuevo Tenant'}
      modalOpen={modalOpen}
      modalWidth={640}
      onModalClose={closeModal}
      modalFooter={null}
    >
      <TenantForm editing={editing} onDone={() => { closeModal(); fetch(); }} />
    </CrudTable>
  );
}
