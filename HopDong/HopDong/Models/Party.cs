using System;
using System.Collections.Generic;

namespace HopDong.Models;

public partial class Party
{
    public string PartyId { get; set; }

    public string PartyName { get; set; }

    public string PartyAddress { get; set; }

    public string PartyContact { get; set; }

    public string PartyAccount { get; set; }

    public string PartyTax { get; set; }

    public string PartyRepresentative { get; set; }

    public string PartyPosition { get; set; }

    public virtual ICollection<ContractsDetail> ContractsDetailPartyANavigations { get; set; } = new List<ContractsDetail>();

    public virtual ICollection<ContractsDetail> ContractsDetailPartyBNavigations { get; set; } = new List<ContractsDetail>();
}
