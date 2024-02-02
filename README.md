Description
This project is a .NET Core and Angular application that demonstrates the integration of backend and frontend technologies. 

Prerequisites
.NET Core SDK (version 6.0)
Node.js 
Angular CLI 
Visual Studio and Visual Studio Code (optional but recommended)
MongoDB

Frontend
1.	Directory Structure: 
  a.	Arranged files and folders logically.
  b.	Common directories include "src" for source code, "service" for service files, “assets” for static files and "components" for UI components.
2.	Separation of Concerns: Component-based architecture ensures a clear separation between UI components, services, and business logic.
3.	Dependency Injection: Used in registering of services in components.
4.	Form Validation: Leverage Angular Reactive Forms for robust form validation and handling.

Backend
1.	Project Structure:
  •	Controllers: For API endpoints.
  •	Services: Business logic.
  •	Repositories: Data access.
  •	Models: Define data models.
  •	Middleware: Custom middleware for error handling
2.	User Authentication: Implemented user authentication for sign-in and sign-up functionalities using a secure approach like JWT (JSON Web Tokens). Use ASP.NET Core Identity authentication mechanism.
3.	MongoDB Integration: Integrated MongoDB for data storage.
4.	Error Handling and Validation: Implemented proper error handling and validation for API requests.
5.	Middleware: Used middleware for exception handling
6.	Service-Repository Pattern: 
  a.	Service: Implement methods for business logic. 
  b.	Repository: Implement methods for data access.
7.	Dependency Injection: Registered services and repositories with Dependency Injection.
8.	CORS (Cross-Origin Resource Sharing): Configure CORS settings in your backend to define which domains can access your API.
9.	All globals like connection string, gmail credentials are set up in app-settings file, that are then set to variables in startup.cs

