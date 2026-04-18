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
      payables:   rawPayables || getEmptyPayables(),
      activities: rawActivities,
      salesTrend: {
        points: (rawSalesTrend?.points ?? rawSalesTrend?.Points ?? []).map(p => ({
          label: p.label ?? p.Label ?? p.day ?? p.Day,
          value: p.value ?? p.Value
        }))
      },
    };
  } catch (err) {
    console.error('[Dashboard] API Error:', err);
    return getEmptyDashboardMetrics();
  }
};

/**
 * Fetches only the KPI cards.
 */
export const getDashboardKpis = async () => {
  try {
    const response = await apiClient.get(`${BASE}/kpis`);
    const data = response?.data ?? response?.Data ?? response;
    return mapKpis(data);
  } catch (err) {
    return mapKpis(null);
  }
};

/**
 * Fetches only the payables summary.
 */
export const getDashboardPayables = async () => {
  try {
    const response = await apiClient.get(`${BASE}/payables`);
    const data = response?.data ?? response?.Data ?? response;
    return data || getEmptyPayables();
  } catch (err) {
    return getEmptyPayables();
  }
};

/**
 * Fetches recent activities unified feed.
 * @param {number} top - Max number of records.
 */
export const getDashboardActivities = async (top = 10) => {
  try {
    const response = await apiClient.get(`${BASE}/activities?top=${top}`);
    const data = response?.data ?? response?.Data ?? response;
    return data || [];
  } catch (err) {
    return [];
  }
};

// ─── Helpers ──────────────────────────────────────────────────────────────────

/**
 * Normalises the KPI object from the API into the shape the components expect.
 */
const mapKpis = (kpis) => {
  const emptyKpi = { value: '-', trend: '-' };
  
  if (!kpis) return {
    totalSales: emptyKpi,
    lowStock: { value: '-', alerts: [] },
    complaints: emptyKpi,
    balance: emptyKpi,
  };

  const getKpi = (obj) => ({
    value: obj?.value ?? obj?.Value ?? '-',
    trend: obj?.trend ?? obj?.Trend ?? '-',
  });

  const rawTotalSales = kpis.totalSales ?? kpis.TotalSales;
  const rawLowStock = kpis.lowStock ?? kpis.LowStock;
  const rawComplaints = kpis.complaints ?? kpis.Complaints;
  const rawBalance = kpis.balance ?? kpis.Balance;

  return {
    totalSales: getKpi(rawTotalSales),
    lowStock: {
      value:  rawLowStock?.value ?? rawLowStock?.Value ?? '-',
      alerts: (rawLowStock?.alerts ?? rawLowStock?.Alerts ?? []).map(a => ({ 
        name: a.name ?? a.Name, 
        count: a.count ?? a.Count 
      })),
    },
    complaints: getKpi(rawComplaints),
    balance: getKpi(rawBalance),
  };
};

const getEmptyPayables = () => ({
  total: '-', today: '-', week: '-', month: '-'
});

// ─── Empty/Error Fallback ──────────────────────────────────────────────────────

const getEmptyDashboardMetrics = () => ({
  kpis: mapKpis(null),
  payables: getEmptyPayables(),
  activities: [],
  salesTrend: { points: [] }
});

