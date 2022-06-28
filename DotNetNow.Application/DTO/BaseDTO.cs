using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetNow.Application.DTO
{
    public class BaseDTO
    {
        public int Id { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool Disabled { get; set; }
        public bool Removed { get; set; }
    }
}
