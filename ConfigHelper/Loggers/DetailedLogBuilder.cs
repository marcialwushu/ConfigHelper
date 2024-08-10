using System.Diagnostics;
using System.Net;

namespace ConfigHelper.Loggers
{
    /// <summary>
    /// Classe responsável por construir logs detalhados, incluindo informações como host, data, mensagem de exceção, stack trace e tempo de execução.
    /// </summary>
    public class DetailedLogBuilder
    {
        /// <summary>
        /// Obtém o nome do host onde a aplicação está sendo executada.
        /// </summary>
        public string Host { get; private set; }

        /// <summary>
        /// Obtém a data e hora (em UTC) em que o log foi criado.
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Obtém a mensagem da exceção que será registrada no log.
        /// </summary>
        public string ExceptionMessage { get; private set; }

        /// <summary>
        /// Obtém o stack trace da exceção que será registrado no log.
        /// </summary>
        public string StackTrace { get; private set; }

        /// <summary>
        /// Obtém o tempo de execução (em milissegundos) que será registrado no log.
        /// </summary>
        public long TimeTaken { get; private set; }

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="DetailedLogBuilder"/>, configurando o host e a data atuais.
        /// </summary>
        public DetailedLogBuilder()
        {
            Host = Dns.GetHostName(); // Obtém o nome do host onde a aplicação está sendo executada
            Date = DateTime.UtcNow; // Define a data e hora atuais em UTC
        }

        /// <summary>
        /// Configura as propriedades <see cref="ExceptionMessage"/> e <see cref="StackTrace"/> com base em uma exceção fornecida.
        /// </summary>
        /// <param name="ex">A exceção cujas informações serão registradas no log.</param>
        /// <returns>A instância atual de <see cref="DetailedLogBuilder"/> para permitir encadeamento de métodos.</returns>
        public DetailedLogBuilder WithException(Exception ex)
        {
            ExceptionMessage = ex.Message; // Define a mensagem da exceção
            StackTrace = ex.StackTrace; // Define o stack trace da exceção
            return this;
        }

        /// <summary>
        /// Configura a propriedade <see cref="TimeTaken"/> com base no tempo de execução medido por um <see cref="Stopwatch"/>.
        /// </summary>
        /// <param name="stopwatch">O <see cref="Stopwatch"/> que mede o tempo de execução.</param>
        /// <returns>A instância atual de <see cref="DetailedLogBuilder"/> para permitir encadeamento de métodos.</returns>
        public DetailedLogBuilder WithTimeTaken(Stopwatch stopwatch)
        {
            TimeTaken = stopwatch.ElapsedMilliseconds; // Define o tempo de execução em milissegundos
            return this;
        }

        /// <summary>
        /// Retorna uma representação em string do log detalhado, contendo host, data, mensagem de exceção, stack trace e tempo de execução.
        /// </summary>
        /// <returns>Uma string formatada com as informações detalhadas do log.</returns>
        public override string ToString()
        {
            return $"Host: {Host}, Date: {Date}, Exception: {ExceptionMessage}, StackTrace: {StackTrace}, TimeTaken: {TimeTaken}ms";
        }

        /// <summary>
        /// Converte implicitamente uma instância de <see cref="DetailedLogBuilder"/> em uma string.
        /// </summary>
        /// <param name="builder">A instância de <see cref="DetailedLogBuilder"/> a ser convertida.</param>
        /// <returns>Uma string contendo as informações detalhadas do log.</returns>
        public static implicit operator string(DetailedLogBuilder builder)
        {
            return builder.ToString();
        }

        /// <summary>
        /// Converte implicitamente uma string em uma instância de <see cref="DetailedLogBuilder"/>.
        /// </summary>
        /// <param name="log">A string contendo as informações detalhadas do log.</param>
        /// <returns>Uma nova instância de <see cref="DetailedLogBuilder"/> preenchida com os valores da string.</returns>
        public static implicit operator DetailedLogBuilder(string log)
        {
            var parts = log.Split(',');
            var host = parts[0].Split(':')[1].Trim();
            var date = DateTime.Parse(parts[1].Split(':')[1].Trim());
            var exception = parts[2].Split(':')[1].Trim();
            var stackTrace = parts[3].Split(':')[1].Trim();
            var timeTaken = long.Parse(parts[4].Split(':')[1].Trim().Replace("ms", string.Empty));

            // Retorna uma nova instância de DetailedLogBuilder preenchida com os valores extraídos
            return new DetailedLogBuilder
            {
                Host = host,
                Date = date,
                ExceptionMessage = exception,
                StackTrace = stackTrace,
                TimeTaken = timeTaken
            };
        }


    }
}
