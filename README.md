# Orqa Skill Assessment Project

## Overview
This project is a solution for the Orqa skill assessment task. It is a desktop application built using Avalonia and C#, designed to manage users and work positions. The application allows administrators to:

- Create new users
- Create new work positions
- Link users with work positions to define user work positions

The app demonstrates key concepts in C#, MVVM architecture, and database integration.

---

## Prerequisites
To run this project, ensure you have the following:

1. **Visual Studio 2022 or Newer**:
   - Download and install [Visual Studio](https://visualstudio.microsoft.com/).
   - Ensure you have the .NET Desktop Development workload installed.

2. **Database Setup**:
   - MySQL server installed and running.
   - A database named `workstationdb` created.
   - The SQL script `workstationdb.sql` located in the root of the repository imported into the database.

---

## Getting Started

### 1. Clone the Repository
Clone the project repository to your local machine:
```bash
git clone https://github.com/Zokky2e/Orqa-Application.git
```

### 2. Open the Project in Visual Studio
1. Open Visual Studio.
2. Click **File > Open > Project/Solution**.
3. Navigate to the cloned repository and select the project solution file (`.sln`).

### 3. Build the Project
Before running the application:
1. Go to **Build > Build Solution** or press `Ctrl+Shift+B`.
2. Ensure there are no build errors.

### 4. Set Up the Database
1. Open your MySQL client or phpMyAdmin.
2. Create a new database named `workstationdb`:
   ```sql
   CREATE DATABASE workstationdb;
   ```
3. Import the `workstationdb.sql` script located in the root of the repository to create the necessary tables and populate the admin user.
   ```bash
   mysql -u <username> -p workstationdb < workstationdb.sql
   ```

### 5. Run the Application
1. In Visual Studio, press `F5` or click **Start** to run the application.
2. The login window will load as the initial screen.

---

## Using the Application

### Logging In
Use the following admin credentials to log in:
- **Username**: `admin`
- **Password**: `admin1234`

### Features
1. **Admin Dashboard**:
   - View existing users and their work positions.
   - Add new users and work positions.
   - Assign work positions to users or update their existing positions.

2. **User Management**:
   - Add new users by filling out their details (username, first name, last name, password).

3. **Work Position Management**:
   - Add new work positions by providing a name and description.

4. **User Work Position Management**:
   - Link users to specific work positions or remove existing assignments.

---

## Notes
- The database structure is defined in the `workstationdb.sql` file. Ensure this script is executed to set up the database correctly.
- All user passwords are hashed using bcrypt for security.
- This project was built and tested using Avalonia and .NET 6.

---

## Troubleshooting

### Common Issues
1. **Database Connection Error**:
   - Ensure your MySQL server is running and the database `workstationdb` exists.
   - Verify the connection string in the project configuration matches your MySQL setup.

2. **Build Errors**:
   - Ensure you have Visual Studio 2022 or newer installed.
   - Verify that the required .NET SDKs and workloads are installed.

3. **Login Issues**:
   - Ensure you imported the `workstationdb.sql` script correctly.
   - Check the admin credentials (`admin` / `admin1234`).

### Logs
Errors and logs can be found in the application output in Visual Studio while debugging.

---

## Future Improvements
- Enhance validation and error handling.
- Add a feature to export user and work position data to a file.
- Add a feature for users to request work positions and products
---
