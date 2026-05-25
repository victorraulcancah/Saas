import { useState, useMemo, useCallback, type ReactNode } from 'react';
import { Table, Grid, Dropdown, Checkbox } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import { SettingOutlined } from '@ant-design/icons';
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

  interface ColInfo { key: string; title: ReactNode; }

  const toggleable: ColInfo[] = columns
    .filter((c): c is ColInfo & typeof c => 'key' in c && !!c.key && c.key !== 'actions')
    .map((c) => ({ key: c.key as string, title: c.title ?? c.key }));

  const initialOrder = useMemo(() => toggleable.map((c) => c.key), [toggleable]);

  const [hiddenKeys, setHiddenKeys] = useState<Set<string>>(new Set());
  const [columnOrder, setColumnOrder] = useState<string[]>(initialOrder);
  const [dragKey, setDragKey] = useState<string | null>(null);
  const [dropKey, setDropKey] = useState<string | null>(null);

  if (columnOrder.length !== initialOrder.length) {
    setColumnOrder(initialOrder);
  }

  const visibleColumns = useMemo(
    () => columns.filter((c) => !('key' in c) || !c.key || !hiddenKeys.has(c.key as string)),
    [columns, hiddenKeys],
  );

  const orderedColumns = useMemo(() => {
    const byKey = new Map(visibleColumns.map((c) => [c.key as string, c]));
    const ordered: ColumnsType<T> = [];
    for (const key of columnOrder) {
      if (byKey.has(key)) ordered.push(byKey.get(key)!);
    }
    for (const col of visibleColumns) {
      if (!('key' in col) || !col.key || !columnOrder.includes(col.key as string)) {
        ordered.push(col);
      }
    }
    return ordered;
  }, [visibleColumns, columnOrder]);

  const reorder = useCallback((from: string, to: string) => {
    setColumnOrder((prev) => {
      const next = [...prev];
      const fi = next.indexOf(from);
      const ti = next.indexOf(to);
      if (fi === -1 || ti === -1) return prev;
      next.splice(fi, 1);
      next.splice(ti, 0, from);
      return next;
    });
  }, []);

  const colsWithDrag = useMemo(() => orderedColumns.map((col) => {
    if (!('key' in col) || !col.key || col.key === 'actions') return col;
    const key = col.key as string;
    return {
      ...col,
      onHeaderCell: () => ({
        'data-key': key,
        draggable: true,
        className: key === dragKey ? 'th-dragging' : key === dropKey && key !== dragKey ? 'th-drop-over' : '',
        onDragStart: () => { setDragKey(key); setDropKey(null); },
        onDragOver: (e: React.DragEvent) => { e.preventDefault(); setDropKey(key); },
        onDragLeave: () => { if (dropKey === key) setDropKey(null); },
        onDrop: () => { if (dragKey && dragKey !== key) reorder(dragKey, key); setDragKey(null); setDropKey(null); },
        onDragEnd: () => { setDragKey(null); setDropKey(null); },
      }),
    } as typeof col;
  }), [orderedColumns, dragKey, dropKey, reorder]);

  const toggleColumn = (key: string) => {
    setHiddenKeys((prev) => {
      const next = new Set(prev);
      if (next.has(key)) next.delete(key);
      else next.add(key);
      return next;
    });
  };

  const dropdownItems = {
    items: toggleable.map((col) => ({
      key: col.key,
      label: (
        <Checkbox
          checked={!hiddenKeys.has(col.key)}
          onChange={() => toggleColumn(col.key)}
          style={{ width: '100%', fontSize: 13 }}
        >
          {col.title}
        </Checkbox>
      ),
    })),
  };

  return (
    <div className="crud-page">
      <PageHeader title={title} subtitle={subtitle} actionLabel={actionLabel} onAction={onAction}>
        {extraActions}
        {!isMobile && toggleable.length > 0 && (
          <Dropdown menu={dropdownItems} trigger={['click']} placement="bottomRight">
            <button className="page-btn-icon" title="Columnas">
              <SettingOutlined />
            </button>
          </Dropdown>
        )}
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
            columns={colsWithDrag}
            rowKey={rowKey}
            loading={loading}
            pagination={false}
            scroll={{ x: 700 }}
            size="small"
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
