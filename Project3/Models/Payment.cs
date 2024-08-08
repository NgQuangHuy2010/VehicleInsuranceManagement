using System;
using System.Collections.Generic;

namespace Project3.Models;

public partial class Payment
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string TransactionId { get; set; } = null!;

    public DateTime PaymentDate { get; set; }

    public bool IsConfirmed { get; set; }

    public int? BillingPolicyId { get; set; }

    public virtual CompanyBillingPolicy? BillingPolicy { get; set; }
}
