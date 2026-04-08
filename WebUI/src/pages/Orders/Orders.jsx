import { useState } from 'react';

const MOCK_ORDERS = [
  { id: 'ORD-7721', customer: 'Sarah Jenkins', product: 'Zenith Watch', date: 'Oct 24, 2023', amount: '$1,240', status: 'Completed' },
  { id: 'ORD-7722', customer: 'Michael Chen', product: 'Quantum Laptop', date: 'Oct 24, 2023', amount: '$2,100', status: 'Processing' },
  { id: 'ORD-7723', customer: 'Elena Rodriguez', product: 'Nordic Vase', date: 'Oct 23, 2023', amount: '$420', status: 'Pending' },
  { id: 'ORD-7724', customer: 'David Smith', product: 'Ergo Chair', date: 'Oct 23, 2023', amount: '$850', status: 'Cancelled' },
  { id: 'ORD-7725', customer: 'Lisa Wang', product: 'Aurelius Pendant', date: 'Oct 22, 2023', amount: '$890', status: 'Completed' },
  { id: 'ORD-7726', customer: 'James Wilson', product: 'Leather Briefcase', date: 'Oct 22, 2023', amount: '$450', status: 'Completed' },
  { id: 'ORD-7727', customer: 'Anna Muller', product: 'Desk Lamp', date: 'Oct 21, 2023', amount: '$120', status: 'Processing' },
  { id: 'ORD-7728', customer: 'Robert Taylor', product: 'Minimalist Shelf', date: 'Oct 21, 2023', amount: '$340', status: 'Completed' },
  { id: 'ORD-7729', customer: 'Sophie Martin', product: 'Ceramic Plate Set', date: 'Oct 20, 2023', amount: '$280', status: 'Pending' },
  { id: 'ORD-7730', customer: 'Kevin Lee', product: 'Wireless Mouse', date: 'Oct 20, 2023', amount: '$85', status: 'Completed' },
  { id: 'ORD-7731', customer: 'Rachel Green', product: 'Silk Cushion', date: 'Oct 19, 2023', amount: '$150', status: 'Completed' },
  { id: 'ORD-7732', customer: 'Chris Evans', product: 'Steel Water Bottle', date: 'Oct 19, 2023', amount: '$45', status: 'Processing' },
  { id: 'ORD-7733', customer: 'Emma Watson', product: 'Woolen Throw', date: 'Oct 18, 2023', amount: '$220', status: 'Completed' },
  { id: 'ORD-7734', customer: 'Tom Hardy', product: 'Canvas Print', date: 'Oct 18, 2023', amount: '$310', status: 'Pending' },
  { id: 'ORD-7735', customer: 'Natalie Portman', product: 'Scented Candle', date: 'Oct 17, 2023', amount: '$35', status: 'Completed' },
];

