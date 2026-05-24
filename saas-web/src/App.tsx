import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './lib/auth-context';
import ErrorBoundary from './components/ErrorBoundary';
import ProtectedRoute from './lib/protected-route';
import Login from './pages/Login';
import Dashboard from './pages/Dashboard';
import TenantsList from './pages/tenants/TenantsList';
import ModulesList from './pages/modules/ModulesList';
import UsersList from './pages/users/UsersList';

export default function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <ErrorBoundary>
          <Routes>
            <Route path="/login" element={<Login />} />
            <Route element={<ProtectedRoute />}>
              <Route element={<Dashboard />}>
                <Route path="/tenants" element={<TenantsList />} />
                <Route path="/modules" element={<ModulesList />} />
                <Route path="/users" element={<UsersList />} />
                <Route path="/" element={<Navigate to="/tenants" replace />} />
              </Route>
            </Route>
          </Routes>
        </ErrorBoundary>
      </AuthProvider>
    </BrowserRouter>
  );
}
