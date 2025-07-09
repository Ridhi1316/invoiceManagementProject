# ---- Build stage ----
    FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
    WORKDIR /src
    
    # Copy and restore
    COPY ["InvoiceManagement1.csproj", "."]
    RUN dotnet restore "InvoiceManagement1.csproj"    
    
    # Copy rest of the code
    WORKDIR "/src"
    COPY . .
    WORKDIR "/src"
    RUN dotnet publish "InvoiceManagement1.csproj" -c Release -o /app/publish    
    
    # ---- Runtime stage ----
    FROM mcr.microsoft.com/dotnet/aspnet:8.0
    WORKDIR /app
    COPY --from=build /app/publish .
    EXPOSE 80
    ENTRYPOINT ["dotnet", "InvoiceManagement1.dll"]
    