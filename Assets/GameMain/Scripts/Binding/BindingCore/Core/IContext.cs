using System;
using GameFramework;

namespace StarForce
{
    public interface IContext
    {
        void SetPropertyValue(string propertyName, object value);

        Property FindProperty(string propertyName);

        Collection FindCollection(string collectionName);

        void AddPropertyRuntime(string propertyName, Type type);
    }
}