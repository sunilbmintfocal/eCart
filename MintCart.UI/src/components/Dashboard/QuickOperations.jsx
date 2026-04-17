export default function QuickOperations() {
  const operations = [
    { title: "Add New Stock", icon: "add_box", variant: "primary" },
    { title: "Register Sale", icon: "point_of_sale", variant: "secondary" },
  ];

  return (
    <div className="bg-surface-container-high rounded-3xl p-6">
      <h4 className="text-title-md font-bold text-on-surface mb-6">Quick Operations</h4>
      <div className="grid grid-cols-1 gap-3">
        {operations.map((op, idx) => (
          <button key={idx} className="flex items-center justify-between p-4 bg-surface-container-lowest rounded-xl hover:shadow-md transition-shadow group">
            <div className="flex items-center gap-4">
              <div className={`w-10 h-10 rounded-lg flex items-center justify-center ${op.variant === 'primary' ? 'bg-primary/10 text-primary' : 'bg-secondary-container text-on-secondary-container'}`}>
                <span className="material-symbols-outlined">{op.icon}</span>
              </div>
              <span className="font-semibold text-sm">{op.title}</span>
            </div>
            <span className="material-symbols-outlined text-slate-300 group-hover:text-primary transition-colors">chevron_right</span>
          </button>
        ))}
      </div>
    </div>
  );
}
