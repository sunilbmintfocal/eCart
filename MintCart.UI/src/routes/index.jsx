import { Navigate } from 'react-router-dom';
import MainLayout from '../layouts/MainLayout';
import Dashboard from '../pages/Dashboard/Dashboard';
import Inventory from '../pages/Inventory/Inventory';
import Customers from '../pages/Customers/Customers';
import Reports from '../pages/Reports/Reports';
import ProtectedRoute from '../components/ProtectedRoute';

const routes = [
  {
    path: "/",
    element: (
      <ProtectedRoute>
        <MainLayout />
      </ProtectedRoute>
    ),
    children: [
      { index: true, element: <Navigate to="/dashboard" replace /> },
      { path: "dashboard", element: <Dashboard /> },
      { path: "inventory", element: <Inventory /> },
      { path: "customers", element: <Customers /> },
      { path: "reports", element: <Reports /> },
      { path: "*", element: <Navigate to="/dashboard" replace /> }
    ]
  }
];

export default routes;
