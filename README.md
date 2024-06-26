## ProductAPI

## Descrição
Este projeto é uma API para gerenciar produtos.

## Escolha do Code-First
Optei por usar o approach Code-First para desenvolver esta API devido à sua facilidade de implementação e manutenção. Com o Code-First, posso definir os modelos de dados e, em seguida, gerar automaticamente o banco de dados correspondente a partir desses modelos, facilitando o desenvolvimento e a evolução do projeto.

## Configuração e Execução

## Requisitos
- [.NET Core SDK 8.0 ou superior](https://dotnet.microsoft.com/download)
- [Git](https://git-scm.com/downloads)
- [SQL Server](https://www.microsoft.com/sql-server)

- ## :computer: Tecnologias
Esse projeto foi feito utilizando as seguintes tecnologias:

* [EF Core](https://learn.microsoft.com/pt-br/ef/)      
* [ASP.Net](https://dotnet.microsoft.com/pt-br/apps/aspnet)      
* [SQL Server](https://www.microsoft.com/sql-server)      
* [XUnit](https://xunit.net)

## :construction_worker: Instalação
```bash
# Clone o Repositório
$ git clone https://github.com/ViniRodriguess/WakeProductAPI.git
```
## 📦 Configuração do Banco de Dados

1. Abra o arquivo `appsettings.json` e configure a conexão com o banco de dados SQL Server.

2. Configure o dotnet-ef para rodar as migrations:

```bash
# Instale o dotnet-ef global
$ dotnet tool install --global dotnet-ef
# Faça update da ferramente
$ dotnet tool update --global dotnet-ef
# Verifique a instalação
$ dotnet ef
```

3. Execute as migrations para criar o banco de dados, as tabelas e os dados iniciais:
```bash
# Rode as migrations (em alguns casos, é necessário especificar o caminho do projeto com a flag --project)
$ dotnet ef database update
```

## Executando a API
1. Para iniciar a aplicação, execute o seguinte comando na raiz do projeto:

```bash
$ dotnet run
```

2. Para rodar os testes unitários e de integração:

```bash
$ dotnet test
```
### Swagger screenshot
![Swagger](https://github.com/ViniRodriguess/WakeProductAPI/assets/79362178/065ffa23-a0e9-4647-85c5-76b48d71df49)

💻 Acesse a API através do URL base: `https://localhost:5001` (ou a url informada no terminal).

## Rotas da API
- `GET /api/Product`: Retorna todos os produtos.
- `GET /api/Product/{id}`: Retorna um produto pelo seu ID.
- `POST /api/Product`: Adiciona um novo produto.
- `PUT /api/Product/{id}`: Atualiza um produto existente.
- `DELETE /api/Product/{id}`: Exclui um produto existente.
- `SEARCH /api/Product/search?name=`: Filtra os dados pelo nome do produto.
- `ORDER /api/Product/order?sortBy=`: Ordena os dados por uma coluna específica.

## Github Actions

- Foi implementado uma pipeline através das Actions do Github.
`.github\workflows\dotnet-desktop.yml`
Este arquivo configura um job que será executado sempre que houver um push na branch main. O job executa os seguintes passos:

1. Faz o checkout do código-fonte.
2. Configura o ambiente .NET Core.
3. Restaura as dependências do projeto.
4. Executa os testes automatizados.

## Contribuição
Contribuições são bem-vindas! Sinta-se à vontade para enviar pull requests ou relatar problemas.


