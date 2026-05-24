interface Props {
  active: boolean;
  trial?: boolean;
}

export default function StatusTag({ active, trial }: Props) {
  if (trial) return <span className="status-badge trial"><span className="dot" /> Trial</span>;
  return active
    ? <span className="status-badge active"><span className="dot" /> Activo</span>
    : <span className="status-badge inactive"><span className="dot" /> Inactivo</span>;
}
