using AutoMapper;
using ClientesApp.Application.Commands;
using ClientesApp.Application.Dtos;
using ClientesApp.Application.Interfaces.Applications;
using ClientesApp.Application.Interfaces.Logs;
using ClientesApp.Application.Interfaces.Messages;
using ClientesApp.Application.Models;
using ClientesApp.Domain.Entities;
using ClientesApp.Domain.Interfaces.Services;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientesApp.Application.Services
{
    public class ClienteAppService : IClienteAppService
    {
        private readonly IClienteDomainService _clienteDomainService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IMessagePublisher _messagePublisher;
        private readonly ILogClienteDataStore _logClienteDataStore;

        public ClienteAppService(IClienteDomainService clienteDomainService, IMapper mapper, IMediator mediator, IMessagePublisher messagePublisher, ILogClienteDataStore logClienteDataStore)
        {
            _clienteDomainService = clienteDomainService;
            _mapper = mapper;
            _mediator = mediator;
            _messagePublisher = messagePublisher;
            _logClienteDataStore = logClienteDataStore;
        }

        public async Task<ClienteResponseDto> AddAsync(ClienteRequestDto request)
        {
            var cliente = _mapper.Map<Cliente>(request);
            cliente.Id = Guid.NewGuid();

            var result = await _clienteDomainService.AddAsync(cliente);

            #region Executar o COMMNAND (CQRS)

            await _mediator.Send(new ClienteCommand
            {
                LogCliente = new LogClienteModel
                {
                    Id = Guid.NewGuid(),
                    DataOperacao = DateTime.Now,
                    TipoOperacao = TipoOperacao.Add,
                    ClienteId = cliente.Id,
                    DadosCliente = JsonConvert.SerializeObject(cliente)
                }
            });

            #endregion

            #region Gerar evento de mensageria

            await _messagePublisher.Send(new Events.ClienteCadastradoEvent
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Email = cliente.Email,
                DataCadastro = DateTime.Now,
                MensagemCadastro = $"Olá, {cliente.Nome}, sua conta foi criada com sucesso!"
            });

            #endregion

            return _mapper.Map<ClienteResponseDto>(result);
        }

        public async Task<ClienteResponseDto> UpdateAsync(Guid id, ClienteRequestDto request)
        {
            var cliente = _mapper.Map<Cliente>(request);
            cliente.Id = id;

            var result = await _clienteDomainService.UpdateAsync(cliente);

            #region Executar o COMMNAND (CQRS)

            await _mediator.Send(new ClienteCommand
            {
                LogCliente = new LogClienteModel
                {
                    Id = Guid.NewGuid(),
                    DataOperacao = DateTime.Now,
                    TipoOperacao = TipoOperacao.Update,
                    ClienteId = cliente.Id,
                    DadosCliente = JsonConvert.SerializeObject(cliente)
                }
            });

            #endregion

            return _mapper.Map<ClienteResponseDto>(result);
        }

        public async Task<ClienteResponseDto> DeleteAsync(Guid id)
        {
            var result = await _clienteDomainService.DeleteAsync(id);

            #region Executar o COMMNAND (CQRS)

            await _mediator.Send(new ClienteCommand
            {
                LogCliente = new LogClienteModel
                {
                    Id = Guid.NewGuid(),
                    DataOperacao = DateTime.Now,
                    TipoOperacao = TipoOperacao.Delete,
                    ClienteId = result.Id,
                    DadosCliente = JsonConvert.SerializeObject(result)
                }
            });

            #endregion

            return _mapper.Map<ClienteResponseDto>(result);
        }

        public async Task<List<ClienteResponseDto>> GetManyAsync(string nome)
        {
            var result = await _clienteDomainService.GetManyAsync(nome);
            return _mapper.Map<List<ClienteResponseDto>>(result);
        }

        public async Task<ClienteResponseDto?> GetByIdAsync(Guid id)
        {
            var result = await _clienteDomainService.GetByIdAsync(id);
            return _mapper.Map<ClienteResponseDto>(result);
        }

        public async Task<LogClienteResponseDto> GetLogs(Guid id, LogClienteRequestDto request)
        {
            //consultar os logs do cliente
            var logs = await _logClienteDataStore.GetAsync(id, request.PageNumber, request.PageSize);

            //consultar a quantidade total de logs do cliente
            var totalCount = await _logClienteDataStore.GetTotalCountAsync(id);

            //retornar o resultado
            return new LogClienteResponseDto
            {
                TotalCount = totalCount,
                PageSize = request.PageSize,
                CurrentPage = request.PageNumber,
                Logs = logs
            };
        }

        public void Dispose()
        {
            _clienteDomainService.Dispose();
        }
    }
}
