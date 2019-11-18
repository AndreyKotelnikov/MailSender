namespace Entities.Abstract
{
    /// <summary>
    /// Именованная сущность
    /// </summary>
    public abstract class NamedEntity : BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
    }
}
