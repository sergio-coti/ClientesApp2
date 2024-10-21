using ClientesApp.Application.Commands;
using ClientesApp.Application.Interfaces.Logs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientesApp.Application.Handlers
{
    /// <summary>
    /// Classe para processar os COMMANDS de cliente disparados pelo MediatR
    /// </summary>
    public class ClienteRequestHandler : IRequestHandler<ClienteCommand>
    {
        private readonly ILogClienteDataStore _logClienteDataStore;

        public ClienteRequestHandler(ILogClienteDataStore logClienteDataStore)
        {
            _logClienteDataStore = logClienteDataStore;
        }

        public async Task Handle(ClienteCommand request, CancellationToken cancellationToken)
        {
            await _logClienteDataStore.AddAsync(request.LogCliente);
        }
    }
}
