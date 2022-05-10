using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace GitLabKit.Runner.Core.Models;

public class Runner
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("description")]
    [Required]
    public string Description { get; set; }
    
    [JsonProperty("ip_address")]
    [Required]
    public string IpAddress { get; set; }
    
    [JsonProperty("active")]
    public bool Active { get; set; }

    [JsonProperty("online")]
    public bool? Online { get; set; }
    
    [JsonProperty("contacted_at")]
    public DateTime ContactedAt { get; set; }

    // from external

    [JsonProperty("tag_list")]
    [Required]
    public IEnumerable<string> TagList { get; set; }

    [Required]
    public IEnumerable<Job> CurrentJob { get; set; }
}