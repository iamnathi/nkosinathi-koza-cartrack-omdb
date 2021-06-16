using AutoFixture;
using System;
using System.Text.Json;

namespace Cartrack.OMDb.Application.Tests.Unit
{
    public abstract class TitleServiceTestsBase
    {
        protected IFixture AutoFixture = new Fixture();

        protected Uri BaseAddressUri => new Uri("http://img.omdbapi.com");

        protected string GenerateJsonOfType<T>()
        {
            var typeObject = AutoFixture.Create<T>();
            return JsonSerializer.Serialize(typeObject);
        }
    }
}