using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace API.Models;

public partial class MessageSending
{
    public int Id { get; set; }

    public int MessageId { get; set; }

    public DateTime SentAt { get; set; }

    public string ConfirmationCode { get; set; } = null!;

    [JsonIgnore]
    public virtual Messages Message { get; set; } = null!;
}
