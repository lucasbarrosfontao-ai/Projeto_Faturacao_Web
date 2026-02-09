# 📑 FaturaFlow (Sistema de Faturação Web)

## 📝 Descrição do Projeto
Este projeto foi desenvolvido durante a Formação em Contexto de Trabalho (FCT) na empresa **Openvia Mobility**. Trata-se de uma aplicação Web para gestão de faturação, que integra conceitos avançados de comunicação assíncrona, contentorização e boas práticas de arquitetura de software.

O foco principal foi criar um sistema robusto capaz de gerir faturas, calcular impostos e notificar clientes de forma automática e desacoplada.

## 🚀 Tecnologias Utilizadas
*   **Linguagem:** C# (.NET 8/9)
*   **Framework Web:** Blazor (Razor Components)
*   **Base de Dados:** MySQL
*   **Mensageria (Message Broker):** RabbitMQ (para processamento de filas de e-mail)
*   **Ambiente Virtualizado:** Docker & Docker-compose
*   **Testes de Email:** Mailtrap (Servidor SMTP de teste)
*   **Controlo de Versões:** Git & GitHub

## 🏗️ Arquitetura
O projeto segue os princípios de **Clean Architecture**, promovendo a separação de responsabilidades em camadas:
- **Domain:** Entidades e regras de negócio.
- **Application:** Casos de uso e interfaces.
- **Infrastructure:** Persistência de dados e integrações externas (RabbitMQ).
- **Presentation (Blazor):** Interface de utilizador.

## ✨ Funcionalidades Principais
- [x] **Gestão de Faturas:** Criação, listagem e detalhe de faturas.
- [x] **Cálculo Automático:** Processamento de IVA, totais líquidos e brutos em tempo real.
- [x] **Notificações Assíncronas:** Ao emitir uma fatura, uma mensagem é enviada para o **RabbitMQ**.
- [x] **Worker Service:** Um serviço em segundo plano consome a fila e envia o e-mail de confirmação via **Mailtrap**.
- [x] **Validações de Dados:** Garante a integridade da informação antes da persistência na base de dados.

## 📦 Como Executar o Projeto
Para rodar o projeto localmente, é necessário ter o **Docker** instalado.

1. Clone o repositório:
     git clone https://github.com/teu-usuario/nome-do-repositorio.git
2. Navegue até a pasta do projeto:
     cd nome-do-repositorio
3. Inicie os serviços via Docker Compose:
      docker-compose up -d
Este comando irá subir a aplicação, a base de dados MySQL e o servidor RabbitMQ automaticamente.
4. Aceda à aplicação no seu browser: 
     http://localhost:5000 (ou a porta configurada).
