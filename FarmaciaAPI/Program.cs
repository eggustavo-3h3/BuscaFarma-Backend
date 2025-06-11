using FarmaciaAPI.Domain.DTOs.AlterarSenha;
using FarmaciaAPI.Domain.DTOs.Base;
using FarmaciaAPI.Domain.DTOs.Categorias;
using FarmaciaAPI.Domain.DTOs.Login;
using FarmaciaAPI.Domain.DTOs.Medicamento;
using FarmaciaAPI.Domain.DTOs.Reserva;
using FarmaciaAPI.Domain.DTOs.ResetSenha;
using FarmaciaAPI.Domain.DTOs.Usuario;
using FarmaciaAPI.Domain.Entities;
using FarmaciaAPI.Domain.Extensions;
using FarmaciaAPI.Infra.Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FarmaciaAPI.Infra.Email;
using FarmaciaAPI.Domain.Validators.Medicamento;
using FarmaciaAPI.Domain.Validators.Reserva;
using FarmaciaAPI.Domain.Validators.Usuario;
using FarmaciaAPI.Domain.Enumerators;
using Microsoft.AspNetCore.Http.HttpResults;

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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Backoffice", policy => policy.RequireRole("Backoffice"));
    options.AddPolicy("Usuario", policy => policy.RequireRole("Usuario", "Backoffice"));
});

builder.Services.AddCors();

var app = builder.Build();

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
.RequireAuthorization()
.WithTags("Categoria");

app.MapGet("categoria/listar", (FarmaciaContext context) =>
{
    var categoria = context.CategoriaSet.Select(c => new CategoriaListarDto
    {
        Id = c.Id,
        Descricao = c.Descricao
    }).ToList();

    if (categoria.Count == 0)
        return Results.NotFound(new BaseResponse("Nenhum medicamento encontrado."));

    return Results.Ok(categoria);
})
.RequireAuthorization()
.WithTags("Categoria");

app.MapGet("categoria/{id}", (FarmaciaContext context, Guid id) =>
{
    var categoria = context.CategoriaSet.FirstOrDefault(c => c.Id == id);

    if (categoria == null)
        return Results.NotFound(new BaseResponse("Categoria não encontrada."));

    return Results.Ok(categoria);
})
.RequireAuthorization()
.WithTags("Categoria");

app.MapPut("categoria/atualizar/{id}", (FarmaciaContext context, Guid id, CategoriaAdicionarDto categoriaDto) =>
{
    var categoria = context.CategoriaSet.FirstOrDefault(c => c.Id == id);

    var resultado = new CategoriaAdicionarDtoValidator().Validate(categoriaDto);
    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    if (categoria == null)
        return Results.NotFound(new BaseResponse("Categoria não encontrada."));

    categoria.Descricao = categoriaDto.Descricao;

    context.SaveChanges();

    return Results.Ok(new BaseResponse("Categoria atualizada com sucesso!"));
})
.RequireAuthorization()
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
.RequireAuthorization()
.WithTags("Categoria");

#endregion

#region Medicamentos

app.MapPost("medicamento/adicionar", (FarmaciaContext context, MedicamentoAdicionarDto medicamentoDto) =>
{
    var resultado = new MedicamentoAdicionarDtoValidator().Validate(medicamentoDto);
    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    context.MedicamentoSet.Add(new Medicamento
    {
        Id = Guid.NewGuid(),
        NomeComercial = medicamentoDto.NomeComercial,
        NomeQuimico = medicamentoDto.NomeQuimico,
        Descricao = medicamentoDto.Descricao,
        Imagem = medicamentoDto.Imagem,
        TipoMedicamento = medicamentoDto.TipoMedicamento,
        Quantidade = medicamentoDto.Quantidade,
        UnidadeMedida = medicamentoDto.UnidadeMedida,
        CategoriaId = medicamentoDto.CategoriaId,
    });

    context.SaveChanges();

    return Results.Created("Created", new BaseResponse("Medicamento Registrado com Sucesso!"));
})
.RequireAuthorization()
.WithTags("Medicamentos");

app.MapGet("medicamento/listar", (FarmaciaContext context) =>
{
    var medicamentos = context.MedicamentoSet.Include(m => m.Categoria).Select(m => new MedicamentoListarDto
    {
        Id = m.Id,
        Descricao = m.Descricao,
        NomeComercial = m.NomeComercial,
        NomeQuimico = m.NomeQuimico,
        TipoMedicamento = m.TipoMedicamento,
        Quantidade = m.Quantidade,
        UnidadeMedida = m.UnidadeMedida,
        Imagem = m.Imagem,
        CategoriaId = m.CategoriaId,
        CategoriaDescricao = m.Categoria.Descricao
    }).ToList();

    if (medicamentos.Count == 0)
        return Results.NotFound(new BaseResponse("Nenhum medicamento encontrado."));

    return Results.Ok(medicamentos);
})
.RequireAuthorization()
.WithTags("Medicamentos");

