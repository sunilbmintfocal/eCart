import { useState } from 'react';
import DataTable from '../../components/DataTable';

const MOCK_ORDERS = [
  { id: 'ORD-7721', customer: 'Sarah Jenkins', product: 'Zenith Watch', date: 'Oct 24, 2023', amount: '₹1,240.00', status: 'Completed' },
  { id: 'ORD-7722', customer: 'Michael Chen', product: 'Quantum Laptop', date: 'Oct 24, 2023', amount: '₹2,100.00', status: 'Processing' },
  { id: 'ORD-7723', customer: 'Elena Rodriguez', product: 'Nordic Vase', date: 'Oct 23, 2023', amount: '₹420.00', status: 'Pending' },
  { id: 'ORD-7724', customer: 'David Smith', product: 'Ergo Chair', date: 'Oct 23, 2023', amount: '₹850.00', status: 'Cancelled' },
  { id: 'ORD-7725', customer: 'Lisa Wang', product: 'Aurelius Pendant', date: 'Oct 22, 2023', amount: '₹890.00', status: 'Completed' },
  { id: 'ORD-7726', customer: 'James Wilson', product: 'Leather Briefcase', date: 'Oct 22, 2023', amount: '₹450.00', status: 'Completed' },
  { id: 'ORD-7727', customer: 'Anna Muller', product: 'Desk Lamp', date: 'Oct 21, 2023', amount: '₹120.00', status: 'Processing' },
  { id: 'ORD-7728', customer: 'Robert Taylor', product: 'Minimalist Shelf', date: 'Oct 21, 2023', amount: '₹340.00', status: 'Completed' },
  { id: 'ORD-7729', customer: 'Sophie Martin', product: 'Ceramic Plate Set', date: 'Oct 20, 2023', amount: '₹280.00', status: 'Pending' },
  { id: 'ORD-7730', customer: 'Kevin Lee', product: 'Wireless Mouse', date: 'Oct 20, 2023', amount: '₹85.00', status: 'Completed' },
  { id: 'ORD-7731', customer: 'Rachel Green', product: 'Silk Cushion', date: 'Oct 19, 2023', amount: '₹150.00', status: 'Completed' },
  { id: 'ORD-7732', customer: 'Chris Evans', product: 'Steel Water Bottle', date: 'Oct 19, 2023', amount: '₹45.00', status: 'Processing' },
  { id: 'ORD-7733', customer: 'Emma Watson', product: 'Woolen Throw', date: 'Oct 18, 2023', amount: '₹220.00', status: 'Completed' },
  { id: 'ORD-7734', customer: 'Tom Hardy', product: 'Canvas Print', date: 'Oct 18, 2023', amount: '₹310.00', status: 'Pending' },
  { id: 'ORD-7735', customer: 'Natalie Portman', product: 'Scented Candle', date: 'Oct 17, 2023', amount: '₹35.00', status: 'Completed' },
];

export default function Orders() {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedOrder, setSelectedOrder] = useState(null);

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

  const columns = [
    {
      header: 'Order ID',
      key: 'id',
      className: 'text-primary'
    },
    {
      header: 'Customer',
      key: 'customer',
    },
    {
      header: 'Product',
      key: 'product',
      className: 'text-on-surface-variant'
    },
    {
      header: 'Date',
      key: 'date',
      className: 'text-on-surface-variant'
    },
    {
      header: 'Amount',
      key: 'amount',
      align: 'right',
    },
    {
      header: 'Status',
      align: 'center',
      render: (order) => (
        <span className={`inline-block w-24 text-center px-3 py-1 rounded-none text-[10px] font-bold uppercase tracking-wider ${getStatusStyle(order.status)}`}>
          {order.status}
        </span>
      )
    },
    {
      header: 'Actions',
      align: 'right',
      render: (order) => (
        <button
          onClick={() => handleEdit(order)}
          className="w-8 h-8 flex items-center justify-center rounded-none hover:bg-primary/10 text-primary transition-all active:scale-95 group-hover:scale-110"
          title="Edit Order"
        >
          <span className="material-symbols-outlined text-[18px]">edit</span>
        </button>
      )
    }
  ];

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
        <DataTable data={MOCK_ORDERS} columns={columns} defaultPageSize={10} />
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
