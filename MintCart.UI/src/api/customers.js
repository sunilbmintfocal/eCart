import { apiClient } from './client';

/**
 * Fetches the list of customers from the backend.
 * Uses the route defined in CustomerController.cs: api/customer/customers
 * @returns {Promise<Array>} A promise that resolves to an array of customers.
 */
export async function getCustomers(skipToken = false) {
  return apiClient.get('/api/customer/all', {}, skipToken);
}

export async function getCustomersPaged(page = 1, pageSize = 10, search = '') {
  const params = new URLSearchParams({ page, pageSize });
  if (search) params.append('search', search);
  return apiClient.get(`/api/customer/list?${params.toString()}`);
}

export async function searchCustomerSuggestions(q) {
  return apiClient.get(`/api/customer/suggest?q=${encodeURIComponent(q)}`);
}

/**
 * Creates or updates a customer.
 * Uses the route defined in CustomerController.cs: api/customer/create
 * @param {Object} customerData - The customer details.
 */
export async function upsertCustomer(customerData) {
  return apiClient.post('/api/customer/create', customerData);
}

/**
 * Fetches a single customer by ID.
 * @param {string|number} id - The customer ID.
 */
export async function getCustomerById(id) {
  return apiClient.get(`/api/customer/${id}`);
}

/**
 * Merges multiple customer profiles into the first (primary) customer.
 * @param {number[]} customerIds - IDs to merge; first ID is the primary.
 * @param {Object} customerDetails - The details to apply to the primary customer.
 */
export async function mergeCustomers(customerIds, customerDetails) {
  return apiClient.post('/api/customer/merge', { CustomerIds: customerIds, CustomerDetails: customerDetails });
}
