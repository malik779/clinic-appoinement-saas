import axios from 'axios'
import type { ApiResponse } from '../types/api'

const baseURL = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5100/api/v1'

export const apiClient = axios.create({
  baseURL,
  timeout: 15000,
})

apiClient.interceptors.request.use((config) => {
  const token = localStorage.getItem('cms.accessToken')
  const tenantId = localStorage.getItem('cms.tenantId')

  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }

  if (tenantId) {
    config.headers['X-Tenant-Id'] = tenantId
  }

  return config
})

export async function apiGet<T>(path: string): Promise<T> {
  const response = await apiClient.get<ApiResponse<T>>(path)
  if (!response.data.success || response.data.data === null) {
    throw new Error(response.data.errors.join(', ') || 'Request failed.')
  }

  return response.data.data
}

export async function apiPost<TResponse, TRequest>(path: string, payload: TRequest): Promise<TResponse> {
  const response = await apiClient.post<ApiResponse<TResponse>>(path, payload)
  if (!response.data.success || response.data.data === null) {
    throw new Error(response.data.errors.join(', ') || 'Request failed.')
  }

  return response.data.data
}
