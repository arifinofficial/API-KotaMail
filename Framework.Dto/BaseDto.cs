namespace Framework.Dto
{
    public abstract class BaseDto<T>
    {
        public T Id { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime LastModifiedDateTime { get; set; }
    }
}
