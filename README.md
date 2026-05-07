# Clean Architecture Web API
This project is a Clean Architecture Web API built with ASP.NET Core. 

- The solution is structured into separate layers (Domain, Application, Infrastructure, and API)

- This API uses JWT authentication together with ASP.NET Identity to secure endpoints and manage users.

## Users can register via:

### POST /api/Auth/register

For example:

{
  "username": "user@test.com",
  "password": "User123!"
}


## Users log in via:

### POST /api/Auth/login

For example:

{
  "username": "user@test.com",
  "password": "User123!"
}

Response:

{
  "token": "JWT_TOKEN_HERE"
}

## When recieving the token:

- Click Authorize
- Paste only the token

## The system uses Role-Based Access Control (RBAC):

#### Role	Access
##### User	→ Read data
##### Admin	→ Create, update, delete

#### An admin user is automatically created:
Email: admin@test.com
Password: Admin123!


## 🧪 Example Flow
Register a user
Login → get token
Authorize in Swagger
Access protected endpoints
Admin can modify data
