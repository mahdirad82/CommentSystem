# CommentSystem

A modular comment system for hotel bookings, built with ASP.NET Core 9 and clean architecture.

## Architecture

This project follows a clean architecture pattern, separating concerns into distinct layers:

*   **CommentSystem.Api:** The entry point of the application, containing ASP.NET Core controllers, API configurations, and presentation logic.
*   **CommentSystem.Application:** Contains application-specific business logic, DTOs (Data Transfer Objects), interfaces for services, and AutoMapper profiles. It orchestrates interactions between the UI/API and the domain/infrastructure layers.
*   **CommentSystem.Domain:** The core of the application, holding enterprise-wide business rules, entities (e.g., `Comment`, `Booking`), and enums. This layer is independent of any technology.
*   **CommentSystem.Infrastructure:** Implements interfaces defined in the `Application` layer, providing concrete implementations for data access (Entity Framework Core, Repositories) and external services.

## Technologies Used

*   **ASP.NET Core:** Web framework for building the API.
*   **Entity Framework Core:** ORM for data access, using SQLite as the database.
*   **SQLite:** Lightweight, file-based relational database.
*   **AutoMapper:** For object-to-object mapping between entities and DTOs.

## Getting Started

### Prerequisites

*   .NET SDK (version 9.0 or later)

### Running the Application

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/mahdirad82/CommentSystem.git
    cd CommentSystem
    ```
2.  **Restore dependencies:**
    ```bash
    dotnet restore
    ```
3.  **Build the solution:**
    ```bash
    dotnet build
    ```
4.  **Run the API project:**
    ```bash
    dotnet run --project CommentSystem.Api
    ```

    The API will typically run on `http://localhost:5000` (or a similar port).

5.  **Access Swagger UI:**
    Once the API is running, you can access the Swagger UI for interactive API documentation and testing at `http://localhost:5000/swagger`.

### Database Setup

The application uses SQLite, and the database file (`commentSystem.db`) will be created automatically in the `Data` directory at the project root when the application runs for the first time and attempts to access the database.

## API Endpoints

The API provides endpoints for managing comments:

*   **Hotel Comments:**
    *   `GET /api/hotels/{hotelId}/comments`: Retrieve approved comments for a specific hotel.
*   **Admin Comments:**
    *   `GET /api/admin/comments`: Get all system comments (admin access required).
    *   `PUT /api/admin/comments/{id}/status`: Update the status of a comment (admin access required).
*   **User Comments:**
    *   `POST /api/user/comments`: Create a new comment (user access required).
    *   `GET /api/user/comments`: Get comments made by the current user (user access required).

