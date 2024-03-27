# Italia Pizza Management System

## Project Description

The "Italia Pizza" company requires the implementation of an information management system to handle product, order, user, and general catalog operations. Currently, they do not have an information system in place, and management is done through paper records, leading to poor control and excessive time spent on order reconciliation and tracking. The solution development must be carried out using current technologies with an optimized database design to avoid storing unnecessary information. Additionally, the user interface design should allow the system to operate with traditional input devices (keyboard and mouse) as well as touch input devices.

The new system is intended to be developed under a client-server architecture and installed locally as a desktop software that connects to a relational database. The proposed technologies for the new system are: WPF (Windows Presentation Foundation) for the application layer and Microsoft SQL Server for the data layer.

## Features

- **User Module:** Registration, editing, and deletion of users. Search functionality by name, address, or phone number. Login authentication.
- **Product Module:** Registration, editing, and deletion of products. Inventory management, including generating inventory reports in PDF format. Inventory validation to detect shortages or excesses.
- **Order Module:** Placement, editing, and status change of orders. Search functionality by user, date, or status.
- **Kitchen Module:** View and manage orders to be prepared. Manage recipes for each dish.
- **Finance Module:** Record income and expenses, including daily balance calculation. Manage suppliers and product orders.

## Technologies Used

- **.NET Framework:** Providing a development environment for building and running desktop applications.
- **Entity Framework:** Object-relational mapping framework used to interact with the database.
- **WPF (Windows Presentation Foundation):** Framework for building desktop applications with rich user interfaces.
- **SQL Server:** Relational database management system for data storage.

## Installation

1. Clone the repository:

```bash
git clone https://github.com/fairyofshampoo/ItaliaPizzaManagementSystem.git
```

2. Open the project in Visual Studio.

3. Configure the database connection string in the application settings.

4. Build and run the application.


## Additional Notes

- This project is developed as part of a software development course.
- Last updated: [26/03/2024]