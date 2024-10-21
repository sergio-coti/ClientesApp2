using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientesApp.Application.Dtos
{
    /// <summary>
    /// DTO para a requisições (saida de dados) de cliente na aplicação
    /// </summary>
    public class ClienteResponseDto
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Cpf { get; set; }
    }
}
