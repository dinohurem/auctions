namespace PomoziAuctions.Core.Auth.Security.Models;

[AttributeUsage(AttributeTargets.Field)]
public class ForFeatureAttribute : Attribute
{
  public string FeatureName { get; private set; }

  public ForFeatureAttribute(string featureName)
  {
    FeatureName = featureName;
  }
}
