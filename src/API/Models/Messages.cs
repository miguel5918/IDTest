using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Messages
{
    public int MessageId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Recipient { get; set; } = null!;

    public string Message { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<MessageSending> MessageSendings { get; set; } = new List<MessageSending>();
}
