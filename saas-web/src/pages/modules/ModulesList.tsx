import { useEffect, useState, useCallback } from 'react';
import { Form, Input, Button, message } from 'antd';
import api from '../../lib/api';
import CrudTable from '../../components/CrudTable';
import type { ModuleResponse } from '../../lib/types';

export default function ModulesList() {
  const [modules, setModules] = useState<ModuleResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);
  const [saving, setSaving] = useState(false);
  const [form] = Form.useForm();

  const fetch = useCallback(async () => {
    setLoading(true);
    try {
      const res = await api.get<ModuleResponse[]>('/superadmin/modules');
      setModules(res.data);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => { fetch(); }, [fetch]);

  const handleCreate = async (values: { name: string; key: string; description?: string }) => {
    setSaving(true);
    try {
      await api.post('/superadmin/modules', values);
      message.success('Módulo creado');
      setModalOpen(false);
      form.resetFields();
      fetch();
    } finally {
      setSaving(false);
    }
  };

  const columns = [
    { title: 'Nombre', dataIndex: 'name', key: 'name' },
    { title: 'Key', dataIndex: 'key', key: 'key', responsive: ['md' as const] },
    { title: 'Descripción', dataIndex: 'description', key: 'description', responsive: ['lg' as const] },
  ];

  return (
    <CrudTable<ModuleResponse>
      title="Módulos"
      subtitle="Catálogo de módulos del sistema"
      actionLabel="Nuevo Módulo"
      onAction={() => setModalOpen(true)}
      columns={columns}
      dataSource={modules}
      loading={loading}
      rowKey="id"
      modalTitle="Nuevo Módulo"
      modalOpen={modalOpen}
      modalWidth={480}
      onModalClose={() => setModalOpen(false)}
      modalFooter={null}
    >
      <Form form={form} layout="vertical" onFinish={handleCreate}>
        <Form.Item name="name" label="Nombre" rules={[{ required: true, message: 'Ingrese el nombre' }]}>
          <Input autoFocus />
        </Form.Item>
        <Form.Item name="key" label="Key" rules={[{ required: true, message: 'Ingrese la key' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="description" label="Descripción">
          <Input.TextArea rows={3} />
        </Form.Item>
        <Form.Item>
          <Button type="primary" htmlType="submit" loading={saving} block>Crear</Button>
        </Form.Item>
      </Form>
    </CrudTable>
  );
}
