namespace QuantumAlgorithms.Models.Error
{
    public class NotFoundDto : IErrorDto
    {
        public string Error => "404 Not Found";
        public string ErrorDescription { get; private set; }

        public static IErrorDto ResourceNotFound(string id) => new NotFoundDto { ErrorDescription = $"Resource with ID {id} was not found." };
        public static IErrorDto ParentNotFound(string id) => new NotFoundDto { ErrorDescription = $"Parent with ID {id} was not found." };

    }
}
