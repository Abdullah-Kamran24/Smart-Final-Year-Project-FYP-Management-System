import axios from 'axios';

const api = axios.create({
  baseURL: "/api",
  headers: { 'Content-Type': 'application/json' },
});

// Attach JWT token to every request automatically
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Handle 401 - redirect to login (unless already on login page / root page)
api.interceptors.response.use(
  (res) => res,
  (err) => {
    if (err.response?.status === 401 && window.location.pathname !== '/login' && window.location.pathname !== '/') {
      localStorage.clear();
      window.location.href = '/';
    }
    return Promise.reject(err);
  }
);

export default api;
