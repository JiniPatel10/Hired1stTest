using System;

namespace twoladder.Core.Domain.Models
{
    public class BaseModel
    {
        public string CreatedBy { get; set; } = "";
        public string LastModifiedBy { get; set; } = "";
        public DateTime Updated { get; set; } = DateTime.Now;
        public DateTime Created{ get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,DateTime.Now.Hour, DateTime.Now.Minute,0,0,DateTimeKind.Utc);

    }
}
