# Floozys Hotel Test Suite

Complete test suite for the Floozys Hotel booking management system with 129 tests covering models, view models, and repositories.

## 📊 Test Overview

- **Model Tests (46):** Validation logic for Booking, Guest, and Room entities
- **ViewModel Tests (41):** Business logic with Moq mocking framework
- **Repository Tests (42):** Database integration tests with SQL Server

**Total: 129 tests following FIRST principles & AAA pattern**

---

## ⚙️ Setup Instructions

### 1. Install NuGet Packages

The test project requires these packages (should auto-restore):

- `MSTest` (4.0.2)
- `MSTest.TestAdapter` (4.0.2)
- `MSTest.TestFramework` (4.0.2)
- `Moq` (4.20.70)
- `Microsoft.Extensions.Configuration`
- `Microsoft.Extensions.Configuration.Json`
- `Microsoft.Extensions.Configuration.FileExtensions`

**If packages are missing:**
```
Right-click on Floozys_Hotel_Tests → Manage NuGet Packages → Restore
```

### 2. Configure Database Connection

**IMPORTANT:** Create your own `appsettings.json` file!
```bash
# In the Floozys_Hotel_Tests folder:
# 1. Copy appsettingsTemplate.json to appsettings.json
# 2. Update YOUR_SERVER_NAME with your SQL Server instance name
```

**Find your SQL Server name:**
- Open SQL Server Management Studio (SSMS)
- Server name is shown in the connection dialog
- Common examples: 
  - `COMPUTERNAME\SQLEXPRESS`
  - `localhost\SQLEXPRESS`
  - `(localdb)\MSSQLLocalDB`
  - Your computer name (if using default instance)

**appsettings.json (DO NOT COMMIT THIS FILE):**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=HotelBooking;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

**Set file copy behavior:**
```
Right-click appsettings.json → Properties → Copy to Output Directory → "Copy if newer"
```

### 3. Rebuild Solution
```
Build → Clean Solution
Build → Rebuild Solution
```

### 4. Run Tests
```
Test → Run All Tests
```

**Expected result:** 129 tests passed ✅

---

## 🚨 Troubleshooting

### Issue: "The ConnectionString property has not been initialized"

**Solution:**
1. Verify `appsettings.json` exists in the test project
2. Verify "Copy to Output Directory" is set to "Copy if newer"
3. Rebuild the solution

### Issue: "Moq could not be found"

**Solution:**
```
Right-click Floozys_Hotel_Tests → Manage NuGet Packages → Browse → Install "Moq" version 4.20.70
```

### Issue: Tests fail with database errors

**Solution:**
1. Verify the `HotelBooking` database exists in your SQL Server
2. Verify the connection string in `appsettings.json` is correct
3. Test the connection in SSMS first
4. Ensure you have permissions to create/modify test data

### Issue: "Microsoft.Extensions.Configuration" not found

**Solution:**
Install the missing configuration packages:
```
1. Microsoft.Extensions.Configuration
2. Microsoft.Extensions.Configuration.Json
3. Microsoft.Extensions.Configuration.FileExtensions
```

---

## 📁 Test Structure
```
Floozys_Hotel_Tests/
├── Models/                              # Validation tests
│   ├── BookingTests.cs (15 tests)
│   ├── GuestTests.cs (15 tests)
│   └── RoomTests.cs (16 tests)
├── ViewModels/                          # Business logic tests
│   ├── BookingOverviewViewModelTests.cs (20 tests)
│   └── NewBookingViewModelTests.cs (21 tests)
├── Repositories/                        # Database integration tests
│   ├── BookingRepoTests.cs (13 tests)
│   ├── GuestRepoTests.cs (13 tests)
│   └── RoomRepoTests.cs (16 tests)
├── MSTestSettings.cs                    # Global test initialization
├── appsettingsTemplate.json             # Template (commit to repo)
├── appsettings.json                     # Personal config (DO NOT COMMIT)
└── README.md                            # This file
```

---

## 🎯 Test Principles

### FIRST Principles
- **Fast:** Tests run quickly (< 5 seconds total)
- **Independent:** Each test can run in isolation
- **Repeatable:** Tests produce consistent results
- **Self-validating:** Clear pass/fail without manual inspection
- **Timely:** Tests written alongside production code

### AAA Pattern
All tests follow the Arrange-Act-Assert pattern:
- **Arrange:** Set up test data and dependencies
- **Act:** Execute the operation being tested
- **Assert:** Verify the expected outcome

### Test Categories

**Model Tests:**
- Focus on validation logic
- Test boundary conditions
- Verify error messages
- No database dependencies

**ViewModel Tests:**
- Test business logic and commands
- Use Moq to mock repository dependencies
- Verify property changes and notifications
- Test command CanExecute logic

**Repository Tests:**
- Integration tests against real database
- Test CRUD operations
- Verify navigation properties
- Include setup and cleanup for test data

---

## 📚 Additional Documentation

For more detailed information, see:
- `START_HER.md` - Quick overview
- `QUICKSTART.md` - Detailed setup guide
- `OVERVIEW.md` - Architecture explanation
- `EXAM_CHEAT_SHEET.md` - Exam reference

---

## 🔧 Project Information

**Framework:** .NET 8.0  
**UI Framework:** WPF with MVVM pattern  
**Testing Framework:** MSTest  
**Mocking Framework:** Moq 4.20.70  
**Database:** SQL Server with ADO.NET  

---

## 📝 Notes

- The `appsettings.json` file is gitignored to prevent committing credentials
- Always use `appsettingsTemplate.json` as the base for creating your personal config
- Repository tests require a working SQL Server connection
- Model and ViewModel tests run without database dependencies
- Tests use English comments and naming conventions throughout

---

**Last Updated:** December 2024  
**Test Count:** 129  
**Success Rate:** 100%
```

---

## 📝 Sådan Tilføjer Du Det:
```
1. ✅ Right-click på "Floozys_Hotel_Tests" projektet
2. ✅ Add → New Item...
3. ✅ Vælg "Text File"
4. ✅ Navn: "README.md"
5. ✅ Click Add
6. ✅ COPY alt teksten ovenfor ⬆️
7. ✅ PASTE ind i filen
8. ✅ Save (Ctrl+S)