import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { ConfigProvider } from 'antd';
import { AuthProvider } from './lib/auth-context';
import { theme, locale } from './styles/theme';
import ErrorBoundary from './components/ErrorBoundary';
import ProtectedRoute from './lib/protected-route';
import Login from './pages/Login';
import Layout from './components/Layout';
import TenantsList from './pages/tenants/TenantsList';
import CatalogPage from './pages/catalog/CatalogPage';
import PlansPage from './pages/plans/PlansPage';
import LicensesPage from './pages/licenses/LicensesPage';
import ModulesList from './pages/modules/ModulesList';
import UsersList from './pages/users/UsersList';
import DashboardPage from './pages/DashboardPage';

export default function App() {
  return (
    <ConfigProvider theme={theme} locale={locale}>
      <BrowserRouter>
        <AuthProvider>
          <ErrorBoundary>
            <Routes>
              <Route path="/login" element={<Login />} />
              <Route element={<ProtectedRoute />}>
                <Route element={<Layout />}>
                  <Route path="/" element={<DashboardPage />} />
                  <Route path="/tenants" element={<TenantsList />} />
                  <Route path="/catalog" element={<CatalogPage />} />
                  <Route path="/plans" element={<PlansPage />} />
                  <Route path="/licenses" element={<LicensesPage />} />
                  <Route path="/modules" element={<ModulesList />} />
                  <Route path="/users" element={<UsersList />} />
                </Route>
              </Route>
            </Routes>
          </ErrorBoundary>
        </AuthProvider>
      </BrowserRouter>
    </ConfigProvider>
  );
}