export default function Orders() {
  const [currentPage, setCurrentPage] = useState(1);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedOrder, setSelectedOrder] = useState(null);
  const itemsPerPage = 8;

  const totalPages = Math.ceil(MOCK_ORDERS.length / itemsPerPage);
  const startIndex = (currentPage - 1) * itemsPerPage;
  const currentOrders = MOCK_ORDERS.slice(startIndex, startIndex + itemsPerPage);

  const handleEdit = (order) => {
    setSelectedOrder(order);
    setIsModalOpen(true);
  };

  const getStatusStyle = (status) => {
    switch (status) {
      case 'Completed': return 'bg-primary-container/20 text-primary';
      case 'Processing': return 'bg-secondary-container/30 text-secondary';
      case 'Pending': return 'bg-tertiary-container/30 text-tertiary';
      case 'Cancelled': return 'bg-error-container/20 text-error';
      default: return 'bg-surface-container-high text-on-surface-variant';
    }
  };

  return (
    <section className="p-8 bg-surface min-h-screen">
      <div className="flex justify-between items-end mb-10">
        <div>
          <span className="text-primary text-[10px] font-medium uppercase tracking-[0.2em] mb-1.5 block">Transaction Registry</span>
          <h1 className="text-4xl font-headline font-bold text-on-surface tracking-tight mb-2">Order Management</h1>
          <p className="text-on-surface-variant font-body font-medium text-sm">Monitor and manage all customer transactions with precision.</p>
        </div>
        <div className="flex gap-3">
          <button className="flex items-center gap-2 px-6 py-2.5 border border-outline-variant/20 text-on-surface rounded-none font-headline text-sm font-bold hover:bg-surface-container-low transition-all">
            <span className="material-symbols-outlined text-lg">filter_list</span>
            Filters
          </button>
          <button className="flex items-center gap-2 px-6 py-2.5 btn-gradient rounded-none font-bold text-sm">
            <span className="material-symbols-outlined text-lg">add</span>
            New Order
          </button>
        </div>
      </div>

      <div className="bg-surface-container-lowest rounded-none ambient-shadow border border-outline-variant/10 overflow-hidden">
        <div className="px-8 py-5 flex justify-between items-center border-b border-outline-variant/5">
          <h3 className="text-xl font-headline font-bold text-on-surface tracking-tight">Recent Orders</h3>
          <span className="text-[10px] font-bold text-on-surface-variant uppercase tracking-[0.15em]">Total: {MOCK_ORDERS.length}</span>
        </div>
        <div className="overflow-x-auto">
          <table className="w-full text-left">
            <thead className="bg-primary/20 text-primary relative z-10">
              <tr>
                <th className="px-8 py-5 text-xs font-bold uppercase tracking-[0.2em] border-r border-primary/20">Order ID</th>
                <th className="px-8 py-5 text-xs font-bold uppercase tracking-[0.2em] border-r border-primary/20">Customer</th>
                <th className="px-8 py-5 text-xs font-bold uppercase tracking-[0.2em] border-r border-primary/20">Product</th>
                <th className="px-8 py-5 text-xs font-bold uppercase tracking-[0.2em] border-r border-primary/20">Date</th>
                <th className="px-8 py-5 text-xs font-bold uppercase tracking-[0.2em] border-r border-primary/20">Amount</th>
                <th className="px-8 py-5 text-xs font-bold uppercase tracking-[0.2em] border-r border-primary/20 text-center">Status</th>
                <th className="px-8 py-5 text-xs font-bold uppercase tracking-[0.2em] text-right">Actions</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-outline-variant/10 text-on-surface">
              {currentOrders.map((order) => (
                <tr key={order.id} className="hover:bg-primary/5 transition-all duration-150 group">
                  <td className="px-8 py-5 text-sm font-mono font-bold text-primary border-r border-outline-variant/10">{order.id}</td>
                  <td className="px-8 py-5 text-sm font-bold border-r border-outline-variant/10">{order.customer}</td>
                  <td className="px-8 py-5 text-sm font-medium text-on-surface-variant border-r border-outline-variant/10">{order.product}</td>
                  <td className="px-8 py-5 text-sm font-medium text-on-surface-variant border-r border-outline-variant/10">{order.date}</td>
                  <td className="px-8 py-5 text-sm font-mono font-bold border-r border-outline-variant/10 tracking-tight text-on-surface">{order.amount}</td>
                  <td className="px-8 py-5 text-center border-r border-outline-variant/10">
                    <span className={`inline-block px-3 py-1 rounded-none text-[10px] font-bold uppercase tracking-wider ${getStatusStyle(order.status)}`}>
                      {order.status}
                    </span>
                  </td>
                  <td className="px-8 py-5 text-right">
                    <button 
                      onClick={() => handleEdit(order)}
                      className="w-8 h-8 flex items-center justify-center rounded-none hover:bg-primary/10 text-primary transition-all active:scale-95 group-hover:scale-110"
                      title="Edit Order"
                    >
                      <span className="material-symbols-outlined text-[18px]">edit</span>
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>

      {/* Pagination */}
      <div className="mt-10 flex items-center justify-between border-t border-outline-variant/10 pt-8">
        <span className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest">
          Showing {startIndex + 1} to {Math.min(startIndex + itemsPerPage, MOCK_ORDERS.length)} of {MOCK_ORDERS.length}
        </span>
        <div className="flex gap-1.5">
          <button 
            disabled={currentPage === 1}
            onClick={() => setCurrentPage(currentPage - 1)}
            className="px-4 py-2 rounded-none border border-outline-variant/20 text-xs font-bold text-on-surface-variant hover:bg-surface-container-low transition-all disabled:opacity-30 disabled:hover:bg-transparent"
          >
            Previous
          </button>
          {[...Array(totalPages)].map((_, i) => (
            <button 
              key={i}
              onClick={() => setCurrentPage(i + 1)}
              className={`w-8 h-8 flex items-center justify-center rounded-none font-bold text-[10px] transition-all ${currentPage === i + 1 ? 'bg-primary text-on-primary shadow-sm' : 'hover:bg-surface-container-low text-on-surface-variant'}`}
            >
              {i + 1}
            </button>
          ))}
          <button 
            disabled={currentPage === totalPages}
            onClick={() => setCurrentPage(currentPage + 1)}
            className="px-4 py-2 rounded-none border border-outline-variant/20 text-xs font-bold text-on-surface-variant hover:bg-surface-container-low transition-all disabled:opacity-30 disabled:hover:bg-transparent"
          >
            Next
          </button>
        </div>
      </div>

      {/* Edit Modal */}
      {isModalOpen && selectedOrder && (
        <div className="fixed inset-0 z-[60] flex items-center justify-center p-4">
          <div className="absolute inset-0 bg-surface-container-highest/60 backdrop-blur-sm" onClick={() => setIsModalOpen(false)}></div>
          <div className="bg-surface relative z-10 w-full max-w-md rounded-none ambient-shadow border border-outline-variant/10 overflow-hidden animate-in fade-in zoom-in duration-200">
            <div className="px-8 py-6 border-b border-outline-variant/10 flex justify-between items-center bg-surface-container-low/30">
              <div>
                <h3 className="text-xl font-headline font-bold text-on-surface tracking-tight">Edit Order</h3>
                <p className="text-[10px] font-bold text-primary uppercase tracking-widest mt-1">Order Ref: {selectedOrder.id}</p>
              </div>
              <button 
                onClick={() => setIsModalOpen(false)}
                className="text-on-surface-variant hover:text-error transition-colors"
              >
                <span className="material-symbols-outlined">close</span>
              </button>
            </div>
            <div className="p-8 space-y-6">
              <div>
                <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">Customer Name</label>
                <input 
                  type="text" 
                  defaultValue={selectedOrder.customer}
                  className="w-full bg-surface-container-lowest border border-outline-variant/20 rounded-none px-4 py-2.5 text-sm font-medium text-on-surface focus:border-primary focus:ring-1 focus:ring-primary/20 transition-all outline-none"
                />
              </div>
              <div>
                <label className="text-[10px] font-bold text-on-surface-variant uppercase tracking-widest mb-2 block">Order Status</label>
                <select 
                  defaultValue={selectedOrder.status}
                  className="w-full bg-surface-container-lowest border border-outline-variant/20 rounded-none px-4 py-2.5 text-sm font-medium text-on-surface focus:border-primary focus:ring-1 focus:ring-primary/20 transition-all outline-none appearance-none"
                >
                  <option>Completed</option>
                  <option>Processing</option>
                  <option>Pending</option>
                  <option>Cancelled</option>
                </select>
              </div>
              <div className="pt-4 flex gap-3">
                <button 
                  onClick={() => setIsModalOpen(false)}
                  className="flex-1 px-6 py-2.5 border border-outline-variant/20 text-on-surface rounded-none font-bold text-sm hover:bg-surface-container-low transition-all"
                >
                  Cancel
                </button>
                <button 
                  onClick={() => setIsModalOpen(false)}
                  className="flex-1 px-6 py-2.5 btn-gradient rounded-none font-bold text-sm"
                >
                  Save Changes
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
    </section>
  );
}
