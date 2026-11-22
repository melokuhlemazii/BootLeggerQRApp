# ?? QR-Based Restaurant Ordering System

A modern, digital ordering platform for restaurants. Customers scan table-specific QR codes to browse the menu, place orders, and pay—all from their mobile devices.

## ? Quick Start

### Prerequisites
- **.NET 8 SDK** or later
- **SQL Server** (local or cloud)
- **Visual Studio 2022** or VS Code with C# extension

### Setup

1. **Clone & Open**
   ```bash
   git clone <repo-url>
   cd BootLeggerQRApp
   ```

2. **Configure Database**
   - Update `appsettings.json`:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=BootLeggerDB;Trusted_Connection=true;"
     }
     ```

3. **Apply Migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run**
   ```bash
   dotnet run
   ```
   Navigate to `https://localhost:5000`

## ?? Features

### ?? Customer Experience
- **QR Code Join**: Scan table QR ? automatic session setup
- **Menu Browsing**: View items with descriptions, images, and pricing
- **Shopping Cart**: Add/remove items, real-time total calculation
- **Multiple Payment Methods**: Cash on delivery or card payment
- **Order Confirmation**: Real-time status updates

### ????? Staff/Admin Dashboard
- Manage tables and QR codes
- View active and completed orders
- Track payment status
- Collect and review customer feedback

### ?? Modern UI
- Responsive Bootstrap 5 design
- Smooth card-based layouts
- Accessible forms and buttons
- Mobile-optimized navigation

## ??? Architecture

### Controllers
- **QRCodeController**: QR generation, table joining, menu display
- **OrdersController**: Order creation, item management, cart operations
- **PaymentController**: Payment method selection, confirmation
- **MenuItemsController**: CRUD for menu items
- **FeedbacksController**: Feedback submission and management
- **TablesController**: Table configuration

### Models
- `Order` / `OrderItem`: Cart and order persistence
- `MenuItem`: Menu catalog with pricing & availability
- `Payment`: Transaction tracking
- `Table`: Physical table mapping
- `Feedback`: Customer reviews

### Services
- **MenuService**: Menu item operations
- **OrderService**: Order workflows
- **PaymentService**: Payment processing
- **QRCodeService**: QR code generation & validation

### Data
- **ApplicationDBContext**: Entity Framework Core context
- **DbInitializer**: Seed sample data (tables, menu items)

## ??? Tech Stack

| Layer | Technology |
|-------|-----------|
| **Backend** | ASP.NET Core 8 MVC, C# 12 |
| **Database** | SQL Server with EF Core 9 |
| **Frontend** | Razor Views, Bootstrap 5, jQuery |
| **QR Codes** | QRCoder v1.6.0 |
| **State** | Server-side sessions (3-hour timeout) |

## ?? Project Structure

```
BootLeggerQRApp/
??? Controllers/           # MVC controllers
??? Models/               # Entity models
??? Services/             # Business logic
??? Views/                # Razor templates
??? Data/                 # EF Core context & seeding
??? Helpers/              # Utility classes
??? ViewModels/           # DTO models for views
??? Migrations/           # EF migrations
??? wwwroot/
    ??? css/              # Site styles
    ??? js/               # Client-side scripts
```

## ?? Usage Flow

1. **Customer arrives** ? Scans table QR code
2. **System registers table** ? Session set with TableId
3. **Browse & order** ? Add items to cart
4. **Review order** ? View items, adjust quantities
5. **Checkout** ? Select payment method
6. **Confirmation** ? Order status & receipt

## ?? Configuration

### Session Settings (Program.cs)
```csharp
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(3);
});
```

### Database (appsettings.json)
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=...;Database=BootLeggerDB;..."
}
```

## ?? Database Migrations

Create new migration:
```bash
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

## ?? Testing

Run the application and test flows:
- `/QRCode/Index` ? Table selection
- `/QRCode/Menu?tableId=1` ? Menu browsing
- `/Orders/MyOrder` ? Cart review
- `/Payment/Index` ? Payment flow
