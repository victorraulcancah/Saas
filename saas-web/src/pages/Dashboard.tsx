import { useState, useRef, useCallback, useEffect } from 'react';
import { Outlet, useNavigate, useLocation } from 'react-router-dom';
import { Menu, Grid, Drawer } from 'antd';
import {
  TeamOutlined, AppstoreOutlined, UserOutlined,
  MenuOutlined, LogoutOutlined,
  DashboardOutlined, SettingOutlined, BellOutlined,
} from '@ant-design/icons';
import { useAuth } from '../lib/auth-context';

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

const CLAMP = (v: number, min: number, max: number) => Math.min(max, Math.max(min, v));

export default function Dashboard() {
  const [collapsed, setCollapsed] = useState(false);
  const [pos, setPos] = useState({ x: 12, y: 12 });
  const { logout, email } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const screens = useBreakpoint();
  const isMobile = !screens.md;

  const dragging = useRef(false);
  const dragHead = useRef({ x: 0, y: 0, px: 0, py: 0 });
  const panelRef = useRef<HTMLDivElement>(null);

  const userInitials = email ? email.substring(0, 2).toUpperCase() : 'SA';

  const handleDragStart = useCallback((clientX: number, clientY: number) => {
    dragging.current = true;
    dragHead.current = { x: clientX, y: clientY, px: pos.x, py: pos.y };
  }, [pos]);

  const handleDragMove = useCallback((clientX: number, clientY: number) => {
    if (!dragging.current) return;
    const dx = clientX - dragHead.current.x;
    const dy = clientY - dragHead.current.y;
    const w = window.innerWidth;
    const h = window.innerHeight;
    const pw = collapsed ? 44 : 230;
    setPos({
      x: CLAMP(dragHead.current.px + dx, 0, w - pw - 8),
      y: CLAMP(dragHead.current.py + dy, 0, h - 80),
    });
  }, [collapsed]);

  const handleDragEnd = useCallback(() => {
    dragging.current = false;
  }, []);

  useEffect(() => {
    if (!isMobile) {
      const onMove = (e: MouseEvent | TouchEvent) => {
        const cx = 'touches' in e ? e.touches[0].clientX : (e as MouseEvent).clientX;
        const cy = 'touches' in e ? e.touches[0].clientY : (e as MouseEvent).clientY;
        handleDragMove(cx, cy);
      };
      const onEnd = () => handleDragEnd();
      window.addEventListener('mousemove', onMove);
      window.addEventListener('mouseup', onEnd);
      window.addEventListener('touchmove', onMove, { passive: true });
      window.addEventListener('touchend', onEnd);
      return () => {
        window.removeEventListener('mousemove', onMove);
        window.removeEventListener('mouseup', onEnd);
        window.removeEventListener('touchmove', onMove);
        window.removeEventListener('touchend', onEnd);
      };
    }
  }, [isMobile, handleDragMove, handleDragEnd]);

  const sidebarContent = (
    <div style={{
      height: '100%',
      display: 'flex',
      flexDirection: 'column',
      background: 'var(--color-bg-card)',
    }}>
      <div
        className="sidebar-drag-handle"
        style={{
          height: 56,
          display: 'flex',
          alignItems: 'center',
          gap: 10,
          padding: '0 18px',
          borderBottom: '1px solid var(--color-border)',
          cursor: 'grab',
          userSelect: 'none',
        }}
        onMouseDown={(e) => { if (!isMobile) handleDragStart(e.clientX, e.clientY); }}
        onTouchStart={(e) => { if (!isMobile) handleDragStart(e.touches[0].clientX, e.touches[0].clientY); }}
      >
        <div style={{
          width: 32, height: 32, borderRadius: 8,
          background: 'var(--color-primary)',
          display: 'flex', alignItems: 'center', justifyContent: 'center',
          flexShrink: 0,
        }}>
          <span style={{ color: '#fff', fontSize: 16, fontWeight: 700 }}>S</span>
        </div>
        <div style={{ lineHeight: 1.2, flex: 1 }}>
          <div style={{ fontSize: 14, fontWeight: 700, color: 'var(--color-text-title)' }}>SaaS Admin</div>
          <div style={{ fontSize: 10, color: 'var(--color-text-light)' }}>Panel de control</div>
        </div>
        <div
          className="sidebar-collapse-btn"
          onClick={() => setCollapsed(true)}
          style={{
            width: 22, height: 22, borderRadius: '50%',
            background: 'var(--color-border-light)',
            display: 'flex', alignItems: 'center', justifyContent: 'center',
            cursor: 'pointer', fontSize: 14, fontWeight: 600,
            color: 'var(--color-text-secondary)', lineHeight: 1,
            flexShrink: 0,
          }}
        >
          −
        </div>
      </div>

      <Menu
        theme="light"
        mode="inline"
        selectedKeys={[location.pathname]}
        defaultOpenKeys={['gestion', 'sistema']}
        items={menuItems}
        onClick={(info) => { navigate(info.key); if (isMobile) setCollapsed(false); }}
        style={{
          border: 'none', background: 'transparent',
          padding: '8px 8px', fontSize: 13, flex: 1,
        }}
      />

      <div style={{
        padding: '12px 14px',
        borderTop: '1px solid var(--color-border)',
        display: 'flex', alignItems: 'center', gap: 9,
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
        <div style={{ flex: 1, minWidth: 0 }}>
          <div style={{ fontSize: 12, fontWeight: 600, color: 'var(--color-text-title)' }}>{email}</div>
          <div style={{ fontSize: 10, color: 'var(--color-text-light)' }}>Super Admin</div>
        </div>
        <LogoutOutlined style={{ fontSize: 14, color: 'var(--color-text-light)', cursor: 'pointer' }} onClick={logout} />
      </div>
    </div>
  );

  return (
    <div style={{ minHeight: '100vh', background: 'var(--color-bg-body)' }}>
      {/* Desktop draggable sidebar */}
      {!isMobile && collapsed && (
        <div
          className="sidebar-float-icon"
          onClick={() => setCollapsed(false)}
          onMouseDown={(e) => { e.stopPropagation(); handleDragStart(e.clientX, e.clientY); }}
          onTouchStart={(e) => { e.stopPropagation(); handleDragStart(e.touches[0].clientX, e.touches[0].clientY); }}
          style={{
            position: 'fixed',
            left: pos.x,
            top: pos.y,
            width: 44,
            height: 44,
            borderRadius: 12,
            background: 'var(--color-primary)',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            cursor: 'grab',
            zIndex: 100,
            boxShadow: '0 4px 16px rgba(249, 115, 22, 0.35)',
            transition: 'box-shadow 0.2s',
            userSelect: 'none',
            color: '#fff',
            fontSize: 20,
          }}
        >
          <MenuOutlined />
        </div>
      )}

      {!isMobile && !collapsed && (
        <aside
          ref={panelRef}
          className="sidebar-floating"
          style={{
            position: 'fixed',
            left: pos.x,
            top: pos.y,
            width: 230,
            background: 'var(--color-bg-card)',
            borderRadius: 14,
            border: '1px solid var(--color-border)',
            boxShadow: '0 4px 24px rgba(16, 24, 40, 0.1)',
            zIndex: 100,
            overflow: 'hidden',
            transition: dragging.current ? 'none' : 'box-shadow 0.2s',
          }}
        >
          {sidebarContent}
        </aside>
      )}

      {/* Mobile drawer */}
      {isMobile && (
        <Drawer
          placement="left"
          open={!collapsed}
          onClose={() => setCollapsed(true)}
          width={260}
          styles={{ body: { padding: 0 } }}
          closeIcon={null}
        >
          {sidebarContent}
        </Drawer>
      )}

      <div style={{
        minHeight: '100vh',
        display: 'flex',
        flexDirection: 'column',
      }}>
        <header style={{
          padding: '0 22px',
          height: 54,
          background: 'var(--color-bg-card)',
          borderBottom: '1px solid var(--color-border)',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'space-between',
          position: 'sticky',
          top: 0,
          zIndex: 50,
        }}>
          <div style={{ display: 'flex', alignItems: 'center', gap: 12 }}>
            {isMobile && (
              <div
                onClick={() => setCollapsed(false)}
                style={{
                  width: 32, height: 32, borderRadius: 8,
                  display: 'flex', alignItems: 'center', justifyContent: 'center',
                  cursor: 'pointer', color: 'var(--color-text-secondary)', fontSize: 16,
                }}
              >
                <MenuOutlined />
              </div>
            )}
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
        </header>

        <main style={{ padding: '18px 22px', flex: 1, maxWidth: '100%' }}>
          <Outlet />
        </main>
      </div>
    </div>
  );
}
