import type { ReactNode } from 'react';
import { Modal } from 'antd';

interface Props {
  title: string;
  open: boolean;
  onCancel: () => void;
  onOk?: () => void;
  loading?: boolean;
  children: ReactNode;
  width?: number;
  footer?: ReactNode;
}

export default function EntityModal({ title, open, onCancel, onOk, loading, children, width, footer }: Props) {
  return (
    <Modal
      title={title}
      open={open}
      onCancel={onCancel}
      onOk={onOk}
      confirmLoading={loading}
      footer={footer}
      destroyOnClose
      width={width ?? 560}
      maskClosable={false}
    >
      {children}
    </Modal>
  );
}
