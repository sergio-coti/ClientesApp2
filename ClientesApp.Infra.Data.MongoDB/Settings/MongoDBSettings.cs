using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientesApp.Infra.Data.MongoDB.Settings
{
    /// <summary>
    /// Classe para capturar as configurações do /appsettings
    /// </summary>
    public class MongoDBSettings
    {
        public string? Url { get; set; }
        public string? Database { get; set; }
    }
}
