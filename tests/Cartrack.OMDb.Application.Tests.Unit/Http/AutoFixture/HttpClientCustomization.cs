using AutoFixture;
using AutoFixture.Kernel;
using System.Net.Http;

namespace Cartrack.OMDb.Application.Tests.Unit.Http.AutoFixture
{
    public class HttpClientCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<HttpClient>(c => c.FromFactory(new MethodInvoker(new SingleParameterConstructorQuery())));
        }
    }
}