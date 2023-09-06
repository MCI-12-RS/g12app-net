using G12App.Domain.Model.Base;
using System;

namespace G12App.Domain.Extensions
{
    public static class BaseEntityExtensions
    {
        public static BaseEntity SetupCreation(this BaseEntity entity)
        {
            entity.CreationDate = DateTime.Now;
            return entity;
        }

        public static BaseEntity SetupLastModified(this BaseEntity entity)
        {
            entity.LastModifiedDate = DateTime.Now;
            return entity;
        }
    }
}
