using FarmaciaAPI.Data.Context;
using FarmaciaAPI.Domain.DTOs.Base;
using FarmaciaAPI.Domain.DTOs.Categorias;
using FarmaciaAPI.Domain.DTOs.Login;
using FarmaciaAPI.Domain.DTOs.Medicamento;
using FarmaciaAPI.Domain.DTOs.Reserva;
using FarmaciaAPI.Domain.DTOs.Usuario;
using FarmaciaAPI.Domain.Entities;
using FarmaciaAPI.Domain.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FarmaciaContext>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Busca Farma API",
        Version = "v1",
        Description = "API para gerenciamento de medicamentos da BuscaFarma"
    });

    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"<b>JWT Autorização</b> <br/> 
                      Digite 'Bearer' [espaço] e em seguida colar seu token na caixa de texto abaixo.
                      <br/> <br/>
                      <b>Exemplo:</b> 'bearer 123456abcdefg...'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    config.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(Options =>
    {
        Options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "busca.farma",
            ValidAudience = "busca.farma",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                "{1058fc1c-b172-4b72-a684-f51febdb2631}"))
        };
    });

builder.Services.AddCors();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

#region Categoria

app.MapPost("categoria/adicionar", (FarmaciaContext context, CategoriaAdicionarDto categoriaDto) =>
{
    var resultado = new CategoriaAdicionarDtoValidator().Validate(categoriaDto);

    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    context.CategoriaSet.Add(new Categoria
    {
        Id = Guid.NewGuid(),
        Descricao = categoriaDto.Descricao
    });

    context.SaveChanges();

    return Results.Created("Created", new BaseResponse("Categoria Registrada com Sucesso!"));
})
//.RequireAuthorization()
.WithTags("Categoria");

app.MapGet("categoria/listar", (FarmaciaContext context) =>
{
    var categorias = context.CategoriaSet.ToList();

    if (categorias.Count == 0)
        return Results.NotFound(new BaseResponse("Nenhuma categoria encontrada."));

    return Results.Ok(categorias);
})
//.RequireAuthorization()
.WithTags("Categoria");

app.MapGet("categoria/{id}", (FarmaciaContext context, Guid id) =>
{
    var categoria = context.CategoriaSet.FirstOrDefault(c => c.Id == id);

    if (categoria == null)
        return Results.NotFound(new BaseResponse("Categoria não encontrada."));

    return Results.Ok(categoria);
})
//.RequireAuthorization()
.WithTags("Categoria");

app.MapPut("categoria/atualizar/{id}", (FarmaciaContext context, Guid id, CategoriaAdicionarDto categoriaDto) =>
{
    var categoria = context.CategoriaSet.FirstOrDefault(c => c.Id == id);

    if (categoria == null)
        return Results.NotFound(new BaseResponse("Categoria não encontrada."));

    categoria.Descricao = categoriaDto.Descricao;

    context.SaveChanges();

    return Results.Ok(new BaseResponse("Categoria atualizada com sucesso!"));
})
//.RequireAuthorization()
.WithTags("Categoria");

app.MapDelete("categoria/excluir/{id}", (FarmaciaContext context, Guid id) =>
{
    var categoria = context.CategoriaSet.FirstOrDefault(c => c.Id == id);

    if (categoria == null)
        return Results.NotFound(new BaseResponse("Categoria não encontrada."));

    context.CategoriaSet.Remove(categoria);
    context.SaveChanges();

    return Results.Ok(new BaseResponse("Categoria excluída com sucesso!"));
})
//.RequireAuthorization()
.WithTags("Categoria");

#endregion

#region Medicamentos

app.MapPost("medicamento/adicionar", (FarmaciaContext context, MedicamentoAdicionar medicamentoDto) =>
{
    context.MedicamentoSet.Add(new Medicamento
    {
        Id = Guid.NewGuid(),
        NomeComercial = medicamentoDto.NomeComercial,
        NomeQuimico = medicamentoDto.NomeQuimico,
        Descricao = medicamentoDto.Descricao,
        TipoMedicamento = medicamentoDto.TipoMedicamento,
        UnidadeMedida = medicamentoDto.UnidadeMedida,
        CategoriaId = medicamentoDto.CategoriaId,
        Categoria =  medicamentoDto.Categoria
    });

    context.SaveChanges();

    return Results.Created("Created", new BaseResponse("Medicamento Registrado com Sucesso!"));
})
//.RequireAuthorization()
.WithTags("Medicamentos");

