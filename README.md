# 🌊 Follow Rivers API

API RESTful construída com ASP.NET Core 8 para monitoramento de pontos de rios, cadastro de pessoas responsáveis e emissão de alertas de inundação.

## 👥 Integrantes

- Vanessa Dias (RM556936)

## 🧭 Justificativa do Domínio e da Arquitetura

O domínio foi escolhido para apoiar equipes responsáveis pelo monitoramento de rios suscetíveis a enchentes:

- **Pessoas** representam os agentes responsáveis pelo monitoramento em campo e pelo registro de alertas.
- **Pontos de rio** (RiverAddresses) concentram informações dos locais monitorados com risco de inundação.
- **Alertas de inundação** (FloodAlerts) documentam ocorrências e níveis de severidade para atuação preventiva.

A API segue uma arquitetura em camadas simples, utilizando Entity Framework Core para persistência e controllers RESTful para exposição dos recursos. As respostas utilizam DTOs, paginação, HATEOAS e validação de dados, garantindo boas práticas de APIs REST.

## 🧰 Tecnologias

- .NET 8 / ASP.NET Core Web API
- Entity Framework Core
- Oracle Database (via Oracle EF Core Provider)
- Swagger / OpenAPI (com anotações e exemplos de payload)

## 🚀 Como Executar

```bash
# 1. Clonar o repositório
git clone https://github.com/seu-usuario/follow-rivers.git
cd follow-rivers

# 2. Restaurar dependências
dotnet restore

# 3. Aplicar migrations (opcional, caso utilize banco Oracle configurado)
dotnet ef database update

# 4. Executar a API
dotnet run --project FollowRivers/FollowRivers.csproj
```

A API ficará disponível em `https://localhost:5001` (HTTPS) e `http://localhost:5000` (HTTP). A documentação interativa do Swagger pode ser acessada em `/swagger` durante o modo de desenvolvimento.

## ✅ Testes

```bash
dotnet test
```

> No momento não há projetos de teste automatizado, mas o comando acima garante a execução automática quando forem adicionados.

## 📘 Documentação OpenAPI

A API expõe documentação no Swagger com:

- Descrição e sumário dos endpoints.
- Anotações de parâmetros e códigos de status.
- Exemplos de payloads para criação/atualização de recursos.
- Modelos de dados com metadados e exemplos.

## 📡 Endpoints

Os endpoints seguem o padrão REST e retornam recursos com links HATEOAS. Todos os `GET` suportam paginação via `pageNumber` (padrão 1) e `pageSize` (padrão 10, máximo 50).

### 👤 Pessoas `/api/person`

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/person` | Lista paginada de pessoas cadastradas. |
| GET | `/api/person/{id}` | Obtém uma pessoa específica. |
| POST | `/api/person` | Cria uma nova pessoa (links de consulta, atualização e remoção). |
| PUT | `/api/person/{id}` | Atualiza os dados de uma pessoa. |
| DELETE | `/api/person/{id}` | Remove uma pessoa do sistema. |

**Payload de criação/atualização**
```json
{
  "name": "Mariana Costa",
  "email": "mariana.costa@email.com",
  "senha": "Senha@2024"
}
```

### 🌍 Pontos de Rio `/api/riveraddress`

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/riveraddress` | Lista paginada de pontos monitorados. |
| GET | `/api/riveraddress/{id}` | Retorna um ponto monitorado específico. |
| POST | `/api/riveraddress` | Registra um novo ponto de monitoramento ligado a uma pessoa. |
| PUT | `/api/riveraddress/{id}` | Atualiza as informações de um ponto monitorado. |
| DELETE | `/api/riveraddress/{id}` | Remove um ponto monitorado. |

**Payload de criação/atualização**
```json
{
  "address": "Margem esquerda do Rio Tietê, km 23",
  "canCauseFlood": true,
  "personId": 1
}
```

### 🚨 Alertas de Inundação `/api/floodalert`

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/floodalert` | Lista paginada de alertas registrados. |
| GET | `/api/floodalert/{id}` | Recupera um alerta específico. |
| POST | `/api/floodalert` | Cria um novo alerta ligado a um ponto monitorado e uma pessoa. |
| PUT | `/api/floodalert/{id}` | Atualiza as informações de um alerta. |
| DELETE | `/api/floodalert/{id}` | Remove um alerta. |

**Payload de criação/atualização**
```json
{
  "title": "Risco crítico de inundação",
  "description": "Volume de chuvas acumulado em 48h ultrapassou o limite seguro. Evacuação recomendada.",
  "severity": "Crítico",
  "personId": 1,
  "riverAddressId": 1
}
```

## 🔗 HATEOAS e Paginação

As respostas retornam objetos com:

- `data`: dados do recurso solicitado.
- `links`: ações relacionadas (`self`, `update`, `delete`, `next`, `previous`).
- `pageNumber`, `pageSize`, `totalItems` e `totalPages` em respostas paginadas.

Exemplo de resposta `GET /api/person`:

```json
{
  "items": [
    {
      "data": {
        "personId": 1,
        "name": "Mariana Costa",
        "email": "mariana.costa@email.com"
      },
      "links": [
        { "href": "https://localhost:5001/api/person/1", "rel": "self", "method": "GET" },
        { "href": "https://localhost:5001/api/person/1", "rel": "update", "method": "PUT" },
        { "href": "https://localhost:5001/api/person/1", "rel": "delete", "method": "DELETE" }
      ]
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalItems": 1,
  "totalPages": 1,
  "hasPrevious": false,
  "hasNext": false,
  "links": [
    { "href": "https://localhost:5001/api/person?pageNumber=1&pageSize=10", "rel": "self", "method": "GET" }
  ]
}
```

## 📦 Possíveis Melhorias Futuras

- Publicação via container Docker.
- Testes automatizados de integração.
- Autenticação e autorização com JWT.
- Observabilidade com logs estruturados e métricas.
