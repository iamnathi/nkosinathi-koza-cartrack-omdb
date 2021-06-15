using Cartrack.OMDb.Application.Caching;
using Cartrack.OMDb.Web.Models.Results.Models;

namespace Cartrack.OMDb.Application.Tests.Unit
{
    public class InMemoryCacheProviderTests
    {
        private readonly InMemoryCacheProvider<TitleResult> _sut;

        public InMemoryCacheProviderTests()
        {
            _sut = new InMemoryCacheProvider<TitleResult>();
        }


    }
}
