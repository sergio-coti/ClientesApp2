using ClientesApp.Application.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientesApp.Application.Interfaces.Messages
{
    /// <summary>
    /// Interface para envio de eventos para a mensageria
    /// </summary>
    public interface IMessagePublisher
    {
        Task Send(ClienteCadastradoEvent @event);
    }
}
