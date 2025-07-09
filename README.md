# Invoice Management System (.NET Core)

A secure and scalable Invoice Management System built using ASP.NET Core, Entity Framework Core, and MySQL. It includes JWT Authentication, advanced filtering, logging, rate limiting, and audit logs.

---

# URLs

- GitHub Repository: https://github.com/Ridhi1316/invoiceManagementProject
- Deployed API URL: https://invoicemanagementsystem.up.railway.app

---


## Setup Instructions

### 1. Prerequisites
 - .NET 8 SDK - https://dotnet.microsoft.com/en-us/download/dotnet/8.0
 - Docker (for containerized deployment) - https://www.docker.com/products/docker-desktop/
 - MySQL Server & MySQL Workbench - https://dev.mysql.com/downloads/
 - Git (To clone the git repository) - https://git-scm.com/downloads
 - Postman - https://www.postman.com/downloads/

### 2. Clone the Repository
```bash
git clone https://github.com/Ridhi1316/invoiceManagementProject.git
cd invoiceManagementProject

2. Testing via Docker
# run command
docker run -d -p 8080:80 --name invoice-api \
  -e "ASPNETCORE_ENVIRONMENT=Production" \
  invoice-app

Serving the app on URL : http://localhost:8080/.

```

### 3. MySQL Setup
 - MySQL Setup Steps (for macOS):
    - brew install mysql
     - brew services start mysql
 - Login to MySQL:
     - mysql -u root -p
 - Create the Database:
     - CREATE DATABASE InvoiceDB;
     - USE InvoiceDB;
 - Import the Dump File:
     - mysql -u root -p InvoiceDB < path_to/invoice_schema_dump.sql

### 4. Features Implemented
- JWT-based user authentication and role-based authorization
- CRUD for Invoices and Invoice Items
- Input validation with model attributes
- Pagination and filtering on listing APIs
- Logging using Serilog
- Audit middleware for request/response logs
- Basic rate limiting using in-memory store
- Swagger documentation (auto-generated)
- Dockerized for deployment
- MySQL DB configuration via environment-specific settings

### 5. API Endpoints
- Visit /swagger on local/production to view all APIs.
- https:///invoicemanagementproject-production.up.railway.app/swagger/v1/swagger.json
- GET /api/invoices
- GET /api/invoices/{id}
- POST /api/invoices
- PUT /api/invoices/{id}
- DELETE /api/invoices/{id}

----