# 🏨 Floozys Hotel - Booking Management System

<div align="center">

![Hotel Management](https://img.shields.io/badge/Hotel-Management-blue)
![WPF](https://img.shields.io/badge/WPF-.NET-purple)
![MVVM](https://img.shields.io/badge/Architecture-MVVM-green)
![Status](https://img.shields.io/badge/Status-In%20Development-yellow)

**A comprehensive hotel booking management system built with WPF and MVVM architecture**

[About](#about) • [Problem & Solution](#-problem-statement--solution) • [Features](#-features) • [Scope](#-scope--limitations) • [Installation](#-installation) • [Documentation](#-documentation)
</div>

---

## 📋 About

**Floozys Hotel** is a modern hotel booking management system developed as an exam project for the Computer Science AP program at University College Lillebælt (UCL), Denmark.

The system is designed to streamline hotel operations by providing an intuitive interface for managing bookings, guests, rooms, and sales data.

### 📅 Project Timeline

| Milestone | Date |
|-----------|------|
| Problem Statement Sent | October 21, 2025 |
| Problem Statement Approved | October 23, 2025 |
| Project Start | November 3, 2025 |
| Project Delivery | December 19, 2025 |
| Oral Examination | January 15, 2026 |

## 🎯 Problem Statement & Solution

### The Problem
Floozys Hotel in Phnom Penh, Cambodia currently manages bookings through a **manual, fragmented system**:

- Bookings scattered across email, Agoda notifications, and walk-in guests
- No centralized overview of room availability
- High risk of double-bookings
- Time-consuming manual calendar updates
- Inefficient communication between reception and housekeeping

### Our Solution
A **Windows desktop booking management system** that provides:

- Centralized calendar view (day/week/month) of all bookings
- Real-time room availability checking
- Prevention of double-bookings through validation
- Guest information management with passport details
- Booking status tracking (Pending → Confirmed → Checked In → Checked Out)
- Room management across 3 floors (4 small + 6 large rooms)

---
### What's NOT Included (Out of Scope)

#### For this version (December 2025 delivery)
- **No payment processing** - Payment tracking is manual (marked paid/unpaid by staff)
- **No Agoda API integration** - Bookings from Agoda must be entered manually
- **No web-based interface** - Desktop application only (WPF)
- **No email notifications** - No automatic emails to guests
- **No online booking portal** - Guests cannot book directly through the system
- **No multi-user/role management** - Basic access control only
- **No real-time synchronization** - Single-machine database
- **No mobile app** - Desktop only
- **No food/restaurant management** - Booking system only
- **No invoice generation** - Accounting reports are manual

#### Why these limitations?
- **Academic constraints** - 2nd semester project scope (6 weeks development time)
- **Learning objectives** - Focus on MVVM, Clean Architecture, and SOLID principles
- **Technical complexity** - API integration and web development are outside curriculum
- **Prototype approach** - This is a proof-of-concept for testing usability

---

## 🛠️ Technology Stack

| Category | Technology |
|----------|------------|
| **Framework** | WPF (.NET) |
| **Language** | C# |
| **Architecture** | MVVM |
| **Database** | SQL Server |
| **Version Control** | Git / GitHub |
| **Methodology** | SCRUM |

---

## ✨ Features

### Core Functionality

- 📅 **Booking Management** - Create, view, and manage hotel reservations
- 👥 **Guest Management** - Track guest information and history
- 🛏️ **Room Management** - Monitor room availability and status
- 📊 **Sales Overview** - View revenue and booking statistics
- 📜 **Guest Policies** - Manage hotel policies and rules

### Technical Features

- 🎨 Clean and modern user interface
- 🏗️ MVVM (Model-View-ViewModel) architecture
- 🔄 SCRUM development methodology
- 💾 SQL Server database integration
- 📱 Responsive WPF design


### Quality Assurance
- **SOLID Principles** - Applied throughout codebase
- **GRASP Patterns** - Information Expert, Creator, Controller, Low Coupling, High Cohesion
- **Code Reviews** - Peer review through GitHub pull requests
- **Testing Strategy** - Unit tests for models, repositories, and ViewModels
- **Normalization** - Database in 3rd Normal Form (3NF)

---

## ⚠️ Known Limitations & Constraints

### Technical Limitations
- **Windows Only** - Requires Windows OS with .NET framework
- **Single Database** - No distributed/cloud database support
- **Manual Data Entry** - Agoda bookings must be entered by hand
- **Local Network Only** - No remote access capability
- **No Backup System** - Manual database backup required

### Business Process Limitations
- **No Financial Integration** - Separate accounting system required
- **Manual Payment Tracking** - Staff must mark bookings as paid
- **Limited Scalability** - Designed for 10 rooms (not 100+)
- **English Interface Only** - No localization/translation support

---


## 🧾 Requested Changes from Floozys Hotel

During development, the hotel owner requested a set of improvements to better match their real-world workflow in Phnom Penh. Yet to be made!

### ✅ Main Requests
- **Passport details with image upload**
  - Add a passport ID field and allow uploading an image of the guest’s passport.
  - Keep booking + passport ID + passport image **in one place** for easy check-in.

- **Walk-in guest support**
  - Remove the email requirement in *New Booking*, because walk-in guests often do not have an email address.

- **Better calendar usability**
  - Highlight the clicked/selected booking in the calendar.
  - Add a clear **color legend** explaining what each booking status means in the calendar.

- **Guest overview improvements**
  - Add a feature to show the guest’s assigned room number (guest ↔ room traceability).

- **Special pricing & discounts**
  - Add discount pricing for rooms **8, 9, and 10**.
  - Ensure the pricing changes are reflected in both **Edit Price** and **Sales Overview**.

### 🌐 Future Goal (Next Version)
- Convert the system into a **web-based solution with an API** for website bookings and platform integrations.

---

## 👥 Team

### Group 6 - UCL Odense Computer Science AP project

---

## 📁 Project Structure

```
Floozys-Hotel/
│
├── 📦 Floozys Hotel/                          # Main WPF Application
│   │
│   ├── 📁 Assets/
│   │   ├── 📁 Images/
│   │   │   ├── BookingCalendar.png
│   │   │   ├── NewBooking.png
│   │   │   └── EditBooking.png
│   │   └── 📁 Logo/
│   │       └── Logo_1-1.png
│   │
│   ├── 📁 Commands/
│   │   └── RelayCommand.cs
│   │
│   ├── 📁 Converters/
│   │   ├── BookingLeftMarginConverter.cs
│   │   ├── BookingWidthConverter.cs
│   │   ├── BookingStatusColorConverter.cs
│   │   ├── CheckInStatusConverter.cs
│   │   ├── DateHeaderConverter.cs
│   │   ├── InverseBoolConverter.cs
│   │   ├── NotNullToBoolConverter.cs
│   │   └── RoomBookingsConverter.cs
│   │
│   ├── 📁 Core/
│   │   ├── BindingProxy.cs
│   │   └── ObservableObject.cs
│   │
│   ├── 📁 Database/
│   │   ├── DatabaseConfig.cs
│   │   └── 📁 SQL_Scripts/
│   │       ├── Database_Schema.sql
│   │       ├── Fix_RoomNumber_Type.sql
│   │       ├── Fix_BookingID_Identity.sql
│   │       ├── Room_StoredProcedures.sql
│   │       └── TestData_Generator.sql
│   │
│   ├── 📁 Models/
│   │   ├── Booking.cs
│   │   ├── BookingStatus.cs
│   │   ├── Guest.cs
│   │   ├── Room.cs
│   │   └── RoomStatus.cs
│   │
│   ├── 📁 Repositories/
│   │   ├── 📁 Interfaces/
│   │   │   ├── IBookingRepo.cs
│   │   │   ├── IGuestRepo.cs
│   │   │   └── IRoomRepo.cs
│   │   ├── BookingRepo.cs
│   │   ├── GuestRepo.cs
│   │   └── RoomRepo.cs
│   │
│   ├── 📁 Validation/
│   │   └── DateGreaterThanAttribute.cs
│   │
│   ├── 📁 ViewModels/
│   │   ├── 📁 FormsViewModel/
│   │   │   └── RoomFormViewModel.cs
│   │   ├── BookingOverviewViewModel.cs
│   │   ├── GuestOverviewViewModel.cs
│   │   ├── GuestPolicyViewModel.cs
│   │   ├── MainViewModel.cs
│   │   ├── NewBookingViewModel.cs
│   │   ├── NewGuestViewModel.cs
│   │   ├── RoomOverviewViewModel.cs
│   │   └── SalesOverviewViewModel.cs
│   │
│   ├── 📁 Views/
│   │   ├── 📁 Forms/
│   │   │   ├── RoomFormView.xaml
│   │   │   └── RoomFormView.xaml.cs
│   │   ├── BookingOverviewView.xaml
│   │   ├── BookingOverviewView.xaml.cs
│   │   ├── GuestOverviewView.xaml
│   │   ├── GuestOverviewView.xaml.cs
│   │   ├── GuestPolicyView.xaml
│   │   ├── GuestPolicyView.xaml.cs
│   │   ├── NewBookingView.xaml
│   │   ├── NewBookingView.xaml.cs
│   │   ├── NewGuestView.xaml
│   │   ├── NewGuestView.xaml.cs
│   │   ├── RoomOverviewView.xaml
│   │   ├── RoomOverviewView.xaml.cs
│   │   ├── SalesOverviewView.xaml
│   │   └── SalesOverviewView.xaml.cs
│   │
│   ├── 📁 Theme/
│   │   ├── DataGridStyles.xaml
│   │   ├── MenuButtonTheme.xaml
│   │   └── MenuDropShadowTheme.xaml
│   │
│   ├── App.xaml
│   ├── App.xaml.cs
│   ├── AssemblyInfo.cs
│   ├── MainWindow.xaml
│   ├── MainWindow.xaml.cs
│   ├── appsettings.json (🔒 NOT in Git)
│   ├── appsettings.EXAMPLE.json
│   └── Floozys Hotel.csproj
│
├── 🧪 Floozys Hotel.Tests/                    # MS Test Project 128 Green Tests
│   │
│   ├── 📁 Models/
│   │   ├── ✅ BookingTests.cs (23 tests)
│   │   ├── ✅ GuestTests.cs ()
│   │   └── ✅ RoomTests.cs ()
│   │
│   ├── 📁 Repositories/
│   │   ├── ✅ BookingRepoTests.cs (~16 tests)
│   │   ├── ✅ GuestRepoTests.cs (~10 tests)
│   │   └── ✅ RoomRepoTests.cs (~10 tests)
│   │
│   ├── 📁 ViewModels/
│   │   ├── ✅ NewBookingViewModelTests.cs ()
│   │   └── ✅ BookingOverviewViewModelTests.cs ()
│   │
│   └── Floozys Hotel.Tests.csproj
│
├── 📁 Documentation/
│   ├── Business Model Canvas.pdf
│   ├── BPMN Diagrams.pdf
│   ├── Use Cases.pdf
│   ├── Domain Model.pdf
│   ├── SSD.pdf
│   ├── Operation Contracts.pdf
│   ├── Sequence Diagrams.pdf
│   ├── Design Class Diagrams.pdf
│   ├── ER Diagram.pdf
│   └── Wireframes.pdf
│
├── README.md
├── .gitignore
└── Floozys Hotel.sln
```

---

## 📚 Documentation

### Project Artifacts

| Document | Description | Priority |
|----------|-------------|----------|
| **Business Model Canvas** | Business overview and value proposition | P0 |
| **Business Case** | Project justification and benefits | - |
| **BPMN Diagrams** | Business process workflows | - |
| **Use Cases** | System functionality descriptions | - |
| **Domain Model** | Core business entities | - |
| **Object Model** | System object relationships | - |
| **SSD** | System Sequence Diagrams | - |
| **User Flow** | User interaction paths | - |
| **Wireframes** | UI mockups and designs | - |
| **Operation Contracts** | System operation specifications | - |
| **Sequence Diagrams** | Interaction sequences | - |
| **DCD** | Design Class Diagrams | - |
| **RDBMS** | Database design | - |
| **ER Diagram** | Entity Relationship Diagram | - |

---

## 🚀 Installation

### Prerequisites

- Visual Studio 2022 or later
- .NET 6.0 or later
- SQL Server (LocalDB or full installation)
- Git (for cloning the repository)

### Quick Start Guide

#### Step 1: Clone the Repository
```bash
git clone https://github.com/ucl-team-6-eksamensprojekt/Floozys-Hotel.git
cd Floozys-Hotel
```

#### Step 2: Open Solution in Visual Studio
- Open `Floozys Hotel.sln` in Visual Studio 2022
- Or use command line:
```bash
  start "Floozys Hotel.sln"
```

#### Step 3: Restore NuGet Packages
Visual Studio will automatically restore packages when opening the solution.

Alternatively, restore manually:
```bash
dotnet restore
```

#### Step 4: Configure Database Connection
1. Locate `appsettings.json` in the main project
2. Update the connection string to match your SQL Server instance:
```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FloozysHotel;Trusted_Connection=True;"
     }
   }
```
3. Save the file

#### Step 5: Initialize Database
1. Navigate to `/Floozys Hotel/Database/SQL_Scripts/`
2. Execute the following scripts in order:
   - `Database_Schema.sql` - Creates tables and structure
   - `TestData_Generator.sql` - (Optional) Adds sample bookings and rooms

**Using SQL Server Management Studio (SSMS):**
- Connect to your SQL Server instance
- Open each script file
- Execute (F5)

#### Step 6: Build and Run
**Option A: Using Visual Studio**
- Press `F5` to build and run with debugging
- Or press `Ctrl+F5` to run without debugging

**Option B: Using Command Line**
```bash
dotnet build
dotnet run
```

### First-Time Setup
After launching the application for the first time:
1. The main menu will appear
2. Navigate to "Room Overview" to verify the 10 rooms are loaded
3. Navigate to "Booking Overview" to see the calendar view
4. Click "New Booking" to create your first reservation

---

### Troubleshooting

#### Database Connection Issues
- Verify SQL Server is running
- Check connection string in `appsettings.json`
- Ensure database "FloozysHotel" exists

#### Build Errors
- Ensure all NuGet packages are restored
- Clean solution: `Build > Clean Solution`
- Rebuild: `Build > Rebuild Solution`

#### Missing Tables/Data
- Re-run `Database_Schema.sql`
- Verify script execution completed without errors


---

## 📸 Screenshots of Program

### Booking Calendar Overview
![Booking Calendar](./Floozys%20Hotel/Assets/Images/BookingCalendar.png)

### New Booking Window
![New Booking](./Floozys%20Hotel/Assets/Images/NewBooking.png)

### Edit Booking Window
![Edit Booking](./Floozys%20Hotel/Assets/Images/EditBooking.png)

---

## 🤝 Contributing

This is an exam project and contributions are limited to team members and Floozys Hotel only.

---

## 📄 License

This project is developed for educational purposes as part of the Computer Science AP program at UCL.

---

## 🎓 Academic Information

- **Institution:** University College Lillebælt (UCL)
- **Program:** Computer Science AP (Datamatiker)
- **Semester:** 2nd Semester
- **Course:** Exam Project
- **Year:** 2025/2026

---

<div align="center">

**Made with ❤️ by Team 6 at UCL @ 2025**

*Floozys Hotel - Your Home Away From Home*

**[⬆ Back to Top](#-floozys-hotel---booking-management-system)**

</div>