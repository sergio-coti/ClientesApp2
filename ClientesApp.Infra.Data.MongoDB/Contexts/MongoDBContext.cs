using ClientesApp.Application.Models;
using ClientesApp.Infra.Data.MongoDB.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientesApp.Infra.Data.MongoDB.Contexts
{
    /// <summary>
    /// Classe para conexão como o MongoDB e mapeamento das collections
    /// </summary>
    public class MongoDBContext
    {
        private readonly MongoDBSettings _mongoDBSettings;
        private IMongoDatabase _mongoDatabase;

        public MongoDBContext(MongoDBSettings mongoDBSettings)
        {
            _mongoDBSettings = mongoDBSettings;

            #region Conexão com o banco de dados

            var settings = MongoClientSettings.FromUrl(new MongoUrl(_mongoDBSettings.Url));
            var client = new MongoClient(settings);

            _mongoDatabase = client.GetDatabase(_mongoDBSettings.Database);

            #endregion
        }

        public IMongoCollection<LogClienteModel> LogClientes
            => _mongoDatabase.GetCollection<LogClienteModel>("LogClientes");
    }
}
