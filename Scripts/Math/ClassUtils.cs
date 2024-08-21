using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;

public static class ClassUtils {
    public delegate object OnEachField(object field, string attributeName);
    public static void SetObjectField(this object obj, string attribute, object value) {
        FieldInfo fieldInfo = obj.GetType().GetField(attribute);
        if (fieldInfo is FieldInfo info) {
            info.SetValue(obj, value);
        } else {
            string[] propertyNames = obj.GetType().GetFields().Select(p => p.Name).ToArray();
            GD.PrintErr("[SetObjectField] " + obj.GetType() + " - " + attribute + " - " + value + " - " + string.Join(", ", propertyNames));
        }
    }
    public static object GetObjectField(this object obj, string attribute) {
        FieldInfo fieldInfo = obj.GetType().GetField(attribute);
        if (fieldInfo is FieldInfo info) {
            return info.GetValue(obj);
        } else {
            GD.PrintErr("[GetField] " + obj + " - " + attribute);
            return default;
        }
    }

    public static Dictionary<string, dynamic> GetObjectFields(this object obj) {
        Dictionary<string, dynamic> result = new();
        var fieldsInfo = obj.GetType().GetFields();
        foreach (var fieldInfo in fieldsInfo) {
            result.Add(fieldInfo.Name, obj.GetObjectField(fieldInfo.Name));
        }
        return result;
    }
}