import { useState } from 'react';
import { Outlet, useNavigate, useLocation } from 'react-router-dom';
import { Layout, Menu, Button, Grid } from 'antd';
import {
  TeamOutlined, AppstoreOutlined, UserOutlined,
  MenuFoldOutlined, MenuUnfoldOutlined, LogoutOutlined,
  DashboardOutlined, SettingOutlined, BellOutlined,
} from '@ant-design/icons';
import { useAuth } from '../lib/auth-context';

const { Header, Sider, Content } = Layout;
const { useBreakpoint } = Grid;

const menuItems = [
  { key: '/', icon: <DashboardOutlined />, label: 'Dashboard' },
  { type: 'divider' as const },
  { key: 'gestion', label: 'Gestión', type: 'group' as const, children: [
    { key: '/tenants', icon: <TeamOutlined />, label: 'Tenants' },
    { key: '/modules', icon: <AppstoreOutlined />, label: 'Módulos' },
    { key: '/users', icon: <UserOutlined />, label: 'Usuarios' },
  ]},
  { type: 'divider' as const },
  { key: 'sistema', label: 'Sistema', type: 'group' as const, children: [
    { key: '/config', icon: <SettingOutlined />, label: 'Configuración' },
  ]},
];

export default function Dashboard() {
  const [collapsed, setCollapsed] = useState(false);
  const { logout, email } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const screens = useBreakpoint();
  const isMobile = !screens.md;

  const handleMenuClick = (info: { key: string }) => {
    navigate(info.key);
  };

  const userInitials = email ? email.substring(0, 2).toUpperCase() : 'SA';

  return (
    <Layout style={{ minHeight: '100vh', background: 'var(--color-bg-body)' }}>
      <Sider
        trigger={null}
        collapsible
        collapsed={collapsed || isMobile}
        width={220}
        collapsedWidth={isMobile ? 0 : 72}
        style={{
          background: 'var(--color-bg-card)',
          borderRight: '1px solid var(--color-border)',
          overflow: 'auto',
          position: isMobile ? 'fixed' : 'relative',
          zIndex: 100,
          height: '100vh',
        }}
      >
        <div style={{
          height: 56,
          display: 'flex',
          alignItems: 'center',
          gap: 10,
          padding: collapsed || isMobile ? '0 16px' : '0 18px',
          borderBottom: '1px solid var(--color-border)',
        }}>
          <div style={{
            width: 32, height: 32, borderRadius: 8,
            background: 'var(--color-primary)',
            display: 'flex', alignItems: 'center', justifyContent: 'center',
            flexShrink: 0,
          }}>
            <span style={{ color: '#fff', fontSize: 16, fontWeight: 700 }}>S</span>
          </div>
          {!collapsed && !isMobile && (
            <div style={{ lineHeight: 1.2 }}>
              <div style={{ fontSize: 14, fontWeight: 700, color: 'var(--color-text-title)' }}>SaaS Admin</div>
              <div style={{ fontSize: 10, color: 'var(--color-text-light)' }}>Panel de control</div>
            </div>
          )}
        </div>

        <Menu
          theme="light"
          mode="inline"
          selectedKeys={[location.pathname]}
          defaultOpenKeys={['gestion', 'sistema']}
          items={menuItems}
          onClick={handleMenuClick}
          style={{ border: 'none', background: 'transparent', padding: '8px 8px', fontSize: 13 }}
        />

        <div style={{
          padding: '12px 14px',
          borderTop: '1px solid var(--color-border)',
          display: 'flex',
          alignItems: 'center',
          gap: 9,
          position: 'absolute',
          bottom: 0,
          width: '100%',
          background: 'var(--color-bg-card)',
        }}>
          <div style={{
            width: 30, height: 30, borderRadius: '50%',
            background: 'linear-gradient(135deg, var(--color-primary), var(--color-primary-dark))',
            display: 'flex', alignItems: 'center', justifyContent: 'center',
            fontSize: 11, fontWeight: 700, color: '#fff', flexShrink: 0,
          }}>
            {userInitials}
          </div>
          {!collapsed && !isMobile && (
            <>
              <div style={{ flex: 1, minWidth: 0 }}>
                <div style={{ fontSize: 12, fontWeight: 600, color: 'var(--color-text-title)' }}>{email}</div>
                <div style={{ fontSize: 10, color: 'var(--color-text-light)' }}>Super Admin</div>
              </div>
              <LogoutOutlined style={{ fontSize: 14, color: 'var(--color-text-light)', cursor: 'pointer' }} onClick={logout} />
            </>
          )}
        </div>
      </Sider>

      <Layout style={{ marginLeft: isMobile ? 0 : undefined }}>
        <Header style={{
          padding: '0 22px',
          height: 54,
          background: 'var(--color-bg-card)',
          borderBottom: '1px solid var(--color-border)',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'space-between',
        }}>
          <div style={{ display: 'flex', alignItems: 'center', gap: 12 }}>
            <Button
              type="text"
              icon={collapsed ? <MenuUnfoldOutlined /> : <MenuFoldOutlined />}
              onClick={() => setCollapsed(!collapsed)}
              style={{ color: 'var(--color-text-secondary)', fontSize: 16 }}
            />
            <div>
              <div style={{ fontSize: 15, fontWeight: 700, color: 'var(--color-text-title)' }}>Dashboard</div>
              <div style={{ fontSize: 11, color: 'var(--color-text-light)' }}>Panel de administración</div>
            </div>
          </div>

          <div style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
            <div className="hide-mobile" style={{
              width: 34, height: 34, borderRadius: 8,
              border: '1px solid var(--color-border)',
              display: 'flex', alignItems: 'center', justifyContent: 'center',
              cursor: 'pointer', color: 'var(--color-text-secondary)', fontSize: 15,
              position: 'relative', background: 'var(--color-bg-card)',
            }}>
              <BellOutlined />
              <div style={{
                position: 'absolute', top: 6, right: 6,
                width: 7, height: 7, borderRadius: '50%',
                background: 'var(--color-primary)',
                border: '1.5px solid var(--color-bg-card)',
              }} />
            </div>
            <div style={{
              width: 32, height: 32, borderRadius: '50%',
              background: 'linear-gradient(135deg, var(--color-primary), var(--color-primary-dark))',
              display: 'flex', alignItems: 'center', justifyContent: 'center',
              fontSize: 11, fontWeight: 700, color: '#fff', cursor: 'pointer',
            }}>
              {userInitials}
            </div>
          </div>
        </Header>

        <Content style={{ padding: '18px 22px' }}>
          <Outlet />
        </Content>
      </Layout>
    </Layout>
  );
}
