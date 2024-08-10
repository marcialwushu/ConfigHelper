/*
 * Classe: ConfigurationService
 * Desenvolvedor: CLEILSON DE SOUSA PEREIRA
 * Ano: 2024
 * Versão: 1.0
 * MTI: marcialwushu
 *
 * Objetivo de Estudo:
 * 
 * A classe `ConfigurationService` foi desenvolvida como parte de um estudo sobre a implementação de serviços
 * de configuração em aplicações .NET, utilizando o AWS Systems Manager Parameter Store como fonte centralizada
 * de configurações. Este serviço permite recuperar valores de configuração armazenados de forma segura e centralizada
 * na AWS, integrando-se com boas práticas de segurança e logging.
 * 
 * Funcionalidades principais:
 * - Recuperação de valores de configuração a partir do AWS Systems Manager Parameter Store.
 * - Suporte à descriptografia automática de valores armazenados de forma segura.
 * - Implementação de logs estruturados utilizando a biblioteca `Serilog`, permitindo integração com Elasticsearch
 *   para análise e monitoramento avançados.
 * - Tratamento robusto de exceções, com logs detalhados incluindo informações como o host, data, mensagem de exceção,
 *   stack trace e tempo de execução.
 * 
 * Este código serve como exemplo prático de como criar e testar serviços de configuração em .NET,
 * abordando práticas recomendadas de segurança, integração com serviços de nuvem e logging estruturado.
 * O projeto também visa demonstrar a importância da captura e tratamento adequado de erros, garantindo
 * que informações críticas sejam registradas para análise futura.
 *
 * Licença: Este código pode ser utilizado para fins educacionais e de estudo. A reprodução para fins comerciais
 * deve ser discutida com o autor.
 *
 * Contato: GitHub - marcialwushu
 */



using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using ConfigHelper.Loggers;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ConfigHelper
{
    /// <summary>
    /// Classe responsável por gerenciar a recuperação de valores de configuração armazenados no AWS Systems Manager Parameter Store.
    /// </summary>
    public class ConfigurationService
    {
        private readonly IAmazonSimpleSystemsManagement _ssmClient;
        private readonly ILogger<ConfigurationService> _logger;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="ConfigurationService"/>.
        /// </summary>
        /// <param name="ssmClient">Cliente do AWS Systems Manager usado para acessar o Parameter Store.</param>
        /// <param name="logger">Instância de <see cref="ILogger"/> para registrar logs de operações e exceções.</param>
        /// <exception cref="ArgumentNullException">Lançada se <paramref name="ssmClient"/> ou <paramref name="logger"/> forem nulos.</exception>
        public ConfigurationService(IAmazonSimpleSystemsManagement ssmClient, ILogger<ConfigurationService> logger)
        {
            _ssmClient = ssmClient ?? throw new ArgumentNullException(nameof(ssmClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Recupera o valor de um parâmetro do AWS Systems Manager Parameter Store.
        /// </summary>
        /// <param name="keyName">O nome da chave do parâmetro a ser recuperado.</param>
        /// <param name="appType">O tipo da aplicação que está solicitando a configuração, usado para construir o caminho completo do parâmetro.</param>
        /// <returns>O valor do parâmetro como uma string.</returns>
        /// <exception cref="ArgumentException">Lançada se <paramref name="keyName"/> ou <paramref name="appType"/> forem nulos ou vazios.</exception>
        /// <exception cref="ParameterNotFoundException">Lançada se o parâmetro solicitado não for encontrado no Parameter Store.</exception>
        /// <exception cref="Exception">Lançada se ocorrer qualquer outro erro ao tentar recuperar o parâmetro.</exception>
        public async Task<string> GetConfigurationValueAsync(string keyName, string appType)
        {
            // Verifica se o nome da chave é nulo ou vazio
            if (string.IsNullOrWhiteSpace(keyName))
            {
                _logger.LogError("Key name cannot be null or whitespace.");
                throw new ArgumentException("Key name cannot be null or whitespace.", nameof(keyName));
            }

            // Verifica se o tipo da aplicação é nulo ou vazio
            if (string.IsNullOrWhiteSpace(appType))
            {
                _logger.LogError("Application type cannot be null or whitespace.");
                throw new ArgumentException("Application type cannot be null or whitespace.", nameof(appType));
            }

            // Constrói o nome completo do parâmetro baseado no tipo da aplicação e no nome da chave
            var parameterName = $"/{appType}/{keyName}";
            _logger.LogInformation("Attempting to retrieve parameter: {ParameterName}", parameterName);

            // Inicia o cronômetro para medir o tempo de execução
            var stopwatch = Stopwatch.StartNew();

            try
            {
                // Cria a solicitação para recuperar o parâmetro do AWS Systems Manager
                var request = new GetParameterRequest
                {
                    Name = parameterName,
                    WithDecryption = true // Define se o parâmetro deve ser descriptografado automaticamente
                };

                var response = await _ssmClient.GetParameterAsync(request);
                stopwatch.Stop(); // Para o cronômetro após a operação ser concluída

                // Loga o sucesso na recuperação do parâmetro, incluindo o tempo de execução
                _logger.LogInformation("Successfully retrieved parameter: {ParameterName} in {TimeTaken}ms", parameterName, stopwatch.ElapsedMilliseconds);

                // Retorna o valor do parâmetro
                return response.Parameter.Value;
            }
            catch (ParameterNotFoundException ex)
            {
                // Para o cronômetro e monta o log detalhado em caso de exceção
                stopwatch.Stop();
                var detailedLog = new DetailedLogBuilder()
                    .WithException(ex)
                    .WithTimeTaken(stopwatch);

                // Loga o erro com os detalhes da exceção e do tempo de execução
                _logger.LogError(detailedLog.ToString());
                throw;
            }
            catch (Exception ex)
            {
                // Para o cronômetro e monta o log detalhado em caso de exceção genérica
                stopwatch.Stop();
                var detailedLog = new DetailedLogBuilder()
                    .WithException(ex)
                    .WithTimeTaken(stopwatch);

                // Loga o erro com os detalhes da exceção e do tempo de execução
                _logger.LogError(detailedLog.ToString());
                throw;
            }
        }
    }
}
