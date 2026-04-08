import { Navigate } from 'react-router-dom';
import MainLayout from '../layouts/MainLayout';
import Dashboard from '../pages/Dashboard/Dashboard';
import Inventory from '../pages/Inventory/Inventory';
import Orders from '../pages/Orders/Orders';

const routes = [
  {
    path: "/",
    element: <MainLayout />,
    children: [
      { index: true, element: <Navigate to="/dashboard" replace /> },
      { path: "dashboard", element: <Dashboard /> },
      { path: "inventory", element: <Inventory /> },
      { path: "orders", element: <Orders /> },
      { path: "*", element: <Navigate to="/dashboard" replace /> }
    ]
  }
];

export default routes;
