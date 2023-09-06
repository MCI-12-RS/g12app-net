using System;

namespace G12App.Domain.Model.Base
{
    public abstract class BaseEntity
    {
        public bool IsDeleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreationDate { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
