using ClientesApp.Application.Events;
using ClientesApp.Application.Interfaces.Messages;
using ClientesApp.Infra.Messages.Settings;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientesApp.Infra.Messages.Publishers
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly RabbitMQSettings _rabbitMQSettings;

        public MessagePublisher(RabbitMQSettings rabbitMQSettings)
        {
            _rabbitMQSettings = rabbitMQSettings;
        }

        public async Task Send(ClienteCadastradoEvent @event)
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMQSettings.Host,
                Port = _rabbitMQSettings.Port,
                UserName = _rabbitMQSettings.Username,
                Password = _rabbitMQSettings.Password,
            };

            using (var connection = await Task.Run(() => factory.CreateConnection()))
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: _rabbitMQSettings.Queue,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                    );

                var json = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(json);

                await Task.Run(() => channel.BasicPublish(
                    exchange: "",
                    routingKey: _rabbitMQSettings.Queue,
                    basicProperties: null,
                    body: body
                    ));
            }
        }
    }
}
