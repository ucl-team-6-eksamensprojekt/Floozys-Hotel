# Azure SQL Database Setup Instructions

## Prerequisites
- Azure SQL Database already created: `g6hotelbooking.database.windows.net`
- Database: `HotelBooking`
- All necessary NuGet packages installed ✅

## Step 1: Execute SQL Scripts

Run the following SQL scripts on your Azure SQL Database in order:

### Option A: Using Azure Portal Query Editor
1. Open [Azure Portal](https://portal.azure.com)
2. Navigate to your SQL Database: `HotelBooking`
3. Click on "Query editor (preview)" in the left sidebar
4. Log in with:
   - **User:** `AdminGroup6`
   - **Password:** `XQaR$65#6izh8B3`
5. Copy and paste `Database/01_CreateSchema.sql` into the editor
6. Click "Run" to execute
7. Repeat for `Database/02_InsertTestData.sql`

### Option B: Using SQL Server Management Studio (SSMS)
1. Open SSMS
2. Connect to: `g6hotelbooking.database.windows.net`
3. Database: `HotelBooking`
4. Credentials: `AdminGroup6` / `XQaR$65#6izh8B3`
5. Open `Database/01_CreateSchema.sql` and execute (F5)
6. Open `Database/02_InsertTestData.sql` and execute (F5)

### Option C: Using Azure Data Studio
1. Open Azure Data Studio
2. Create new connection:
   - **Server:** `g6hotelbooking.database.windows.net`
   - **Authentication:** SQL Login
   - **User:** `AdminGroup6`
   - **Password:** `XQaR$65#6izh8B3`
   - **Database:** `HotelBooking`
3. Open and execute both SQL scripts

## Step 2: Verify Connection

After executing the scripts, the application should automatically connect to the database when you run it.

### Test the connection:
```bash
dotnet run
```

The calendar should now display bookings from the Azure SQL Database!

## Database Schema

### Tables Created:
- **ROOM** (8 test rooms)
  - RoomID, RoomNumber, Floor, RoomSize, Capacity, Status
- **GUEST** (5 test guests)
  - GuestID, FirstName, LastName, PassportNumber, Email, Country, PhoneNumber
- **BOOKING** (7 test bookings)
  - BookingID, StartDate, EndDate, CheckInTime, CheckOutTime, Status, RoomID, GuestID

### Enum Mappings:
- **RoomStatus:** 0=Available, 1=OutOfService, 2=Maintenance
- **BookingStatus:** 0=Pending, 1=Confirmed, 2=CheckedIn, 3=CheckedOut, 4=Cancelled

## Troubleshooting

### Connection Issues:
1. Verify Azure SQL firewall allows your IP address
2. Check connection string in `App.config`
3. Test connection using `DatabaseConfig.TestConnectionWithDetails()`

### Build Issues:
- Ensure `Microsoft.Data.SqlClient` is installed
- Ensure `System.Configuration.ConfigurationManager` is installed

## Security Note
⚠️ **IMPORTANT:** The connection string currently contains the password in `App.config`. For production or when sharing code on GitHub, use one of these alternatives:

1. **Environment Variables:**
   ```bash
   setx HOTEL_BOOKING_CONN_STRING "Server=tcp:g6hotelbooking..."
   ```

2. **User Secrets** (recommended for development):
   ```bash
   dotnet user-secrets set "ConnectionStrings:HotelBooking" "Server=tcp:g6hotelbooking..."
   ```

3. **Azure Key Vault** (recommended for production)

## Repository Methods Available

### RoomRepo:
- `GetAllRooms()` - Get all rooms
- `GetRoomById(int)` - Get specific room
- `AddRoom(Room)` - Add new room
- `UpdateRoom(Room)` - Update existing room
- `DeleteRoom(int)` - Delete room

### GuestRepo:
- `GetAllGuests()` - Get all guests
- `GetGuestById(int)` - Get specific guest
- `AddGuest(Guest)` - Add new guest
- `UpdateGuest(Guest)` - Update existing guest
- `DeleteGuest(int)` - Delete guest

### BookingRepo:
- `GetAllBookings()` - Get all bookings
- `GetBookingById(int)` - Get specific booking
- `GetBookingsByRoomId(int)` - Get bookings for a room
- `AddBooking(Booking)` - Add new booking
- `UpdateBooking(Booking)` - Update existing booking
- `DeleteBooking(int)` - Delete booking

## Files Created
```
Database/
  ├── 01_CreateSchema.sql       # Creates tables and indexes
  ├── 02_InsertTestData.sql     # Inserts test data
  └── DatabaseConfig.cs          # Connection string management

Floozys Hotel/
  ├── App.config                 # Connection string configuration
  └── Repositories/
      ├── RoomRepo.cs            # Updated with Azure SQL
      ├── GuestRepo.cs           # Updated with Azure SQL
      └── BookingRepo.cs         # Updated with Azure SQL
```
