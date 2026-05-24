import { useState } from 'react';
import { useAuth } from '../lib/auth-context';
import { useNavigate } from 'react-router-dom';
import { Form, Input, Button, message } from 'antd';
import { LockOutlined, MailOutlined } from '@ant-design/icons';

export default function Login() {
  const { login } = useAuth();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);

  const onFinish = async (values: { email: string; password: string }) => {
    setLoading(true);
    try {
      await login(values);
      message.success('Inicio de sesión exitoso');
      navigate('/');
    } catch {
      message.error('Credenciales inválidas');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{
      minHeight: '100vh',
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center',
      background: 'var(--color-bg-body)',
      padding: 20,
    }}>
      <div style={{
        background: 'var(--color-bg-card)',
        borderRadius: 'var(--radius-xl)',
        border: '1px solid var(--color-border)',
        padding: 40,
        width: '100%',
        maxWidth: 400,
        boxShadow: 'var(--shadow-card)',
      }}>
        <div style={{ textAlign: 'center', marginBottom: 32 }}>
          <div style={{
            width: 48, height: 48, borderRadius: 12,
            background: 'var(--color-primary)',
            display: 'inline-flex', alignItems: 'center', justifyContent: 'center',
            marginBottom: 16,
          }}>
            <span style={{ color: '#fff', fontSize: 24, fontWeight: 800 }}>S</span>
          </div>
          <h1 style={{ fontSize: 20, fontWeight: 700, color: 'var(--color-text-title)', margin: 0 }}>
            SaaS Admin
          </h1>
          <p style={{ fontSize: 13, color: 'var(--color-text-light)', marginTop: 4 }}>
            Ingresa tus credenciales
          </p>
        </div>

        <Form onFinish={onFinish} layout="vertical" size="large">
          <Form.Item
            name="email"
            rules={[{ required: true, message: 'Ingrese su email' }]}
            style={{ marginBottom: 20 }}
          >
            <Input
              prefix={<MailOutlined style={{ color: 'var(--color-text-light)' }} />}
              placeholder="Email"
              style={{ borderRadius: 8, height: 44 }}
            />
          </Form.Item>
          <Form.Item
            name="password"
            rules={[{ required: true, message: 'Ingrese su contraseña' }]}
            style={{ marginBottom: 24 }}
          >
            <Input.Password
              prefix={<LockOutlined style={{ color: 'var(--color-text-light)' }} />}
              placeholder="Contraseña"
              style={{ borderRadius: 8, height: 44 }}
            />
          </Form.Item>
          <Form.Item style={{ marginBottom: 0 }}>
            <Button
              type="primary"
              htmlType="submit"
              loading={loading}
              block
              size="large"
              style={{ height: 44, borderRadius: 8, fontWeight: 600, fontSize: 14 }}
            >
              Iniciar Sesión
            </Button>
          </Form.Item>
        </Form>
      </div>
    </div>
  );
}
