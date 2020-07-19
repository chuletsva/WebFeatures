using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using Domian.Common;

namespace Application.Tests.Common.Helpers
{
    internal class EntitySpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (!(request is PropertyInfo property)) return new NoSpecimen();

            if (property.Name == nameof(ISoftDelete.IsDeleted)) return false;

            if (property.PropertyType.IsSubclassOf(typeof(Entity))
             || property.PropertyType.IsGenericType 
             && property.PropertyType.GetInterfaces()
                   .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>))
             || property.PropertyType.IsGenericType
             && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return null;
            }

            return new NoSpecimen();

        }
    }
}