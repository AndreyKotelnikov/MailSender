using System;

namespace Entities.Abstract
{
    public abstract class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
