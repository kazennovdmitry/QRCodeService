# QR Code Service

An example of ASP.NET Core Web API for managing QR codes.

## Features

- Create QR codes via BPM integration
- Retrieve QR code file information
- Delete QR code files
- Basic authentication protection
- Swagger API documentation

## Tech Stack

- .NET 7.0
- ASP.NET Core Web API
- Docker
- Swagger/OpenAPI

## Project Structure

```
QRCodeService/
├── QRCodeServiceApi/          # API layer
├── QRCodeBusinessService/     # Business logic
├── QRCodeBankConnector/       # Bank integration connector
├── QRCodeFileResotitory/      # File storage repository
└── QRCodeServiceAuthentication/ # Authentication handlers
```

## Getting Started

### Prerequisites

- Docker
- .NET 7.0 SDK (for local development)

### Running with Docker

```bash
docker-compose up -d
```

The API will be available at `http://localhost:80`

Swagger UI is accessible at `http://localhost:80/`

### Running Locally

```bash
cd QRCodeServiceApi
dotnet run
```

## Configuration

Configuration is stored in `appsettings.json`:

```json
{
  "Authentication": {
    "Username": "<auth-username>",
    "Password": "<auth-password>"
  }
}
```

## API Endpoints

### Create QR Codes
```
POST /api/qrcode
```

### Get QR Code Files
```
GET /api/qrcode
```

### Delete QR Code Files
```
DELETE /api/qrcode
```

All endpoints require Basic authentication.

## Contact

Dmitry Kazennov - kazennovdmitriy@gmail.com
