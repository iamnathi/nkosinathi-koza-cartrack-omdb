using AutoFixture;
using AutoFixture.Xunit2;

namespace Cartrack.OMDb.Application.Tests.Unit.Http.AutoFixture
{
    public class AutoDomainDataAttribute : AutoDataAttribute
    {
        public AutoDomainDataAttribute()
            : base(() =>
            {
                return new Fixture().Customize(new HttpClientCustomization());
            }) { }
    }
}