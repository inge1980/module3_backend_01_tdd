### T.D.D. - Test Driven Developement

First I wrote some tests for the existing webapi from class last week, to get familarized with the methods. 
Then I started with Test Driven Developement. See commits for more details.

## Added Endpoints

### GET `/api/Tasks/overdue`

Returns all open tasks where the due date has passed.

### GET `/api/Tasks/status/{status}`

Returns all tasks matching the requested status.

Example statuses:

- `Open`
- `Completed`

### PUT `/api/Tasks/{id}/status`

Updates the status of an existing task.

---

## Tests

The new functionality was developed using TDD (Test Driven Development).

Implemented tests include:

- Verifying overdue tasks only return open tasks with expired due dates.
- Verifying filtering tasks by status using xUnit `[Theory]` with multiple status values.
- Verifying that an existing task status can be updated.

The red ? green development process can be followed through the Git commit history.

## How to run automatic Tests

Run the tests:
   ```bash
   dotnet test
   ```


## How to run webapi and swagger UI

1. **Build the project**:
   ```bash
   dotnet build
   ```

2. **Run the server**:
   ```bash
   dotnet run --project webapi/webapi.csproj 
   ```


## Testing API Endpoints using swagger

1. The API can be tested manually using Swagger UI.
    ```
    http://localhost:5179/swagger/index.html
    ```

    or using cURL / Postman / HTTPie with
    ```
    http://localhost:5179/api/tasks/
    ```