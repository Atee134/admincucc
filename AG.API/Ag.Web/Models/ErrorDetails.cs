using Newtonsoft.Json;

namespace Ag.Web.Models
{
    public class ErrorDetails
    {
        public int Status { get; set; }
        public string[] Messages { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
