using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Southport.Messaging.Phone.Vonage.Shared.Jwt;

/// <summary>
/// Specially formated ACLs that a given token is to be given access to
/// <see href="https://developer.nexmo.com/conversation/guides/jwt-acl#acls">see here</see>
/// for a more detailed explenation
/// </summary>
[JsonConverter(typeof(PathSerializer))]
public class Acls
{
    /// <summary>
    /// the set of paths to use for serialization. NOTE: this list will seralize as an object
    /// rather than a standard JSON array
    /// </summary>
    [JsonProperty("paths")]
    public List<AclPath> Paths { get; set; }

    /// <summary>
    /// This generates an ACLS object permitting the bearer to have access to all 
    /// </summary>
    /// <returns></returns>
    public static Acls FullAcls()
    {
        return new Acls
        {
            Paths = new List<AclPath>
            {
                Users,
                Conversations,
                Sessions,
                Devices,
                Image,
                Media,
                Applications,
                Push,
                Knocking
            }
        };
    }

    public static AclPath Users => new() { ApiVersion = "*", ResourceType = "users", Resource = "**" };
    public static AclPath Conversations => new() { ApiVersion = "*", ResourceType = "conversations", Resource = "**" };
    public static AclPath Sessions => new() { ApiVersion = "*", ResourceType = "sessions", Resource = "**" };
    public static AclPath Devices => new() { ApiVersion = "*", ResourceType = "devices", Resource = "**" };
    public static AclPath Image => new() { ApiVersion = "*", ResourceType = "image", Resource = "**" };
    public static AclPath Media => new() { ApiVersion = "*", ResourceType = "media", Resource = "**" };
    public static AclPath Applications => new() { ApiVersion = "*", ResourceType = "applications", Resource = "**" };
    public static AclPath Push => new() { ApiVersion = "*", ResourceType = "push", Resource = "**" };
    public static AclPath Knocking => new() { ApiVersion = "*", ResourceType = "knocking", Resource = "**" };
}