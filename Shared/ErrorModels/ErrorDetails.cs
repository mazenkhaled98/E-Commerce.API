using System.Text.Json;

namespace Shared.ErrorModels
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }

        public string ErrorMessage { get; set; }= string.Empty;

        public IEnumerable<string>? Errors { get; set; }

        override public string ToString()
        {
            return JsonSerializer.Serialize(this);
        }


    }
}
