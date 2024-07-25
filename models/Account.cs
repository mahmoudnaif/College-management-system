using System;
using System.Collections.Generic;

namespace College_managemnt_system.models;

public partial class Account
{
    public long AccountId { get; set; }

    public string Email { get; set; } = null!;

    public byte[] Password { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public string Role { get; set; } = null!;

    public bool Verified { get; set; }

    public virtual Professor? Professor { get; set; }

    public virtual Student? Student { get; set; }

    public virtual TeachingAssistance? TeachingAssistance { get; set; }
}
