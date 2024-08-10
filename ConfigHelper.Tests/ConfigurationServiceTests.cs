/*
 * Projeto: ConfigHelper.Tests
 * Desenvolvedor: CLEILSON DE SOUSA PEREIRA
 * Ano: 2024
 * Versão: 1.0
 * MTI: marcialwushu
 *
 * Objetivo de Estudo:
 * 
 * Este projeto de testes foi desenvolvido como parte de um estudo sobre a criação e testes de bibliotecas em .NET.
 * O foco principal está na criação de um serviço de configuração que interage com o AWS Systems Manager Parameter Store
 * e na implementação de logs estruturados usando o Serilog, com integração para Elasticsearch.
 * 
 * O projeto de testes inclui:
 * - Verificação do comportamento do serviço de configuração, incluindo retorno de valores de parâmetros e tratamento
 *   de exceções, como a ausência de parâmetros no AWS Systems Manager.
 * - Uso de mocks para simular interações com o AWS Systems Manager, utilizando a biblioteca Moq.
 * - Implementação de um logger de testes para capturar e verificar mensagens de log geradas durante os testes.
 * 
 * Este código serve como base para o estudo e aplicação de boas práticas no desenvolvimento e teste de bibliotecas .NET,
 * fornecendo um exemplo prático de como testar serviços que interagem com APIs externas e como capturar e verificar logs
 * em um ambiente de teste.
 *
 * Licença: Este código pode ser utilizado para fins educacionais e de estudo. A reprodução para fins comerciais
 * deve ser discutida com o autor.
 *
 * Contato: GitHub - marcialwushu
 */

using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using ConfigHelper.Tests.TestHelpers;
using Moq;

namespace ConfigHelper.Tests
{
    /// <summary>
    /// Classe de testes unitários para a classe <see cref="ConfigurationService"/>.
    /// Utiliza Moq para simular o comportamento do serviço AWS Systems Manager Parameter Store
    /// e testa o comportamento esperado do serviço de configuração. 
    /// </summary>
    public class ConfigurationServiceTests
    {
        private readonly Mock<IAmazonSimpleSystemsManagement> _mockSsmClient;
        private readonly TestLogger<ConfigurationService> _testLogger;
        private readonly ConfigurationService _service;

        /// <summary>
        /// Construtor da classe <see cref="ConfigurationServiceTests"/> que inicializa os mocks e o serviço de configuração.
        /// </summary>
        public ConfigurationServiceTests()
        {
            // Mock do cliente AWS Systems Manager para simular interações com o Parameter Store
            _mockSsmClient = new Mock<IAmazonSimpleSystemsManagement>();
            // Logger de teste para capturar e verificar mensagens de log geradas pelo ConfigurationService
            _testLogger = new TestLogger<ConfigurationService>();
            // Instancia o serviço de configuração utilizando o mock do SSM client e o logger de teste
            _service = new ConfigurationService(_mockSsmClient.Object, _testLogger);
        }

        /// <summary>
        /// Testa se o método <see cref="ConfigurationService.GetConfigurationValueAsync"/> retorna o valor correto 
        /// quando o parâmetro existe no AWS Systems Manager Parameter Store.
        /// </summary>
        [Fact]
        public async Task GetConfigurationValueAsync_ReturnsValue_WhenParameterExists()
        {
            // Arrange: Configura o mock do SSM client para retornar um valor esperado quando o parâmetro é solicitado
            var parameterName = "/app/test";
            var expectedValue = "test-value";

            _mockSsmClient.Setup(s => s.GetParameterAsync(It.IsAny<GetParameterRequest>(), default))
                .ReturnsAsync(new GetParameterResponse
                {
                    Parameter = new Parameter { Value = expectedValue }
                });

            // Act: Chama o método GetConfigurationValueAsync do ConfigurationService
            var result = await _service.GetConfigurationValueAsync("test", "app");

            // Assert: Verifica se o valor retornado é o esperado e se o log contém a mensagem de sucesso
            Assert.Equal(expectedValue, result);
            Assert.Contains(_testLogger.Logs, log => log.Contains("Successfully retrieved parameter"));
        }

        /// <summary>
        /// Testa se o método <see cref="ConfigurationService.GetConfigurationValueAsync"/> lança uma exceção 
        /// <see cref="ParameterNotFoundException"/> quando o parâmetro não é encontrado no AWS Systems Manager Parameter Store.
        /// </summary>
        [Fact]
        public async Task GetConfigurationValueAsync_ThrowsException_WhenParameterNotFound()
        {
            // Arrange: Configura o mock do SSM client para lançar uma exceção quando o parâmetro não for encontrado
            _mockSsmClient.Setup(s => s.GetParameterAsync(It.IsAny<GetParameterRequest>(), default))
                .ThrowsAsync(new ParameterNotFoundException("Parameter not found"));

            // Act & Assert: Verifica se o método lança a exceção esperada e se o log contém a mensagem de erro
            await Assert.ThrowsAsync<ParameterNotFoundException>(() => _service.GetConfigurationValueAsync("test", "app"));
            Assert.Contains(_testLogger.Logs, log => log.Contains("Parameter not found"));
        }
    }
}
