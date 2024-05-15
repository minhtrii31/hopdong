using System;
using System.Collections.Generic;

namespace HopDong.Models;

public partial class User
{
    public string UserId { get; set; }

    public string UserFullName { get; set; }

    public string Username { get; set; }

    public string UserPassword { get; set; }

    public string UserRole { get; set; }

    public DateTime? CreateAt { get; set; }
}
