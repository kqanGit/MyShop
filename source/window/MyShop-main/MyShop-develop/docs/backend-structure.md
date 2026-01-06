```mermaid
graph TD

    %% ===============================
    %% DATABASE (Node.js Management)
    %% ===============================
    subgraph "Database Management (Node.js)"
        Knex[Knex.js Migrations & Seeds]
        DB[(PostgreSQL)]
        Knex -->|Tạo bảng & Seed| DB
    end

    %% ===============================
    %% CLIENT (WINUI 3 - MVVM)
    %% ===============================
    subgraph "Frontend: WinUI 3 (MVVM)"
        View[View (XAML UI)]
        VM[ViewModel (C# Logic)]
        Model[Model (DTO)]
        Service[HttpClient Service]

        View <-->|Data Binding| VM
        VM -->|Sử dụng| Model
        VM -->|Gọi API| Service
    end

    %% ===============================
    %% HTTP LAYER
    %% ===============================
    Service -->|HTTP Request (JSON)| API_Layer

    %% ===============================
    %% BACKEND (CLEAN ARCHITECTURE)
    %% ===============================
    subgraph "Backend: ASP.NET Core (Clean Architecture)"
        direction TB

        API_Layer[MyShop.API\n(Controllers)]
        App_Layer[MyShop.Application\n(Interfaces + Services)]
        Domain_Layer[MyShop.Domain\n(Entities)]
        Infra_Layer[MyShop.Infrastructure\n(EF Core Repository)]

        API_Layer -->|Gọi| App_Layer
        API_Layer -->|Truy cập| Infra_Layer

        App_Layer -->|Dùng| Domain_Layer

        Infra_Layer -->|Implement| App_Layer
        Infra_Layer -->|Dùng| Domain_Layer
    end

    %% ===============================
    %% DATABASE <-> BACKEND
    %% ===============================
    Infra_Layer <-->|Query / Save\n(EF Core)| DB


```