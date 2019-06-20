using System;

namespace Domain.Models
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}