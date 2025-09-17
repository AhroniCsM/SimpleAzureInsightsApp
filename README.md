# Simple Azure Application Insights App

A simple .NET 8 web application that sends traces to Azure Application Insights for monitoring and observability.

## Prerequisites

Before running this application, make sure you have the following installed on your Windows machine:

### 1. Install .NET 8 SDK

1. Go to [https://dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
2. Download the **.NET 8.0 SDK** (not just the runtime)
3. Run the installer and follow the installation wizard
4. Verify installation by opening Command Prompt or PowerShell and running:
   ```cmd
   dotnet --version
   ```
   You should see version 8.x.x

### 2. Install Visual Studio Code (Optional but Recommended)

1. Download from [https://code.visualstudio.com/](https://code.visualstudio.com/)
2. Install the C# extension for better development experience

## Project Structure

```
SimpleAzureInsightsApp/
├── Controllers/
│   └── WeatherController.cs          # API controller with sample endpoints
├── Services/
│   ├── ITraceService.cs             # Interface for trace service
│   └── TraceService.cs              # Service that generates sample traces
├── SimpleAzureInsightsApp.csproj    # Project file with dependencies
├── Program.cs                       # Application entry point
├── appsettings.json                # Configuration with Azure App Insights
└── README.md                       # This file
```

## Configuration

The application is already configured with your Azure Application Insights connection string in `appsettings.json`:

```json
{
  "ApplicationInsights": {
    "ConnectionString": "InstrumentationKey=c2069dda-b2eb-46cf-a2b6-7767b3367e02;IngestionEndpoint=https://westeurope-5.in.applicationinsights.azure.com/;LiveEndpoint=https://westeurope.livediagnostics.monitor.azure.com/;ApplicationId=054391ef-cf83-4e15-911f-1667702d07da"
  }
}
```

## How to Run the Application

### Method 1: Using Command Prompt/PowerShell

1. **Open Command Prompt or PowerShell**
   - Press `Win + R`, type `cmd` or `powershell`, and press Enter

2. **Navigate to the project directory**
   ```cmd
   cd "C:\path\to\your\SimpleAzureInsightsApp"
   ```
   (Replace with the actual path where you placed the project)

3. **Restore dependencies**
   ```cmd
   dotnet restore
   ```

4. **Run the application**
   ```cmd
   dotnet run
   ```

5. **You should see output like:**
   ```
   info: Microsoft.Hosting.Lifetime[14]
         Now listening on: https://localhost:7xxx
         Now listening on: http://localhost:5xxx
   ```

### Method 2: Using Visual Studio Code

1. **Open the project folder in VS Code**
   - Open VS Code
   - File → Open Folder → Select the `SimpleAzureInsightsApp` folder

2. **Open Terminal in VS Code**
   - Terminal → New Terminal

3. **Run the application**
   ```cmd
   dotnet run
   ```

## Testing the Application

Once the application is running, you can test it using:

### 1. Using a Web Browser

Open your browser and navigate to:
- **Swagger UI**: `https://localhost:7xxx/swagger` (replace 7xxx with your actual port)
- **Weather API**: `https://localhost:7xxx/weather`

### 2. Using PowerShell/Command Prompt

Open a new Command Prompt or PowerShell window and run:

```cmd
# Test the weather endpoint
curl https://localhost:7xxx/weather

# Test the error endpoint (to generate error traces)
curl https://localhost:7xxx/weather/error
```

### 3. Using Postman (Optional)

1. Download and install Postman from [https://www.postman.com/downloads/](https://www.postman.com/downloads/)
2. Create a new request
3. Set the URL to `https://localhost:7xxx/weather`
4. Send the request

## What the Application Does

The application includes:

1. **Weather Controller** (`/weather` endpoint):
   - Returns sample weather data
   - Generates traces for each request
   - Includes custom activities and tags

2. **Error Endpoint** (`/weather/error`):
   - Simulates an error for testing error tracking
   - Generates error traces in Application Insights

3. **Trace Service**:
   - Generates sample business logic traces
   - Demonstrates nested activities and spans

## Viewing Traces in Azure Application Insights

1. **Go to Azure Portal**
   - Navigate to [https://portal.azure.com](https://portal.azure.com)
   - Sign in with your Azure account

2. **Find your Application Insights resource**
   - Search for ".net-application-insight" in the search bar
   - Click on your Application Insights resource

3. **View traces and logs**
   - Go to **Logs** in the left menu
   - Run queries like:
     ```kusto
     // View all traces
     traces
     | order by timestamp desc
     | take 50
     
     // View requests
     requests
     | order by timestamp desc
     | take 20
     
     // View exceptions
     exceptions
     | order by timestamp desc
     | take 20
     ```

4. **View Application Map**
   - Go to **Application Map** to see the flow of requests
   - Go to **Performance** to see response times
   - Go to **Failures** to see error rates

## Troubleshooting

### Common Issues:

1. **"dotnet command not found"**
   - Make sure .NET 8 SDK is installed
   - Restart your Command Prompt/PowerShell after installation

2. **Port already in use**
   - The application will automatically find an available port
   - Check the console output for the actual URLs

3. **HTTPS certificate issues**
   - The application uses development certificates
   - You might need to trust the development certificate:
     ```cmd
     dotnet dev-certs https --trust
     ```

4. **No data in Application Insights**
   - Wait 2-5 minutes for data to appear
   - Check that the connection string is correct
   - Make sure you're making requests to the application

### Getting Help:

- Check the console output for error messages
- Verify your .NET version with `dotnet --version`
- Ensure all files are in the correct directory structure

## Stopping the Application

To stop the application:
- In the Command Prompt/PowerShell where it's running, press `Ctrl + C`
- Wait for the application to shut down gracefully

## Next Steps

Once you have the application running and can see traces in Azure Application Insights, you can:

1. **Modify the code** to add your own business logic
2. **Add more endpoints** to generate different types of traces
3. **Customize the telemetry** by adding more tags and properties
4. **Set up alerts** in Azure Application Insights for monitoring

The application is now ready to send traces to your Azure Application Insights instance!
