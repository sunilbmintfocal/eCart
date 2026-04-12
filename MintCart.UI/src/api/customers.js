import { apiClient } from './client';

/**
 * Fetches the list of customers from the backend.
 * Uses the route defined in CustomerController.cs: api/customer/customers
 * @returns {Promise<Array>} A promise that resolves to an array of customers.
 */
export async function getCustomers(skipToken = false) {
  return apiClient.get('/api/customer/customers', {}, skipToken);
}

/**
 * Fetches a single customer by ID.
 * @param {string|number} id - The customer ID.
 */
export async function getCustomerById(id) {
  return apiClient.get(`/api/customer/${id}`);
}

/**
 * Creates a new customer.
 * @param {Object} customerData - The customer details.
 */
export async function createCustomer(customerData) {
  return apiClient.post('/api/customer', customerData);
}
