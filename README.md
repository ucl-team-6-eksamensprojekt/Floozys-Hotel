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

## 👥 Team

### Group 6 - UCL Eksamensprojekt

| Name | Email |
|------|-------|
| **Michael Kragh** | mikr71394@edu.ucl.dk |
| **Anna Olina Lundin** | aolu71339@edu.ucl.dk |
| **Bjarke Teglgaard Pagh** | btpa67373@edu.ucl.dk |
| **Johnny Torben Bjerring** | jtbj76616@edu.ucl.dk |
| **Nickolai Søren Christian Nygaard** | nscn71402@edu.ucl.dk |

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

## 📁 Project Structure

```
Floozys-Hotel/
├── 📂 Models/
│   ├── Booking.cs
│   ├── Guest.cs
│   ├── Room.cs
│   └── ...
├── 📂 ViewModels/
│   ├── BookingOverviewViewModel.cs
│   ├── GuestOverviewViewModel.cs
│   ├── RoomOverviewViewModel.cs
│   └── ...
├── 📂 Views/
│   ├── BookingOverviewView.xaml
│   ├── GuestOverviewView.xaml
│   ├── GuestPolicyView.xaml
│   ├── RoomOverviewView.xaml
│   ├── SalesOverviewView.xaml
│   └── ...
├── 📂 Commands/
│   └── RelayCommand.cs
├── 📂 Services/
│   └── ...
├── 📂 Data/
│   └── ...
└── 📂 Documentation/
    ├── Business Model Canvas
    ├── Business Case
    ├── BPMN Diagrams
    ├── Use Cases
    ├── Domain Model
    ├── ER Diagram
    └── ...
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

*Screenshots will be added as the project progresses*

---

## 🤝 Contributing

This is an exam project and contributions are limited to team members only.

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

**Made with ❤️ by Team 6 at UCL**

*Floozys Hotel - Your Home Away From Home*

</div>