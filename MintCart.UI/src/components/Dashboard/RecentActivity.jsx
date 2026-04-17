export default function RecentActivity({ activities = [] }) {
  return (
    <div className="bg-surface-container-lowest rounded-3xl p-8 border border-outline-variant/10 overflow-hidden">
      <div className="flex justify-between items-center mb-8">
        <h3 className="text-xl font-bold text-on-surface tracking-tight">Recent Activity</h3>
        <div className="flex gap-2">
          <span className="px-3 py-1 bg-primary/10 text-primary text-xs font-bold rounded-full cursor-pointer">All Items</span>
          <span className="px-3 py-1 text-slate-400 text-xs font-bold rounded-full hover:bg-slate-50 cursor-pointer transition-colors">Sales Only</span>
          <span className="px-3 py-1 text-slate-400 text-xs font-bold rounded-full hover:bg-slate-50 cursor-pointer transition-colors">Complaints Only</span>
        </div>
      </div>
      <div className="w-full overflow-x-auto">
        <table className="w-full text-left border-collapse">
          <thead>
            <tr className="border-b border-outline-variant/10">
              <th className="pb-4 text-label-sm font-bold text-slate-400 uppercase tracking-widest px-4">Entity</th>
              <th className="pb-4 text-label-sm font-bold text-slate-400 uppercase tracking-widest px-4">Activity Type</th>
              <th className="pb-4 text-label-sm font-bold text-slate-400 uppercase tracking-widest px-4 text-right">Value</th>
              <th className="pb-4 text-label-sm font-bold text-slate-400 uppercase tracking-widest px-4">Timestamp</th>
              <th className="pb-4 text-label-sm font-bold text-slate-400 uppercase tracking-widest px-4">Status</th>
              <th className="pb-4 px-4"></th>
            </tr>
          </thead>
          <tbody className="divide-y divide-transparent">
            {activities.map((act, idx) => (
              <tr key={idx} className="group hover:bg-surface-container-low/50 transition-colors">
                <td className="py-6 px-4">
                  <div className="flex items-center gap-3">
                    <div className={`w-8 h-8 rounded-full flex items-center justify-center ${
                      act.statusVariant === 'primary' ? 'bg-primary-fixed text-on-primary-fixed-variant' :
                      act.statusVariant === 'error' ? 'bg-tertiary-fixed text-on-tertiary-fixed-variant' :
                      'bg-secondary-fixed text-on-secondary-fixed'
                    }`}>
                      <span className="material-symbols-outlined text-sm">
                        {act.icon === 'assignment_late' ? 'report' : 'person'}
                      </span>
                    </div>
                    <div>
                      <p className="text-sm font-bold">{act.name}</p>
                      <p className="text-[10px] text-slate-400">ID: {act.id}</p>
                    </div>
                  </div>
                </td>
                <td className="py-6 px-4">
                  <div className={`flex items-center gap-2 ${act.statusVariant === 'error' ? 'text-error' : ''}`}>
                    <span className={`material-symbols-outlined text-lg ${
                      act.statusVariant === 'primary' ? 'text-primary' :
                      act.statusVariant === 'secondary' ? 'text-secondary' : ''
                    }`}>{act.icon}</span>
                    <span className="text-sm font-medium">{act.type}</span>
                  </div>
                </td>
                <td className="py-6 px-4 text-right">
                  <p className={`text-sm font-black ${act.value === '--' ? 'text-slate-300' : ''}`}>{act.value}</p>
                </td>
                <td className="py-6 px-4">
                  <p className="text-xs text-slate-500">{act.time}</p>
                </td>
                <td className="py-6 px-4">
                  <span className={`px-3 py-1 text-[10px] font-bold rounded-full ${
                    act.statusVariant === 'primary' ? 'bg-primary-fixed text-on-primary-fixed-variant' :
                    act.statusVariant === 'error' ? 'bg-error-container text-on-error-container' :
                    'bg-secondary-container text-on-secondary-container'
                  }`}>{act.status}</span>
                </td>
                <td className="py-6 px-4 text-right">
                  <button className="material-symbols-outlined text-slate-300 hover:text-on-surface">more_vert</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
