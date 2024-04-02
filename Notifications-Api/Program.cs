using Microsoft.AspNetCore.SignalR;
using Notifications_Api.ChatHub;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

//builder.Services.AddHostedService<ServerTimeNotifier>();

builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.MapPost("broadcast", async (string message, IHubContext<ChatHub, IChatClient> context) =>
{
    await context.Clients.All.ReceiveMessage(message);

    return Results.NoContent();
});

app.UseHttpsRedirection();

//app.MapHub<NotificationsHub>("notifications");
app.MapHub<ChatHub>("chat-hub");

app.Run();
