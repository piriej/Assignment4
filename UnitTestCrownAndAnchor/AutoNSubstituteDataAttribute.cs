using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Xunit;
using Xunit.Extensions;

namespace UnitTestCrownAndAnchor
{
    public class AutoNSubstituteDataAttribute : AutoDataAttribute
    {
        internal AutoNSubstituteDataAttribute()
            : base(new Fixture().Customize(new AutoNSubstituteCustomization()))
        {
        }
    }


    internal class AutoNSubstitutePropertyDataAttribute : CompositeDataAttribute
    {
        internal AutoNSubstitutePropertyDataAttribute(string propertyName)
            : base(
                new DataAttribute[] {
                new PropertyDataAttribute(propertyName),
                new AutoNSubstituteDataAttribute() })
        {
        }
    }
}