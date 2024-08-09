using System;
using System.Collections.Generic;

namespace Project3.Models;

public partial class Coverage
{
    public int Id { get; set; }

    public string CoverageName { get; set; } = null!;

    public string? Description { get; set; }

    public int? EstimateId { get; set; }

    public virtual Estimate? Estimate { get; set; }
}
