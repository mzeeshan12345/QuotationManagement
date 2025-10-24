# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory
WORKDIR /src

# Copy the project files and restore dependencies
COPY ["Quotation_Management/Quotation_Management.csproj", "Quotation_Management/"]
RUN dotnet restore "Quotation_Management/Quotation_Management.csproj"

# Copy the rest of the source code
COPY . .

# Build the app
WORKDIR "/src/Quotation_Management"
RUN dotnet build "Quotation_Management.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "Quotation_Management.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the official ASP.NET Core runtime image for the final stage (Linux-based)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final

# Set the working directory
WORKDIR /app

# Copy the build and publish output
COPY --from=publish /app/publish .

# Expose the necessary ports (80 is standard for web applications)
EXPOSE 80

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "Quotation_Management.dll"]
