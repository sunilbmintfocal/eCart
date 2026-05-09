
export const getReportsList = async () => {
  // Simulate API delay
  await new Promise(resolve => setTimeout(resolve, 800));
  
  return [
    { id: 'daily-revenue', title: 'Daily Revenue Reconciliation', category: 'Financials', description: 'Real-time financial matching for high-velocity SKUs' },
    { id: 'inventory-audit', title: 'Full Inventory Audit', category: 'Inventory', description: 'Comprehensive stock levels and valuation' },
    { id: 'sku-performance', title: 'SKU Velocity & Performance', category: 'Analytics', description: 'Analysis of top-performing products over time' },
    { id: 'warehouse-efficiency', title: 'Warehouse Efficiency Report', category: 'Analytics', description: 'Picking and packing speed metrics by zone' },
    { id: 'return-analytics', title: 'Returns & Defect Rates', category: 'Inventory', description: 'Tracking product quality and return reasons' }
  ];
};

export const getReportData = async (reportId) => {
  await new Promise(resolve => setTimeout(resolve, 1000));
  
  if (reportId === 'daily-revenue') {
    return {
      title: 'Daily Revenue Reconciliation',
      description: 'Real-time financial matching for high-velocity SKUs',
      stats: [
        { label: 'Total Revenue', value: '$124,500.00', change: '+12.5%', trend: 'up' },
        { label: 'Reconciled', value: '98.2%', change: '+0.5%', trend: 'up' },
        { label: 'Pending', value: '$2,340.00', change: '-5.2%', trend: 'down' }
      ],
      transactions: [
        { id: 1, name: 'Precision Monitor XL', sku: 'MON-449', amount: '$1,299.00', status: 'Reconciled', date: '2026-04-25 14:20' },
        { id: 2, name: 'Artisan Tactile Deck', sku: 'KBD-912', amount: '$450.00', status: 'Reconciled', date: '2026-04-25 14:15' },
        { id: 3, name: 'Fiber-Optic Interconnect', sku: 'CBL-004', amount: '$85.00', status: 'Pending', date: '2026-04-25 14:10' },
        { id: 4, name: 'Advanced Sensor Core', sku: 'SNS-201', amount: '$3,200.00', status: 'Reconciled', date: '2026-04-25 13:55' },
        { id: 5, name: 'ErgoPoint Laser Mouse', sku: 'MSE-109', amount: '$120.00', status: 'Reconciled', date: '2026-04-25 13:45' }
      ]
    };
  }
  
  return null;
};
