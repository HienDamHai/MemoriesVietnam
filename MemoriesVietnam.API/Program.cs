using MemoriesVietnam.Application.Interfaces;
using MemoriesVietnam.Application.Services;
using MemoriesVietnam.Domain.IBasic;
using MemoriesVietnam.Domain.IRepositories;
using MemoriesVietnam.Infrastructure.Basic;
using MemoriesVietnam.Infrastructure.Data;
using MemoriesVietnam.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IOAuthAccountRepository, OAuthAccountRepository>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<TagService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IPodcastRepository, PodcastRepository>();
builder.Services.AddScoped<PodcastService>();
builder.Services.AddScoped<PodcastEpisodeService>();
builder.Services.AddScoped<IBookmarksRepository, BookmarkRepository>();
builder.Services.AddScoped<BookmarkService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<IEraRepository, EraRepository>();
builder.Services.AddScoped<EraService>();
builder.Services.AddScoped<IArticleAudioRepository, ArticleAudioRepository>();
builder.Services.AddScoped<ArticleAudioService>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<ArticleService>();
builder.Services.AddScoped<IArticleTagRepository, ArticleTagRepository>();
builder.Services.AddScoped<ArticleTagService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<CommentService>();
builder.Services.AddScoped<IHistoryLogRepository, HistoryLogRepository>();
builder.Services.AddScoped<HistoryLogService>();
builder.Services.AddScoped<ILikeTableRepository, LikeTableRepository>();
builder.Services.AddScoped<LikeTableService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(option =>
{
    // Hi?n th? param ki?u camelCase
    option.DescribeAllParametersInCamelCase();

    // N?u có duplicate API thì l?y cái ??u tiên
    option.ResolveConflictingActions(conf => conf.First());

    // C?u hình Security Definition cho JWT
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Bearer {your token}",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    // B?t Swagger yêu c?u token cho t?t c? API (tr? khi [AllowAnonymous])
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    DbSeeder.SeedAdmin(context, config);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
