using System;
using System.Collections.Generic;

namespace API.Models;

public partial class TwilioCredentials
{
    public int Id { get; set; }

    public string Accountid { get; set; } = null!;

    public string AuthToken { get; set; } = null!;

    public string FromNumber { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
