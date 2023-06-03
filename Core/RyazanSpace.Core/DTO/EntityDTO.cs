﻿using RyazanSpace.Interfaces.Entities;

namespace RyazanSpace.Core.DTO
{
    public abstract class EntityDTO<T> where T : class, IBaseEntity
    {
        public EntityDTO() { }
        public EntityDTO(T entity)
        {
            if (entity != null) 
            {
                InitByEntity(entity);
            }
        }

        protected abstract void InitByEntity(T entity);
        public abstract T MapToEntity();
        public abstract T UpdateEntity(T entity);
    }
}
