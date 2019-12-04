using RepositoryAbstract;
using System;

namespace Entities.Abstract
{
    public interface IBaseEntity : IUniqueEntity
    {
        DateTime CreatedDate { get; set; }

        byte[] RowVersion { get; set; }
    }
}
