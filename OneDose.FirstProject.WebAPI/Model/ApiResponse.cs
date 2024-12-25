using System.Net;
using System.Runtime.Serialization;

namespace OneDose.FirstProject.WebAPI.Model
{
    [DataContract]
    public class ApiResponse<T>
    {
        public ApiResponse(bool hasError, T result, object errorMessage = null)
        {
            HasError = hasError;
            Result = result;
            ErrorMessage = errorMessage;
        }

        public ApiResponse()
        {

        }

        [DataMember]
        public string Version { get { return "1.0"; } }

        [DataMember]
        public bool HasError { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public object ErrorMessage { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public T Result { get; set; }
    }
}
