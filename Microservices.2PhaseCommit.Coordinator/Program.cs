using Microservices._2PhaseCommit.Coordinator.Models.Context;
using Microservices._2PhaseCommit.Coordinator.Services;
using Microservices._2PhaseCommit.Coordinator.Services.Abstraction;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args); 

builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TwoPhaseCommitContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer")));

builder.Services.AddHttpClient("OrderAPI", client => client.BaseAddress = new("https://localhost:7122/"));
builder.Services.AddHttpClient("StockAPI", client => client.BaseAddress = new("https://localhost:7250/"));
builder.Services.AddHttpClient("PaymentAPI", client => client.BaseAddress = new("https://localhost:7150/"));

builder.Services.AddTransient<ITransactionService, TransactionService>();

var app = builder.Build();
 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/create-order-transaction", async (ITransactionService transactionService) =>
{
    //Phase 1 - Prepare
    var transactionId = await transactionService.CreateTransactionAsync();
    await transactionService.PrepareServicesAsync(transactionId);
    bool transactionState = await transactionService.CheckReadyServicesAsync(transactionId);
    if (transactionState)
    {
        //Phase 2
        await transactionService.CommitAsync(transactionId);
        transactionState = await transactionService.CheckTransactionStateServiceAsync(transactionId);
    }
    if(!transactionState)
        await transactionService.RollBackAsync(transactionId);
});

app.Run();
