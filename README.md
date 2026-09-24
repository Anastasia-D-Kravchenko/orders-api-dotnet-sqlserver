# Orders API: ASP.NET Core + EF Core + SQL Server in Docker

A small REST API for orders (clients, products, order lines and order statuses) built with **ASP.NET Core (.NET 10)**, **Entity Framework Core 9** (code-first, migrations, seeded data) and **SQL Server 2022 running in Docker**.

## Endpoints

| Method | Path | Description |
|---|---|---|
| `GET` | `/api/orders/{id}` | Order with client, status and its product lines (404 if it does not exist) |
| `PUT` | `/api/orders/{id}/fulfill` | Body `{"statusName": "Completed"}`: sets the new status and `FulfilledAt`, and clears the order's product lines. `404` if the order or status does not exist, `409` if the order is already fulfilled |

Requests you can run directly are in [`OrdersApp/OrdersApp.http`](OrdersApp/OrdersApp.http). In Development, OpenAPI and Swagger UI are enabled.

## Run it

Prerequisites: [Docker](https://www.docker.com/) and the [.NET 10 SDK](https://dotnet.microsoft.com/).

```bash
# 1. Start SQL Server (password comes from .env, default is a local-only placeholder)
cp .env.example .env
docker compose up -d

# 2. Create the schema and seed data
dotnet tool restore
dotnet ef database update --project OrdersApp

# 3. Run the API (http://localhost:5000)
dotnet run --project OrdersApp
```

Then try `GET http://localhost:5000/api/orders/2`.

Stop and remove everything with `docker compose down -v`.

## Database model

`Client` 1—* `Order` *—1 `Status`, and `Order` *—* `Product` through `Product_Order` (composite key, `Amount`). Seed data: two clients, two products, three statuses (Ongoing, Completed, Cancelled) and two orders.

## Notes on the SQL Server container

* The `sa` password is read from the `MSSQL_SA_PASSWORD` variable (`.env`, not committed). `appsettings.json` contains the same local placeholder; for anything beyond a local demo use an environment variable or user-secrets instead: `ConnectionStrings__DefaultConnection`.
* On Apple Silicon the image runs under emulation (`platform: linux/amd64`), which is slower to start; the health check waits for it.
* Handy commands: `docker ps`, `docker compose logs -f db`, `docker exec -it orders-sqlserver /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa`.

## Common errors

| Symptom | Fix |
|---|---|
| `Cannot open server ... / connection refused` | The container is still starting; wait for `docker compose ps` to show `healthy` |
| `Login failed for user 'sa'` | The password in `.env` and in `appsettings.json` (or the connection-string variable) must match |
| `Cannot drop database ... in use` | Stop the API before running `dotnet ef database drop` |
| Port 1433 already in use | Stop the other SQL Server container or change the left side of `"1433:1433"` and the port in the connection string |

## License

MIT, see [LICENSE](LICENSE).
