using System.Text;
using Microsoft.AspNetCore.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

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

app.MapGet("/switch", async () =>
    {
        if (!System.IO.File.Exists("switchState.txt"))
        {
            // Create the file if it doesn't exist
            using (FileStream fs = System.IO.File.Create("switchState.txt"))
            {
                byte[] info = new UTF8Encoding(true).GetBytes("off");
                fs.Write(info, 0, info.Length);
            }
        }

        var state = await System.IO.File.ReadAllTextAsync("switchState.txt");
        return Results.Ok(state);
    })
    .WithName("GetSwitchState")
    .WithOpenApi();

app.MapPost("/switch", async ([FromBody] string state) =>
    {
        if (state != "on" && state != "off")
        {
            return Results.BadRequest("Invalid state. State can be either 'on' or 'off'");
        }

        await System.IO.File.WriteAllTextAsync("switchState.txt", state);
        return Results.Ok();
    })
    .WithName("SetSwitchState")
    .WithOpenApi();

app.Run();