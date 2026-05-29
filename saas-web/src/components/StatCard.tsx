import type { ReactNode } from 'react';
import { ArrowUpOutlined, ArrowDownOutlined } from '@ant-design/icons';

interface StatCardProps {
  title: string;
  value: string | number;
  icon: ReactNode;
  trend?: {
    value: number;
    label: string;
  };
  color?: string;
  subtitle?: string;
}

export default function StatCard({ title, value, icon, trend, color = 'var(--color-primary)', subtitle }: StatCardProps) {
  const isPositive = trend && trend.value >= 0;

  return (
    <div className="stat-card">
      <div className="stat-card-icon" style={{ background: `${color}15`, color }}>
        {icon}
      </div>
      <div className="stat-card-content">
        <div className="stat-card-title">{title}</div>
        <div className="stat-card-value">{value}</div>
        {subtitle && <div className="stat-card-subtitle">{subtitle}</div>}
        {trend && (
          <div className={`stat-card-trend ${isPositive ? 'positive' : 'negative'}`}>
            {isPositive ? <ArrowUpOutlined /> : <ArrowDownOutlined />}
            <span>{Math.abs(trend.value)}%</span>
            <span className="trend-label">{trend.label}</span>
          </div>
        )}
      </div>
    </div>
  );
}