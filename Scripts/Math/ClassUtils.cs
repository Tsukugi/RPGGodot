
using System.Linq;
using System.Reflection;
using Godot;

public static class ClassUtils {
    public static void SetObjectProperty(this object obj, string attribute, object value) {
        PropertyInfo propertyInfo = obj.GetType().GetProperty(attribute);
        if (propertyInfo is PropertyInfo info) {
            info.SetValue(obj, value, null);
        } else {
            string[] propertyNames = obj.GetType().GetProperties().Select(p => p.Name).ToArray();
            GD.PrintErr("[SetObjectProperty] " + obj + " - " + attribute + " - " + value + " - " + string.Join(", ", propertyNames));
        }
    }
    public static object? GetObjectProperty(this object obj, string attribute) {
        PropertyInfo propertyInfo = obj.GetType().GetProperty(attribute);
        if (propertyInfo is PropertyInfo info) {
            return info.GetValue(obj, null);
        } else {
            GD.PrintErr("[GetObjectProperty] " + obj + " - " + attribute);
            return null;
        }
    }
}