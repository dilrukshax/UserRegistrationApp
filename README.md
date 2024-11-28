# **User Registration and Login System using Blazor, ASP.NET Core, and Entity Framework**

## **Project Overview**

This project is a **User Registration and Login System** built using **Blazor**, **ASP.NET Core**, and **Entity Framework Core**. It allows users to register, securely store their credentials using password hashing (BCrypt), and log in with their credentials. The backend is implemented using ASP.NET Core API endpoints, while the frontend is developed using Blazor components.

### **Key Features**
- **User Registration**: Register with a username, email, and password. Passwords are securely hashed before being stored in the database.
- **User Login**: Validate credentials by comparing the entered password with the securely hashed password in the database.
- **Data Access**: Leverages Entity Framework Core for seamless database interactions and management of user data.

---

## **Technologies Used**

### Core Technologies:
![Blazor](https://img.shields.io/badge/Blazor-512BD4?style=for-the-badge&logo=blazor&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/Entity_Framework_Core-512BD4?style=for-the-badge&logo=.net&logoColor=white)

### Additional Technologies:
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![GitHub](https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white)

---

## **Project Structure**

### **Backend Structure (ASP.NET Core API)**

```
WebApi/
├── Controllers/
│   └── AccountController.cs     # Handles User registration and login API endpoints
├── Models/
│   ├── UserProfile.cs           # User model with Email and Password properties
├── Services/
│   └── ApplicationDbContext.cs  # Contains DbContext for database interactions
├── Migrations/                  # Database migrations for EF Core
├── appsettings.json             # Configuration for database connection and settings
├── Program.cs                   # Configures services, middleware, and endpoints
└── Properties/                  # Project settings and metadata
```

### **Frontend Structure (Blazor Web Application)**

```
Frontend/
├── Pages/
│   ├── Auth/
│   │   ├── Register.razor       # User registration page
│   │   └── Login.razor          # User login page
│   ├── home.razor               # Home page
│   └── profile.razor            # User profile page
├── Components/                  # Reusable Blazor components
│   ├── mainlayout.razor         # Main layout
│   └── NavMenu.razor            # Navigation menu
├── Models/                      # DTOs and models
│   ├── RegisterDto.cs           # DTO for registration form data
│   └── UserProfile.cs           # Represents user profile
├── wwwroot/                     # Static files
│   ├── css/
│   │   └── app.css              # Styles for the frontend
│   └── js/
│       └── app.js               # JavaScript (if any)
├── Services/                    # Custom Blazor services
│   └── CustomAuthStateProvider.cs # Handles authentication state
├── _Imports.razor               # Common namespace imports
├── app.razor                    # Main entry point
└── Program.cs                   # Configures Blazor services and components
```

---

## **Setup Instructions**

Follow these steps to set up and run the project locally.

### **1. Clone the Repository**

Clone the repository to your local machine:

```bash
git clone https://github.com/your-username/UserRegistrationApp.git
cd UserRegistrationApp
```

### **2. Install Dependencies**

Ensure you have the **.NET SDK** and **Entity Framework Core** installed. Run the following command to restore dependencies:

```bash
dotnet restore
```

### **3. Configure the Database**

The project uses **SQL Server** for user data storage. Update the connection string in `appsettings.json` as needed. The default connection string uses a local SQL Server instance:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=UserRegistrationDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

Modify this if you are using a different database server.

### **4. Apply Migrations**

Set up the database schema using EF Core migrations. Run the following commands:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### **5. Run the Application**

Start the application:

```bash
dotnet run
```

The backend and frontend services will be hosted locally.

---

## **Contributing**

Contributions are welcome. You can contribute by:

- Fixing bugs
- Adding new features
- Improving documentation
- Suggesting enhancements

### **How to Contribute**

1. Fork this repository.
2. Clone your fork locally.
3. Create a new branch for your feature or fix:
   ```bash
   git checkout -b feature-branch-name
   ```
4. Commit your changes with a descriptive message:
   ```bash
   git commit -m "Add feature description"
   ```
5. Push your changes to your fork:
   ```bash
   git push origin feature-branch-name
   ```
6. Open a pull request to the main repository.

---

## **Licenses and Acknowledgements**

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

Acknowledgements:
- **BCrypt**: For secure password hashing.
- **Entity Framework Core**: ORM for .NET applications.
- **Blazor**: For building interactive UIs with C#.

---

## **Contact**

For inquiries or support, feel free to open an issue on the repository or contact me directly:

- **Email**: dilndilruksha0@gmail.com
- **GitHub**: [@dilrukshax](https://github.com/dilrukshax)

---

This updated version includes badges for the technologies used in the project, providing a more visually engaging and professional look. Let me know if you need further refinements!
