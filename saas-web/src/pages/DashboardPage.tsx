import {
  TeamOutlined, AppstoreOutlined, UserOutlined, ShoppingCartOutlined,
  FileTextOutlined, RiseOutlined, DollarOutlined, ClockCircleOutlined,
  CheckCircleOutlined, ExclamationCircleOutlined, DesktopOutlined,
  SafetyOutlined
} from '@ant-design/icons';
import StatCard from '../components/StatCard';
import '../styles/global.css';

const fakeStats = {
  tenants: 24,
  users: 156,
  activePlans: 18,
  monthlyRevenue: 45890,
};

const fakeActivity = [
  { id: 1, icon: <TeamOutlined />, color: '#F97316', text: '<strong>TecnoPerú</strong> activó el plan Professional', time: 'Hace 5 min' },
  { id: 2, icon: <AppstoreOutlined />, color: '#12B76A', text: 'Nuevo módulo <strong>CRM</strong> agregado al catálogo', time: 'Hace 23 min' },
  { id: 3, icon: <CheckCircleOutlined />, color: '#1570EF', text: '<strong>CompuMundo</strong> renovó suscripción anual', time: 'Hace 1 hora' },
  { id: 4, icon: <ExclamationCircleOutlined />, color: '#F59E0B', text: '<strong>DataSys</strong> necesita renovación de licencia', time: 'Hace 2 horas' },
  { id: 5, icon: <UserOutlined />, color: '#7F56D9', text: 'Nuevo usuario <strong>admin@ferremax.com</strong> creado', time: 'Hace 3 horas' },
];

const fakeTopPlans = [
  { name: 'Starter', count: 12, percent: 50, color: '#F97316' },
  { name: 'Professional', count: 8, percent: 33, color: '#12B76A' },
  { name: 'Enterprise', count: 4, percent: 17, color: '#1570EF' },
];

const fakeRecentTenants = [
  { name: 'TecnoPerú S.A.', plan: 'Professional', users: 12, status: 'active' },
  { name: 'CompuMundo', plan: 'Enterprise', users: 45, status: 'active' },
  { name: 'DataSys Peru', plan: 'Starter', users: 3, status: 'trial' },
  { name: 'Ferremax S.A.C.', plan: 'Professional', users: 8, status: 'active' },
];

const fakeModuleUsage = [
  { name: 'Facturación', count: 22, icon: <FileTextOutlined />, color: '#F97316' },
  { name: 'Inventario', count: 18, icon: <ShoppingCartOutlined />, color: '#12B76A' },
  { name: 'Compras', count: 14, icon: <ShoppingCartOutlined />, color: '#1570EF' },
  { name: 'CRM', count: 10, icon: <DesktopOutlined />, color: '#7F56D9' },
  { name: 'RRHH', count: 6, icon: <UserOutlined />, color: '#F59E0B' },
];

