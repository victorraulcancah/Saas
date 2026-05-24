import type { ThemeConfig } from 'antd';
import esES from 'antd/locale/es_ES';

export const theme: ThemeConfig = {
  token: {
    colorPrimary: '#F97316',
    borderRadius: 8,
    fontFamily: '"Inter", -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif',
    colorBgLayout: '#F7F8FA',
    colorBorder: '#EAECF0',
    colorText: '#101828',
    colorTextSecondary: '#667085',
  },
  components: {
    Menu: { itemBorderRadius: 8, itemBg: 'transparent' },
    Button: { borderRadius: 8, controlHeight: 36 },
    Input: { borderRadius: 8, controlHeight: 40 },
    Table: { headerBg: '#F9FAFB', headerColor: '#667085', rowHoverBg: '#FFF4ED' },
    Modal: { borderRadius: 12 },
    Card: { borderRadius: 10 },
  },
};

export const locale = esES;
