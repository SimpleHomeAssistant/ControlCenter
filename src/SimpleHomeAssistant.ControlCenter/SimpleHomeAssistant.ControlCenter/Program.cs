using MediatR;
using SimpleHomeAssistant.ControlCenter.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json")
                     .AddJsonFile("appsettings.Development.json")
    .AddEnvironmentVariables("SHA_");

#region add services

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMqttTopicReceiverAndHandlers();
builder.Services.AddMediatR(typeof(Program).Assembly);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

#endregion add services

#region config options

builder.Services.Configure<TopicRecieverOptions>(builder.Configuration.GetSection("MQTT"));

#endregion config options

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}
app.UseMqttTopicReceiver();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();