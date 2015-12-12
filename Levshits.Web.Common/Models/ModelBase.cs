using System;

namespace Levshits.Web.Common.Models
{
    public abstract class ModelBase
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
    }
}