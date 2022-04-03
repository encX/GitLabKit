using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace GitLabKit.Runner.Core.Models;

public class Job
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("status")]
    [Required]
    public string Status { get; set; }
    
    [JsonProperty("name")]
    [Required]
    public string Name { get; set; }
    
    [JsonProperty("ref")]
    [Required]
    public string Ref { get; set; }
    
    [JsonProperty("started_at")]
    public DateTime StartedAt { get; set; }

    [JsonProperty("duration")]
    public double Duration { get; set; }
    
    [JsonProperty("web_url")]
    [Required]
    public string WebUrl { get; set; }
    
    [JsonProperty("finished_at")]
    public DateTime? FinishedAt { get; set; }

    [JsonProperty("project")]
    public Project Project { get; set; }
}