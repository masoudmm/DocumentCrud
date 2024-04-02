using DocumentCrud.Domain.Entities;
using NetArchTest.Rules;
using System.Reflection;

namespace DocumentCrud.Domain.Tests;
public class DomainTests
{
    private static Assembly DomainAssembly => typeof(Invoice).Assembly;

    protected static void AssertFailingTypes(IEnumerable<Type> types)
    {
        Assert.True(types == null || !types.GetEnumerator().MoveNext());
    }

    [Fact]
    public void Entity_Cannot_Have_Reference_To_Other_AggregateRoot()
    {
        var entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(IEntity)).GetTypes();

        var aggregateRoots = Types.InAssembly(DomainAssembly)
            .That().ImplementInterface(typeof(IAggregateRoot)).GetTypes().ToList();

        const BindingFlags bindingFlags = BindingFlags.DeclaredOnly |
                                          BindingFlags.NonPublic |
                                          BindingFlags.Instance;

        List<Type> failingTypes = [];
        foreach (var type in entityTypes)
        {
            var fields = type.GetFields(bindingFlags);

            foreach (var field in fields)
            {
                if (aggregateRoots.Contains(field.FieldType) ||
                    field.FieldType.GenericTypeArguments.Any(x => aggregateRoots.Contains(x)))
                {
                    failingTypes.Add(type);
                    break;
                }
            }

            var properties = type.GetProperties(bindingFlags);
            foreach (var property in properties)
            {
                if (aggregateRoots.Contains(property.PropertyType) ||
                    property.PropertyType.GenericTypeArguments.Any(x => aggregateRoots.Contains(x)))
                {
                    failingTypes.Add(type);
                    break;
                }
            }
        }

        AssertFailingTypes(failingTypes);
    }

    [Fact]
    public void Entity_Should_Have_Parameterless_Private_Constructor()
    {
        var entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(IEntity)).GetTypes();

        List<Type> failingTypes = [];
        foreach (var entityType in entityTypes)
        {
            bool hasPrivateParameterlessConstructor = false;
            var constructors = entityType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var constructorInfo in constructors)
            {
                if (constructorInfo.IsPrivate && constructorInfo.GetParameters().Length == 0)
                {
                    hasPrivateParameterlessConstructor = true;
                }
            }

            if (!hasPrivateParameterlessConstructor)
            {
                failingTypes.Add(entityType);
            }
        }

        AssertFailingTypes(failingTypes);
    }

    [Fact]
    public void Domain_Object_Should_Have_Only_Private_Constructors()
    {
        var domainObjectTypes = Types.InAssembly(DomainAssembly)
            .That()
                    .Inherit(typeof(IEntity))
            .GetTypes();

        List<Type> failingTypes = [];
        foreach (var domainObjectType in domainObjectTypes)
        {
            var constructors = domainObjectType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach (var constructorInfo in constructors)
            {
                if (!constructorInfo.IsPrivate)
                {
                    failingTypes.Add(domainObjectType);
                }
            }
        }

        AssertFailingTypes(failingTypes);
    }
}