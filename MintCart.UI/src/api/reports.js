import { apiClient } from './client';

const BASE = '/api/report';

// ─── Normaliser ───────────────────────────────────────────────────────────────

const mapSalesRow = (row, index) => ({
  id:             row.SlNo            ?? row.slNo            ?? index + 1,
  Company:        row.Company         ?? row.company         ?? '',
  ItemCode:       row.ItemCode        ?? row.itemCode        ?? '',
  ItemName:       row.ItemName        ?? row.itemName        ?? '',
  InvoiceNo:      row.InvoiceNo       ?? row.invoiceNo       ?? '',
  SaleDateString: row.SaleDateString  ?? row.saleDateString  ?? '',
  Rate:           row.Rate            ?? row.rate            ?? 0,
  Quantity:       row.Quantity        ?? row.quantity        ?? 0,
  TaxableAmount:  row.TaxableAmount   ?? row.taxableAmount   ?? 0,
  GSTRate:        row.GSTRate         ?? row.gstRate         ?? 0,
  TotalGSTAmount: row.TotalGSTAmount  ?? row.totalGSTAmount  ?? 0,
  TotalAmount:    row.TotalAmount     ?? row.totalAmount     ?? 0,
});

// ─── Exports ──────────────────────────────────────────────────────────────────

/**
 * Fetches daily sales report data from the backend.
 * Falls back to an empty list if the API is unavailable.
 *
 * @param {Object} filters - Optional filters forwarded as query params.
 * @param {string} filters.fromDate - ISO date string (yyyy-MM-dd).
 * @param {string} filters.toDate   - ISO date string (yyyy-MM-dd).
 * @param {string} filters.company  - Company name to filter by.
 * @returns {Promise<Array>} Normalised array of daily sales rows.
 */
export const getDailySalesReport = async (filters = {}) => {
  try {
    const params = new URLSearchParams();
    if (filters.fromDate) params.append('fromDate', filters.fromDate);
    if (filters.toDate)   params.append('toDate',   filters.toDate);
    if (filters.company && filters.company !== 'All') params.append('company', filters.company);

    const query = params.toString() ? `?${params.toString()}` : '';
    const response = await apiClient.get(`${BASE}/daily-sales${query}`);
    const data = response?.data ?? response?.Data ?? response;

    return Array.isArray(data) ? data.map(mapSalesRow) : [];
  } catch (err) {
    console.error('[Reports] Daily Sales API error:', err);
    return [];
  }
};
