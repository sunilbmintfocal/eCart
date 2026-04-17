// Mock dashboard API service
export const getDashboardMetrics = () => {
  return new Promise((resolve) => {
    setTimeout(() => {
      resolve({
        kpis: {
          totalSales: {
            value: "₹1,02,482.00",
            trend: "+14.2% from yesterday",
          },
          lowStock: {
            value: "08 Items",
            alerts: [
              { name: "Pro Laptops", count: 2 },
              { name: "Smartphones", count: 6 }
            ]
          },
          complaints: {
            value: "05",
            trend: "3 pending immediate action",
          },
          balance: {
            value: "₹3,98,220.50",
            trend: "Pending collections",
          }
        },
        payables: {
          total: "₹1,50,82,450",
          today: "₹12,450",
          week: "₹82,000",
          month: "₹2,45,000"
        },
        activities: [
          {
            id: "#4402",
            name: "James Wilson",
            type: "Sale: 2x Wireless Buds",
            icon: "shopping_bag",
            value: "₹32,998.00",
            time: "Today, 02:14 PM",
            status: "Completed",
            statusVariant: "primary"
          },
          {
            id: "#8812",
            name: "Elena Rodriguez",
            type: "Complaint: Screen Flicker",
            icon: "assignment_late",
            value: "--",
            time: "Today, 11:30 AM",
            status: "Pending",
            statusVariant: "error"
          },
          {
            id: "#SUP-10",
            name: "TechDistro Inc.",
            type: "Restock: 50x Pro Laptops",
            icon: "local_shipping",
            value: "₹37,45,000.00",
            time: "Yesterday, 04:45 PM",
            status: "In Transit",
            statusVariant: "secondary"
          }
        ]
      });
    }, 800);
  });
};
