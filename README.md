📑 FaturaFlow — Sistema de Faturação Web
🧭 Visão Geral

O FaturaFlow é uma aplicação Web de faturação e gestão de inventário desenvolvida em .NET com foco em arquitetura moderna, escalabilidade e automação de processos.
O sistema foi concebido para simular o contexto real de uma Pequena ou Média Empresa (PME), onde a performance e a fiabilidade são fatores críticos.

Este projeto foi desenvolvido no âmbito da Formação em Contexto de Trabalho (FCT) e da Prova de Aptidão Profissional (PAP) do curso de Técnico de Informática de Gestão, na empresa Openvia Mobility.

🎯 Objetivo do Projeto

Criar um sistema de faturação que:

     automatize o ciclo completo de venda;
     evite bloqueios da interface durante tarefas pesadas;
     utilize comunicação assíncrona para geração de documentos e envio de emails;
     seja facilmente replicável em qualquer ambiente através de Docker.

🚀 Tecnologias Utilizadas
Categoria	               Tecnologias
Linguagem	C#             (.NET 8)
Framework Web	          Blazor Server (Razor Components)
Base de Dados	          MySQL
ORM	                    Entity Framework Core
Mensageria	          RabbitMQ
Automação	               Worker Service (.NET)
PDFs	                    QuestPDF
Email	               MailKit + Mailtrap (ambiente de testes)
Infraestrutura	          Docker & Docker Compose
Controlo de Versões	     Git & GitHub

🏗️ Arquitetura da Solução

O FaturaFlow segue princípios de Clean Architecture e desacoplamento, separando claramente responsabilidades técnicas e de negócio.

Componentes Principais
     Web Application (Blazor)
          Interface de utilizador responsável pela gestão de clientes, produtos, fornecedores e faturação.

     RabbitMQ (Message Broker)
          Responsável por receber mensagens de faturação e distribuí-las de forma assíncrona.

     Worker Service
          Serviço independente que consome mensagens da fila para:
               gerar o PDF da fatura;
               enviar o email ao cliente;
               reportar erros sem comprometer a faturação.

     MySQL
          Base de dados relacional que garante persistência e integridade dos dados.

📌 O utilizador nunca fica bloqueado enquanto tarefas pesadas são executadas.

🔄 Fluxo de Funcionamento (Resumo)

     1- O utilizador cria uma fatura na aplicação Web.
     2- A fatura é validada e gravada na base de dados.
     3- O sistema envia uma mensagem para o RabbitMQ.
     4- O Worker Service consome a mensagem.
     5- O Worker gera o PDF da fatura.
     6- O Worker tenta enviar o email ao cliente:
          se falhar, o erro é registado e comunicado;
          se o cliente não tiver email, o envio é ignorado.
     7- A faturação permanece válida em qualquer cenário.

✨ Funcionalidades Principais

     ✔️ Gestão de Clientes, Fornecedores e Produtos
     ✔️ Emissão de Faturas com cálculo automático de IVA
     ✔️ Atualização automática de stock
     ✔️ Geração dinâmica de faturas em PDF
     ✔️ Envio automático de emails (assíncrono)
     ✔️ Tratamento de erros sem perda de dados
     ✔️ Dashboard de vendas (Business Intelligence)
     ✔️ Ambiente totalmente contentorizado (Docker)

⚠️ Limitações Atuais
     Este projeto foi desenvolvido em contexto académico e possui algumas limitações assumidas:
          Ausência de autenticação e perfis de utilizador (login);
          Não integração com APIs oficiais da Autoridade Tributária;
          Ausência de testes automatizados (unitários/integrados).
     Estas limitações são reconhecidas e documentadas como pontos de evolução futura.

🧠 Aprendizagens-Chave

Este projeto permitiu consolidar competências em:
     arquiteturas distribuídas;
     comunicação assíncrona;
     contentorização de aplicações;
     lógica de negócio aplicada à gestão;
     tratamento de erros e resiliência de sistemas.

📚 Contexto Académico

Projeto desenvolvido no âmbito da:
     Formação em Contexto de Trabalho (FCT)
     Prova de Aptidão Profissional (PAP)
     Curso: Técnico de Informática de Gestão

📦 Como Executar o Projeto

     O FaturaFlow utiliza Docker Compose para garantir que todos os serviços (Web, Worker, Base de Dados e RabbitMQ) funcionem de forma integrada e consistente em qualquer ambiente.

🔧 Pré-requisitos

Antes de começar, certifica-te de que tens instalado:

     Docker
     Docker Compose
     Git

▶️ Passo 1 — Clonar o Repositório
     git clone https://github.com/lucasbarrosfontao-ai/Projeto_Faturacao_Web.git

cd Projeto_Faturacao_Web

▶️ Passo 2 — Configuração do ficheiro .env

     O projeto utiliza variáveis de ambiente para configurar credenciais e serviços externos.

     Na raiz do projeto, localiza o ficheiro:

          .env_exemplo

     Cria uma cópia com o nome:

          .env

     Edita o ficheiro .env e preenche os valores conforme o teu ambiente.

     Exemplo:

          # Base de Dados
          MYSQL_ROOT_PASSWORD=root
          MYSQL_DATABASE=faturaflow
          MYSQL_USER=faturaflow_user
          MYSQL_PASSWORD=faturaflow_pass

          # Email (Mailtrap - ambiente de testes)
          SMTP_HOST=sandbox.smtp.mailtrap.io
          SMTP_PORT=587
          SMTP_USER=teu_user_mailtrap
          SMTP_PASS=tua_pass_mailtrap

          # RabbitMQ
          RABBITMQ_DEFAULT_USER=guest
          RABBITMQ_DEFAULT_PASS=guest


     📌 Nota:
          Durante a apresentação da PAP, recomenda-se o uso do Mailtrap, garantindo que nenhum email real seja enviado.

▶️ Passo 3 — Iniciar o Projeto com Docker Compose

Após configurar o .env, executa:

     docker-compose up -d


Este comando irá:

     criar e iniciar os contentores;
     configurar automaticamente a base de dados MySQL;
     iniciar o RabbitMQ;
     arrancar a aplicação Web e o Worker Service.

     📌 O sistema inclui mecanismos de retry para garantir que os serviços aguardem pela disponibilidade da base de dados antes de iniciar.

▶️ Passo 4 — Aceder à Aplicação

     Aplicação Web:

          http://localhost:8080

     Painel de gestão do RabbitMQ:
          http://localhost:15672

     Credenciais padrão:

          Utilizador: guest
          Password: guest

▶️ Passo 5 — Teste do Envio de Emails

     Os emails enviados pelo sistema podem ser visualizados no Mailtrap, permitindo validar:
          envio correto do PDF;
          comportamento do sistema em caso de erro;
          funcionamento do Worker Service.

🛑 Encerrar os Serviços

     Para parar todos os contentores:
          docker-compose down