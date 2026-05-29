import { useEffect, useState, useCallback } from 'react';
import { Form, Input, message } from 'antd';
import { FolderOutlined } from '@ant-design/icons';
import api from '../../lib/api';
import DataTable from '../../components/DataTable';
import type { CategoryResponse } from '../../lib/types';

export default function CategoriesPage() {
  const [categories, setCategories] = useState<CategoryResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);
  const [form] = Form.useForm();

  const fetchCategories = useCallback(async () => {
    setLoading(true);
    try {
      const res = await api.get<CategoryResponse[]>('/erp/categories');
      setCategories(res.data);
    } catch {
      message.error('Error al cargar categorías');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => { fetchCategories(); }, [fetchCategories]);

  const handleCreate = async (values: { name: string; description?: string }) => {
    try {
      await api.post('/erp/categories', values);
      message.success('Categoría creada');
      setModalOpen(false);
      form.resetFields();
      fetchCategories();
    } catch {
      message.error('Error al crear categoría');
    }
  };

  const columns = [
    {
      title: 'Nombre',
      key: 'name',
      render: (_: unknown, r: CategoryResponse) => (
        <div style={{ display: 'flex', alignItems: 'center', gap: 10 }}>
          <div style={{
            width: 32, height: 32, borderRadius: 6,
            background: 'var(--color-primary-light)',
            display: 'flex', alignItems: 'center', justifyContent: 'center',
            color: 'var(--color-primary)', fontSize: 14,
          }}>
            <FolderOutlined />
          </div>
          <span style={{ fontWeight: 600 }}>{r.name}</span>
        </div>
      ),
    },
    { title: 'Descripción', dataIndex: 'description', key: 'description', render: (v: string | null) => v || '-' },
    {
      title: 'Estado',
      dataIndex: 'isActive',
      key: 'isActive',
      render: (v: boolean) => v ? <span className="badge badge-success">Activo</span> : <span className="badge badge-warning">Inactivo</span>,
    },
  ];

  return (
    <div className="categories-page">
      <DataTable
        title="Categorías"
        subtitle="Categorías de productos"
        actionLabel="Nueva Categoría"
        onAction={() => { form.resetFields(); setModalOpen(true); }}
        columns={columns}
        dataSource={categories}
        loading={loading}
        rowKey="id"
        modalTitle="Nueva Categoría"
        modalOpen={modalOpen}
        modalWidth={480}
        onModalClose={() => setModalOpen(false)}
        modalFooter={null}
      >
        <Form form={form} layout="vertical" onFinish={handleCreate}>
          <Form.Item name="name" label="Nombre" rules={[{ required: true }]}>
            <Input autoFocus />
          </Form.Item>
          <Form.Item name="description" label="Descripción">
            <Input.TextArea rows={3} />
          </Form.Item>
          <button type="submit" className="page-btn-primary" style={{ width: '100%' }}>Crear</button>
        </Form>
      </DataTable>
    </div>
  );
}