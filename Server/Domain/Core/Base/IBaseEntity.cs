namespace Domain.Core.Base
{
    public interface IBaseEntity<TKey>
    {
        TKey Id { get; set; }
    }
}