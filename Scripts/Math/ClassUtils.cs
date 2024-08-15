using System.Reflection;

public static class ClassUtils {
    public static void SetObjectProperty(this object obj, string attribute, object value) {
        PropertyInfo propertyInfo = obj.GetType().GetProperty(attribute);
        propertyInfo?.SetValue(obj, value, null);
    }
}