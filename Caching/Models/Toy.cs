using System;
using System.Collections.Generic;

namespace Caching.Models
{
    public partial class Toy
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
    }
}
