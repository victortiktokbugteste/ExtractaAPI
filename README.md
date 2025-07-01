# Projeto IntegracaoAngular

Este é um projeto de API para sistema de seguros utilizando .NET 8.0 com arquitetura limpa.

## Requisitos

- Docker
- Docker Compose
- Acesso ao banco de dados SQL Server configurado no appsettings.json

## Configuração e Execução com Docker

### 1. Clone o repositório

```bash
git clone <url-do-repositorio>
cd IntegracaoAngular
```

### 2. Execute com Docker Compose

```bash
docker-compose up --build -d
```

Este comando irá:
- Construir a imagem da API .NET
- Iniciar a API na porta 8080 (HTTP) e 8443 (HTTPS)
- Usar a conexão de banco de dados configurada no appsettings.json

### 3. Acesse a API

A API estará disponível em:
- Swagger UI: http://localhost:8080/swagger
- API: http://localhost:8080

### 4. Login da Aplicação

```bash
Usuário: teste@gmail.com
Senha: 1234
```

## Estrutura do Projeto

- **IntegracaoAngular**: Projeto principal da API (Controllers, Middleware)
- **Application**: Camada de aplicação (Commands, Queries, DTOs)
- **Domain**: Camada de domínio (Entidades, Interfaces)
- **Infrastructure**: Camada de infraestrutura (Repositórios, Serviços)

## Banco de Dados

O projeto está configurado para usar o banco de dados especificado no arquivo appsettings.json:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=tcp:claimdb.database.windows.net,1433; Database=claimdb; User Id=victor; Password=@Dev2025;Trusted_Connection=False;Encrypt=True;"
}
```


## LOG DE ERROS

Os LOGS DE ERRO ficam na tabela ApplicationMiddlewareLogError.

```json
select * from ApplicationMiddlewareLogError
```