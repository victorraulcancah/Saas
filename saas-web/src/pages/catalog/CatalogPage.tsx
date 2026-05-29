import { useEffect, useState, useCallback } from 'react';
import { Form, Input, Select, message, Tabs } from 'antd';
import { AppstoreOutlined, FolderOutlined, FileOutlined } from '@ant-design/icons';
import api from '../../lib/api';
import DataTable from '../../components/DataTable';
import type { SaasSystemResponse, SaasModuleResponse, SaasSubModuleResponse } from '../../lib/types';

export default function CatalogPage() {
  const [systems, setSystems] = useState<SaasSystemResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);
  const [form] = Form.useForm();
  const [activeTab, setActiveTab] = useState('systems');

  const fetchCatalog = useCallback(async () => {
    setLoading(true);
    try {
      const res = await api.get<SaasSystemResponse[]>('/superadmin/saas-catalog');
      setSystems(res.data);
    } catch {
      message.error('Error al cargar el catálogo');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => { fetchCatalog(); }, [fetchCatalog]);

  const handleCreateSystem = async (values: { name: string; key: string; description?: string; icon?: string }) => {
    try {
      await api.post('/superadmin/saas-catalog/systems', values);
      message.success('Sistema creado');
      setModalOpen(false);
      form.resetFields();
      fetchCatalog();
    } catch {
      message.error('Error al crear sistema');
    }
  };

  const handleCreateModule = async (values: { systemId: string; name: string; key: string; description?: string }) => {
    try {
      await api.post('/superadmin/saas-catalog/modules', values);
      message.success('Módulo creado');
      setModalOpen(false);
      form.resetFields();
      fetchCatalog();
    } catch {
      message.error('Error al crear módulo');
    }
  };

  const handleCreateSubModule = async (values: { moduleId: string; name: string; key: string; description?: string; routePath?: string }) => {
    try {
      await api.post('/superadmin/saas-catalog/submodules', values);
      message.success('Submódulo creado');
      setModalOpen(false);
      form.resetFields();
      fetchCatalog();
    } catch {
      message.error('Error al crear submódulo');
    }
  };

  const systemColumns = [
    { title: 'Nombre', dataIndex: 'name', key: 'name' },
    { title: 'Key', dataIndex: 'key', key: 'key' },
    { title: 'Descripción', dataIndex: 'description', key: 'description', render: (v: string | null) => v || '-' },
    { title: 'Icono', dataIndex: 'icon', key: 'icon', render: (v: string | null) => v || '-' },
    { title: 'Estado', dataIndex: 'isActive', key: 'isActive', render: (v: boolean) => v ? 'Activo' : 'Inactivo' },
  ];

  const moduleColumns = [
    { title: 'Nombre', dataIndex: 'name', key: 'name' },
    { title: 'Key', dataIndex: 'key', key: 'key' },
    { title: 'Descripción', dataIndex: 'description', key: 'description', render: (v: string | null) => v || '-' },
    { title: 'Base Path', dataIndex: 'basePath', key: 'basePath', render: (v: string | null) => v || '-' },
  ];

  const subModuleColumns = [
    { title: 'Nombre', dataIndex: 'name', key: 'name' },
    { title: 'Key', dataIndex: 'key', key: 'key' },
    { title: 'Route Path', dataIndex: 'routePath', key: 'routePath', render: (v: string | null) => v || '-' },
  ];

  const allModules: SaasModuleResponse[] = systems.flatMap(s => s.modules);
  const allSubModules: SaasSubModuleResponse[] = allModules.flatMap(m => m.subModules);

  const openCreate = () => {
    form.resetFields();
    setModalOpen(true);
  };

  const renderForm = () => {
    if (activeTab === 'systems') {
      return (
        <Form form={form} layout="vertical" onFinish={handleCreateSystem}>
          <Form.Item name="name" label="Nombre" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="key" label="Key" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="description" label="Descripción">
            <Input.TextArea />
          </Form.Item>
          <Form.Item name="icon" label="Icono (clase CSS)">
            <Input placeholder="Ej: layout-dashboard" />
          </Form.Item>
          <button type="submit" className="page-btn-primary" style={{ width: '100%' }}>Crear Sistema</button>
        </Form>
      );
    }

    if (activeTab === 'modules') {
      return (
        <Form form={form} layout="vertical" onFinish={handleCreateModule}>
          <Form.Item name="systemId" label="Sistema" rules={[{ required: true }]}>
            <Select placeholder="Seleccionar sistema">
              {systems.map(s => <Select.Option key={s.id} value={s.id}>{s.name}</Select.Option>)}
            </Select>
          </Form.Item>
          <Form.Item name="name" label="Nombre" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="key" label="Key" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="description" label="Descripción">
            <Input.TextArea />
          </Form.Item>
          <button type="submit" className="page-btn-primary" style={{ width: '100%' }}>Crear Módulo</button>
        </Form>
      );
    }

    return (
      <Form form={form} layout="vertical" onFinish={handleCreateSubModule}>
        <Form.Item name="moduleId" label="Módulo" rules={[{ required: true }]}>
          <Select placeholder="Seleccionar módulo">
            {allModules.map(m => <Select.Option key={m.id} value={m.id}>{m.name}</Select.Option>)}
          </Select>
        </Form.Item>
        <Form.Item name="name" label="Nombre" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item name="key" label="Key" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item name="routePath" label="Route Path">
          <Input placeholder="Ej: /erp/facturacion/comprobantes" />
        </Form.Item>
        <button type="submit" className="page-btn-primary" style={{ width: '100%' }}>Crear Submódulo</button>
      </Form>
    );
  };

  const items = [
    {
      key: 'systems',
      label: <span><AppstoreOutlined /> Sistemas</span>,
      children: (
        <DataTable
          title="Sistemas"
          subtitle="Sistemas vendibles"
          actionLabel="Nuevo Sistema"
          onAction={() => { setActiveTab('systems'); openCreate(); }}
          columns={systemColumns}
          dataSource={systems}
          loading={loading}
          rowKey="id"
          modalTitle="Nuevo Sistema"
          modalOpen={modalOpen && activeTab === 'systems'}
          modalWidth={480}
          onModalClose={() => setModalOpen(false)}
          modalFooter={null}
        >
          {renderForm()}
        </DataTable>
      ),
    },
    {
      key: 'modules',
      label: <span><FolderOutlined /> Módulos</span>,
      children: (
        <DataTable
          title="Módulos"
          subtitle="Módulos vendibles"
          actionLabel="Nuevo Módulo"
          onAction={() => { setActiveTab('modules'); openCreate(); }}
          columns={moduleColumns}
          dataSource={allModules}
          loading={loading}
          rowKey="id"
          modalTitle="Nuevo Módulo"
          modalOpen={modalOpen && activeTab === 'modules'}
          modalWidth={480}
          onModalClose={() => setModalOpen(false)}
          modalFooter={null}
        >
          {renderForm()}
        </DataTable>
      ),
    },
    {
      key: 'submodules',
      label: <span><FileOutlined /> Submódulos</span>,
      children: (
        <DataTable
          title="Submódulos"
          subtitle="Submódulos vendibles"
          actionLabel="Nuevo Submódulo"
          onAction={() => { setActiveTab('submodules'); openCreate(); }}
          columns={subModuleColumns}
          dataSource={allSubModules}
          loading={loading}
          rowKey="id"
          modalTitle="Nuevo Submódulo"
          modalOpen={modalOpen && activeTab === 'submodules'}
          modalWidth={480}
          onModalClose={() => setModalOpen(false)}
          modalFooter={null}
        >
          {renderForm()}
        </DataTable>
      ),
    },
  ];

  return (
    <div className="catalog-page">
      <div className="page-header">
        <div className="page-header-start">
          <h1 className="page-title">Catálogo SaaS</h1>
          <span className="page-subtitle">Sistemas, módulos y submódulos</span>
        </div>
      </div>
      <Tabs activeKey={activeTab} onChange={(k) => setActiveTab(k)} items={items} style={{ marginTop: 0 }} />
    </div>
  );
}