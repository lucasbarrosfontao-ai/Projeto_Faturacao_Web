📑 FaturaFlow — Sistema de Faturação Web

⚠️ AVISO IMPORTANTE (Disclaimer): Este projeto foi desenvolvido exclusivamente para fins académicos e de demonstração técnica no âmbito de uma Prova de Aptidão Profissional (PAP). O software não possui certificação da Autoridade Tributária (AT) e não cumpre os requisitos legais e técnicos obrigatórios para utilização em ambientes empresariais reais. Não deve ser utilizado para fins comerciais ou fiscais.

🧭 Visão Geral

O FaturaFlow é uma plataforma robusta de faturação e gestão de inventário desenvolvida em .NET 8. O sistema foi desenhado com foco em arquitetura de microsserviços (através de um Worker independente), escalabilidade e segurança, simulando o ambiente real de uma PME (Pequena ou Média Empresa).

Este projeto foi desenvolvido como Prova de Aptidão Profissional (PAP) para o curso de Técnico de Informática de Gestão, com o apoio da empresa Openvia Mobility.

🚀 Novidades de Segurança (Implementado)

Recentemente, o sistema foi atualizado para incluir camadas críticas de proteção:
     Autenticação Robusta: Sistema de login seguro para proteger os dados financeiros.
     Recuperação de Conta: Fluxo completo de recuperação de palavra-passe via e-mail com código de verificação temporário.
     Integridade de Dados: Validações rigorosas em todos os formulários para garantir que a base de dados permanece consistente.

✨ Funcionalidades Principais
     ✔️ Gestão Completa: Clientes, Fornecedores e Produtos.
     ✔️ Ciclo de Venda Automatizado: Emissão de faturas com cálculo de IVA e atualização de stock em tempo real.
     ✔️ Arquitetura Assíncrona: Geração de PDFs e envio de e-mails processados em background (RabbitMQ + Worker).
     ✔️ Segurança: Sistema de utilizadores com recuperação de password.
     ✔️ Business Intelligence: Dashboard visual com métricas de vendas.
     ✔️ Docker Ready: Deploy simplificado com contentores.

🏗️ Arquitetura da Solução

     O FaturaFlow utiliza uma arquitetura desacoplada para garantir que a interface nunca fique bloqueada por processos pesados.
     Web Application (Blazor Server): Onde ocorre a interação, gestão de utilizadores (Identity) e emissão de documentos.
     RabbitMQ (Message Broker): Atua como o intermediário, recebendo tarefas de envio de e-mail e geração de PDF.
     Worker Service (.NET): O "motor" de processamento. Consome a fila do RabbitMQ, gera os documentos via QuestPDF e envia e-mails via MailKit.
     MySQL: Base de dados relacional para persistência segura.

🛠️ Tecnologias Utilizadas
Categoria	               Tecnologias
     Backend & Web	          .NET 8, Blazor Server, Entity Framework Core
     Segurança	               ASP.NET Core Identity
     Base de Dados	          MySQL
     Mensageria	          RabbitMQ
     Processamento	          Worker Service
     Documentos	          QuestPDF
     E-mail	               MailKit & Mailtrap (Ambiente de Testes)
     DevOps	               Docker & Docker Compose


🔄 Fluxo de Funcionamento (Recovery & Email)

     1. O utilizador solicita a recuperação de password ou emite uma fatura.
     2. A Web App envia um evento para o RabbitMQ.
     3. O Worker Service deteta o evento e assume a tarefa.
     4. O Worker comunica com o servidor SMTP (Mailtrap) para entregar o código/fatura.
     5. O utilizador recebe a informação sem que a aplicação principal tenha tido qualquer abrandamento.

⚠️ Limitações Assumidas

Embora funcional, o projeto mantém as seguintes limitações de âmbito académico:
     Certificação Fiscal: O software não é certificado pela Autoridade Tributária.
     Assinatura Digital: Não inclui a assinatura digital qualificada exigida em faturas PDF legais.
     Comunicação AT: Não comunica dados via webservice ou exportação de ficheiro SAF-T (PT).
     Segurança Avançada: Embora tenha autenticação, não foi submetido a testes de intrusão profissionais.

📦 Como Executar o Projeto
🔧 Pré-requisitos

     Docker Desktop
     Git

▶️ Passo a Passo

Clonar o repositório:
Em uma pasta designada para esse projeto, abra o terminal do git e digite o comando abaixo:
     git clone https://github.com/lucasbarrosfontao-ai/Projeto_Faturacao_Web.git
     cd Projeto_Faturacao_Web

Configurar Variáveis de Ambiente:

Renomeia o ficheiro .env_exemplo para .env.
Preenche as tuas credenciais do Mailtrap (essencial para testar a recuperação de password e envio de faturas).

Subir os Serviços:
Na pasta Raiz (onde você ver o arquivo docker-compose.yml), digite esse comando:
     docker-compose up -d

Aceder ao Sistema:

Aplicação: http://localhost:8080
Gestão RabbitMQ: http://localhost:15672 (User/Pass: guest)

🧠 Aprendizagens-Chave

A execução deste projeto permitiu dominar:
     A implementação de Sistemas de Identidade e segurança em .NET.
     O uso de Message Brokers para resolver problemas de concorrência e performance.
     A gestão de infraestrutura moderna com Docker.
     A aplicação de regras de negócio complexas (gestão de stocks e impostos).

Desenvolvido por Lucas Barros Fontão
Técnico de Informática de Gestão | Prova de Aptidão Profissional