import type { ReactNode } from 'react';

interface Props {
  title: string;
  subtitle?: string;
  actionLabel?: string;
  onAction?: () => void;
  children?: ReactNode;
}

export default function PageHeader({ title, subtitle, actionLabel, onAction, children }: Props) {
  return (
    <div className="page-header">
      <div className="page-header-start">
        <h1 className="page-title">{title}</h1>
        {subtitle && <span className="page-subtitle">{subtitle}</span>}
      </div>
      <div className="page-header-actions">
        {children}
        {actionLabel && onAction && (
          <button className="page-btn-primary" onClick={onAction}>
            + {actionLabel}
          </button>
        )}
      </div>
    </div>
  );
}
