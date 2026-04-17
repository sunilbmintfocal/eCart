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

    const rawKpis = data?.kpis ?? data?.Kpis;
    const rawPayables = data?.payables ?? data?.Payables;
    const rawActivities = data?.activities ?? data?.Activities ?? [];
    const rawSalesTrend = data?.salesTrend ?? data?.SalesTrend;

    return {
      kpis:       mapKpis(rawKpis),
      payables:   rawPayables,
      activities: rawActivities,
      salesTrend: {
        points: (rawSalesTrend?.points ?? rawSalesTrend?.Points ?? []).map(p => ({
          label: p.label ?? p.Label ?? p.day ?? p.Day,
          value: p.value ?? p.Value
        }))
      },
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
  const data = response?.data ?? response?.Data ?? response;
  return mapKpis(data);
};

/**
 * Fetches only the payables summary.
 */
export const getDashboardPayables = async () => {
  const response = await apiClient.get(`${BASE}/payables`);
  const data = response?.data ?? response?.Data ?? response;
  return data;
};

/**
 * Fetches recent activities unified feed.
 * @param {number} top - Max number of records.
 */
export const getDashboardActivities = async (top = 10) => {
  const response = await apiClient.get(`${BASE}/activities?top=${top}`);
  const data = response?.data ?? response?.Data ?? response;
  return data;
};

// ─── Helpers ──────────────────────────────────────────────────────────────────

/**
 * Normalises the KPI object from the API into the shape the components expect.
 */
const mapKpis = (kpis) => {
  if (!kpis) return {
    totalSales: { value: '₹0.00', trend: '' },
    lowStock: { value: '0 Items', alerts: [] },
    complaints: { value: '0', trend: '' },
    balance: { value: '₹0.00', trend: '' },
  };

  const getKpi = (obj) => ({
    value: obj?.value ?? obj?.Value ?? '',
    trend: obj?.trend ?? obj?.Trend ?? '',
  });

  const rawTotalSales = kpis.totalSales ?? kpis.TotalSales;
  const rawLowStock = kpis.lowStock ?? kpis.LowStock;
  const rawComplaints = kpis.complaints ?? kpis.Complaints;
  const rawBalance = kpis.balance ?? kpis.Balance;

  return {
    totalSales: getKpi(rawTotalSales),
    lowStock: {
      value:  rawLowStock?.value ?? rawLowStock?.Value ?? '0 Items',
      alerts: (rawLowStock?.alerts ?? rawLowStock?.Alerts ?? []).map(a => ({ 
        name: a.name ?? a.Name, 
        count: a.count ?? a.Count 
      })),
    },
    complaints: getKpi(rawComplaints),
    balance: getKpi(rawBalance),
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
  salesTrend: {
    points: [
      { label: '13 Apr', value: 12000 },
      { label: '14 Apr', value: 18000 },
      { label: '15 Apr', value: 15000 },
      { label: '16 Apr', value: 22000 },
      { label: '17 Apr', value: 35000 },
    ]
  }
});
