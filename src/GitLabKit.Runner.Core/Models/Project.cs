using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace GitLabKit.Runner.Core.Models;

public class Project
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("name")]
    [Required]
    public string Name { get; set; }
    
    [JsonProperty("path_with_namespace")]
    [Required]
    public string PathWithNamespace { get; set; }
}