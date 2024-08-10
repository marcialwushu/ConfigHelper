ADR (Architecture Decision Record) para o projeto `ConfigHelper`:

---

# ADR 001: Arquitetura e Design do Projeto ConfigHelper

## Status

**Aprovado** - 10 de Agosto de 2024

## Contexto

O projeto `ConfigHelper` foi desenvolvido para fornecer uma solução centralizada de gerenciamento de configurações em aplicações .NET, utilizando o AWS Systems Manager Parameter Store para armazenar e recuperar parâmetros de configuração. Além disso, foi necessária a implementação de um sistema de logging estruturado que pudesse integrar-se facilmente ao Elasticsearch para monitoramento e análise.

O projeto busca garantir segurança, escalabilidade e facilidade de manutenção, permitindo que configurações sensíveis sejam armazenadas de forma segura e que logs possam ser monitorados em tempo real.

## Decisão

### 1. **Uso do AWS Systems Manager Parameter Store**

- **Motivação**: O AWS Systems Manager Parameter Store foi escolhido para armazenar configurações devido à sua capacidade de gerenciar parâmetros de configuração de forma segura e centralizada. Ele também suporta a descriptografia automática de valores sensíveis, o que adiciona uma camada extra de segurança sem exigir trabalho adicional dos desenvolvedores.

- **Detalhes Técnicos**:
  - Configurações sensíveis (como chaves de API, strings de conexão, etc.) são armazenadas com criptografia AWS KMS.
  - As configurações são recuperadas usando a API do AWS SDK, com suporte a múltiplas regiões e a descriptografia automática.

- **Pontos de Atenção**:
  - É crucial garantir que as permissões IAM estejam configuradas corretamente para que a aplicação tenha acesso aos parâmetros necessários.
  - O tempo de resposta pode ser impactado por problemas de rede ou latência na comunicação com a AWS.

### 2. **Implementação de Logging Estruturado com Serilog**

- **Motivação**: Serilog foi escolhido por sua facilidade de uso, suporte a logging estruturado e integração nativa com vários sinks, incluindo Elasticsearch. O logging estruturado permite que os dados sejam organizados em um formato que facilita a análise em ferramentas como Kibana.

- **Detalhes Técnicos**:
  - Os logs são configurados para serem enviados para o Elasticsearch, com suporte a autenticação via AWS Secrets Manager.
  - Logs incluem informações detalhadas, como host, data, stack trace, e tempo de execução, construídos usando a classe `DetailedLogBuilder`.

- **Pontos de Atenção**:
  - Configurações erradas no Serilog podem resultar em falhas de envio de logs, o que dificulta o monitoramento da aplicação.
  - O Elasticsearch precisa estar configurado para suportar a carga de logs gerados, com índices adequados para retenção de dados.

### 3. **Testes e Mocks**

- **Motivação**: Para garantir que o `ConfigurationService` funcione corretamente e que todas as exceções sejam tratadas conforme esperado, foram criados testes de unidade utilizando Moq para simular o AWS Systems Manager e um logger personalizado para capturar logs durante os testes.

- **Detalhes Técnicos**:
  - Mocks são utilizados para simular as respostas do AWS Systems Manager, permitindo testes de unidade independentes de serviços externos.
  - Um `TestLogger` captura todos os logs gerados durante os testes, permitindo a verificação de que as mensagens corretas foram registradas.

- **Pontos de Atenção**:
  - Testes dependentes de mocks precisam ser atualizados sempre que houver mudanças nas APIs simuladas.
  - É importante garantir que os testes cubram todas as possíveis exceções e cenários, incluindo falhas de rede e problemas de autenticação.

## Consequências

- **Positivas**:
  - As decisões tomadas permitem que a aplicação gerencie configurações de forma segura e centralizada, com suporte a logging estruturado para monitoramento avançado.
  - A abordagem facilita a escalabilidade e a manutenção do código, com a capacidade de adicionar novas funcionalidades sem comprometer a segurança ou a performance.

- **Negativas**:
  - Dependência direta dos serviços AWS, o que pode limitar o uso do `ConfigHelper` em ambientes que não utilizam AWS.
  - A complexidade da configuração inicial, especialmente no que diz respeito ao Serilog e à integração com o Elasticsearch.

## Alternativas Consideradas

1. **Uso do Azure Key Vault**:
   - Avaliado, mas não escolhido devido à preferência existente pela AWS na infraestrutura atual.

2. **Logging via Application Insights**:
   - Considerado, mas Serilog com Elasticsearch foi preferido devido à flexibilidade e à familiaridade da equipe com a ferramenta.

3. **Armazenamento Local de Configurações**:
   - Rejeitado por questões de segurança e dificuldades em gerenciar configurações em ambientes distribuídos.

## Ações Futuras

- Avaliar a possibilidade de abstrair a dependência do AWS Systems Manager para permitir suporte a múltiplas plataformas de gerenciamento de configurações (por exemplo, Azure Key Vault).
- Monitorar o desempenho do Elasticsearch e ajustar os índices conforme necessário para garantir que o sistema de logging seja eficiente.

## Autor

**CLEILSON DE SOUSA PEREIRA**  
GitHub: [marcialwushu](https://github.com/marcialwushu)

---

### Explicação do Conteúdo

- **Status**: Indica que a decisão foi aprovada e a data da aprovação.
- **Contexto**: Descreve o cenário que levou à necessidade de tomar essas decisões arquiteturais.
- **Decisão**: Detalha as principais escolhas feitas, com explicações técnicas e pontos de atenção.
- **Consequências**: Explica os impactos positivos e negativos das decisões.
- **Alternativas Consideradas**: Descreve outras opções que foram avaliadas, mas não escolhidas, com justificativas.
- **Ações Futuras**: Identifica possíveis evoluções da arquitetura ou melhorias que podem ser consideradas no futuro.
- **Autor**: Credita o autor responsável pelas decisões e fornece uma forma de contato.

Este ADR fornece um registro detalhado das decisões arquiteturais, ajudando a orientar o desenvolvimento futuro e fornecendo contexto para decisões que possam precisar de revisão ou ajuste.
