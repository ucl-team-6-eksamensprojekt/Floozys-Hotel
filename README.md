# 🏨 Floozys Hotel - Booking Management System

<div align="center">

![Hotel Management](https://img.shields.io/badge/Hotel-Management-blue)
![WPF](https://img.shields.io/badge/WPF-.NET-purple)
![MVVM](https://img.shields.io/badge/Architecture-MVVM-green)
![Status](https://img.shields.io/badge/Status-In%20Development-yellow)

**A comprehensive hotel booking management system built with WPF and MVVM architecture**

[About](#about) • [Features](#features) • [Team](#team) • [Installation](#installation) • [Documentation](#documentation)

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

### Group 6 - UCL Eksamensprojekt

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

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/ucl-team-6-eksamensprojekt/Floozys-Hotel.git
   ```

2. **Open the solution**
   ```bash
   cd Floozys-Hotel
   start Floozys-Hotel.sln
   ```

3. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

4. **Update database connection string**
   - Open `App.config` or `appsettings.json`
   - Update the connection string to match your SQL Server instance

5. **Run the application**
   - Press `F5` in Visual Studio or run:
   ```bash
   dotnet run
   ```

---

## 📸 Screenshots

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

</div>