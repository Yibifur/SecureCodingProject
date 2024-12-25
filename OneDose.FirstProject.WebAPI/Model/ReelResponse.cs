using System.Text.Json.Serialization;

namespace OneDose.FirstProject.WebAPI.Model
{
    public class ReelResponse<T>
    {
        public T Data { get; private set; }
        [JsonIgnore]
        public int StatusCode { get; private set; }
        public bool IsSuccesful { get; private set; }
        public List<string> Errors { get; set; }

        //Static Factory Meethod
        public static ReelResponse<T> Success(T data, int statusCode) // Data donmuyor
        {
            return new ReelResponse<T> { Data = data, StatusCode = statusCode, IsSuccesful = true };
        }
        public static ReelResponse<T> Success(int statusCode)//Data bos donme durumu
        {
            return new ReelResponse<T> { Data = default(T), StatusCode = statusCode, IsSuccesful = true };
        }
        public static ReelResponse<T> Fail(List<string> Errors, int statusCode)
        {
            return new ReelResponse<T>
            {
                Errors = Errors,
                StatusCode = statusCode,
                IsSuccesful = false
            };
        }
        public static ReelResponse<T> Fail(string Error, int statusCode)
        {

            return new ReelResponse<T> { Errors = new List<string>() { Error }, StatusCode = statusCode, IsSuccesful = false };
        }


    }
}
