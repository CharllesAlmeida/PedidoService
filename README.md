# PedidoService - Microserviço de Pedidos com SQL Server, MongoDB, Redis e RabbitMQ via Docker.

Este projeto implementa um microserviço para gerenciamento de pedidos com integração a plataformas parceiras, seguindo princípios de arquitetura limpa.

## ✅ Funcionalidades
- Criação de pedidos via API REST.
- Armazenamento híbrido: dados principais no SQL Server e itens no MongoDB.
- Publicação de evento assíncrono após criação do pedido.
- Endpoint GET com resposta integrada e cache em Redis por tempo configurável.
- Validações com FluentValidation e mensagens em `.resx`.
- Testes unitários para camadas de aplicação e infraestrutura.

---

## ⚙️ Serviços necessários

Este projeto depende dos seguintes serviços para funcionar corretamente:

- 📦 **SQL Server** – armazenamento dos dados principais do pedido
- 📦 **MongoDB** – armazenamento da lista de itens do pedido
- 📦 **Redis** – cache da resposta do endpoint GET `/pedidos/{id}`
- 🚀 **PedidoService.API** – aplicação principal em .NET 8 (executada via contêiner)

Todos esses serviços já estão definidos e configurados no `docker-compose.yml` do projeto.

---

## 🚀 Como executar localmente

### ✅ Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) instalado e em execução

---

### ▶️ Executar tudo com Docker Compose

O projeto já contém os arquivos `Dockerfile` e `docker-compose.yml` configurados.

Para subir todos os serviços e executar a aplicação automaticamente, execute na raiz do projeto:

```bash
docker-compose up --build
```

Esse comando irá:

- 🔧 Compilar e iniciar a aplicação `PedidoService.API` em um contêiner
- 🚀 Subir os seguintes serviços:
  - SQL Server (porta `1433`)
  - MongoDB (porta `27017`)
  - Redis (porta `6379`)

Acesse a API via Swagger:

🔗 [http://localhost:8080/swagger](http://localhost:8080/swagger)

---

### 💻 Alternativa: executar manualmente fora do Docker

Se preferir rodar a aplicação no ambiente local:

```bash
dotnet restore
dotnet build
dotnet run --project PedidoService.API
```

> Certifique-se de que os serviços Redis, MongoDB e SQL Server estejam rodando (via Docker ou localmente).

---

## 🧠 Decisões Técnicas

### 🧱 Arquitetura

- **Clean Architecture**
- **DDD (Domain-Driven Design)**
- **Camadas**:
  - `Domain`: entidades e regras de negócio
  - `Application`: casos de uso, validações
  - `Infrastructure`: persistência (SQL, Mongo), cache (Redis)
  - `API`: endpoints e injeção de dependências

#### 💡 Justificativa da Arquitetura

A arquitetura limpa (Clean Architecture) foi escolhida por suas vantagens em projetos de médio e grande porte que exigem:

- **Separação clara de responsabilidades**, promovendo organização e modularidade.
- **Independência da infraestrutura**, permitindo trocar bancos de dados, mensageria ou cache sem impactar a lógica de negócio.
- **Alta testabilidade**, com regras de negócio isoladas em camadas puras.
- **Facilidade de manutenção e evolução**, alinhada com os princípios SOLID.
- **Adoção natural de DDD**, com as entidades e regras de domínio no centro da aplicação.

Essa abordagem oferece uma base sólida para crescimento, integração com novos serviços e adaptação a mudanças futuras.

---

### 🔄 Redis para Cache

O endpoint `GET /pedidos/{id}` utiliza cache Redis com tempo configurável via `appsettings.json`:

```json
"CacheSettings": {
  "PedidoPorIdSegundos": 120
}
```

---

### 📏 FluentValidation com `.resx`

- Validação no `CriarPedidoDtoValidator`
- Mensagens armazenadas em `ValidationMessages.resx`
- Suporte à internacionalização e organização de mensagens

---

### 🧩 Design Patterns aplicados

- **CQRS**: separação entre comandos (escrita) e queries (leitura)
- **Dependency Injection**: via `IServiceCollection`
- **Event-Driven**: evento `PedidoCriado` publicado após criação do pedido

---

## 🧪 Testes Unitários

Testes inclusos no projeto:

- ✅ `CriarPedidoCommandHandlerTests` – valida criação de pedidos
- ✅ `PedidoQueryServiceTests` – valida uso de Redis e leitura integrada

---

© 2025 DevSenior.NET
=======


