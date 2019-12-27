using System.Text;

namespace Potestas.API.Plugin
{
    public static class Settings
    {
        public static string BaseAddress { get => "http://localhost:5000/"; }
        public static Encoding EncodingType { get => Encoding.UTF8; }
        public static string MediaType { get => "application/json";  }
    }
}
