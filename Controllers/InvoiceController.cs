// Handles HTTP requests (GET, POST, PUT, DELETE) and calls services.
// Contains API endpoints.
using InvoiceManagement1.DTOs;
using InvoiceManagement1.Models;
using InvoiceManagement1.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceManagement1.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Route: api/invoice
    [Authorize] // JWT auth required
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceRepository _repository;

        public InvoiceController(IInvoiceRepository repository)
        {
            _repository = repository;
        }

        // Get: api/invoice - pagination applied
        [HttpGet("allInvoices")]
        public async Task<IActionResult> GetInvoices(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? status = null,
            [FromQuery] string? customerName = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var result = await  _repository.GetFilteredInvoicesAsync(page, pageSize, status, customerName, fromDate, toDate);

            if (result.Items.Count == 0)
                return NotFound("No invoices found for the given filters.");

            return Ok(result);
        }

        // GET: api/invoice/{id}
        [HttpGet("getInvoiceById")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var invoice = await _repository.GetByIdAsync(id);
            if (invoice == null)
                return NotFound();
            return Ok(invoice);
        }

        // POST: api/invoice
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InvoiceDto invoiceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var invoice = new Invoice
            {
                CustomerName = invoiceDto.CustomerName,
                Status = invoiceDto.Status,
                TotalAmount = invoiceDto.TotalAmount,
                Items = invoiceDto.Items?.Select(item => new InvoiceItem
                {
                    Description = item.Description,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            };

            var resultMessage = await _repository.CreateAsync(invoice);

            if (resultMessage.Contains("already exists"))
                return Conflict(new { message = resultMessage });

            return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, invoice);
        }

        // PUT: api/invoice/by-id/?id=1
        [HttpPut("updateInvoice")]
        public async Task<IActionResult> Update([FromBody] InvoiceDto dto)
        {
            if (dto.Id <= 0)
            {
                Console.WriteLine("Invalid ID provided in request.");
                return BadRequest("Invalid ID.");
            }

            var invoice = await _repository.GetByIdAsync(dto.Id);
            if (invoice == null)
            {
                Console.WriteLine($"Invoice with ID {dto.Id} not found.");
                return NotFound($"Invoice with ID {dto.Id} not found.");
            }

            invoice.CustomerName = dto.CustomerName;
            invoice.Status = dto.Status;
            invoice.UpdatedAt = DateTime.UtcNow;
            invoice.TotalAmount = dto.Items.Sum(i => i.UnitPrice * i.Quantity);
            invoice.Items = dto.Items.Select(i => new InvoiceItem
            {
                Description = i.Description,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList();

            await _repository.UpdateAsync(invoice);

            Console.WriteLine($"Invoice Updated: ID={invoice.Id}, Customer={invoice.CustomerName}");

            return Ok(new
            {
                Message = "Invoice updated successfully",
                Id = invoice.Id,
                CustomerName = invoice.CustomerName
            });
        }

        // DELETE: api/invoice/{id}
        [HttpDelete("deleteInvoiceById")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var invoice = await _repository.GetByIdAsync(id);
            if (invoice == null)
            {
                Console.WriteLine($"[DELETE] Invoice with ID {id} not found.");
                return NotFound(new { message = $"Invoice with ID {id} not found." });
            }

            await _repository.DeleteAsync(id);

            Console.WriteLine($"[DELETE] Invoice deleted. ID: {invoice.Id}, Customer: {invoice.CustomerName}");

            return Ok(new
            {
                message = "Invoice deleted successfully",
                invoiceId = invoice.Id,
                customer = invoice.CustomerName
            });
        }

    }
}
