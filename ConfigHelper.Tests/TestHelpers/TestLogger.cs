using Microsoft.Extensions.Logging;

namespace ConfigHelper.Tests.TestHelpers
{
    /// <summary>
    /// Implementação de um logger para testes que captura e armazena logs em uma lista para verificação.
    /// </summary>
    /// <typeparam name="T">O tipo associado ao logger. Geralmente, é o tipo da classe que está sendo testada.</typeparam>
    public class TestLogger<T> : ILogger<T>, IDisposable
    {
        /// <summary>
        /// Lista que armazena todas as mensagens de log registradas durante os testes.
        /// </summary>
        public List<string> Logs { get; } = new List<string>();

        /// <summary>
        /// Inicia um escopo lógico para operações de log. Neste logger de teste, o escopo não é utilizado, 
        /// mas o método é implementado para cumprir o contrato de <see cref="ILogger{T}"/>.
        /// </summary>
        /// <typeparam name="TState">O tipo do estado associado ao escopo.</typeparam>
        /// <param name="state">O estado associado ao escopo de logging.</param>
        /// <returns>Um <see cref="IDisposable"/> que representa o escopo. Neste caso, retorna o próprio logger.</returns>
        public IDisposable BeginScope<TState>(TState state) => this;

        /// <summary>
        /// Método de descarte para liberar recursos. Como não há recursos não gerenciados, o método está vazio.
        /// Implementado para cumprir o contrato de <see cref="IDisposable"/>.
        /// </summary>
        public void Dispose() { }

        /// <summary>
        /// Determina se o logger está habilitado para o nível de log especificado.
        /// Neste logger de teste, todos os níveis de log estão habilitados.
        /// </summary>
        /// <param name="logLevel">O nível de log a ser verificado.</param>
        /// <returns>Sempre retorna <c>true</c>, indicando que todos os níveis de log estão habilitados.</returns>
        public bool IsEnabled(LogLevel logLevel) => true;

        /// <summary>
        /// Registra uma mensagem de log. A mensagem formatada é adicionada à lista de logs para posterior verificação.
        /// </summary>
        /// <typeparam name="TState">O tipo do objeto de estado que contém as informações de log.</typeparam>
        /// <param name="logLevel">O nível de log que está sendo registrado.</param>
        /// <param name="eventId">O identificador do evento associado ao log.</param>
        /// <param name="state">O objeto de estado que contém as informações de log.</param>
        /// <param name="exception">A exceção associada ao log, se houver. Pode ser <c>null</c>.</param>
        /// <param name="formatter">A função que formata o estado e a exceção em uma string de log.</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var log = formatter(state, exception);
            Logs.Add(log);
        }
    }
}
