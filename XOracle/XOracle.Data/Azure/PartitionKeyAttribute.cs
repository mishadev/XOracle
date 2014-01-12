using System;

namespace XOracle.Data.Azure
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PartitionKeyAttribute : Attribute
    { }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RowKeyAttribute : Attribute
    { }
}
