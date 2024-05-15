using System;
using System.Collections.Generic;

namespace HopDong.Models;

public partial class ContractsDetail
{
    public string ContractDetailId { get; set; }

    public string ContractId { get; set; }

    public string ContractDetailContent { get; set; }

    public int? ContractDetailValue { get; set; }

    public string ContractDetailStatus { get; set; }

    public int? ContractDetailResign { get; set; }

    public string PartyA { get; set; }

    public string PartyB { get; set; }

    public virtual Contract Contract { get; set; }

    public virtual Party PartyANavigation { get; set; }

    public virtual Party PartyBNavigation { get; set; }
}