app.MapGet("medicamento/listar", (FarmaciaContext context) =>
{
    var medicamentos = context.MedicamentoSet.ToList();

    if (medicamentos.Count == 0)
        return Results.NotFound(new BaseResponse("Nenhum medicamento encontrado."));

    return Results.Ok(medicamentos);
})
//.RequireAuthorization()
.WithTags("Medicamentos");

app.MapGet("medicamento/{id}", (FarmaciaContext context, Guid id) =>
{
    var medicamento = context.MedicamentoSet.FirstOrDefault(m => m.Id == id);

    if (medicamento == null)
        return Results.NotFound(new BaseResponse("Medicamento não encontrado."));

    return Results.Ok(medicamento);
})
//.RequireAuthorization()
.WithTags("Medicamentos");

app.MapPut("medicamento/atualizar/{id}", (FarmaciaContext context, Guid id, MedicamentoAtualizar medicamentoDto) =>
{
    var medicamento = context.MedicamentoSet.FirstOrDefault(m => m.Id == id);

    if (medicamento == null)
        return Results.NotFound(new BaseResponse("Medicamento não encontrado."));

    medicamento.NomeComercial = medicamentoDto.NomeComercial;
    medicamento.NomeQuimico = medicamentoDto.NomeQuimico;
    medicamento.Descricao = medicamentoDto.Descricao;
    medicamento.TipoMedicamento = medicamentoDto.TipoMedicamento;
    medicamento.UnidadeMedida = medicamentoDto.UnidadeMedida;
    medicamento.CategoriaId = medicamentoDto.CategoriaId;

    context.SaveChanges();

    return Results.Ok(new BaseResponse("Medicamento atualizado com sucesso!"));
})
//.RequireAuthorization()
.WithTags("Medicamentos");

app.MapDelete("medicamento/excluir/{id}", (FarmaciaContext context, Guid id) =>
{
    var medicamento = context.MedicamentoSet.FirstOrDefault(m => m.Id == id);

    if (medicamento == null)
        return Results.NotFound(new BaseResponse("Medicamento não encontrado."));

    context.MedicamentoSet.Remove(medicamento);
    context.SaveChanges();

    return Results.Ok(new BaseResponse("Medicamento excluído com sucesso!"));
})
//.RequireAuthorization()
.WithTags("Medicamentos");

#endregion

#region Reserva

app.MapPost("reserva/adicionar", (FarmaciaContext context, ReservaAdicionar reservaDto) =>
{
    context.ReservaSet.Add(new Reserva
    {
        Id = Guid.NewGuid(),
        UsuarioId = reservaDto.UsuarioId,
        MedicamentoId = reservaDto.MedicamentoId,
        DataReserva = reservaDto.DataReserva,
        ImagemReceita = reservaDto.ImagemReceita,
        EnumTipoAtendimento = reservaDto.EnumTipoAtendimento
    });

    context.SaveChanges();

    return Results.Created("Created", new BaseResponse("Reserva registrada com sucesso!"));
})
.RequireAuthorization()
.WithTags("Reserva");

app.MapGet("reserva/listar", (FarmaciaContext context) =>
{
    var reservas = context.ReservaSet.ToList();

    if (reservas.Count == 0)
        return Results.NotFound(new BaseResponse("Nenhuma reserva encontrada."));

    return Results.Ok(reservas);
})
.RequireAuthorization()
.WithTags("Reserva");

app.MapGet("reserva/{id}", (FarmaciaContext context, Guid id) =>
{
    var reserva = context.ReservaSet.FirstOrDefault(r => r.Id == id);

    if (reserva == null)
        return Results.NotFound(new BaseResponse("Reserva não encontrada."));

    return Results.Ok(reserva);
})
.RequireAuthorization()
.WithTags("Reserva");

app.MapPut("reserva/atualizar/{id}", (FarmaciaContext context, Guid id, ReservaAtualizar reservaDto) =>
{
    var reserva = context.ReservaSet.FirstOrDefault(r => r.Id == id);

    if (reserva == null)
        return Results.NotFound(new BaseResponse("Reserva não encontrada."));

    reserva.UsuarioId = reservaDto.UsuarioId;
    reserva.MedicamentoId = reservaDto.MedicamentoId;
    reserva.DataReserva = reservaDto.DataReserva;
    reserva.ImagemReceita = reservaDto.ImagemReceita;
    reserva.EnumTipoAtendimento = reservaDto.EnumTipoAtendimento;

    context.SaveChanges();

    return Results.Ok(new BaseResponse("Reserva atualizada com sucesso!"));
})
.RequireAuthorization()
.WithTags("Reserva");

