/**
 * Shared API client for making HTTP requests to the backend.
 * Automatically handles authentication headers by extracting the OIDC token from storage.
 */

const API_BASE_URL = 'https://localhost:7200';

import { startLoading, stopLoading } from './loadingService';

/**
 * Retrieves the access token from session storage.
 * react-oidc-context (oidc-client-ts) stores use information in sessionStorage
 * with keys starting with 'oidc.user:'.
 */
const getAuthToken = () => {
  const oidcKey = Object.keys(sessionStorage).find(key => key.startsWith('oidc.user:'));
  if (oidcKey) {
    try {
      const user = JSON.parse(sessionStorage.getItem(oidcKey));
      return user?.access_token;
    } catch (e) {
      console.error('Error parsing OIDC user from storage:', e);
      return null;
    }
  }
  return null;
};

/**
 * Common response handler for fetch calls.
 */
const handleResponse = async (response) => {
  if (!response.ok) {
    let errorData;
    try {
      errorData = await response.json();
    } catch (e) {
      errorData = { message: response.statusText };
    }

    // Create a descriptive error
    const error = new Error(errorData.message || `API Request failed with status ${response.status}`);
    error.status = response.status;
    error.data = errorData;
    throw error;
  }

  if (response.status === 204) return null;
  return response.json();
};

/**
 * Core request function.
 */
const request = async (method, url, data = null, customHeaders = {}, skipToken = false) => {
  const token = skipToken ? null : getAuthToken();

  const headers = {
    'Content-Type': 'application/json',
    'Accept': 'application/json',
    ...customHeaders,
  };

  if (token) {
    headers['Authorization'] = `Bearer ${token}`;
  }

  const config = {
    method,
    headers,
  };

  if (data) {
    config.body = JSON.stringify(data);
  }

  // Ensure leading slash for relative URLs
  const path = url.startsWith('/') ? url : `/${url}`;
  const fullUrl = url.startsWith('http') ? url : `${API_BASE_URL}${path}`;

  startLoading();
  try {
    console.log(`[API Client] Requesting: ${method} ${fullUrl}`, { headers });
    const response = await fetch(fullUrl, config);
    console.log(`[API Client] Response: ${method} ${fullUrl} [${response.status}]`);
    return await handleResponse(response);
  } catch (error) {
    console.error(`[API Client Error] ${method} ${fullUrl}:`, error);
    throw error;
  } finally {
    stopLoading();
  }
};

/**
 * Standard API methods.
 */
export const apiClient = {
  get: (url, headers, skipToken) => request('GET', url, null, headers, skipToken),
  post: (url, data, headers, skipToken) => request('POST', url, data, headers, skipToken),
  put: (url, data, headers, skipToken) => request('PUT', url, data, headers, skipToken),
  patch: (url, data, headers, skipToken) => request('PATCH', url, data, headers, skipToken),
  delete: (url, headers, skipToken) => request('DELETE', url, null, headers, skipToken),
};