export default function DashboardPage() {
  return (
    <div className="dashboard-page">
      <div className="stats-grid">
        <StatCard
          title="Total Tenants"
          value={fakeStats.tenants}
          icon={<TeamOutlined />}
          trend={{ value: 12, label: 'vs mes anterior' }}
          color="var(--color-primary)"
          subtitle="Empresas activas"
        />
        <StatCard
          title="Usuarios Totales"
          value={fakeStats.users}
          icon={<UserOutlined />}
          trend={{ value: 8, label: 'vs mes anterior' }}
          color="#1570EF"
          subtitle="En todos los tenants"
        />
        <StatCard
          title="Planes Activos"
          value={fakeStats.activePlans}
          icon={<SafetyOutlined />}
          trend={{ value: 5, label: 'nuevos este mes' }}
          color="#12B76A"
          subtitle="Suscripciones vigentes"
        />
        <StatCard
          title="Ingresos Mensuales"
          value={`$${fakeStats.monthlyRevenue.toLocaleString()}`}
          icon={<DollarOutlined />}
          trend={{ value: 15, label: 'vs mes anterior' }}
          color="#7F56D9"
          subtitle="USD facturado"
        />
      </div>

      <div className="widgets-grid">
        <div className="widget-card">
          <div className="widget-header">
            <span className="widget-title">Actividad Reciente</span>
            <ClockCircleOutlined style={{ color: 'var(--color-text-light)', fontSize: 14 }} />
          </div>
          <div className="widget-body">
            <div className="activity-list">
              {fakeActivity.map((item) => (
                <div key={item.id} className="activity-item">
                  <div className="activity-icon" style={{ background: `${item.color}15`, color: item.color }}>
                    {item.icon}
                  </div>
                  <div className="activity-content">
                    <div className="activity-text" dangerouslySetInnerHTML={{ __html: item.text }} />
                    <div className="activity-time">{item.time}</div>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>

        <div className="widget-card">
          <div className="widget-header">
            <span className="widget-title">Distribución por Plan</span>
            <RiseOutlined style={{ color: 'var(--color-text-light)', fontSize: 14 }} />
          </div>
          <div className="widget-body">
            {fakeTopPlans.map((plan) => (
              <div key={plan.name} className="progress-item">
                <div className="progress-header">
                  <span className="progress-label">{plan.name}</span>
                  <span className="progress-value">{plan.count} tenants</span>
                </div>
                <div className="progress-bar">
                  <div className="progress-fill" style={{ width: `${plan.percent}%`, background: plan.color }} />
                </div>
              </div>
            ))}
          </div>
        </div>

        <div className="widget-card">
          <div className="widget-header">
            <span className="widget-title">Últimos Tenants</span>
            <TeamOutlined style={{ color: 'var(--color-text-light)', fontSize: 14 }} />
          </div>
          <div className="widget-body">
            {fakeRecentTenants.map((tenant, idx) => (
              <div key={idx} className="list-item">
                <div className="list-item-left">
                  <div className="list-item-icon" style={{ background: 'var(--color-primary-light)', color: 'var(--color-primary)' }}>
                    {tenant.name.charAt(0)}
                  </div>
                  <div className="list-item-content">
                    <div className="list-item-title">{tenant.name}</div>
                    <div className="list-item-subtitle">{tenant.users} usuarios · {tenant.plan}</div>
                  </div>
                </div>
                <div className="list-item-right">
                  <span className={`badge ${tenant.status === 'active' ? 'badge-success' : 'badge-warning'}`}>
                    {tenant.status === 'active' ? 'Activo' : 'Trial'}
                  </span>
                </div>
              </div>
            ))}
          </div>
        </div>

        <div className="widget-card">
          <div className="widget-header">
            <span className="widget-title">Uso por Módulo</span>
            <AppstoreOutlined style={{ color: 'var(--color-text-light)', fontSize: 14 }} />
          </div>
          <div className="widget-body">
            {fakeModuleUsage.map((mod) => (
              <div key={mod.name} className="list-item">
                <div className="list-item-left">
                  <div className="list-item-icon" style={{ background: `${mod.color}15`, color: mod.color }}>
                    {mod.icon}
                  </div>
                  <div className="list-item-content">
                    <div className="list-item-title">{mod.name}</div>
                    <div className="list-item-subtitle">Módulo activo</div>
                  </div>
                </div>
                <div className="list-item-right">
                  <div className="list-item-value">{mod.count}</div>
                  <div className="list-item-meta">tenants</div>
                </div>
              </div>
            ))}
          </div>
        </div>

        <div className="widget-card">
          <div className="widget-header">
            <span className="widget-title">Resumen Rápido</span>
            <ExclamationCircleOutlined style={{ color: 'var(--color-text-light)', fontSize: 14 }} />
          </div>
          <div className="widget-body">
            <div className="list-item">
              <div className="list-item-left">
                <div className="list-item-icon" style={{ background: 'var(--color-success-bg)', color: 'var(--color-success-text)' }}>
                  <CheckCircleOutlined />
                </div>
                <div className="list-item-content">
                  <div className="list-item-title">Sistemas Configurados</div>
                </div>
              </div>
              <div className="list-item-right">
                <div className="list-item-value">4</div>
              </div>
            </div>
            <div className="list-item">
              <div className="list-item-left">
                <div className="list-item-icon" style={{ background: 'var(--color-info-bg)', color: 'var(--color-info)' }}>
                  <AppstoreOutlined />
                </div>
                <div className="list-item-content">
                  <div className="list-item-title">Módulos Totales</div>
                </div>
              </div>
              <div className="list-item-right">
                <div className="list-item-value">18</div>
              </div>
            </div>
            <div className="list-item">
              <div className="list-item-left">
                <div className="list-item-icon" style={{ background: 'var(--color-primary-light)', color: 'var(--color-primary)' }}>
                  <FileTextOutlined />
                </div>
                <div className="list-item-content">
                  <div className="list-item-title">Submódulos Vendibles</div>
                </div>
              </div>
              <div className="list-item-right">
                <div className="list-item-value">45</div>
              </div>
            </div>
            <div className="list-item">
              <div className="list-item-left">
                <div className="list-item-icon" style={{ background: 'var(--color-warning-bg)', color: 'var(--color-warning-text)' }}>
                  <ClockCircleOutlined />
                </div>
                <div className="list-item-content">
                  <div className="list-item-title">Licencias por Vencer</div>
                </div>
              </div>
              <div className="list-item-right">
                <div className="list-item-value" style={{ color: 'var(--color-warning-text)' }}>3</div>
              </div>
            </div>
          </div>
        </div>

        <div className="widget-card">
          <div className="widget-header">
            <span className="widget-title">Tendencia de Ingresos</span>
            <RiseOutlined style={{ color: 'var(--color-text-light)', fontSize: 14 }} />
          </div>
          <div className="widget-body">
            <div className="chart-placeholder">
              [Gráfico de líneas - últimos 6 meses]
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}