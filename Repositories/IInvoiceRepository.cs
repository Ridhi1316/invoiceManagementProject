using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InvoiceManagement1.Models;
using InvoiceManagement1.DTOs;

namespace InvoiceManagement1.Repositories
{
    public interface IInvoiceRepository
    {
        Task<Invoice> GetByIdAsync(int id);
        Task<PagedResult<Invoice>> GetFilteredInvoicesAsync(int page, int pageSize, string? status, string? customerName, DateTime? fromDate, DateTime? toDate);
        Task<string> CreateAsync(Invoice invoice);
        Task UpdateAsync(Invoice invoice);
        Task DeleteAsync(int id);
    }
}
