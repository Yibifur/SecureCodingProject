# Use the official .NET 8 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the compiled .dll and dependencies
COPY ./out/ .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "OneDose.FirstProject.WebAPI.dll"]

# Expose the WebAPI port
EXPOSE 5249
