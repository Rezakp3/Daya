using Newtonsoft.Json;

namespace Daya
{
    public class StandardResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class StandardResult<T> : StandardResult
    {
        public T? Result { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

}