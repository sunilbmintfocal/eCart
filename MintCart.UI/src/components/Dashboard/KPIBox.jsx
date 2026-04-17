export default function KPIBox({ title, value, subtext, icon, trend, variant = "primary" }) {
  const variantStyles = {
    primary: "bg-primary-fixed/20 group-hover:bg-primary-fixed/40",
    tertiary: "bg-tertiary-fixed/30 group-hover:bg-tertiary-fixed/50",
    error: "bg-error-container/30 group-hover:bg-error-container/50",
    secondary: "bg-secondary-fixed/30 group-hover:bg-secondary-fixed/50",
  };

  const textColors = {
    primary: "text-primary",
    tertiary: "text-tertiary",
    error: "text-on-error-container",
    secondary: "text-on-secondary-container",
  };

  return (
    <div className="bg-surface-container-lowest p-6 rounded-2xl border border-outline-variant/10 shadow-sm relative overflow-hidden group">
      <div className={`absolute -right-4 -top-4 w-24 h-24 rounded-full blur-2xl transition-all ${variantStyles[variant]}`}></div>
      <p className="text-label-sm font-semibold text-slate-500 uppercase tracking-widest mb-1">{title}</p>
      <h3 className={`text-2xl font-black font-headline mb-4 ${textColors[variant]}`}>{value}</h3>
      {trend ? (
        <div className={`flex items-center gap-2 text-xs font-bold ${textColors[variant]}`}>
          <span className="material-symbols-outlined text-sm">{icon}</span>
          {trend}
        </div>
      ) : (
        <div className="flex items-center gap-3">
          {subtext}
        </div>
      )}
    </div>
  );
}
