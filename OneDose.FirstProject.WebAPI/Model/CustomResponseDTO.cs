using Azure;
using System.Text.Json.Serialization;

namespace OneDose.FirstProject.WebAPI.Model
{
    public class CustomResponseDTO<T>
    {
        public T Data { get; private set; }
        [JsonIgnore]
        public int StatusCode { get; private set; }
        public bool IsSuccesful { get; private set; }
        public List<string> Errors { get; set; }

        //Static Factory Meethod
        public static CustomResponseDTO<T> Success(T data, int statusCode) // Data donmuyor
        {
            return new CustomResponseDTO<T> { Data = data, StatusCode = statusCode, IsSuccesful = true };
        }
        public static CustomResponseDTO<T> Success(int statusCode)//Data bos donme durumu
        {
            return new CustomResponseDTO<T> { Data = default(T), StatusCode = statusCode, IsSuccesful = true };
        }
        public static CustomResponseDTO<T> Fail(List<string> Errors, int statusCode)
        {
            return new CustomResponseDTO<T>
            {
                Errors = Errors,
                StatusCode = statusCode,
                IsSuccesful = false
            };
        }
        public static CustomResponseDTO<T> Fail(string Error, int statusCode)
        {

            return new CustomResponseDTO<T> { Errors = new List<string>() { Error }, StatusCode = statusCode, IsSuccesful = false };
        }
    }
}
