import { useState, useEffect } from "react";
import { getDashboardMetrics } from "../../api/dashboard";
import KPIBox from "../../components/Dashboard/KPIBox";
import SalesTrend from "../../components/Dashboard/SalesTrend";
import CompanyPayables from "../../components/Dashboard/CompanyPayables";
import QuickOperations from "../../components/Dashboard/QuickOperations";
import RecentActivity from "../../components/Dashboard/RecentActivity";

export default function Dashboard() {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);

  const fetchData = async (isRefreshing = false) => {
    if (isRefreshing) setRefreshing(true);
    else setLoading(true);

    try {
      const res = await getDashboardMetrics();
      setData(res);
    } catch (error) {
      console.error("Failed to fetch dashboard metrics:", error);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  if (loading) {
    return (
      <div className="flex-1 flex items-center justify-center min-h-screen">
        <div className="flex flex-col items-center gap-4">
          <span className="material-symbols-outlined text-5xl text-primary animate-spin">progress_activity</span>
          <p className="text-sm text-slate-500 uppercase tracking-widest font-bold">Loading Dashboard...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="flex-1 flex flex-col min-h-screen">
      <section className="p-8 space-y-8">
        {/* Header Welcome */}
        <div className="flex justify-between items-center">
          <div>
            <h2 className="text-display-lg text-3xl font-extrabold text-on-surface tracking-tight">Dashboard</h2>
            <p className="text-slate-500 mt-1">Real-time electronics retail performance metrics.</p>
          </div>
          <button
            onClick={() => fetchData(true)}
            disabled={refreshing}
            title="Refresh Dashboard"
            className="group flex items-center justify-center w-12 h-12 rounded-full bg-surface-container-high hover:bg-surface-container-highest transition-all duration-300 border border-outline-variant/30 active:scale-90 disabled:opacity-50 shadow-sm hover:shadow-md"
          >
            <span className={`material-symbols-outlined text-2xl text-primary transition-transform duration-700 ${refreshing ? 'animate-spin' : 'group-hover:rotate-180'}`}>
              sync
            </span>
          </button>
        </div>

        {/* KPI Bento Grid */}
        <div className="grid grid-cols-1 md:grid-cols-4 gap-6">
          <KPIBox
            title="Total Sales (Today)"
            value={data.kpis.totalSales.value}
            trend={data.kpis.totalSales.trend}
            icon="trending_up"
            variant="primary"
          />
          <KPIBox
            title="Low Stock Alerts"
            value={data.kpis.lowStock.value}
            subtext={
              <>
                {data.kpis.lowStock.alerts.map((a, i) => (
                  <span key={i} className="px-2 py-0.5 bg-tertiary-fixed text-on-tertiary-fixed-variant text-[10px] rounded-full font-bold">
                    {a.name} ({a.count})
                  </span>
                ))}
              </>
            }
            variant="tertiary"
          />
          <KPIBox
            title="Active Complaints"
            value={data.kpis.complaints.value}
            trend={data.kpis.complaints.trend}
            icon="priority_high"
            variant="error"
          />
          <KPIBox
            title="Customer Balance"
            value={data.kpis.balance.value}
            trend={data.kpis.balance.trend}
            icon="account_balance"
            variant="secondary"
          />
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          <SalesTrend trendData={data.salesTrend} />
          <div className="space-y-6">
            <CompanyPayables data={data.payables} />
            <QuickOperations />
          </div>
        </div>

        <RecentActivity activities={data.activities} />
      </section>

      {/* Footer */}
      <footer className="mt-auto py-8 px-8 border-t border-outline-variant/10 text-center">

      </footer>
    </div>
  );
}