app.MapGet("medicamento/{id}", (FarmaciaContext context, Guid id) =>
{

    var medicamento = context.MedicamentoSet.Include(M => M.Categoria).FirstOrDefault(m => m.Id == id);

    if (medicamento == null)
        return Results.NotFound(new BaseResponse("Medicamento não encontrado."));

    var medicamentoDto = new MedicamentoObterDto
    {
        Id = medicamento.Id,
        Descricao = medicamento.Descricao,
        NomeComercial = medicamento.NomeComercial,
        NomeQuimico = medicamento.NomeQuimico,
        TipoMedicamento = medicamento.TipoMedicamento,
        Quantidade = medicamento.Quantidade,
        UnidadeMedida = medicamento.UnidadeMedida,
        Imagem = medicamento.Imagem,
        CategoriaId = medicamento.CategoriaId,
        CategoriaDescricao = medicamento.Categoria.Descricao
    };
    
    return Results.Ok(medicamentoDto);
})
.RequireAuthorization()
.WithTags("Medicamentos");

app.MapPut("medicamento/atualizar/{id}", (FarmaciaContext context, Guid id, MedicamentoAtualizarDto medicamentoDto) =>
{
    var medicamento = context.MedicamentoSet.FirstOrDefault(m => m.Id == id);

    var resultado = new MedicamentoAtualizarDtoValidator().Validate(medicamentoDto);
    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));


    if (medicamento == null)
        return Results.NotFound(new BaseResponse("Medicamento não encontrado."));

    medicamento.NomeComercial = medicamentoDto.NomeComercial;
    medicamento.NomeQuimico = medicamentoDto.NomeQuimico;
    medicamento.Descricao = medicamentoDto.Descricao;
    medicamento.Quantidade = medicamentoDto.Quantidade;
    medicamento.TipoMedicamento = medicamentoDto.TipoMedicamento;
    medicamento.UnidadeMedida = medicamentoDto.UnidadeMedida;
    medicamento.CategoriaId = medicamentoDto.CategoriaId;

    context.SaveChanges();

    return Results.Ok(new BaseResponse("Medicamento atualizado com sucesso!"));
})
.RequireAuthorization()
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
.RequireAuthorization()
.WithTags("Medicamentos");

#endregion

#region Reserva

app.MapPost("reserva/adicionar", (FarmaciaContext context, ReservaAdicionarDto reservaDto) =>
{
    var resultado = new ReservaAdicionarDtoValidator().Validate(reservaDto);
    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    var usuario = context.UsuarioSet.FirstOrDefault(u => u.Id == reservaDto.UsuarioId);
    if (usuario is null)
        return Results.BadRequest("Usuário não Localizado.");

    var medicamento = context.MedicamentoSet.FirstOrDefault(u => u.Id == reservaDto.MedicamentoId);
    if (medicamento is null)
        return Results.BadRequest("Medicamento não Localizado.");

    context.ReservaSet.Add(new Reserva
    {
        Id = Guid.NewGuid(),
        UsuarioId = reservaDto.UsuarioId,
        MedicamentoId = reservaDto.MedicamentoId,
        DataReserva = reservaDto.DataReserva,
        ImagemReceita = reservaDto.ImagemReceita,
        EnumTipoAtendimento = reservaDto.EnumTipoAtendimento,
        DataRetirada = reservaDto.DataRetirada,
        RetiranteNome = reservaDto.RetiranteNome,
        RetiranteCpf = reservaDto.RetiranteCpf,
    });

    context.SaveChanges();

    return Results.Created("Created", new BaseResponse("Reserva registrada com sucesso!"));
})
.RequireAuthorization()
.WithTags("Reserva");

