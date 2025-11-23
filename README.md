<h2>🌱 EcoWork — API .NET 8</h2>
<p>Global Solution – Futuro do Trabalho • FIAP 2025</p>

---

# Visão Geral

A **EcoWork API** é uma solução tecnológica voltada ao tema **“O Futuro do Trabalho”**, oferecendo uma plataforma simples e escalável para:

- Gerenciar **Departamentos**  
- Registrar **Metas Sustentáveis**  
- Realizar operações através de uma **API RESTful moderna**  
- Aplicar **boas práticas corporativas** de desenvolvimento back-end  
- Demonstrar habilidades de **observabilidade, testes e versionamento de API**

O objetivo da API é representar como empresas podem utilizar tecnologia para orientar colaboradores rumo a práticas mais eficientes, sustentáveis e rastreáveis no ambiente de trabalho.

---

# Arquitetura da Aplicação

✔️ **.NET 8 Web API**  
✔️ **Entity Framework Core + Migrations**  
✔️ **PostgreSQL (Render Cloud)**  
✔️ **AutoMapper para DTOs**  
✔️ **xUnit + TestServer (Testes de Integração)**  
✔️ **Swagger/OpenAPI**  
✔️ **OpenTelemetry (Tracing)**  
✔️ **HealthChecks (Monitoramento)**  
✔️ **Dockerfile + Deploy no Render**

Estrutura do projeto:

```
EcoWork.Api/
 ├── Controllers/
 │    └── v1/
 ├── DTOs/
 ├── Models/
 ├── Mappings/
 ├── Persistence/
 ├── Utils/
 ├── Program.cs
 ├── Dockerfile
 └── appsettings.json

EcoWork.Tests/
 ├── DepartamentoTests.cs
 ├── CustomWebApplicationFactory.cs
 └── EcoWork.Tests.csproj
```

---

# **1. Versionamento da API (10 pts)**

Toda a API segue o padrão:

```
/api/v1/...
```

Nova versão futura pode ser criada com:  
```
/api/v2
```

---

# **2. Boas Práticas REST (30 pts)**

### Paginação
Todas as rotas de listagem possuem:

```
?page=1&pageSize=10
```

### HATEOAS
Cada recurso retorna links navegáveis:

```json
"links": {
  "self": "/api/v1/departamentos/3",
  "update": "/api/v1/departamentos/3",
  "delete": "/api/v1/departamentos/3"
}
```

### Status Codes corretos
- `200 OK`
- `201 Created`
- `204 NoContent`
- `404 NotFound`
- `400 BadRequest`

### Verbos HTTP
- GET  
- POST  
- PUT  
- DELETE  

Tudo implementado corretamente.

---

# **3. Monitoramento e Observabilidade (15 pts)**

Implementado:

 **Health Check**  
• Rota: `/health`  
• Verifica PostgreSQL + self-check

 **OpenTelemetry Tracing**
Inclui:
- ASP.NET Core instrumentation  
- HTTP Client  
- Entity Framework  
- Console Exporter  

 **Logging Console**  
Com níveis configurados.

---

# 🗄️ **4. Integração e Persistência (30 pts)**

### PostgreSQL
Banco hospedado no **Render**.

### Entity Framework Core + Migrations
Exemplo:

```
Add-Migration InitialCreate
Update-Database
```

### DbContext
Totalmente configurado.

### Deploy funcional
API rodando totalmente online.

---

# **5. Testes Integrados (15 pts)**

Implementados usando:

- xUnit  
- Microsoft.AspNetCore.Mvc.Testing  
- CustomWebApplicationFactory  
- InMemoryDatabase  

Exemplo de teste:

```csharp
[Fact]
public async Task Get_DeveRetornarOk()
{
    var response = await _client.GetAsync("/api/v1/departamentos");
    response.StatusCode.Should().Be(HttpStatusCode.OK);
}
```

Todos os testes passaram com sucesso ✔️

---

# Deploy

### API Online (Render)
**https://ecowork-api-dotnet.onrender.com**

