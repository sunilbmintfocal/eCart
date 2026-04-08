import { NavLink } from 'react-router-dom';

export default function Sidebar({ isCollapsed, setIsCollapsed }) {
  return (
    <aside className={`h-screen fixed left-0 top-0 bg-surface-container-low flex flex-col p-4 gap-2 z-50 transition-all duration-300 ${isCollapsed ? 'w-20' : 'w-64'}`}>
      <div className="mb-8 px-2 py-2 flex items-center justify-between">
        {!isCollapsed && (
          <div>
            <h2 className="text-lg font-bold text-on-surface font-headline overflow-hidden text-ellipsis whitespace-nowrap">Emerald Precision</h2>
            <p className="text-[10px] text-on-surface-variant font-medium uppercase tracking-[0.1em]">The Clinical Artisan</p>
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
        <NavLink to="/dashboard" className={({isActive}) => `flex items-center ${isCollapsed ? 'justify-center px-0' : 'gap-3 px-4'} py-3 rounded-none cursor-pointer transition-all duration-300 font-body font-medium text-sm ${isActive ? 'bg-surface-container-lowest text-on-surface font-bold shadow-sm' : 'text-on-surface-variant hover:text-on-surface hover:translate-x-1'}`}>
          {({isActive}) => (
            <>
              <span className="material-symbols-outlined" data-icon="dashboard" style={{ fontVariationSettings: isActive ? "'FILL' 1" : "'FILL' 0" }}>dashboard</span>
              {!isCollapsed && <span>Dashboard</span>}
            </>
          )}
        </NavLink>
        <NavLink to="/inventory" className={({isActive}) => `flex items-center ${isCollapsed ? 'justify-center px-0' : 'gap-3 px-4'} py-3 rounded-none cursor-pointer transition-all duration-300 font-body font-medium text-sm ${isActive ? 'bg-surface-container-lowest text-on-surface font-bold shadow-sm' : 'text-on-surface-variant hover:text-on-surface hover:translate-x-1'}`}>
          {({isActive}) => (
            <>
              <span className="material-symbols-outlined" data-icon="inventory_2" style={{ fontVariationSettings: isActive ? "'FILL' 1" : "'FILL' 0" }}>inventory_2</span>
              {!isCollapsed && <span>Inventory</span>}
            </>
          )}
        </NavLink>
        <NavLink to="/orders" className={({isActive}) => `flex items-center ${isCollapsed ? 'justify-center px-0' : 'gap-3 px-4'} py-3 rounded-none cursor-pointer transition-all duration-300 font-body font-medium text-sm ${isActive ? 'bg-surface-container-lowest text-on-surface font-bold shadow-sm' : 'text-on-surface-variant hover:text-on-surface hover:translate-x-1'}`}>
          {({isActive}) => (
            <>
              <span className="material-symbols-outlined" data-icon="receipt_long" style={{ fontVariationSettings: isActive ? "'FILL' 1" : "'FILL' 0" }}>receipt_long</span>
              {!isCollapsed && <span>Orders</span>}
            </>
          )}
        </NavLink>
        <div className={`flex items-center ${isCollapsed ? 'justify-center px-0' : 'gap-3 px-4'} py-3 text-on-surface-variant hover:text-on-surface hover:translate-x-1 transition-all cursor-pointer font-body font-medium text-sm rounded-none`}>
          <span className="material-symbols-outlined" data-icon="payments">payments</span>
          {!isCollapsed && <span>Billing</span>}
        </div>
        <div className={`flex items-center ${isCollapsed ? 'justify-center px-0' : 'gap-3 px-4'} py-3 text-on-surface-variant hover:text-on-surface hover:translate-x-1 transition-all cursor-pointer font-body font-medium text-sm rounded-none`}>
          <span className="material-symbols-outlined" data-icon="bar_chart">bar_chart</span>
          {!isCollapsed && <span>Analytics</span>}
        </div>
      </nav>
      <div className="mt-auto px-1 pb-4">
        <button className={`w-full btn-gradient py-3 rounded-none font-headline font-bold flex justify-center items-center ${isCollapsed ? 'px-0' : 'px-4'}`}>
          {isCollapsed ? <span className="material-symbols-outlined">add</span> : 'Create New Order'}
        </button>
      </div>
    </aside>
  );
}