app.MapGet("reserva/listar", (FarmaciaContext context) =>
{
    var reservas = context.ReservaSet
    .Include(p => p.Usuario)
    .Include(p => p.Medicamento).ThenInclude(c => c.Categoria)
    .ToList()
    .Select(p => new ReservaListarDto
    {
        Id = p.Id,
        UsuarioId = p.UsuarioId,
        MedicamentoId = p.MedicamentoId,
        DataReserva = p.DataReserva,
        ImagemReceita = p.ImagemReceita,
        EnumTipoAtendimento = p.EnumTipoAtendimento,
        Status = p.Status,
        DataRetirada = p.DataRetirada,
        RetiranteNome = p.RetiranteNome,
        RetiranteCpf = p.RetiranteCpf,
        Usuario = new UsuarioObterDto
        {
            CPF = p.Usuario.CPF,
            Id = p.Usuario.Id,
            Nome = p.Usuario.Nome,
            Telefone = p.Usuario.Telefone,
            Email = p.Usuario.Email
        },
        Medicamento = new MedicamentoObterDto
        {
            Id = p.Medicamento.Id,
            Descricao = p.Medicamento.Descricao,
            NomeComercial = p.Medicamento.NomeComercial,
            NomeQuimico = p.Medicamento.NomeQuimico,
            TipoMedicamento = p.Medicamento.TipoMedicamento,
            Quantidade = p.Medicamento.Quantidade,
            UnidadeMedida = p.Medicamento.UnidadeMedida,
            Imagem = p.Medicamento.Imagem,
            CategoriaId = p.Medicamento.CategoriaId,
            CategoriaDescricao = p.Medicamento.Categoria.Descricao
        }
    }).ToList();

    if (reservas.Count == 0)
        return Results.NotFound(new BaseResponse("Nenhuma reserva encontrada."));

    return Results.Ok(reservas);
})
.RequireAuthorization()
.WithTags("Reserva");

app.MapGet("reserva/{id}", (FarmaciaContext context, Guid id) =>
{
    var reservas = context.ReservaSet
    .Include(r => r.Usuario)
    .Include(r => r.Medicamento).ThenInclude(c => c.Categoria)
    .ToList()
    .Where(r => r.Id == id)
    .Select(r => new ReservaChamarDto
    {
        Id = r.Id,
        UsuarioId = r.UsuarioId,
        MedicamentoId = r.MedicamentoId,
        DataReserva = r.DataReserva,
        ImagemReceita = r.ImagemReceita,
        EnumTipoAtendimento = r.EnumTipoAtendimento,
        Status = r.Status,
        DataRetirada = r.DataRetirada,
        RetiranteNome = r.RetiranteNome,
        RetiranteCpf = r.RetiranteCpf,
        Usuario = new UsuarioObterDto
        {
            CPF = r.Usuario.CPF,
            Id = r.Usuario.Id,
            Nome = r.Usuario.Nome,
            Telefone = r.Usuario.Telefone,
            Email = r.Usuario.Email
        },
        Medicamento = new MedicamentoObterDto
        {
            Id = r.Medicamento.Id,
            Descricao = r.Medicamento.Descricao,
            NomeComercial = r.Medicamento.NomeComercial,
            NomeQuimico = r.Medicamento.NomeQuimico,
            TipoMedicamento = r.Medicamento.TipoMedicamento,
            Quantidade = r.Medicamento.Quantidade,
            UnidadeMedida = r.Medicamento.UnidadeMedida,
            Imagem = r.Medicamento.Imagem,
            CategoriaId = r.Medicamento.CategoriaId,
            CategoriaDescricao = r.Medicamento.Categoria.Descricao
        }
    }).ToList();

    if (reservas.Count == 0)
        return Results.NotFound(new BaseResponse("Reserva não encontrada."));

    return Results.Ok(reservas);
})
.RequireAuthorization()
.WithTags("Reserva");

app.MapGet("reserva/usuario/{usuarioId}", (FarmaciaContext context, Guid usuarioId) =>
{
    var reservas = context.ReservaSet
    .Include(r => r.Usuario)
    .Include(r => r.Medicamento).ThenInclude(c => c.Categoria)
    .ToList()
    .Where(r => r.UsuarioId == usuarioId)
    .Select(r => new ReservaChamarDto
    {
        Id = r.Id,
        UsuarioId = r.UsuarioId,
        MedicamentoId = r.MedicamentoId,
        DataReserva = r.DataReserva,
        ImagemReceita = r.ImagemReceita,
        EnumTipoAtendimento = r.EnumTipoAtendimento,
        Status = r.Status,
        DataRetirada = r.DataRetirada,
        RetiranteNome = r.RetiranteNome,
        RetiranteCpf = r.RetiranteCpf,
        Usuario = new UsuarioObterDto
        {
            CPF = r.Usuario.CPF,
            Id = r.Usuario.Id,
            Nome = r.Usuario.Nome,
            Telefone = r.Usuario.Telefone,
            Email = r.Usuario.Email
        },
        Medicamento = new MedicamentoObterDto
        {
            Id = r.Medicamento.Id,
            Descricao = r.Medicamento.Descricao,
            NomeComercial = r.Medicamento.NomeComercial,
            NomeQuimico = r.Medicamento.NomeQuimico,
            TipoMedicamento = r.Medicamento.TipoMedicamento,
            Quantidade = r.Medicamento.Quantidade,
            UnidadeMedida = r.Medicamento.UnidadeMedida,
            Imagem = r.Medicamento.Imagem,
            CategoriaId = r.Medicamento.CategoriaId,
            CategoriaDescricao = r.Medicamento.Categoria.Descricao
        }
    }).ToList();

    if (reservas.Count == 0)
        return Results.NotFound(new BaseResponse("Nenhuma reserva encontrada para este usuário."));

    return Results.Ok(reservas);
})
.RequireAuthorization()
.WithTags("Reserva");

