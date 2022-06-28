using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetNow.Domain
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public bool Disabled { get; set; }
        public bool Removed { get; set; }

        protected BaseEntity()
        {
            CreatedTime = DateTime.Now;
        }
    }
}
