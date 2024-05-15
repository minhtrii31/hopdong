using System;
using System.Collections.Generic;

namespace HopDong.Models;

public partial class Contract
{
    public string ContractId { get; set; }

    public string ContractName { get; set; }

    public DateTime? ContractDate { get; set; }

    public string ContractLocation { get; set; }

    public string ContractType { get; set; }

    public DateTime? ContractStart { get; set; }

    public DateTime? ContractEnd { get; set; }

    public virtual ContractType ContractTypeNavigation { get; set; }

    public virtual ICollection<ContractsDetail> ContractsDetails { get; set; } = new List<ContractsDetail>();

    public virtual ICollection<Term> Terms { get; set; } = new List<Term>();
}
