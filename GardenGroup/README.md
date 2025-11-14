# The GardenGroup

The GardenGroup is a desktop application built in **C# using WPF**, following the **MVVM (Model-View-ViewModel)** design pattern.

## Overview

This project is fully developed by me. MVVM is used because it removes hard links between the user interface (View) and the logic (ViewModel/Model), making the code cleaner, easier to maintain, and simpler to extend.

MVVM ensures that changes in the UI or logic do not directly affect each other, reducing bugs and improving overall code quality.

Almost the entire project has been rewritten and improved, except for a few parts (likely only the "Edit User" section) which remain from a previous attempt from last year.

## Key Features

- Clear separation between UI and business logic using MVVM
- Fully implemented in WPF for a modern desktop interface
- Well-structured and maintainable code for easier updates and extensions

## Test Data

To fill the database with fake data, use the following login:

- **Username:** `TEST`
- **Password:** `TEST`

## Login Credentials

- **Admin**
  - Email: `admin@example.com`
  - Password: `AdminPassword123`
  
- **Service Desk**
  - Email: `servicedesk@example.com`
  - Password: `ServiceDeskPassword123`
  
- **Employee**
  - Email: `employee@example.com`
  - Password: `EmployeePassword123`

## Notes

- Make sure to update the MongoDB connection string inside `App.config` with your username, password, and database **before running the seeder**.

## AI Assistance

- AI tools were occasionally used to help with code completion and to explore better ways of doing certain things, but every suggestion was carefully reviewed, tested, and decided by me.