app.MapPut("reserva/atender/{id}", (FarmaciaContext context, Guid id, ReservaAtenderDto reservaAtenderDto) =>
    {
        var reserva = context.ReservaSet.FirstOrDefault(r => r.Id == id);

        if (reserva == null)
            return Results.NotFound(new BaseResponse("Reserva não encontrada."));

        reserva.EnumTipoAtendimento = reservaAtenderDto.EnumTipoAtendimento;
        reserva.Status = reservaAtenderDto.Status;
        reserva.RetiranteNome = reservaAtenderDto.RetiranteNome;
        reserva.RetiranteCpf = reservaAtenderDto.RetiranteCpf;

        context.SaveChanges();

        return Results.Ok(new BaseResponse("Registro de Atendimento realizado com sucesso!"));
    })
    .RequireAuthorization()
    .WithTags("Reserva");

app.MapPut("reserva/atualizar/{id}", (FarmaciaContext context, Guid id, ReservaAtualizarDto reservaDto) =>
{
    var reserva = context.ReservaSet.FirstOrDefault(r => r.Id == id);

    var resultado = new ReservaAtualizarDtoValidator().Validate(reservaDto);
    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    if (reserva == null)
        return Results.NotFound(new BaseResponse("Reserva não encontrada."));

    reserva.UsuarioId = reservaDto.UsuarioId;
    reserva.MedicamentoId = reservaDto.MedicamentoId;
    reserva.DataReserva = reservaDto.DataReserva;
    reserva.ImagemReceita = reservaDto.ImagemReceita;
    reserva.EnumTipoAtendimento = reservaDto.EnumTipoAtendimento;
    reserva.DataRetirada = reservaDto.DataRetirada;
    reserva.RetiranteNome = reservaDto.RetiranteNome;
    reserva.RetiranteCpf = reservaDto.RetiranteCpf;

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
    var resultado = new UsuarioAdicionarDtoValidator().Validate(usuarioDto);
    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    var cpfExistente = context.UsuarioSet.Any(p => p.CPF == usuarioDto.CPF);
    if (cpfExistente)
        return Results.BadRequest(new BaseResponse("CPF Já Cadastrado."));

    context.UsuarioSet.Add(new Usuario
    {
        Id = Guid.NewGuid(),
        Nome = usuarioDto.Nome,
        CPF = usuarioDto.CPF,
        Telefone = usuarioDto.Telefone,
        Senha = usuarioDto.Senha.EncryptPassword(),
        Email = usuarioDto.Email,
    });

    context.SaveChanges();

    return Results.Created("Created", new BaseResponse("Usuário registrado com sucesso!"));
}).WithTags("Usuario");

app.MapGet("usuario/listar", (FarmaciaContext context) =>
{
    var usuario = context.UsuarioSet.Select(u => new UsuarioListarDto
    {
        Id = u.Id,
        Nome = u.Nome,
        CPF = u.CPF,
        Telefone = u.Telefone,
        Tipo = u.Tipo,
        Email = u.Email
    }).ToList();

    if (usuario.Count == 0)
        return Results.NotFound(new BaseResponse("Nenhum medicamento encontrado."));

    return Results.Ok(usuario);

})
.RequireAuthorization()
.WithTags("Usuario");

app.MapGet("usuario/id/{id}", (FarmaciaContext context, Guid id) =>
{
    var usuario = context.UsuarioSet.FirstOrDefault(u => u.Id == id);

    if (usuario == null)
        return Results.NotFound(new BaseResponse("Usuário não encontrado."));

    return Results.Ok(usuario);
})
.RequireAuthorization()
.WithTags("Usuario");

app.MapGet("usuario/cpf/{cpf}", (FarmaciaContext context, string cpf) =>
{
    var usuario = context.UsuarioSet.FirstOrDefault(u => u.CPF == cpf);

    if (usuario == null)
        return Results.NotFound(new BaseResponse("Usuário não encontrado."));

    return Results.Ok(usuario);
})
.RequireAuthorization()
.WithTags("Usuario");

