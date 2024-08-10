using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Amazon.SimpleSystemsManagement;
using ConfigHelper.Models;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Text.Json;

namespace ConfigHelper
{
    /// <summary>
    /// Classe estática que contém métodos de extensão para configurar serviços de configuração e logging em uma aplicação.
    /// </summary>
    public static class ConfigurationServiceExtensions
    {
        /// <summary>
        /// Classe estática que contém métodos de extensão para configurar serviços de configuração e logging em uma aplicação.
        /// </summary>
        /// <param name="services">A coleção de serviços na qual o serviço de configuração será registrado.</param>
        /// <returns>A coleção de serviços para permitir a chamada encadeada de métodos</returns>
        public static IServiceCollection AddConfigurationService(this IServiceCollection services)
        {
            // Registra o cliente do AWS Systems Manager no contêiner de injeção de dependências
            services.AddAWSService<IAmazonSimpleSystemsManagement>();
            // Registra o ConfigurationService como um serviço singleton
            services.AddSingleton<ConfigurationService>();
            // Retorna a coleção de serviços para permitir encadeamento de chamadas
            return services;
        }

        /// <summary>
        /// Adiciona o serviço de configuração e configura o Serilog para enviar logs ao Elasticsearch,
        /// utilizando credenciais obtidas do AWS Secrets Manager.
        /// </summary>
        /// <param name="services">A coleção de serviços na qual o serviço de configuração e logging será registrado.</param>
        /// <param name="secretName">O nome do segredo armazenado no AWS Secrets Manager que contém as credenciais do Elasticsearch</param>
        /// <returns>A coleção de serviços para permitir a chamada encadeada de métodos.</returns>
        public static IServiceCollection AddConfigurationServiceWithElasticsearchLoggingFromSecretsManager(this IServiceCollection services, string secretName)
        {
            // Criar um cliente para o AWS Secrets Manager na região US East (Norte da Virgínia)
            var secretsManagerClient = new AmazonSecretsManagerClient(RegionEndpoint.USEast1);

            // Cria a solicitação para obter o segredo do AWS Secrets Manager com base no nome fornecido
            var secretValueRequest = new GetSecretValueRequest { SecretId = secretName };
            // Executa a solicitação de forma síncrona para obter o valor do segredo
            var secretValueResponse = secretsManagerClient.GetSecretValueAsync(secretValueRequest).Result;
            // Desserializa o conteúdo do segredo para um objeto ElasticsearchCredentials
            var secret = JsonSerializer.Deserialize<ElasticsearchCredentials>(secretValueResponse.SecretString);

            // Configura o Serilog com o sink do Elasticsearch, utilizando as credenciais obtidas
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext() // Enriquecer o log com contexto adicional (por exemplo, propriedades do ambiente)
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(secret.Uri))
                {
                    AutoRegisterTemplate = true, // Registra automaticamente o template do índice no Elasticsearch
                    IndexFormat = "logs-{0:yyyy.MM.dd}", // Define o formato do índice de log no Elasticsearch
                    ModifyConnectionSettings = x => x.BasicAuthentication(secret.Username, secret.Password) // Configura autenticação básica
                })
                .CreateLogger();

            // Adiciona o Serilog ao sistema de logging do .NET
            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true)
            );

            // Chama o método AddConfigurationService para registrar o serviço de configuração
            return services.AddConfigurationService();
        }
    }
}
