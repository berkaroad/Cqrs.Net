using System;

namespace CqrsFramework
{
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        private readonly Guid _entityId;
        private readonly string _entityType;

        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(Guid entityId) : base(entityId.ToString())
        {
            this._entityId = entityId;
        }

        public EntityNotFoundException(Guid entityId, string entityType)
            : base(entityType + ": " + entityId.ToString())
        {
            this._entityId = entityId;
            this._entityType = entityType;
        }

        public EntityNotFoundException(Guid entityId, string entityType, string message, Exception inner)
            : base(message, inner)
        {
            this._entityId = entityId;
            this._entityType = entityType;
        }

        public Guid EntityId
        {
            get { return this._entityId; }
        }

        public string EntityType
        {
            get { return this._entityType; }
        }
    }
}
