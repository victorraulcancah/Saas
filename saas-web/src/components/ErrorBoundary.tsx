import { Component, type ReactNode, type ErrorInfo } from 'react';
import { Result, Button } from 'antd';

interface Props { children: ReactNode }
interface State { hasError: boolean; error: Error | null }

export default class ErrorBoundary extends Component<Props, State> {
  state: State = { hasError: false, error: null };

  static getDerivedStateFromError(error: Error) {
    return { hasError: true, error };
  }

  componentDidCatch(error: Error, info: ErrorInfo) {
    console.error('ErrorBoundary caught:', error, info);
  }

  render() {
    if (this.state.hasError) {
      return (
        <Result
          status="error"
          title="Algo salió mal"
          subTitle={this.state.error?.message}
          extra={<Button type="primary" onClick={() => window.location.reload()}>Recargar</Button>}
        />
      );
    }
    return this.props.children;
  }
}
