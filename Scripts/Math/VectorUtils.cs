using System;
using Godot;

public static class VectorUtils {
    private const float DegToRad = (float)Math.PI / 180;

    public static Vector2 Rotate(this Vector2 vector, float degrees) {
        return vector.RotateRadians(degrees * DegToRad);
    }

    public static Vector2 RotateRadians(this Vector2 vector, float radians) {
        float cosA = (float)Math.Cos(radians);
        float sinA = (float)Math.Sin(radians);
        return new Vector2(cosA * vector.X - sinA * vector.Y, sinA * vector.X + cosA * vector.Y);
    }
}