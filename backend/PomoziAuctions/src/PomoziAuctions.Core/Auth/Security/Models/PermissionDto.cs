using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Ardalis.GuardClauses;

namespace PomoziAuctions.Core.Auth.Security.Models;

public class PermissionDto
{
  public string Name { get; }

  public string ShortName { get; }

  public string GroupName { get; }

  public string Description { get; }

  public string RequiredFeature { get; }

  private PermissionDto(string name, string groupName, string shortName, string description, string requiredFeature = null)
  {
    Name = Guard.Against.NullOrEmpty(name);
    GroupName = Guard.Against.NullOrEmpty(groupName);
    ShortName = shortName;
    Description = description;
    RequiredFeature = requiredFeature;
  }

  public static List<PermissionDto> MapFromEnum<TEnum>()
    where TEnum : struct, IConvertible, IFormattable, IComparable
  {
    var permissions = new List<PermissionDto>();

    foreach (var permissionName in Enum.GetNames(typeof(TEnum)))
    {
      var memberInfo = typeof(TEnum).GetMember(permissionName);

      var displayAttribute = memberInfo[0].GetCustomAttribute<DisplayAttribute>();
      if (displayAttribute == null || displayAttribute.AutoGenerateField == true)
      {
        continue;
      }

      permissions.Add(new PermissionDto(permissionName, displayAttribute.GroupName, displayAttribute.Name, displayAttribute.Description));
    }

    return permissions;
  }
}
