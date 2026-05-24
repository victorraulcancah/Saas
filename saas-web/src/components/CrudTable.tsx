import type { ReactNode } from 'react';
import { Table, Grid } from 'antd';
import type { ColumnsType, TablePaginationConfig } from 'antd/es/table';
import PageHeader from './PageHeader';
import EntityModal from './EntityModal';

const { useBreakpoint } = Grid;

interface CrudTableProps<T> {
  title: string;
  subtitle?: string;
  actionLabel?: string;
  onAction?: () => void;
  columns: ColumnsType<T>;
  dataSource: T[];
  loading: boolean;
  rowKey: string;
  renderCard?: (item: T) => ReactNode;
  modalTitle: string;
  modalOpen: boolean;
  modalWidth?: number;
  onModalClose: () => void;
  modalFooter?: ReactNode;
  children?: ReactNode;
  extraActions?: ReactNode;
}

export default function CrudTable<T>({
  title, subtitle, actionLabel, onAction,
  columns, dataSource, loading, rowKey,
  renderCard,
  modalTitle, modalOpen, modalWidth, onModalClose, modalFooter,
  children, extraActions,
}: CrudTableProps<T>) {
  const screens = useBreakpoint();
  const isMobile = !screens.md;

  const pagination: TablePaginationConfig = {
    pageSize: 10,
    showSizeChanger: !isMobile,
    showTotal: !isMobile ? (total: number) => `${total} registros` : undefined,
    responsive: true,
  };

  return (
    <div className="crud-page">
      <PageHeader title={title} subtitle={subtitle} actionLabel={actionLabel} onAction={onAction}>
        {extraActions}
      </PageHeader>

      {isMobile && renderCard ? (
        <div className="crud-cards">
          {dataSource.map((item) => (
            <div key={(item as Record<string, unknown>)[rowKey] as string} className="crud-card">
              {renderCard(item)}
            </div>
          ))}
          {!loading && dataSource.length === 0 && (
            <div className="crud-empty">Sin registros</div>
          )}
        </div>
      ) : (
        <div className="crud-table-wrap">
          <Table
            dataSource={dataSource}
            columns={columns}
            rowKey={rowKey}
            loading={loading}
            pagination={pagination}
            scroll={{ x: isMobile ? 600 : undefined }}
            size={isMobile ? 'small' : 'middle'}
            locale={{ emptyText: 'Sin registros' }}
            bordered
          />
        </div>
      )}

      <EntityModal
        title={modalTitle}
        open={modalOpen}
        onCancel={onModalClose}
        footer={modalFooter}
        width={modalWidth ?? (isMobile ? 360 : 560)}
      >
        {children}
      </EntityModal>
    </div>
  );
}
