namespace EmailMarketing.Data
{
    public abstract class IEntity<TKey>
    {
        public TKey Id { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        public IEntity()
        {
            this.IsDeleted = false;
            this.IsActive = true;
        }
    }
}
