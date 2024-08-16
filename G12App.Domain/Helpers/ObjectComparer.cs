using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace G12App.Domain.Helpers
{
    public record PropertyDifference
    {
        public string PropertyPath { get; init; }
        public object Value1 { get; init; }
        public object Value2 { get; init; }

        public override string ToString()
        {
            return $"{PropertyPath}: {Value1} != {Value2}";
        }
    }

    /// <summary>
    /// Compares two objects of the same type and returns a list of differences in property values
    /// </summary>
    public static class ObjectComparer
    {
        public static List<PropertyDifference> CompareObjects(object obj1, object obj2, string parentPath = "")
        {
            List<PropertyDifference> differences = [];

            if (obj1 == null && obj2 == null)
            {
                return differences;
            }
            else if (obj1 == null || obj2 == null)
            {
                differences.Add(new PropertyDifference
                {
                    PropertyPath = parentPath,
                    Value1 = obj1,
                    Value2 = obj2
                });
                return differences;
            }

            Type type1 = obj1.GetType();
            Type type2 = obj2.GetType();

            if (type1 != type2)
            {
                differences.Add(new PropertyDifference
                {
                    PropertyPath = parentPath,
                    Value1 = obj1,
                    Value2 = obj2
                });

                return differences;
            }

            if (IsPrimitive(type1))
            {
                if (!obj1.Equals(obj2))
                {
                    differences.Add(new PropertyDifference
                    {
                        PropertyPath = parentPath,
                        Value1 = obj1,
                        Value2 = obj2
                    });
                }
                return differences;
            }

            PropertyInfo[] properties = type1.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                object value1 = property.GetValue(obj1);
                object value2 = property.GetValue(obj2);
                string propertyPath = string.IsNullOrEmpty(parentPath) ? property.Name : $"{parentPath}.{property.Name}";

                if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string))
                {
                    var list1 = ((IEnumerable)value1)?.Cast<object>().ToList() ?? [];
                    var list2 = ((IEnumerable)value2)?.Cast<object>().ToList() ?? [];

                    int count = Math.Max(list1.Count, list2.Count);
                    for (int i = 0; i < count; i++)
                    {
                        object item1 = i < list1.Count ? list1[i] : null;
                        object item2 = i < list2.Count ? list2[i] : null;
                        differences.AddRange(CompareObjects(item1, item2, $"{propertyPath}[{i}]"));
                    }
                }
                else
                {
                    // recursively compare objects
                    differences.AddRange(CompareObjects(value1, value2, propertyPath));
                }
            }

            return differences;
        }

        private static bool IsPrimitive(Type type)
        {
            return type.IsPrimitive || type.IsValueType || type == typeof(string) || type == typeof(DateTime);
        }
    }
}
