var builder = WebApplication.CreateBuilder(args);

 

var app = builder.Build();

app.MapGet("/ready", () =>
{
    Console.WriteLine("Order Service is ready");
    return true;
});
app.MapGet("/commit", () =>
{
    Console.WriteLine("Order Service is commit");
    return true;
});
app.MapGet("/RoolBack", () =>
{
    Console.WriteLine("Order Service is roolback"); 
});
app.Run();