app.MapDelete("reserva/excluir/{id}", (FarmaciaContext context, Guid id) =>
{
    var reserva = context.ReservaSet.FirstOrDefault(r => r.Id == id);

    if (reserva == null)
        return Results.NotFound(new BaseResponse("Reserva não encontrada."));

    context.ReservaSet.Remove(reserva);
    context.SaveChanges();

    return Results.Ok(new BaseResponse("Reserva excluída com sucesso!"));
})
.RequireAuthorization()
.WithTags("Reserva");

#endregion

#region Usuario

app.MapPost("usuario/adicionar", (FarmaciaContext context, UsuarioAdicionarDto usuarioDto) =>
{
    if (usuarioDto.Senha != usuarioDto.ConfirmarSenha)
        return Results.BadRequest(new BaseResponse("As senhas não coincidem."));

    context.UsuarioSet.Add(new Usuario
    {
        Id = Guid.NewGuid(),
        Nome = usuarioDto.Nome,
        CPF = usuarioDto.CPF,
        Telefone = usuarioDto.Telefone,
        Senha = usuarioDto.Senha.EncryptPassword()
    });

    context.SaveChanges();

    return Results.Created("Created", new BaseResponse("Usuário registrado com sucesso!"));
})
//.RequireAuthorization()
.WithTags("Usuario");

app.MapGet("usuario/listar", (FarmaciaContext context) =>
{
    var usuarios = context.UsuarioSet.ToList();

    if (usuarios.Count == 0)
        return Results.NotFound(new BaseResponse("Nenhum usuário encontrado."));

    return Results.Ok(usuarios);
})
.RequireAuthorization()
.WithTags("Usuario");

app.MapGet("usuario/{id}", (FarmaciaContext context, Guid id) =>
{
    var usuario = context.UsuarioSet.FirstOrDefault(u => u.Id == id);

    if (usuario == null)
        return Results.NotFound(new BaseResponse("Usuário não encontrado."));

    return Results.Ok(usuario);
})
.RequireAuthorization()
.WithTags("Usuario");

app.MapPut("usuario/atualizar/{id}", (FarmaciaContext context, Guid id, UsuarioAtualizarDto usuarioDto) =>
{
    var usuario = context.UsuarioSet.FirstOrDefault(u => u.Id == id);

    if (usuario == null)
        return Results.NotFound(new BaseResponse("Usuário não encontrado."));

    usuario.Nome = usuarioDto.Nome;
    usuario.CPF = usuarioDto.CPF;
    usuario.Telefone = usuarioDto.Telefone;

    context.SaveChanges();

    return Results.Ok(new BaseResponse("Usuário atualizado com sucesso!"));
})
.RequireAuthorization()
.WithTags("Usuario");

app.MapDelete("usuario/excluir/{id}", (FarmaciaContext context, Guid id) =>
{
    var usuario = context.UsuarioSet.FirstOrDefault(u => u.Id == id);

    if (usuario == null)
        return Results.NotFound(new BaseResponse("Usuário não encontrado."));

    context.UsuarioSet.Remove(usuario);
    context.SaveChanges();

    return Results.Ok(new BaseResponse("Usuário excluído com sucesso!"));
})
.RequireAuthorization()
.WithTags("Usuario");

#endregion

app.MapPost("autenticar", (FarmaciaContext context, LoginDto loginDto) =>
{
    var usuario = context.UsuarioSet.FirstOrDefault(u => u.CPF == loginDto.Login && u.Senha == loginDto.Senha.EncryptPassword());
    if (usuario is null)
        return Results.BadRequest(new BaseResponse("Usuário e senha inválidos"));
    {
        var claims = new[]
        {
            new Claim("Nome", usuario.Nome),
            new Claim("Telefone", usuario.Telefone),
        };


        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("{1058fc1c-b172-4b72-a684-f51febdb2631}"));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "busca.farma",
            audience: "busca.farma",
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        return Results.Ok(
            new JwtSecurityTokenHandler().WriteToken(token));
    }

})
.WithTags("Autenticador");

app.Run();
