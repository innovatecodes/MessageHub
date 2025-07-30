using Common.Constants;
using Common.Exceptions;
using Common.Interfaces;
using Common.Options;
using FormReceiver.DTOs.Request;
using FormReceiver.DTOs.Response;
using FormReceiver.Services;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddScoped<IEmailService<InputRequest, Response>, EmailService>();

builder.Services.AddScoped<INotificationService<InputRequest, Response>, NotificationService>();

builder.Services.AddScoped<IAutoReplyNotificationService<Response>, AutoReplyNotificationService>();

builder.Services.AddHttpClient<WhatsAppNotificationService>(http =>
{
    http.BaseAddress = new Uri(builder.Configuration["Endpoints:ZapHub:Url"]!);
    http.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {builder.Configuration["Endpoints:ZapHub:Token"]}");
});

builder.Services.AddScoped<IWhatsAppNotificationService<Response>, WhatsAppNotificationService>();

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        var clientIpAddressPartitionKey = httpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;

        return RateLimitPartition.GetFixedWindowLimiter(clientIpAddressPartitionKey, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 4,
            Window = TimeSpan.FromMinutes(1),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 2
        });
    }, null);

    options.AddFixedWindowLimiter("fixed-window", options =>
    {
        options.PermitLimit = 4; // Permite 4 requisiçõees por janela
        options.Window = TimeSpan.FromMinutes(1); // A janela de tempo é de 1 minuto
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; // Se exceder, enfileira as mais antigas primeiro
        options.QueueLimit = 2; // Permite até 2 requisições aguardando na fila

    }).RejectionStatusCode = (int)System.Net.HttpStatusCode.TooManyRequests; // Retorna 429 Too Many Requests quando a taxa é excedida
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .WithMethods("POST")
               .AllowAnyHeader();
    });
});

ILogger<Program> logger = AddLogger();

AddApplicationInfo(builder);

AddSmtpInfo(builder, logger);

var app = builder.Build();

try
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseExceptionHandler("/error");

    app.UseHttpsRedirection();

    //app.UseStaticFiles();

    app.UseCors();

    app.UseRouting();

    app.UseRateLimiter();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (OptionsValidationException ex) 
{
    logger.LogError(ex, "### Erro ao validar as opções de ApplicationInfo ###\n"); 
    Environment.Exit(1);
}

static void AddApplicationInfo(WebApplicationBuilder builder) 
{
    builder.Services.AddOptions<ApplicationInfo>()
                    .Bind(builder.Configuration.GetSection("ApplicationInfo"))
                    .Validate(applicationInfo =>
                       !string.IsNullOrWhiteSpace(applicationInfo.WhatsApp) && !string.IsNullOrWhiteSpace(applicationInfo.Site), AppConstants.CONFIG_LOAD_FAILURE_ERROR)
                    .ValidateOnStart(); 
}

static void AddSmtpInfo(WebApplicationBuilder builder, ILogger logger) 
{
    try
    {
        var smtpInfo = builder.Configuration.GetSection("SmtpInfo").Get<SmtpInfo>() ?? new();

        smtpInfo.Validate();

        builder.Services.AddSingleton<SmtpInfo>(smtpInfo);
    }
    catch (ConfigurationException ex)
    {
        logger.LogError(ex, "### Erro ao validar as opções de SmtpInfo ###\n");
        Environment.Exit(1);
    }
}

static ILogger<Program> AddLogger()
{
    return LoggerFactory.Create(config =>
    {
        config.AddConsole();
    }).CreateLogger<Program>();
}