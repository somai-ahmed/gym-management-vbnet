# 🏋️ Gym Management Application

---

## Description

A Windows Forms application built in **VB.NET** that digitizes and streamlines the full management of a gym facility: members, staff, class schedules, payments, and real-time statistics.

---

## 🇫🇷 Language Note

This application is fully in **French** — including:
- All Windows Forms UI (labels, buttons, menus)
- All error and confirmation messages (MessageBox)
- All data files and field names
- All variable and method naming conventions

This was a requirement of the academic project at ISG Tunis.

## Features

### 🧑‍🤝‍🧑 Member Management
- Registration with: first name, last name, date of birth, ID number (8 digits), address, phone
- Browse members via DataGridView
- Edit personal information
- Subscription history management (` | ` separator)
- Delete a specific subscription or remove a member entirely

### 👔 Staff Management
- Add staff with role: Manager, Coach, Nutritionist, Receptionist, Technician
- Uniqueness validation on phone number and full name
- Edit and delete with confirmation dialog

### 📅 Schedule Management
- Create sessions: day, start/end time, activity, type, assigned coach
- Duration calculated automatically
- Validation: max 3 hours, end time must be after start time
- Dynamic ComboBoxes loaded from data files

### 💳 Payment Management
- Subscription types: Monthly (90 DT) · Quarterly (200 DT) · Semi-annual (350 DT) · Annual (600 DT)
- Amount auto-filled based on subscription type
- Payment methods: Bank card, Bank transfer, Cash
- Duplicate payment detection

### 📊 Dashboard (Form1)
- Total number of registered members
- Monthly revenue calculated automatically
- Navigation to all modules via MenuStrip
- Fade-out animation on close

---

##  Project Structure

```
PROJET/
├── Form1.vb                  # Main dashboard
├── Membresconsult.vb         # Read-only member list
├── Miseajourmembre.vb        # Member CRUD + subscription history
├── Staff.vb                  # Staff CRUD
├── Planning.vb               # Schedule CRUD
├── paiement.vb               # Payment CRUD + revenue calculation
├── Activities.vb             # Activity management
└── data/
    ├── membres.txt           # LastName;FirstName;DOB;ID;Address;Phone;History
    ├── personnel.txt         # LastName;FirstName;Role;Phone;0
    ├── activites.txt         # Activity;Type
    ├── planning.txt          # Day;StartTime;EndTime;Activity;Type;Duration;Coach
    └── Paiment.txt           # FullName;SubscriptionType;Amount;PaymentMethod;Date
```

### Data File Format

All data is stored in **plain text files (.txt)** with semicolon (`;`) delimiters:

```
# membres.txt
Ben Ahmed;Mohamed;15/03/1995;12345678;Tunis;98765432;Monthly | Quarterly

# planning.txt
Monday;08:00;09:30;Yoga;Group class;90;Fatma Karoui

# Paiment.txt
Mohamed Ben Ahmed;Monthly;90,00;Bank card;15/12/2025
```

---

## Tech Stack

| Component | Details |
|---|---|
| Language | VB.NET |
| Framework | .NET (Visual Studio) |
| UI | Windows Forms |
| Storage | Plain text files (.txt) |
| IDE | Visual Studio 2022 |

---

## 🚀 Getting Started

### Prerequisites
- Visual Studio 2019 or later
- .NET Framework 4.x+

### Steps

```bash
# 1. Clone the repository
git clone https://github.com/somai-ahmed/gym-management-vbnet.git

# 2. Open the solution
# Double-click PROJET.sln in Visual Studio

# 3. Build and run (F5)
# Data .txt files are created automatically on first launch
```

> ⚠️ Data files (`.txt`) are excluded from the repository via `.gitignore`. The application creates them automatically on startup if they don't exist.

---

## 📸 UI Modules

| Module | Description |
|---|---|
| **Form1** | Dashboard — key stats + MenuStrip navigation |
| **Membresconsult** | Read-only member list (DataGridView) |
| **Miseajourmembre** | Add / Edit / Delete members |
| **Staff** | Full staff management |
| **Planning** | Class scheduling with coach assignment |
| **Paiement** | Payment tracking and subscription management |

---

## ✅ Test Results

### Members Module
| Scenario | Result |
|---|---|
| Add member with valid data |  Passed |
| Duplicate ID number |  Error shown |
| Duplicate phone number |  Error shown |
| Letters typed in ID field |  Blocked |
| Delete a specific subscription |  Passed |

### Schedule Module
| Scenario | Result |
|---|---|
| End time ≤ start time |  Error shown |
| Duration > 3 hours |  Blocked |
| Coach ComboBox filtered |  Coaches only |

### Payment Module
| Scenario | Result |
|---|---|
| Auto-fill amount by type |  Passed |
| Duplicate payment |  Detected |
| Monthly revenue calculation |  Correct |

---

## 🔍 Technical Highlights

**Multi-layer validation** — KeyPress + LostFocus + pre-save check:
```vb
' Block non-numeric characters in the ID field
Private Sub txtCIN_KeyPress(sender As Object, e As KeyPressEventArgs)
    If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> Chr(8) Then
        e.Handled = True
    End If
End Sub
```

**Fade-out animation** on close:
```vb
Dim timer As New Timer()
timer.Interval = 10
AddHandler timer.Tick, Sub()
    Me.Opacity -= 0.05
    If Me.Opacity <= 0 Then
        timer.Stop()
        Application.Exit()
    End If
End Sub
timer.Start()
```

**Monthly revenue calculation** — robust parsing with regional format handling:
```vb
Dim montantStr As String = parts(2).Replace(",", ".")
Decimal.TryParse(montantStr, NumberStyles.Any,
                 CultureInfo.InvariantCulture, montant)
```

---

## 🚧 Known Limitations & Future Work

**Current limitations:**
- Text file storage has performance limits at scale
- No multi-user / concurrent access support
- Risk of data corruption on unexpected shutdown

**Planned improvements:**
- Migrate storage to SQLite or SQL Server
- Role-based authentication (Admin / Receptionist)
- PDF report export
- Graphical revenue charts
- Web or mobile version

---

> Developed by: **Ahmed Somai**
