// Helps isolate data access logic using the Repository Pattern. Makes testing easier (mock repository).
// Clean data access abstraction (helps with unit tests, SOLID)
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InvoiceManagement1.Data;
using InvoiceManagement1.Models;
using InvoiceManagement1.DTOs;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManagement1.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly AppDbContext _context;

        public InvoiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Invoice> GetByIdAsync(int id)
        {
            return await _context.Invoices.Include(i => i.Items).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<PagedResult<Invoice>> GetFilteredInvoicesAsync(
            int page, int pageSize,
            string? status,
            string? customerName,
            DateTime? fromDate,
            DateTime? toDate)
        {
            var query = _context.Invoices
                                .Include(i => i.Items)
                                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(i => i.Status.ToLower() == status.ToLower());

            if (!string.IsNullOrWhiteSpace(customerName))
                query = query.Where(i => i.CustomerName.ToLower().Contains(customerName.ToLower()));

            if (fromDate.HasValue)
                query = query.Where(i => i.CreatedAt >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(i => i.CreatedAt <= toDate.Value.Date.AddDays(1).AddTicks(-1));

            var totalCount = await query.CountAsync();

            // If no data matched the filters, return empty response with pagination metadata
            if (totalCount == 0)
            {
                return new PagedResult<Invoice>
                {
                    Items = new List<Invoice>(),
                    TotalCount = 0,
                    Page = page,
                    PageSize = pageSize,
                    Message = "No invoices matched with the given filters."
                };
            }

            var invoices = await query
                .OrderBy(i => i.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Invoice>
            {
                Items = invoices,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Message = "Success"
            };
        }

        public async Task<string> CreateAsync(Invoice invoice)
        {
            var existingInvoice = await _context.Invoices
                .FirstOrDefaultAsync(i =>
                    (i.CustomerName == invoice.CustomerName &&
                    i.Status == invoice.Status) ||
                    i.TotalAmount == invoice.TotalAmount);

            if (existingInvoice != null)
            {
                return "Invoice already exists.";
            }

            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();

            return "Invoice created successfully.";
        }


        public async Task UpdateAsync(Invoice invoice)
        {
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
                await _context.SaveChangesAsync();
            }
        }
    }
}
