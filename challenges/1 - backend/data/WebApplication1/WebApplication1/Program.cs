using System.Text;
using Microsoft.AspNetCore.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Database.Entities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql("Host=localhost;Port=5438;Database=postgres;Username=postgres;Password=password"));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

if (!System.IO.File.Exists("switchState.txt"))
{
    using (FileStream fs = System.IO.File.Create("switchState.txt"))
    {
        byte[] info = new UTF8Encoding(true).GetBytes("off");
        fs.Write(info, 0, info.Length);
    }
}

app.MapGet("/switch", async (AppDbContext db) =>
    {
        var switchState = await db.SwitchStates.FirstOrDefaultAsync();
        return switchState != null ? Results.Ok(switchState.State) : Results.NotFound();
    })
    .WithName("GetSwitchState")
    .WithOpenApi();

app.MapPost("/switch", async ([FromBody] string state, AppDbContext db) =>
    {
        if (state != "on" && state != "off")
        {
            return Results.BadRequest("Invalid state. State can be either 'on' or 'off'");
        }

        var switchState = await db.SwitchStates.FirstOrDefaultAsync();
        if (switchState != null)
        {
            switchState.State = state;
        }
        else
        {
            db.SwitchStates.Add(new SwitchState { State = state });
        }

        await db.SaveChangesAsync();
        return Results.Ok();
    })
    .WithName("SetSwitchState")
    .WithOpenApi();

app.Run();