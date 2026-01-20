# Desafio Técnico - Hiper Software

Este projeto é um MVP de processamento de pedidos desenvolvido para o desafio técnico da Hiper.

## Tecnologias Utilizadas
- .NET 9 (Web API & Worker Service)
- SQL Server Express (via Docker)
- RabbitMQ (via Docker)
- Entity Framework Core
- React (Frontend - Em breve)

## Como rodar o projeto
1. Certifique-se de ter o Docker instalado.
2. Na raiz do projeto, execute: `docker-compose up -d`
3. Execute as migrations: `dotnet ef database update --project Hiper.Desafio.Infra --startup-project Hiper.Desafio.Api`
4. Rode a API: `dotnet run --project Hiper.Desafio.Api`

## Patterns Implementados
- [x] Repository Pattern
- [x] Dependency Injection
- [ ] Strategy Pattern (Próximo passo)
- [ ] Publisher/Subscriber com RabbitMQ (Próximo passo)
