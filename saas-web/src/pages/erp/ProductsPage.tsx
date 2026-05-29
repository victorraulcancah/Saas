import { useEffect, useState, useCallback } from 'react';
import { Form, Input, InputNumber, Select, message } from 'antd';
import { ShoppingOutlined } from '@ant-design/icons';
import api from '../../lib/api';
import DataTable from '../../components/DataTable';
import StatCard from '../../components/StatCard';
import type { ProductResponse } from '../../lib/types';
import { DollarOutlined, ShoppingCartOutlined, AppstoreOutlined } from '@ant-design/icons';

const fakeProducts: ProductResponse[] = [
  { id: '1', name: 'Laptop HP ProBook 450', sku: 'LP-HP-450', price: 3500, stock: 15, categoryId: '1', categoryName: 'Computadoras' },
  { id: '2', name: 'Monitor Dell 27"', sku: 'MN-DL-27', price: 1200, stock: 8, categoryId: '1', categoryName: 'Computadoras' },
  { id: '3', name: 'Teclado Mecánico RGB', sku: 'TK-RGB-01', price: 250, stock: 45, categoryId: '2', categoryName: 'Accesorios' },
  { id: '4', name: 'Mouse Inalámbrico', sku: 'MS-WL-02', price: 85, stock: 120, categoryId: '2', categoryName: 'Accesorios' },
  { id: '5', name: 'Impresora Epson L3250', sku: 'IM-EP-3250', price: 980, stock: 6, categoryId: '3', categoryName: 'Impresoras' },
];

export default function ProductsPage() {
  const [products, setProducts] = useState<ProductResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);
  const [form] = Form.useForm();

  const fetchProducts = useCallback(async () => {
    setLoading(true);
    try {
      const res = await api.get<ProductResponse[]>('/erp/products');
      setProducts(res.data);
    } catch {
      setProducts(fakeProducts);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => { fetchProducts(); }, [fetchProducts]);

  const handleCreate = async (values: unknown) => {
    try {
      await api.post('/erp/products', values);
      message.success('Producto creado');
      setModalOpen(false);
      form.resetFields();
      fetchProducts();
    } catch {
      message.error('Error al crear producto');
    }
  };

  const columns = [
    {
      title: 'Producto',
      key: 'name',
      render: (_: unknown, r: ProductResponse) => (
        <div style={{ display: 'flex', alignItems: 'center', gap: 10 }}>
          <div style={{
            width: 32, height: 32, borderRadius: 6,
            background: 'var(--color-primary-light)',
            display: 'flex', alignItems: 'center', justifyContent: 'center',
            color: 'var(--color-primary)', fontSize: 14,
          }}>
            <ShoppingOutlined />
          </div>
          <div>
            <div style={{ fontWeight: 600, fontSize: 13 }}>{r.name}</div>
            <div style={{ fontSize: 11, color: 'var(--color-text-light)' }}>SKU: {r.sku}</div>
          </div>
        </div>
      ),
    },
    { title: 'Categoría', dataIndex: 'categoryName', key: 'categoryName' },
    { title: 'Precio', dataIndex: 'price', key: 'price', render: (v: number) => `$${v.toFixed(2)}` },
    { title: 'Stock', dataIndex: 'stock', key: 'stock', render: (v: number) => v < 10 ? <span style={{ color: 'var(--color-warning-text)' }}>{v}</span> : v },
  ];

  const totalValue = products.reduce((acc, p) => acc + p.price * p.stock, 0);

  return (
    <div className="products-page">
      <div className="stats-grid">
        <StatCard
          title="Total Productos"
          value={products.length}
          icon={<AppstoreOutlined />}
          color="var(--color-primary)"
        />
        <StatCard
          title="Valor Inventario"
          value={`$${totalValue.toLocaleString()}`}
          icon={<DollarOutlined />}
          color="#12B76A"
        />
        <StatCard
          title="Stock Total"
          value={products.reduce((acc, p) => acc + p.stock, 0)}
          icon={<ShoppingCartOutlined />}
          color="#1570EF"
        />
      </div>

      <DataTable
        title="Productos"
        subtitle="Catálogo de productos"
        actionLabel="Nuevo Producto"
        onAction={() => { form.resetFields(); setModalOpen(true); }}
        columns={columns}
        dataSource={products}
        loading={loading}
        rowKey="id"
        modalTitle="Nuevo Producto"
        modalOpen={modalOpen}
        modalWidth={520}
        onModalClose={() => setModalOpen(false)}
        modalFooter={null}
      >
        <Form form={form} layout="vertical" onFinish={handleCreate}>
          <Form.Item name="name" label="Nombre" rules={[{ required: true }]}>
            <Input autoFocus />
          </Form.Item>
          <Form.Item name="sku" label="SKU" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="categoryId" label="Categoría" rules={[{ required: true }]}>
            <Select placeholder="Seleccionar">
              <Select.Option value="1">Computadoras</Select.Option>
              <Select.Option value="2">Accesorios</Select.Option>
              <Select.Option value="3">Impresoras</Select.Option>
            </Select>
          </Form.Item>
          <Form.Item name="price" label="Precio" rules={[{ required: true }]}>
            <InputNumber style={{ width: '100%' }} min={0} prefix="$" />
          </Form.Item>
          <Form.Item name="stock" label="Stock inicial">
            <InputNumber style={{ width: '100%' }} min={0} />
          </Form.Item>
          <button type="submit" className="page-btn-primary" style={{ width: '100%' }}>Crear</button>
        </Form>
      </DataTable>
    </div>
  );
}