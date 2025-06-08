Projeto backend em .NET que permite o gerenciamento de pessoas e endereços de rios associados a essas pessoas. Cada endereço pode indicar se é uma região com risco de alagamento, promovendo controle e mapeamento inteligente de áreas de risco.

🛠️ Tecnologias Utilizadas
ASP.NET Core

Entity Framework Core

SQL Server (ou outro banco relacional via EF)

C#

📁 Estrutura Principal
PersonController: gerencia operações de CRUD para pessoas.

RiverAddressController: gerencia operações de CRUD para endereços de rios vinculados às pessoas.

📦 Instalação e Execução
bash
Copy
Edit
# 1. Clone o repositório
git clone https://github.com/seu-usuario/follow-rivers.git
cd follow-rivers

# 2. Restaure os pacotes NuGet
dotnet restore

# 3. Atualize o banco de dados (se estiver usando EF Migrations)
dotnet ef database update

# 4. Execute o projeto
dotnet run
🚀 Endpoints da API
📍 PersonController /api/person
Verbo	Rota	Ação
GET	/api/person	Lista todas as pessoas
POST	/api/person	Cria uma nova pessoa
POST	/api/person/login	Realiza login com email e senha
PUT	/api/person/{id}	Atualiza os dados de uma pessoa
DELETE	/api/person/{id}	Remove uma pessoa do sistema

DTO esperado no POST/PUT/Login:

json
Copy
Edit
{
  "email": "usuario@email.com",
  "senha": "123456",
  "name": "Nome do Usuário"
}
🌍 RiverAddressController /api/riveraddress
Verbo	Rota	Ação
GET	/api/riveraddress	Lista todos os endereços de rios
POST	/api/riveraddress	Cria um novo endereço de rio vinculado a uma pessoa
PUT	/api/riveraddress/{id}	Atualiza um endereço de rio
DELETE	/api/riveraddress/{id}	Deleta um endereço de rio

DTO esperado no POST/PUT:

json
Copy
Edit
{
  "address": "Rua do Rio, 123",
  "canCauseFlood": true,
  "personId": 1
}
🧠 Observações Técnicas
O projeto não utiliza autenticação JWT; o login apenas valida as credenciais e retorna dados do usuário.

Os relacionamentos são feitos via EF Core com Include() para carregamento das entidades relacionadas.

Os DTOs ajudam a proteger e estruturar os dados de entrada.

🗃️ Banco de Dados
Tabelas:

Persons

RiverAddresses (com chave estrangeira para Persons)
