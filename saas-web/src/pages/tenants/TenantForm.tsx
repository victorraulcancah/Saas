import { useState, useEffect } from 'react';
import { Form, Input, Button, message, Row, Col, Divider } from 'antd';
import api from '../../lib/api';
import type { TenantResponse } from '../../lib/types';

interface Props {
  editing: TenantResponse | null;
  onDone: () => void;
}

export default function TenantForm({ editing, onDone }: Props) {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (editing) form.setFieldsValue(editing);
    else form.resetFields();
  }, [editing, form]);

  const onFinish = async (values: Record<string, unknown>) => {
    setLoading(true);
    try {
      if (editing) {
        await api.put(`/superadmin/tenants/${editing.id}`, { ...values, isActive: editing.isActive });
        message.success('Tenant actualizado');
      } else {
        await api.post('/superadmin/tenants', values);
        message.success('Tenant creado');
      }
      onDone();
    } catch {
      message.error('Error al guardar el tenant');
    } finally {
      setLoading(false);
    }
  };

  const requiredRule = (label: string) => ({ required: true, message: `Ingrese ${label}` });

  return (
    <Form form={form} layout="vertical" onFinish={onFinish} scrollToFirstError>
      <Divider plain>Identificación</Divider>
      <Row gutter={16}>
        <Col span={12}>
          <Form.Item name="name" label="Nombre" rules={[requiredRule('el nombre')]}>
            <Input placeholder="Ej: Ferretería ABC" />
          </Form.Item>
        </Col>
        <Col span={12}>
          <Form.Item name="slug" label="Slug" rules={[requiredRule('el slug')]}>
            <Input placeholder="Ej: ferreteria-abc" />
          </Form.Item>
        </Col>
      </Row>

      <Divider plain>Datos SUNAT</Divider>
      <Row gutter={16}>
        <Col span={12}>
          <Form.Item name="ruc" label="RUC" rules={[{ required: true, len: 11, message: 'RUC debe tener 11 dígitos' }]}>
            <Input maxLength={11} placeholder="12345678901" />
          </Form.Item>
        </Col>
        <Col span={12}>
          <Form.Item name="razonSocial" label="Razón Social" rules={[requiredRule('la razón social')]}>
            <Input placeholder="Razón social registrada en SUNAT" />
          </Form.Item>
        </Col>
      </Row>
      <Form.Item name="nombreComercial" label="Nombre Comercial">
        <Input placeholder="Opcional" />
      </Form.Item>

      <Divider plain>Contacto</Divider>
      <Row gutter={16}>
        <Col span={12}>
          <Form.Item name="email" label="Email">
            <Input type="email" placeholder="correo@empresa.com" />
          </Form.Item>
        </Col>
        <Col span={12}>
          <Form.Item name="emailFacturacion" label="Email Facturación">
            <Input type="email" placeholder="facturacion@empresa.com" />
          </Form.Item>
        </Col>
      </Row>
      <Row gutter={16}>
        <Col span={12}>
          <Form.Item name="phone" label="Teléfono">
            <Input placeholder="+51 999 999 999" />
          </Form.Item>
        </Col>
        <Col span={12}>
          <Form.Item name="website" label="Web">
            <Input placeholder="https://empresa.com" />
          </Form.Item>
        </Col>
      </Row>

      <Divider plain>Dirección</Divider>
      <Form.Item name="address" label="Dirección">
        <Input placeholder="Dirección general" />
      </Form.Item>
      <Row gutter={16}>
        <Col span={8}>
          <Form.Item name="departamento" label="Departamento"><Input /></Form.Item>
        </Col>
        <Col span={8}>
          <Form.Item name="provincia" label="Provincia"><Input /></Form.Item>
        </Col>
        <Col span={8}>
          <Form.Item name="distrito" label="Distrito"><Input /></Form.Item>
        </Col>
      </Row>

      <Divider plain>Suscripción</Divider>
      <Form.Item name="subscriptionPlan" label="Plan">
        <Input placeholder="Ej: Basic, Pro, Enterprise" />
      </Form.Item>

      <Form.Item>
        <Button type="primary" htmlType="submit" loading={loading} block size="large">
          {editing ? 'Actualizar Tenant' : 'Crear Tenant'}
        </Button>
      </Form.Item>
    </Form>
  );
}
