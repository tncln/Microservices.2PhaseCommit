var builder = WebApplication.CreateBuilder(args);



var app = builder.Build();

app.MapGet("/ready", () =>
{
    Console.WriteLine("Stock Service is ready");
    return true;
});
app.MapGet("/commit", () =>
{
    Console.WriteLine("Stock Service is commit");
    return true;
});
app.MapGet("/RoolBack", () =>
{
    Console.WriteLine("Stock Service is roolback");
});
app.Run();
