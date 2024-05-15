using System;
using System.Collections.Generic;

namespace HopDong.Models;

public partial class ContractType
{
    public string ContractTypeId { get; set; }

    public string ContractTypeName { get; set; }

    public byte[] ContractFile { get; set; }

    public Guid? FileRow { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}
