# Desafio Técnico - Hiper Software

Este projeto é um MVP de processamento de pedidos desenvolvido para o desafio técnico da Hiper.

## Tecnologias Utilizadas
- .NET 9 (Web API & Worker Service)
- SQL Server Express (via Docker)
- RabbitMQ (via Docker)
- Entity Framework Core
- React

## Como rodar o projeto
1. Certifique-se de ter o Docker instalado.
2. Na raiz do projeto, execute: `docker-compose up -d`
3. Execute as migrations: `dotnet ef database update --project Hiper.Desafio.Infra --startup-project Hiper.Desafio.Api`
4. Terminal 1 - Rode a API: `dotnet run --project Hiper.Desafio.Api`
5. Terminal 2 - Rode o worker: 'dotnet run --project Hiper.Desafio.Worker'
6. Terminal 3 (Web) - 'cd hiper-desafio-web && npm start'

## Patterns Implementados
- [x] Repository Pattern
- [x] Dependency Injection
- [x] Strategy Pattern 
- [x] Publisher/Subscriber com RabbitMQ 

## Testando o Fluxo
Para testar a integração completa (API -> RabbitMQ -> Worker):

Envie um POST para http://localhost:5033/api/pedidos:

JSON

{
  "descricao": "Venda Senior Hiper",
  "valor": 1000,
  "tipoCliente": "VIP"
}
Verifique o log do Worker. Você verá o pedido sendo recebido e processado em tempo real.
Acesse o painel do RabbitMQ em http://localhost:15672 (guest/guest) para monitorar as filas.

## Fluxo da Aplicação
Entrada: O usuário cadastra um pedido via React.
Negócio: A API recebe o pedido e utiliza o Strategy Pattern para identificar se o cliente é VIP ou Comum, aplicando o desconto sem poluir o Controller com if/else.
Persistência: O pedido é salvo no SQL Server com o status "Processando".
Evento: A API publica o pedido no RabbitMQ.
Consumo: O Worker Service captura a mensagem da fila, simulando o processamento final (como faturamento ou integração externa).
