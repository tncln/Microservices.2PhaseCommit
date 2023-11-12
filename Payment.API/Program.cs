var builder = WebApplication.CreateBuilder(args);



var app = builder.Build();

app.MapGet("/ready", () =>
{
    Console.WriteLine("Payment Service is ready");
    return true;
});
app.MapGet("/commit", () =>
{
    Console.WriteLine("Payment Service is commit");
    return true;
});
app.MapGet("/RoolBack", () =>
{
    Console.WriteLine("Payment Service is roolback");
});
app.Run();
