using System.ComponentModel.DataAnnotations;

namespace Domain.Core.Base
{
    public abstract class BaseEntity<TKey> : IBaseEntity<TKey>
    {
        [Key]
        public TKey Id { get; set; }
    }
}
