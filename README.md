# 🚀 CRM Warranty Microservices Platform

A backend platform built with **.NET 8 Web API** for managing warranty claims, customers, employees, and products.

The system is designed using a **Microservices Architecture**, where each service owns its own API, business logic, and PostgreSQL database. This approach allows services to be developed, maintained, and deployed independently.

To improve warranty claim processing, the platform integrates with **Groq LLM (Llama 3.3 70B Versatile)** to automatically classify customer complaints and assign severity levels based on predefined business rules.

---

# 📌 Project Overview
<img width="959" alt="image" src="https://github.com/salisKingdoms/CRM_Proj/assets/149958647/93dd0ec7-7923-48ec-a061-9a3fd3e4e45c">

The platform consists of four domain-focused services.

| Service           | Description                                                           |
| ----------------- | --------------------------------------------------------------------- |
| 🛠️ WS_CRM        | Warranty claims, ticket management, monitoring, and AI classification |
| 📦 WS_Catalog     | Product and spare part management                                     |
| 👤 WS_Customer    | Customer management                                                   |
| 🧑‍💼 WS_Employee | Employee and sales management                                         |

Each service:

* Has its own API
* Has its own PostgreSQL database
* Contains its own business logic
* Can be deployed independently
* Maintains clear ownership of its domain

---

# 🏗️ Architecture

This project follows a Microservices Architecture with a Database-per-Service pattern.

```text
                         Client Applications
                                  │
                                  ▼

 ┌─────────────────────────────────────────┐
 │              Microservices              │
 └─────────────────────────────────────────┘

        ┌──────────────┐
        │   WS_CRM     │ ─────► CRM_DB
        └──────────────┘

        ┌──────────────┐
        │ WS_Customer  │ ─────► CUSTOMER_DB
        └──────────────┘

        ┌──────────────┐
        │ WS_Employee  │ ─────► EMPLOYEE_DB
        └──────────────┘

        ┌──────────────┐
        │ WS_Catalog   │ ─────► CATALOG_DB
        └──────────────┘
```

For development convenience, all services are maintained in a single repository while preserving service boundaries and independent deployment capability.

---

# 📂 Project Structure

Example structure inside a service:

```text
Feature
│
├── Activity
│   ├── dao
│   │   ├── ActivityRepo.cs
│   │   ├── IActivityRepo.cs
│   │   └── ActivityService.cs
│   │
│   ├── dto
│   │   ├── Request DTOs
│   │   └── Response DTOs
│   │
│   ├── Model
│   │
│   └── ActivityController.cs
│
├── Config
├── Helper
├── Program.cs
└── appsettings.json
```

---

# ⚙️ Layer Responsibilities

### Controller

Responsible for:

* API endpoints
* Request handling
* Response formatting

### Service

Responsible for:

* Business logic
* Validation
* Transaction processing
* AI integration

### Repository (DAO)

Responsible for:

* Database access
* Data persistence
* Query execution

### DTO

Responsible for:

* Request contracts
* Response contracts

---

# 🔐 Security

The platform uses JWT Bearer Authentication.

Features:

* JWT Authentication
* Protected API Endpoints
* Swagger Authorization Support

Authenticated users must provide a valid token before accessing secured endpoints.

---

# 🤖 AI-Powered Warranty Classification

The CRM service integrates with **Groq LLM API** using **Llama 3.3 70B Versatile** to assist with warranty complaint classification.

Instead of allowing free-form AI responses, the model is constrained using predefined business rules and structured prompts.

### Classification Categories

The AI can only classify complaints into one of the following categories:

* Hardware Failure
* Software Issue
* Installation Problem
* User Error
* Cosmetic Damage

### Severity Levels

The AI can only assign one of the following severity levels:

* Low
* Medium
* High
* Critical

### Controlled AI Behavior

To improve consistency and reduce random outputs:

* Temperature is set to **0**
* Categories are predefined
* Severity levels are predefined
* AI returns structured JSON only
* System prompts restrict the model's response format

The model's responsibility is limited to classification based on the complaint text provided by the user.

### Example

Input:

```json
{
  "complaint": "Machine produces smoke and cannot start."
}
```

Output:

```json
{
  "category": "Hardware Failure",
  "severity": "Critical"
}
```

This approach helps maintain predictable AI responses while supporting support teams in prioritizing warranty cases.

---

# 🔥 Core Features

## 🛠️ WS_CRM

* Warranty Activation
* Warranty Claim Submission
* Ticket Management
* Claim Monitoring
* AI Complaint Classification
* Severity Assessment

## 📦 WS_Catalog

* Product Management
* Product Lookup

## 👤 WS_Customer

* Customer Registration
* Customer Profile Management
* Customer Lookup

## 🧑‍💼 WS_Employee

* Employee Management
* Sales Information Management
* Employee Lookup

---

# 🛠️ Technology Stack

### Backend

* .NET 8
* ASP.NET Core Web API
* C#

### Database

* PostgreSQL

### Authentication

* JWT Bearer Authentication

### AI Integration

* Groq API
* Llama 3.3 70B Versatile
* Structured JSON Responses
* Temperature 0 Configuration

### Documentation

* Swagger / OpenAPI

### Tools

* Visual Studio Code
* Git
* GitHub

---

# ⭐ Technical Highlights

* .NET 8 Web API
* Microservices Architecture
* Database-per-Service Pattern
* Independent Service Deployment
* PostgreSQL
* Layered Architecture
* Controller → Service → Repository Pattern
* DTO Pattern
* JWT Authentication
* Swagger Documentation
* Groq LLM Integration
* Controlled AI Classification Workflow
* Structured JSON AI Responses
* Separation of Business Logic and API Layer

---

# 🚀 Deployment

Each service is designed to be deployed independently.

Since every service owns its own database and business logic, updates can be released without affecting unrelated services.

Examples:

* Updating WS_Customer does not require redeploying WS_CRM
* Updating WS_Catalog does not affect WS_Employee
* Database changes remain isolated within their owning service

---

# 🔮 Future Improvements

* API Gateway (YARP / Ocelot)
* Docker Containerization
* CI/CD Pipeline
* Centralized Logging
* Distributed Tracing
* RabbitMQ / Kafka Integration
* Unit Testing
* Integration Testing

---

# 👨‍💻 Author

**Salis Aryani**

Software Engineer focused on:
* .NET Development
* REST API Development
* PostgreSQL
* Microservices Architecture
* Enterprise Applications
* AI Integration
