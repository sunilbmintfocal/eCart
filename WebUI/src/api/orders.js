import mockOrders from '../data/orders.json';

/**
 * Fetches the list of orders.
 * Currently uses mock data. When the real API is ready, replace this implementation
 * with a standard `fetch` or `axios` call.
 * 
 * @returns {Promise<Array>} A promise that resolves to an array of orders.
 */
export async function getOrders() {
  // Simulate network latency (e.g., 500ms) to represent real API call behavior
  return new Promise((resolve) => {
    setTimeout(() => {
      resolve(mockOrders);
    }, 500);
  });
}
