import { NavLink } from 'react-router-dom';
import Logo from '../components/Logo';

export default function Sidebar({ isCollapsed, setIsCollapsed }) {
  return (
    <aside className={`h-screen fixed left-0 top-0 bg-surface-container-low flex flex-col p-4 gap-2 z-50 transition-all duration-300 ${isCollapsed ? 'w-20' : 'w-64'}`}>
      <div className="mb-8 px-2 py-2 flex items-center justify-between">
        {!isCollapsed && (
          <div>
            <Logo showText={true} iconSize="h-6" />
            <p className="text-[10px] text-on-surface-variant font-medium uppercase tracking-[0.1em] mt-1">Retail Suite</p>
          </div>
        )}
        <button
          onClick={() => setIsCollapsed(!isCollapsed)}
          className={`text-on-surface-variant hover:text-primary transition-colors p-1 ${isCollapsed ? 'mx-auto' : ''}`}
          title="Toggle Sidebar"
        >
          <span className="material-symbols-outlined">{isCollapsed ? 'menu' : 'menu_open'}</span>
        </button>
      </div>
      <nav className="flex-1 space-y-1">
        <NavLink to="/dashboard" className={({ isActive }) => `flex items-center ${isCollapsed ? 'justify-center px-0' : 'gap-3 px-4'} py-3 rounded-none cursor-pointer transition-all duration-300 font-body font-medium text-sm ${isActive ? 'bg-surface-container-lowest text-on-surface font-bold shadow-sm' : 'text-on-surface-variant hover:text-on-surface hover:translate-x-1'}`}>
          {({ isActive }) => (
            <>
              <span className="material-symbols-outlined" data-icon="dashboard" style={{ fontVariationSettings: isActive ? "'FILL' 1" : "'FILL' 0" }}>dashboard</span>
              {!isCollapsed && <span>Dashboard</span>}
            </>
          )}
        </NavLink>

        <NavLink to="/inventory" className={({ isActive }) => `flex items-center ${isCollapsed ? 'justify-center px-0' : 'gap-3 px-4'} py-3 rounded-none cursor-pointer transition-all duration-300 font-body font-medium text-sm ${isActive ? 'bg-surface-container-lowest text-on-surface font-bold shadow-sm' : 'text-on-surface-variant hover:text-on-surface hover:translate-x-1'}`}>
          {({ isActive }) => (
            <>
              <span className="material-symbols-outlined" data-icon="inventory_2" style={{ fontVariationSettings: isActive ? "'FILL' 1" : "'FILL' 0" }}>inventory_2</span>
              {!isCollapsed && <span>Inventory</span>}
            </>
          )}
        </NavLink>

        <NavLink to="/customers" className={({ isActive }) => `flex items-center ${isCollapsed ? 'justify-center px-0' : 'gap-3 px-4'} py-3 rounded-none cursor-pointer transition-all duration-300 font-body font-medium text-sm ${isActive ? 'bg-surface-container-lowest text-on-surface font-bold shadow-sm' : 'text-on-surface-variant hover:text-on-surface hover:translate-x-1'}`}>
          {({ isActive }) => (
            <>
              <span className="material-symbols-outlined" data-icon="person" style={{ fontVariationSettings: isActive ? "'FILL' 1" : "'FILL' 0" }}>person</span>
              {!isCollapsed && <span>Customer</span>}
            </>
          )}
        </NavLink>

        <NavLink to="/reports" className={({ isActive }) => `flex items-center ${isCollapsed ? 'justify-center px-0' : 'gap-3 px-4'} py-3 rounded-none cursor-pointer transition-all duration-300 font-body font-medium text-sm ${isActive ? 'bg-surface-container-lowest text-on-surface font-bold shadow-sm' : 'text-on-surface-variant hover:text-on-surface hover:translate-x-1'}`}>
          {({ isActive }) => (
            <>
              <span className="material-symbols-outlined" data-icon="assessment" style={{ fontVariationSettings: isActive ? "'FILL' 1" : "'FILL' 0" }}>assessment</span>
              {!isCollapsed && <span>Reports</span>}
            </>
          )}
        </NavLink>

      </nav>
      <div className="mt-auto px-4 pb-4">
        <div className={`text-[10px] text-on-surface-variant font-mono transition-opacity duration-300 ${isCollapsed ? 'opacity-0' : 'opacity-40'}`}>
          v{import.meta.env.VITE_APP_VERSION || '0.0.0'}
        </div>
      </div>
    </aside>
  );
}
