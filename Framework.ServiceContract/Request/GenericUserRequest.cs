namespace Framework.ServiceContract.Request
{
    public class GenericUserRequest<T> : GenericRequest<T>
    {
        public string UserId { get; set; }
    }
}
