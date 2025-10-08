# Trainin

Projeto fullstack para gerenciamento/treinamento (frontend em React + backend em .NET). Este repositório contém o frontend (Vite + React), o backend (ASP.NET Core 9) e arquivos de infraestrutura para execução via Docker Compose.

## Visão geral

- Frontend: React + Vite, código em `frontend/`.
- Backend: ASP.NET Core (API) em `backend/API/`.
- Infraestrutura: compose para MySQL, Redis, API e frontend em `infrastructure/docker-compose.yml`.

## Arquitetura do Backend

O backend segue os princípios da **Clean Architecture** (inspirada em Domain-Driven Design - DDD), organizando o código em camadas independentes para promover manutenibilidade, testabilidade e separação de responsabilidades.

### Camadas Principais

1. **Domain Layer** (`backend/API/Domain/`):
   - Contém as regras de negócio centrais.
   - Entidades (Entities), Value Objects, Repositórios (interfaces) e Serviços de Domínio.
   - Independente de frameworks externos.

2. **Application Layer** (`backend/API/Application/`):
   - Orquestra operações de negócio.
   - Contém Serviços de Aplicação, DTOs, Interfaces e Mapeamentos (AutoMapper).
   - Define contratos (interfaces) para repositórios e serviços.

3. **Infrastructure Layer** (`backend/API/Infrastructure/`):
   - Implementa detalhes técnicos (persistência, serviços externos).
   - Repositórios concretos, configurações de banco, integrações (Redis, etc.).

4. **Presentation Layer** (`backend/API/Controllers/`):
   - Controllers ASP.NET Core que expõem APIs REST.
   - Usa injeção de dependência para serviços da Application Layer.

5. **Shared** (`backend/API/Shared/`):
   - Constantes, validadores e utilitários compartilhados.

### Design Patterns Utilizados

- **Repository Pattern**: Abstrai o acesso a dados. Interfaces em `Domain/Repositories/` (ex.: `IUserRepository`) e implementações em `Infrastructure/Persistence/`. Permite troca de provedores de dados sem afetar a lógica de negócio.

- **Unit of Work Pattern**: Gerencia transações e salva mudanças em lote. Implementado em `UnitOfWork.cs`, coordena múltiplas operações de repositório.

- **Notification Pattern**: Centraliza mensagens de erro, aviso e sucesso. `INotificationContext` e `NotificationContext` em `Application/Logger/`. Evita lançar exceções para validações, permitindo acumular notificações e retornar ao cliente.

- **Dependency Injection (DI)**: Usado extensivamente via ASP.NET Core DI. Controllers e serviços injetam interfaces, facilitando testes e desacoplamento.

- **CQRS (Command Query Responsibility Segregation)**: Embora não totalmente implementado, há separação implícita em serviços (ex.: métodos de query vs. command).

- **Value Object**: Em `Domain/ValueObject/`, objetos imutáveis representando conceitos como endereços ou configurações.

- **Mapper Pattern**: AutoMapper para conversão entre Entities e DTOs.

## Estrutura principal

- `frontend/` — aplicação cliente (Vite, TypeScript, Tailwind).
- `backend/API/` — projeto ASP.NET Core (controllers, serviços, domain, migrations).
- `infrastructure/` — docker-compose e orquestração.
- `backend/API/Migrations/` — migrations do Entity Framework.

## Pré-requisitos

- .NET 9 SDK (para rodar o backend localmente)
- Node.js 18+ / npm (para o frontend)
- Docker & Docker Compose (para rodar via containers)
- MySQL (se optar por rodar local sem Docker)

## Rodando localmente (desenvolvimento)

Observação: os comandos abaixo assumem que você está na raiz do repositório.

1) Backend (API)

Vá para a pasta do projeto backend e execute:

```bash
cd backend/API
dotnet restore
dotnet build
dotnet run
```

Por padrão (conforme `Properties/launchSettings.json`) a API é exposta em:

- HTTP: http://localhost:5237
- HTTPS: https://localhost:7181 (quando configurado/ativo)

2) Frontend (desenvolvimento)

```bash
cd frontend
npm install
npm run dev
```

O Vite geralmente expõe a aplicação em `http://localhost:5173` (porta padrão do Vite). Ajuste a URL da API no arquivo `.env` local ou utilizando a variável `VITE_API_URL` conforme seu fluxo.

## Rodando com Docker Compose (produção/local com containers)

Existe um `docker-compose.yml` em `infrastructure/` que descreve serviços: frontend (Nginx), api, mysql, redis e redis-insight.

Para subir os containers:

```bash
cd infrastructure
docker compose up --build
```

Pontos importantes do compose (valores e portas mostrados no arquivo):

- Frontend (Nginx): mapeado em `80:80`.
- API: exposta como `8080:8080` (variável `PORT=8080` passada ao container).
- MySQL: `3306:3306`.
- Redis: `6379:6379`.
- Redis Insight: `8001:8001`.

Se os contextos do build (paths relativos) não baterem com sua estrutura, ajuste os caminhos `context:` no compose para apontar para `./frontend` e `./backend/API` conforme necessário.

### Variáveis de ambiente (principais)

- `DATABASE_URL` / `DB_USER` / `DB_PASSWORD` / `DB_HOST` — conexão com o MySQL.
- `PORT` — porta usada pela API no container (ex.: `8080`).
- `JWT_SECRET` — segredo para geração/validação de tokens JWT.
- `GOOGLE_API_KEY` — chave para integrações Google (OAuth etc.).
- `VITE_API_URL` — URL base da API usada pelo frontend em tempo de build/execução.

Defina essas variáveis no ambiente (ou em um arquivo `.env`) antes de subir os containers, ou preencha via `docker-compose.yml`/secrets quando for o caso.

## Testes

O projeto contém testes unitários/integrados no backend (`backend/API.Test/` e pastas `Unit/` e `Integrations/`). Para rodar os testes do backend:

```bash
cd backend
dotnet test
```

Front-end: não há testes configurados no `package.json` do frontend além de lint (ESLint). Rode:

```bash
cd frontend
npm run lint
```

## Build para produção

1) Backend

```bash
cd backend/API
dotnet publish -c Release -o ./publish
```

2) Frontend

```bash
cd frontend
npm run build
```

O Dockerfile do frontend já empacota o build no Nginx (`/usr/share/nginx/html`). O Dockerfile do backend publica a aplicação e roda o `API.dll`.

## Migrações de banco de dados

As migrations do EF estão em `backend/API/Migrations/`. Para aplicar (local):

```bash
cd backend/API
dotnet tool restore
dotnet ef database update
```

Observação: configure a `ConnectionStrings:DefaultConnection` em `appsettings.json` ou como variável de ambiente (`DATABASE_URL`) antes de aplicar as migrations.

## Internacionalização

O projeto contém arquivos de idioma em `backend/API/locales/` (por exemplo `pt-BR` e `en-US`). O frontend também usa i18n via `react-i18next`.