### Exemplos:
- Swagger:  
  https://ecowork-api-dotnet.onrender.com/swagger

- Healthcheck:  
  https://ecowork-api-dotnet.onrender.com/health

---

# Como Rodar Localmente

### 1. Clonar o repositório
```bash
git clone https://github.com/murilors27/ecowork-api-dotnet.git
cd ecowork-api-dotnet/EcoWork.Api
```

### 2. Criar banco local (opcional)

```bash
Update-Database
```

### 3. Rodar API

```bash
dotnet run
```

Swagger aparecerá em:

```
https://localhost:5001/swagger
```

---

# Rotas Principais

## Departamentos
| Método | Rota | Descrição |
|--------|----------|-------------|
| GET | /api/v1/departamentos | Lista paginada |
| GET | /api/v1/departamentos/{id} | Detalhes |
| POST | /api/v1/departamentos | Cria |
| PUT | /api/v1/departamentos/{id} | Atualiza |
| DELETE | /api/v1/departamentos/{id} | Remove |

## Metas Sustentáveis
| Método | Rota | Descrição |
|--------|----------|-------------|
| GET | /api/v1/metassustentaveis | Lista paginada |
| GET | /api/v1/metassustentaveis/{id} | Detalhes |
| POST | /api/v1/metassustentaveis | Cria |
| PUT | /api/v1/metassustentaveis/{id} | Atualiza |
| DELETE | /api/v1/metassustentaveis/{id} | Remove |

---

# Vídeo Demonstrativo (5 min)

Sugestão do fluxo:

1. Abrir Swagger  
2. Mostrar as rotas  
3. Criar um novo recurso  
4. Listar com paginação  
5. Mostrar HATEOAS no retorno  
6. Executar um PUT  
7. Excluir com DELETE  
8. Mostrar `/health`  
9. Mostrar o tracing aparecendo no console  
10. Mostrar testes rodando com sucesso  

---

# Links Importantes

| Tipo | Link |
|------|------|
| Repositório GitHub | https://github.com/murilors27/ecowork-api-dotnet |
| Deploy da API | https://ecowork-api-dotnet.onrender.com |
| Swagger | https://ecowork-api-dotnet.onrender.com/swagger |
| Health | https://ecowork-api-dotnet.onrender.com/health |

---

# Instruções ao Professor

A API está hospedada no Render, e **não utiliza autenticação**, garantindo acesso direto aos endpoints.

Para facilitar a correção, basta:

1. Acessar o **Swagger** pelo link acima  
2. Testar live todos os endpoints  
3. Verificar paginação e HATEOAS nas respostas  
4. Acessar `/health` para validar o PostgreSQL  
5. Confirmar versionamento em `/api/v1/...`  
6. Avaliar testes executando via `dotnet test`  

---

# Conclusão

Este projeto demonstra:

- Desenvolvimento profissional em **.NET 8**  
- Implementação sólida de **REST**, **DDD básico**, **DTOs**, **mapeamentos**, **paginação**, **HATEOAS**  
- Infraestrutura em nuvem com **Render**  
- Banco de dados real com **PostgreSQL**  
- Testes automatizados confiáveis  
- Observabilidade moderna  

A EcoWork API representa uma solução clara, escalável e alinhada com o tema **Futuro do Trabalho**.

---
## Apresentação e Demonstração Técnica 

🔗 *Link para o vídeo:* [em breve]

---

## Equipe de Desenvolvimento

| Nome                                | RM       | GitHub                                |
|-------------------------------------|----------|----------------------------------------|
| **Murilo Ribeiro Santos**           | RM555109 | [@murilors27](https://github.com/murilors27) |
| **Thiago Garcia Tonato**            | RM99404  | [@thiago-tonato](https://github.com/thiago-tonato) |
| **Ian Madeira Gonçalves da Silva**  | RM555502 | [@IanMadeira](https://github.com/IanMadeira) |

**Curso:** Análise e Desenvolvimento de Sistemas  
**Instituição:** FIAP — Faculdade de Informática e Administração Paulista  
**Ano:** 2025
