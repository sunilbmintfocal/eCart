import { useState, useEffect } from 'react';
import DataTable from '../../components/DataTable';
import { getOrders } from '../../api/orders';
export default function Orders() {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedOrder, setSelectedOrder] = useState(null);

  useEffect(() => {
    const fetchOrders = async () => {
      try {
        setLoading(true);
        const data = await getOrders();
        setOrders(data);
      } catch (error) {
        console.error('Failed to fetch orders', error);
      } finally {
        setLoading(false);
      }
    };
    fetchOrders();
  }, []);

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
          <span className="text-[10px] font-bold text-on-surface-variant uppercase tracking-[0.15em]">Total: {orders.length}</span>
        </div>
        <DataTable 
          data={orders} 
          columns={columns} 
          defaultPageSize={10} 
          emptyMessage={loading ? "Loading orders..." : "No records found."}
        />
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
