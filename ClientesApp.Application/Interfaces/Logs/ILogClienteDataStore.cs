using ClientesApp.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientesApp.Application.Interfaces.Logs
{
    public interface ILogClienteDataStore
    {
        Task AddAsync(LogClienteModel model);

        Task<List<LogClienteModel>> GetAsync(Guid clienteId, int pageNumber, int pageSize);

        Task<int> GetTotalCountAsync(Guid clienteId);
    }
}
