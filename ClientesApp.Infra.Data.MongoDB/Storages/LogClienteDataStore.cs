using ClientesApp.Application.Interfaces.Logs;
using ClientesApp.Application.Models;
using ClientesApp.Infra.Data.MongoDB.Contexts;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientesApp.Infra.Data.MongoDB.Storages
{
    /// <summary>
    /// Implementação da interface da camada de aplicação
    /// para geração dos logs de clientes no MongoDB
    /// </summary>
    public class LogClienteDataStore : ILogClienteDataStore
    {
        private readonly MongoDBContext _mongoDBContext;

        public LogClienteDataStore(MongoDBContext mongoDBContext)
        {
            _mongoDBContext = mongoDBContext;
        }

        public async Task AddAsync(LogClienteModel model)
        {
            await _mongoDBContext.LogClientes.InsertOneAsync(model);
        }

        public async Task<List<LogClienteModel>> GetAsync(Guid clienteId, int pageNumber, int pageSize)
        {
            //definindo o filtro para consultar somente logs de um determinado cliente
            var filter = Builders<LogClienteModel>.Filter.Eq(log => log.ClienteId, clienteId);

            //construindo a consulta com a paginação
            var result = await _mongoDBContext.LogClientes
                .Find(filter) //aplicando o filtro
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .SortByDescending(log => log.DataOperacao)
                .ToListAsync();

            return result;
        }

        public async Task<int> GetTotalCountAsync(Guid clienteId)
        {
            //definindo o filtro para consultar somente logs de um determinado cliente
            var filter = Builders<LogClienteModel>.Filter.Eq(log => log.ClienteId, clienteId);

            return (int) await _mongoDBContext.LogClientes.CountDocumentsAsync(filter);
        }
    }
}
