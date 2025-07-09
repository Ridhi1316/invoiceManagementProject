using InvoiceManagement1.Controllers;
using InvoiceManagement1.Repositories;
using InvoiceManagement1.Models;
using InvoiceManagement1.DTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace InvoiceManagement1.Tests
{
    public class InvoiceTests
    {

        [Fact]
        public async Task GetFiltered_Invoices()
        {
            // Arrange
            var mockRepo = new Mock<IInvoiceRepository>();
            var expectedInvoices = new PagedResult<Invoice>
            {
                Items = new List<Invoice>
                {
                    new Invoice { Id = 1, CustomerName = "Test", Status = "Paid", TotalAmount = 1000 }
                },
                TotalCount = 1,
                Page = 1,
                PageSize = 10
            };

            mockRepo.Setup(repo => repo.GetFilteredInvoicesAsync(1, 10, "Paid", "Test", null, null))
                    .ReturnsAsync(expectedInvoices);

            var controller = new InvoiceController(mockRepo.Object);

            // Act
            var result = await controller.GetInvoices(1, 10, "Paid", "Test", null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResult = Assert.IsType<PagedResult<Invoice>>(okResult.Value);
            Assert.Single(returnedResult.Items);
            Assert.Equal("Test", returnedResult.Items.First().CustomerName);
        }

        [Fact]
        public async Task GetById_Invoice()
        {
            var invoiceId = 1;
            var mockInvoice = new Invoice { Id = invoiceId, CustomerName = "Ridhi" };

            var mockRepo = new Mock<IInvoiceRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(invoiceId))
                    .ReturnsAsync(mockInvoice);

            var controller = new InvoiceController(mockRepo.Object);
            var result = await controller.GetById(invoiceId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedInvoice = Assert.IsType<Invoice>(okResult.Value);
            Assert.Equal(invoiceId, returnedInvoice.Id);
        }

        [Fact]
        public async Task Create_Invoice()
        {
            var invoice = new InvoiceDto
            {
                CustomerName = "Dyta",
                Status = "Draft",
                TotalAmount = 2000
            };

            var mockRepo = new Mock<IInvoiceRepository>();
            mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Invoice>()))
                    .ReturnsAsync("Invoice created successfully.");

            var controller = new InvoiceController(mockRepo.Object);

            var result = await controller.Create(invoice);

            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async Task Update_Invoice()
        {
            // Arrange
            var dto = new InvoiceDto
            {
                Id = 1,
                CustomerName = "Updated",
                Status = "Sent",
                Items = new List<InvoiceItemDto>
                {
                    new InvoiceItemDto { Description = "Service A", Quantity = 2, UnitPrice = 1000 }
                }
            };

            var existingInvoice = new Invoice
            {
                Id = dto.Id,
                CustomerName = "Old",
                Status = "Draft",
                TotalAmount = 0,
                Items = new List<InvoiceItem>()
            };

            var mockRepo = new Mock<IInvoiceRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(dto.Id)).ReturnsAsync(existingInvoice);
            mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Invoice>())).Returns(Task.CompletedTask);

            var controller = new InvoiceController(mockRepo.Object);

            // Act
            var result = await controller.Update(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as dynamic;

            Assert.Equal("Invoice updated successfully", (string)response.Message);
            Assert.Equal(dto.Id, (int)response.Id);
            Assert.Equal(dto.CustomerName, (string)response.CustomerName);
        }

        [Fact]
        public async Task Delete_Invoice()
        {
            // Arrange
            int invoiceId = 1;
            var invoice = new Invoice
            {
                Id = invoiceId,
                CustomerName = "ToDelete",
                Status = "Draft",
                TotalAmount = 500
            };

            var mockRepo = new Mock<IInvoiceRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(invoiceId)).ReturnsAsync(invoice);
            mockRepo.Setup(repo => repo.DeleteAsync(invoiceId)).Returns(Task.CompletedTask);

            var controller = new InvoiceController(mockRepo.Object);

            // Act
            var result = await controller.Delete(invoiceId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
