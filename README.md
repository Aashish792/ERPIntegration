# Acumatica ERP Integration System (Login/Logout)

## Overview

The **ERP Integration System** is a .NET Core web application designed to authenticate users, retrieve customer data from an ERP system, and manage session-based authentication tokens. It interacts with an external ERP system via REST APIs to fetch customer details while maintaining security and efficiency.

## Features

- Secure authentication and token retrieval
- Fetch customer details from an ERP system
- Session-based authentication management
- Logging and error handling
- User logout functionality

---

## Project Screenshot

<img width="1424" alt="Screenshot 2025-02-24 at 8 08 41 PM" src="https://github.com/user-attachments/assets/38310aa1-ca9d-460a-8258-153097adff2d" />

<img width="1460" alt="Screenshot 2025-02-24 at 8 10 36 PM" src="https://github.com/user-attachments/assets/2453e4aa-656d-43d9-82ef-0dd1e17b1996" />


---

## Technologies Used

- **.NET Core 6+**
- **ASP.NET MVC**
- **Entity Framework Core**
- **HttpClient for API Calls**
- **Session Management**
- **Bootstrap 4 for UI**
- **JSON Serialization (System.Text.Json & Newtonsoft.Json)**

---

## Getting Started

### Prerequisites

Ensure you have the following installed:

- [.NET SDK 6+](https://dotnet.microsoft.com/en-us/download)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/)
- SQL Server (if database integration is required)
- Postman (for API testing, optional)

---

### Installation & Setup

#### 1. Clone the Repository

```sh
git clone https://github.com/your-repo/ERPIntegration.git
cd ERPIntegration
```

#### 2. Configure Environment Variables

Create an **appsettings.json** file in the project root with the following structure:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Authentication": {
    "TokenUrl": "<TokenUrl>",
    "LogoutUrl": "<LogoutUrl>",
    "CustomersUrl": "<CustomersUrl>",
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret",
    "Username": "your-username",
    "Password": "your-password",
    "GrantType": "password",
    "Scope": "api"
  },
  "AllowedHosts": "*"
}
```

---

### 3. Build and Run

Run the following commands to restore dependencies and start the application:

```sh
dotnet restore
dotnet build
dotnet run
```

The application should now be running at:

```
http://localhost:5000
```

For HTTPS:

```
https://localhost:5001
```

---

## Project Structure

```
ERPIntegration/
│── Controllers/
│   ├── HomeController.cs      # Manages authentication and customer retrieval
│── Models/
│   ├── TokenAndCustomerViewModel.cs  # Stores token and customer JSON data
│   ├── ErrorViewModel.cs             # Handles error reporting
│   ├── TokenResponse.cs              # Models authentication response
│── Services/
│   ├── IErpIntegrationService.cs      # Interface for ERP API service
│   ├── ErpIntegrationService.cs       # Implementation of ERP API service
│── Views/
│   ├── Home/
│   │   ├── Index.cshtml         # Displays customer list
│   │   ├── LogoutSuccess.cshtml # Confirms logout
│   ├── Shared/
│── wwwroot/                        # Static assets (CSS, JS, images)
│── appsettings.json                 # Application configuration
│── Program.cs                        # Application startup
│── Startup.cs                        # Dependency injection & middleware
│── README.md                         # Project documentation
```

---

## API Endpoints

### 1. **Authenticate and Retrieve Token**
- **Endpoint:** `POST /token`
- **Description:** Authenticates and retrieves an access token for API requests.
- **Response Example:**
  ```json
  {
    "access_token": "eyJhbGciOi...",
    "expires_in": 3600
  }
  ```

### 2. **Fetch Customers**
- **Endpoint:** `GET /customers`
- **Headers:**
  - `Authorization: Bearer <token>`
- **Response Example:**
  ```json
  [
    {
      "CustomerID": "C001",
      "CustomerName": "ABC Corp"
    },
    {
      "CustomerID": "C002",
      "CustomerName": "XYZ Inc"
    }
  ]
  ```

### 3. **Logout**
- **Endpoint:** `POST /logout`
- **Headers:**
  - `Authorization: Bearer <token>`
- **Response:**
  - `200 OK` on successful logout

---

## Session Management

- The **session token** is stored in `HttpContext.Session` for **30 minutes**.
- If the token is **not found**, a new one is retrieved automatically.
- Logging out **clears the session** to enforce security.

---

## Error Handling & Logging

- **All exceptions are logged** using `ILogger<HomeController>`.
- Errors display a **user-friendly page** with request IDs.
- Logs are written to **console and debug output**.

---

## Security Considerations

- **API credentials** should be stored **securely** (Environment Variables / Secret Manager).
- **Session expiration** is set to **30 minutes** for security.
- **All API requests** require authentication tokens.

---

## Contributing

We welcome contributions! Please follow these steps:

1. **Fork** the repository
2. **Create a feature branch** (`git checkout -b feature-xyz`)
3. **Commit changes** (`git commit -m "Added new feature XYZ"`)
4. **Push to GitHub** (`git push origin feature-xyz`)
5. **Submit a pull request**