app.MapPut("usuario/atualizar/{id}", (FarmaciaContext context, Guid id, UsuarioAtualizarDto usuarioDto) =>
{
    var usuario = context.UsuarioSet.FirstOrDefault(u => u.Id == id);

    var resultado = new UsuarioAtualizarDtoValidator().Validate(usuarioDto);
    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));


    if (usuario == null)
        return Results.NotFound(new BaseResponse("Usuário não encontrado."));

    usuario.Nome = usuarioDto.Nome;
    usuario.CPF = usuarioDto.CPF;
    usuario.Telefone = usuarioDto.Telefone;
    usuario.Email = usuarioDto.Email;

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

#region Segurança

app.MapPost("autenticar", (FarmaciaContext context, LoginDto loginDto) =>
{
    var usuario = context.UsuarioSet.FirstOrDefault(u => u.CPF == loginDto.Login && u.Senha == loginDto.Senha.EncryptPassword());
    if (usuario is null)
        return Results.BadRequest(new BaseResponse("Usuário e senha inválidos"));
    {
        var claims = new[]
        {
            new Claim("Id", usuario.Id.ToString()),
            new Claim("Nome", usuario.Nome),
            new Claim("Telefone", usuario.Telefone),
            new Claim(ClaimTypes.Role, usuario.Tipo.ToString())
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
.WithTags("Segurança");

app.MapPost("gerar-chave-reset-senha", (FarmaciaContext context, GerarResetSenhaDto gerarResetSenhaDto) =>
{
    var resultado = new GerarResetSenhaDtoValidator().Validate(gerarResetSenhaDto);
    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    var usuario = context.UsuarioSet.FirstOrDefault(p => p.Email == gerarResetSenhaDto.Email);

    if (usuario is not null)
    {
        var codigo = new Random().Next(100000, 999999).ToString();
        usuario.CodigoResetSenha = codigo;
        context.UsuarioSet.Update(usuario);
        context.SaveChanges();

        var emailService = new EmailService();
        var corpoEmail = $"Seu código para redefinir a senha é: <b>{codigo}</b>";
        var enviarEmailResponse = emailService.EnviarEmail(gerarResetSenhaDto.Email, "Código de Redefinição de Senha", corpoEmail, true);
        if (!enviarEmailResponse.Sucesso)
            return Results.BadRequest(new BaseResponse("Erro ao enviar o e-mail: " + enviarEmailResponse.Mensagem));
    }

    return Results.Ok(new BaseResponse("Se o e-mail informado estiver correto, você receberá o código por e-mail."));
}).WithTags("Segurança");


app.MapPut("resetar-senha", (FarmaciaContext context, ResetSenhaDto resetSenhaDto) =>
{
    var resultado = new ResetSenhaDtoValidator().Validate(resetSenhaDto);
    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    var usuario = context.UsuarioSet.FirstOrDefault(p =>
        p.Email == resetSenhaDto.Email &&
        p.CodigoResetSenha == resetSenhaDto.Codigo);

    if (usuario is null)
        return Results.BadRequest(new BaseResponse("Código inválido ou expirado."));

    usuario.Senha = resetSenhaDto.NovaSenha.EncryptPassword();
    usuario.CodigoResetSenha = null;

    context.UsuarioSet.Update(usuario);
    context.SaveChanges();

    return Results.Ok(new BaseResponse("Senha alterada com sucesso."));
}).WithTags("Segurança");


app.MapPut("alterar-senha", (FarmaciaContext context, ClaimsPrincipal claims, AlterarSenhaDto alterarSenhaDto) =>
{
    var resultado = new AlterarSenhaDtoValidator().Validate(alterarSenhaDto);
    if (!resultado.IsValid)
        return Results.BadRequest(resultado.Errors.Select(error => error.ErrorMessage));

    var userIdClaim = claims.FindFirst("Id")?.Value;
    if (userIdClaim == null)
        return Results.Unauthorized();

    var userId = Guid.Parse(userIdClaim);
    var startup = context.UsuarioSet.FirstOrDefault(p => p.Id == userId);
    if (startup == null)
        return Results.NotFound(new BaseResponse("Usuário não encontrado."));

    startup.Senha = alterarSenhaDto.NovaSenha.EncryptPassword();
    context.UsuarioSet.Update(startup);
    context.SaveChanges();

    return Results.Ok(new BaseResponse("Senha alterada com sucesso."));
}).WithTags("Segurança");

#endregion

app.Run();
