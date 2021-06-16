using AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cartrack.OMDb.Application.Tests.Unit.Http.AutoFixture
{
    public class SingleParameterConstructorQuery : IMethodQuery
    {
        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            return from m in type.GetConstructors()
                   let parameters = m.GetParameters()
                   where parameters.Length == 1
                   select new ConstructorMethod(m) as IMethod;
        }
    }
}