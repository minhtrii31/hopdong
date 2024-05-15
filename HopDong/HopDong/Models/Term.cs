using System;
using System.Collections.Generic;

namespace HopDong.Models;

public partial class Term
{
    public string TermId { get; set; }

    public string ContractId { get; set; }

    public string TermHeader { get; set; }

    public string TermContent { get; set; }

    public virtual Contract Contract { get; set; }
}
