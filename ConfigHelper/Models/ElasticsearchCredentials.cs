namespace ConfigHelper.Models
{
    /// <summary>
    /// Representa as credenciais necessárias para se conectar a uma instância do Elasticsearch.
    /// </summary>
    public class ElasticsearchCredentials
    {
        /// <summary>
        /// Obtém ou define o URI da instância do Elasticsearch.
        /// </summary>
        /// <value>
        /// Uma string que representa o URI onde o Elasticsearch está localizado.
        /// </value>
        public string Uri { get; set; }

        /// <summary>
        /// Obtém ou define o nome de usuário usado para autenticação no Elasticsearch.
        /// </summary>
        /// <value>
        /// Uma string que representa o nome de usuário para autenticação.
        /// </value>
        public string Username { get; set; }

        /// <summary>
        /// Obtém ou define a senha usada para autenticação no Elasticsearch.
        /// </summary>
        /// <value>
        /// Uma string que representa a senha para autenticação.
        /// </value>
        public string Password { get; set; }
    }
}
