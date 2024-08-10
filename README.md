# ConfigHelper

## Descrição do Projeto

O `ConfigHelper` é uma biblioteca .NET criada para facilitar o gerenciamento e a recuperação de configurações centralizadas, utilizando o AWS Systems Manager Parameter Store como fonte segura de armazenamento de parâmetros. Além disso, a biblioteca integra logging estruturado com Serilog, permitindo o envio de logs para o Elasticsearch, o que possibilita um monitoramento e análise avançados.

## Objetivos do Projeto

Este projeto foi desenvolvido como parte de um estudo para explorar as melhores práticas em:
- **Gerenciamento de Configurações**: Recuperação centralizada de configurações a partir do AWS Systems Manager Parameter Store.
- **Segurança**: Suporte à descriptografia automática de valores armazenados de forma segura no AWS.
- **Logging Estruturado**: Implementação de logs estruturados usando Serilog, com integração ao Elasticsearch para análise e monitoramento.
- **Testes de Unidade**: Utilização de Moq para simular interações com serviços externos e criação de loggers personalizados para verificar logs gerados durante os testes.

## Estrutura do Projeto

```plaintext
ConfigHelper/
├── src/
│   ├── ConfigHelper/
│   │   ├── Loggers/
│   │   │   ├── DetailedLogBuilder.cs
│   │   ├── Models/
│   │   │   ├── ElasticsearchCredentials.cs
│   │   ├── ConfigurationService.cs
│   │   ├── ConfigurationServiceExtensions.cs
│   │   ├── SecureCredentialsHelper.cs
│   │   ├── ConfigHelper.csproj
│   └── ConfigHelper.Tests/
│       ├── ConfigHelper.Tests.csproj
│       ├── ConfigurationServiceTests.cs
│       ├── Mocks/
│       │   ├── MockAmazonSimpleSystemsManagement.cs
│       └── TestHelpers/
│           ├── TestLogger.cs
├── .gitignore
├── README.md
└── LICENSE
```

## 1. ConfigHelper/
Contém a implementação principal da biblioteca, incluindo as seguintes classes e funcionalidades:

Loggers/DetailedLogBuilder.cs: Classe para construir logs detalhados com informações como host, data, mensagem de exceção, stack trace e tempo de execução.
Models/ElasticsearchCredentials.cs: Classe para encapsular as credenciais do Elasticsearch obtidas de forma segura.
ConfigurationService.cs: Serviço responsável por recuperar valores de configuração do AWS Systems Manager Parameter Store.
ConfigurationServiceExtensions.cs: Métodos de extensão para facilitar a configuração dos serviços da biblioteca.
SecureCredentialsHelper.cs: Classe para auxiliar na criptografia e descriptografia de credenciais.
## 2. ConfigHelper.Tests/
Projeto de testes que verifica a funcionalidade do ConfigHelper. Inclui:

ConfigurationServiceTests.cs: Testes de unidade para o ConfigurationService, garantindo que os valores sejam recuperados corretamente e que exceções sejam tratadas adequadamente.
Mocks/MockAmazonSimpleSystemsManagement.cs: Contém mocks para simular o comportamento de dependências externas como o AWS Systems Manager.
TestHelpers/TestLogger.cs: Inclui um logger personalizado para capturar e verificar logs durante os testes.

## 3. Arquivos Gerais

- .gitignore: Arquivo de configuração que especifica quais arquivos e diretórios devem ser ignorados pelo controle de versão Git.
- README.md: Documento atual que fornece uma visão geral e detalhes sobre o projeto.
- LICENSE: Arquivo que contém os termos da licença sob a qual o projeto está distribuído.

# Funcionalidades Principais

## 1. Recuperação de Configurações
A biblioteca permite recuperar configurações centralizadas a partir do AWS Systems Manager Parameter Store. As configurações podem ser armazenadas de forma segura e descriptografadas automaticamente ao serem recuperadas.

## 2. Logging Estruturado com Serilog
O ConfigHelper integra Serilog para fornecer logging estruturado, permitindo que os logs sejam enviados para o Elasticsearch. Isso facilita o monitoramento e a análise dos logs através de ferramentas como Kibana.

## 3. Integração Segura com AWS Secrets Manager
As credenciais para o Elasticsearch são obtidas de forma segura usando o AWS Secrets Manager, garantindo que informações sensíveis não sejam expostas no código-fonte.

# Como Usar
## 1. Instalação
Clone o repositório e adicione a referência ao seu projeto .NET:

```bash
git clone https://github.com/marcialwushu/ConfigHelper.git
```



## 2. Configuração do AWS Systems Manager
Certifique-se de que suas configurações estejam armazenadas no AWS Systems Manager Parameter Store e que você tenha as permissões apropriadas para acessá-las.

## 3. Configuração do Serilog e Elasticsearch
No seu projeto, configure o Serilog para enviar logs ao Elasticsearch:

```bash
services.AddConfigurationServiceWithElasticsearchLoggingFromSecretsManager("your-elastic-secret-name");

```

## 4. Executando os Testes
Os testes de unidade podem ser executados usando o comando:

```cs
dotnet test ConfigHelper.Tests/
```

# Contribuição
Contribuições são bem-vindas! Sinta-se à vontade para abrir issues e pull requests para melhorar este projeto.

# Licença
Este projeto está licenciado sob os termos da licença MIT. Consulte o arquivo LICENSE para mais detalhes.

# Autor
- CLEILSON DE SOUSA PEREIRA
- GitHub: marcialwushu


### Explicação do Conteúdo

- **Estrutura do Projeto**: A estrutura de pastas detalhada é incluída para ajudar os desenvolvedores a entenderem a organização do projeto.
- **Descrição das Pastas e Arquivos**: Cada parte da estrutura é explicada, mostrando o que cada diretório e arquivo contém, facilitando a navegação e compreensão do projeto.
- **Funcionalidades Principais, Como Usar, Contribuição e Licença**: Explicações adicionais sobre como utilizar o projeto, contribuir e quais licenças se aplicam.

Este `README.md` é abrangente e fornece todas as informações necessárias para que outros desenvolvedores possam entender, utilizar e contribuir com o projeto `ConfigHelper`.
