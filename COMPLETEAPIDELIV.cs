using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<EmployeeDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/employees", async (AppDbContext db) =>
    await db.Employees.ToListAsync());

app.MapGet("/employees/{id}", async (int id, AppDbContext db) =>
    await db.Employees.FindAsync(id)
        is Employee employee
            ? Results.Ok(employee)
            : Results.NotFound());

app.MapPost("/employees", async (Employee employee, AppDbContext db) =>
{
    db.Employees.Add(employee);
    await db.SaveChangesAsync();
    return Results.Created($"/employees/{employee.Id}", employee);
});

app.MapPut("/employees/{id}", async (int id, Employee inputEmployee, AppDbContext db) =>
{
    var employee = await db.Employees.FindAsync(id);
    if (employee is null) return Results.NotFound();

    employee.FirstName = inputEmployee.FirstName;
    employee.MiddleName = inputEmployee.MiddleName;
    employee.LastName = inputEmployee.LastName;
    employee.DepartmentId = inputEmployee.DepartmentId;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/employees/{id}", async (int id, AppDbContext db) =>
{
    if (await db.Employees.FindAsync(id) is Employee employee)
    {
        db.Employees.Remove(employee);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});

// Evaluation Endpoints
app.MapGet("/evaluations", async (AppDbContext db) =>
    await db.Evaluations.ToListAsync());

app.MapGet("/evaluations/{id}", async (int id, AppDbContext db) =>
    await db.Evaluations.FindAsync(id)
        is Evaluation evaluation
            ? Results.Ok(evaluation)
            : Results.NotFound());

app.MapPost("/evaluations", async (Evaluation evaluation, AppDbContext db) =>
{
    db.Evaluations.Add(evaluation);
    await db.SaveChangesAsync();
    return Results.Created($"/evaluations/{evaluation.Id}", evaluation);
});

app.MapPut("/evaluations/{id}", async (int id, Evaluation inputEvaluation, AppDbContext db) =>
{
    var evaluation = await db.Evaluations.FindAsync(id);
    if (evaluation is null) return Results.NotFound();

    evaluation.ManagerId = inputEvaluation.ManagerId;
    evaluation.EmployeeId = inputEvaluation.EmployeeId;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/evaluations/{id}", async (int id, AppDbContext db) =>
{
    if (await db.Evaluations.FindAsync(id) is Evaluation evaluation)
    {
        db.Evaluations.Remove(evaluation);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});

// Department Endpoints
app.MapGet("/departments", async (AppDbContext db) =>
    await db.Departments.ToListAsync());

app.MapGet("/departments/{id}", async (int id, AppDbContext db) =>
    await db.Departments.FindAsync(id)
        is Department department
            ? Results.Ok(department)
            : Results.NotFound());

app.MapPost("/departments", async (Department department, AppDbContext db) =>
{
    db.Departments.Add(department);
    await db.SaveChangesAsync();
    return Results.Created($"/departments/{department.Id}", department);
});

app.MapPut("/departments/{id}", async (int id, Department inputDepartment, AppDbContext db) =>
{
    var department = await db.Departments.FindAsync(id);
    if (department is null) return Results.NotFound();

    department.Name = inputDepartment.Name;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/departments/{id}", async (int id, AppDbContext db) =>
{
    if (await db.Departments.FindAsync(id) is Department department)
    {
        db.Departments.Remove(department);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});

app.Run();

