using ClientesApp.Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientesApp.Application.Commands
{
    /// <summary>
    /// Representação de um "COMMAND" CQRS para cliente
    /// </summary>
    public class ClienteCommand : IRequest
    {
        /// <summary>
        /// Registro do log de operação com o cliente (Add, Update, Delete)
        /// </summary>
        public LogClienteModel? LogCliente { get; set; }
    }
}
