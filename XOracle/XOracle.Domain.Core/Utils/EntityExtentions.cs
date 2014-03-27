using System;

namespace XOracle.Domain.Core.Utils
{
    public static class EntityExtentions
    {
        public static TProperty GetProperty<TProperty, TEntity>(this TEntity entity, Func<TEntity, TProperty> get, TProperty @default = default(TProperty))
            where TEntity : Entity
        {
            if (entity != null)
                return get(entity);

            return @default;
        }

        public static Guid GetIdentifier<TEntity>(this TEntity entity, Guid @default = default(Guid))
            where TEntity : Entity
        {
            return entity.GetProperty(e => e.Id, @default);
        }
    }
}
