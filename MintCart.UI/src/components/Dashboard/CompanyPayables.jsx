export default function CompanyPayables({ data }) {
  if (!data) return null;

  return (
    <div className="bg-surface-container-lowest rounded-3xl p-6 border border-outline-variant/10 shadow-sm relative overflow-hidden group">
      <div className="absolute -right-4 -top-4 w-24 h-24 bg-primary/5 rounded-full blur-2xl group-hover:bg-primary/10 transition-all"></div>
      <h4 className="text-title-md font-bold mb-4 text-on-surface opacity-90">Company Payables</h4>
      <div className="space-y-4">
        <div>
          <p className="text-[10px] font-bold uppercase tracking-widest text-slate-500">Total Outstanding Balance</p>
          <p className="text-2xl font-black text-on-surface">{data.total}</p>
        </div>
        <div className="pt-4 border-t border-outline-variant/10">
          <p className="text-[10px] font-bold uppercase tracking-widest text-slate-500 mb-3">Payments Made</p>
          <div className="grid grid-cols-3 gap-2">
            <div>
              <p className="text-[8px] text-slate-400 uppercase font-bold tracking-tighter">Today</p>
              <p className="text-sm font-bold text-primary">{data.today}</p>
            </div>
            <div>
              <p className="text-[8px] text-slate-400 uppercase font-bold tracking-tighter">This Week</p>
              <p className="text-sm font-bold text-on-surface">{data.week}</p>
            </div>
            <div>
              <p className="text-[8px] text-slate-400 uppercase font-bold tracking-tighter">This Month</p>
              <p className="text-sm font-bold text-on-surface">{data.month}</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
