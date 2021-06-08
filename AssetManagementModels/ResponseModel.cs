using System;
namespace AssetManagementSystem.Models
{
    public class Response
    {
        public string Tokens { get; set; }
        public DateTime Expiration { get; set; }
        public string LoggerMessage { get; set;}

    }
}
