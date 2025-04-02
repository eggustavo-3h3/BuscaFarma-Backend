using FarmaciaAPI.Data;
using FarmaciaAPI.Domain;
using FarmaciaAPI.DTOs.Base;
using FarmaciaAPI.DTOs.Categorias;
using FarmaciaAPI.DTOs.Login;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados
builder.Services.AddDbContext<FarmaciaContext>();

// Configuração dos serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//teste

//builder.Services.AddAuthentication(JwtBearerDefauts.AuthenticationScheme).AddBearerToken(options =>
//    {
//        options.TokenValidationParameters =
//        new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidateIssuer = "busca.farm",
//            ValidateAudience = "busca.farm",
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
//                "{1058fc1c-b172-4b72-a684-f51febdb2631}"))
//        };
//    });

var app = builder.Build();

// Configuração do Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

#region Categoria

app.MapPost("categoria/adicionar", (FarmaciaContext context, CategoriaAdicionarDto categoriaDto) =>
{
    context.CategoriaSet.Add(new Categoria
    {
        Id = Guid.NewGuid(),
        Descricao = categoriaDto.Descricao
    });

    context.SaveChanges();

    return Results.Created("Created", new BaseResponse("Categoria Registrada com Sucesso!"));
});


//GET (Listar)
//GET (Obter)
//POST
//PUT
//DELETE

#endregion



//app.MapPost("/medicamento/adicionar", (FarmaciaContext context, Medicamento medicamento) =>
//{
//    context.MedicamentoSet.Add(medicamento);
//    context.SaveChanges();
//    return Results.Created("Created", "Medicamento registrado com sucesso.");
//});

//app.MapGet("/medicamento/listar", (FarmaciaContext context) =>
//{
//    var medicamentos = context.MedicamentoSet.ToList();
//    return Results.Ok(medicamentos);
//});

//app.MapPut("/medicamento/atualizar", (FarmaciaContext context, Medicamento medicamento) =>
//{
//    context.MedicamentoSet.Update(medicamento);
//    context.SaveChanges();
//    return Results.Ok("Medicamento atualizado com sucesso.");
//});

//// 📌 USUÁRIOS
//app.MapPost("/usuario/adicionar", (FarmaciaContext context, Usuario usuario) =>
//{
//    context.UsuarioSet.Add(usuario);
//    context.SaveChanges();
//    return Results.Created("Created", "Usuário registrado com sucesso.");
//});

//app.MapGet("/usuario/listar", (FarmaciaContext context) =>
//{
//    var usuarios = context.UsuarioSet.ToList();
//    return Results.Ok(usuarios);
//});

//app.MapPut("/usuario/atualizar", (FarmaciaContext context, Usuario usuario) =>
//{
//    context.UsuarioSet.Update(usuario);
//    context.SaveChanges();
//    return Results.Ok("Usuário atualizado com sucesso.");
//});

//// 📌 RESERVAS
//app.MapPost("/reserva/adicionar", (FarmaciaContext context, Reserva reserva) =>
//{
//    context.ReservaSet.Add(reserva);
//    context.SaveChanges();
//    return Results.Created("Created", "Reserva registrada com sucesso.");
//});

//app.MapGet("/reserva/listar", (FarmaciaContext context) =>
//{
//    var reservas = context.ReservaSet.ToList();
//    return Results.Ok(reservas);
//});

//app.MapPut("/reserva/atualizar", (FarmaciaContext context, Reserva reserva) =>
//{
//    context.ReservaSet.Update(reserva);
//    context.SaveChanges();
//    return Results.Ok("Reserva atualizada com sucesso.");
//});














app.MapPost("autenticar", (FarmaciaContext context, LoginDto loginDto) =>

{
    if (loginDto.Login == "Etec" &&
        loginDto.Senha == "123")
    {
        var claims = new[]
        {
            new Claim("Nome", loginDto.Login)
        };


        var key =
        new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("" +
            "{1058fc1c-b172-4b72-a684-f51febdb2631}"));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "busca.farm",
            audience: "busca.farm",
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
            );

        return Results.Ok(
            new JwtSecurityTokenHandLer()
            .WriteToken(token));
    }

    return Results.BadRequest(new BaseResponse("Usuário e senha invalidos"));
});
    

app.Run();
