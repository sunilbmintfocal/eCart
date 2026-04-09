import React from 'react';

/**
 * A reusable data table component designed for the eCart enterprise aesthetic.
 * 
 * @param {Array} data - The JSON data to render (array of objects).
 * @param {Array} columns - Configuration for columns: 
 *    { 
 *      header: string, 
 *      key: string, 
 *      render: (item) => ReactNode, 
 *      className: string,
 *      headerClassName: string,
 *      align: 'left' | 'center' | 'right'
 *    }
 * @param {string} className - Additional table container styling.
 * @param {boolean} hoverEffect - Whether to show hover highlights on rows.
 * @param {string} emptyMessage - Message to show when data is empty.
 */
export default function DataTable({ 
  data = [], 
  columns = [], 
  className = "", 
  hoverEffect = true,
  emptyMessage = "No records found."
}) {
  return (
    <div className={`overflow-x-auto ${className}`}>
      <table className="w-full text-left border-collapse">
        <thead className="bg-primary/20 text-primary sticky top-0 z-10">
          <tr>
            {columns.map((col, index) => (
              <th 
                key={index} 
                className={`px-8 py-5 text-xs font-bold uppercase tracking-[0.2em] border-r border-primary/20 last:border-r-0 ${col.headerClassName || ''} ${
                  col.align === 'center' ? 'text-center' : col.align === 'right' ? 'text-right' : ''
                }`}
              >
                {col.header}
              </th>
            ))}
          </tr>
        </thead>
        <tbody className="divide-y divide-outline-variant/10 text-on-surface">
          {data.length > 0 ? (
            data.map((item, rowIndex) => (
              <tr 
                key={item.id || rowIndex} 
                className={`${hoverEffect ? 'hover:bg-primary/5 transition-all duration-150 group' : ''}`}
              >
                {columns.map((col, colIndex) => (
                  <td 
                    key={colIndex} 
                    className={`px-8 py-5 text-sm border-r border-outline-variant/10 last:border-r-0 ${col.className || ''} ${
                      col.align === 'center' ? 'text-center' : col.align === 'right' ? 'text-right' : ''
                    }`}
                  >
                    {col.render ? col.render(item) : item[col.key]}
                  </td>
                ))}
              </tr>
            ))
          ) : (
            <tr>
              <td 
                colSpan={columns.length} 
                className="px-8 py-12 text-center text-sm font-medium text-on-surface-variant italic"
              >
                {emptyMessage}
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
}
