using DSHI_diplom.Components;
using DSHI_diplom.Model;
using Microsoft.EntityFrameworkCore;
using DSHI_diplom.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DSHI_diplom.Services.Interfaces;
using Blazored.LocalStorage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(); 

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddDbContext<DiplomContext>(options =>
    options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<IApplicationFileService, ApplicationFileService>();
builder.Services.AddScoped<IAudioFileService, AudioFileService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<ICollectionOfNoteService, CollectionOfNoteService>();
builder.Services.AddScoped<ICollectionOfNotesWithNoteService, CollectionOfNotesWithNoteService>();
builder.Services.AddScoped<ICollectionOfTheoreticalMaterialService, CollectionOfTheoreticalMaterialService>();
builder.Services.AddScoped<ICollectionOfThMlWithThService, CollectionOfThMlWithThService>();
builder.Services.AddScoped<IComposerService, ComposerService>();
builder.Services.AddScoped<IInstrumentService, InstrumentService>();
builder.Services.AddScoped<IMusicalFormService, MusicalFormService>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<ITestResultService, TestResultService>();
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<ITheoreticalMaterialService, TheoreticalMaterialService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "your_issuer",
            ValidAudience = "your_audience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key"))
        };
    });
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorization();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7202/") });


builder.Services.AddControllers();

builder.Services.AddHttpClient();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseDeveloperExceptionPage();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();

app.Run();
