FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/BookingService.Booking.Api/BookingService.Booking.Api.csproj", "src/BookingService.Booking.Api/"]
COPY ["src/BookingService.Booking.Persistense/BookingService.Booking.Persistense.csproj", "src/BookingService.Booking.Persistense/"]
COPY ["src/BookingService.Booking.Api.Contracts/BookingService.Booking.Api.Contracts.csproj", "src/BookingService.Booking.Api.Contracts/"]
COPY ["src/BookingService.Booking.Application/BookingService.Booking.Application.csproj", "src/BookingService.Booking.Application/"]
RUN dotnet restore "./src/BookingService.Booking.Api/BookingService.Booking.Api.csproj"
COPY . .
WORKDIR "/src/src/BookingService.Booking.Api"
RUN dotnet build "./BookingService.Booking.Api.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BookingService.Booking.Api.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookingService.Booking.Api.dll"]