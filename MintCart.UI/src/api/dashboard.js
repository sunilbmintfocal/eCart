import { apiClient, API_URLS } from './client';

const BASE = `${API_URLS.dashboard}/api/dashboard`;

/**
 * Fetches all dashboard metrics (KPIs + Payables + Activities) in one call.
 * Falls back gracefully to mock data if the API is unavailable.
 */
export const getDashboardMetrics = async () => {
  try {
    const response = await apiClient.get(`${BASE}/metrics`);
    // The backend wraps responses — unwrap if needed
    const data = response?.data ?? response?.Data ?? response;
    return {
      kpis:       mapKpis(data.kpis),
      payables:   data.payables,
      activities: data.activities ?? [],
    };
  } catch (err) {
    console.warn('[Dashboard] API unavailable, falling back to mock data.', err);
    return getMockDashboardMetrics();
  }
};

/**
 * Fetches only the KPI cards.
 */
export const getDashboardKpis = async () => {
  const response = await apiClient.get(`${BASE}/kpis`);
  return mapKpis(response?.data ?? response?.Data ?? response);
};

/**
 * Fetches only the payables summary.
 */
export const getDashboardPayables = async () => {
  const response = await apiClient.get(`${BASE}/payables`);
  return response?.data ?? response?.Data ?? response;
};

/**
 * Fetches recent activities unified feed.
 * @param {number} top - Max number of records.
 */
export const getDashboardActivities = async (top = 10) => {
  const response = await apiClient.get(`${BASE}/activities?top=${top}`);
  return response?.data ?? response?.Data ?? response;
};

// ─── Helpers ──────────────────────────────────────────────────────────────────

/**
 * Normalises the KPI object from the API into the shape the components expect.
 */
const mapKpis = (kpis) => {
  if (!kpis) return {};
  return {
    totalSales: {
      value: kpis.totalSales?.value ?? '₹0.00',
      trend: kpis.totalSales?.trend ?? '',
    },
    lowStock: {
      value:  kpis.lowStock?.value ?? '00 Items',
      alerts: kpis.lowStock?.alerts?.map(a => ({ name: a.name, count: a.count })) ?? [],
    },
    complaints: {
      value: kpis.complaints?.value ?? '00',
      trend: kpis.complaints?.trend ?? '',
    },
    balance: {
      value: kpis.balance?.value ?? '₹0.00',
      trend: kpis.balance?.trend ?? '',
    },
  };
};

// ─── Mock Fallback ────────────────────────────────────────────────────────────

const getMockDashboardMetrics = () => ({
  kpis: {
    totalSales:  { value: '₹1,02,482.00', trend: '+14.2% from yesterday' },
    lowStock:    { value: '08 Items', alerts: [{ name: 'Pro Laptops', count: 2 }, { name: 'Smartphones', count: 6 }] },
    complaints:  { value: '05', trend: '3 pending immediate action' },
    balance:     { value: '₹3,98,220.50', trend: 'Pending collections' },
  },
  payables: {
    total: '₹1,50,82,450',
    today: '₹12,450',
    week:  '₹82,000',
    month: '₹2,45,000',
  },
  activities: [
    {
      id: '#4402', name: 'James Wilson', type: 'Sale: 2x Wireless Buds',
      icon: 'shopping_bag', value: '₹32,998.00',
      time: 'Today, 02:14 PM', status: 'Completed', statusVariant: 'primary',
    },
    {
      id: '#8812', name: 'Elena Rodriguez', type: 'Complaint: Screen Flicker',
      icon: 'assignment_late', value: '--',
      time: 'Today, 11:30 AM', status: 'Pending', statusVariant: 'error',
    },
    {
      id: '#SUP-10', name: 'TechDistro Inc.', type: 'Restock: 50x Pro Laptops',
      icon: 'local_shipping', value: '₹37,45,000.00',
      time: 'Yesterday, 04:45 PM', status: 'In Transit', statusVariant: 'secondary',
    },
  ],
});
