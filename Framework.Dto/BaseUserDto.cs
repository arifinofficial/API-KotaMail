namespace Framework.Dto
{
    public class BaseUserDto<T> : BaseDto<T>
    {
        public string UserId { get; set; }
    }
}
