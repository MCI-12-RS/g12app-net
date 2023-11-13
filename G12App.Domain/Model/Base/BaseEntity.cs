using System;
using System.ComponentModel.DataAnnotations;

namespace G12App.Domain.Model.Base
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreationDate { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
