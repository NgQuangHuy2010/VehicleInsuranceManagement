using System;
using System.Collections.Generic;

namespace Project3.Models;

public partial class VehiclePolicyType
{
    public int PolicyTypeId { get; set; }

    public string? PolicyName { get; set; }

    public string? PolicyDetails { get; set; }
}
