
-- Create Invoices table
CREATE TABLE IF NOT EXISTS Invoices (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    CustomerName VARCHAR(100) NOT NULL,
    Status VARCHAR(50) NOT NULL,
    TotalAmount DECIMAL(10, 2) NOT NULL,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Create InvoiceItems table
CREATE TABLE IF NOT EXISTS InvoiceItems (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    InvoiceId INT NOT NULL,
    Description VARCHAR(255) NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (InvoiceId) REFERENCES Invoices(Id) ON DELETE CASCADE
);

-- Sample Data
INSERT INTO Invoices (CustomerName, Status, TotalAmount) VALUES
('John Doe', 'Paid', 200.00),
('Jane Smith', 'Unpaid', 150.00);

INSERT INTO InvoiceItems (InvoiceId, Description, Quantity, UnitPrice) VALUES
(1, 'Product A', 2, 50.00),
(1, 'Product B', 1, 100.00),
(2, 'Product C', 3, 50.00);
