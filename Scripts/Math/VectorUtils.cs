using System;
using Godot;

public static class VectorUtils {
    const float degToRad = (float)Math.PI / 180;
    public static readonly Vector3 FarAway = Vector3.One * -9999;
    public static float DegToRad => degToRad;

    public static Vector2 Rotate(this Vector2 vector, float degrees) {
        return vector.RotateRadians(degrees * degToRad);
    }

    public static Vector2 RotateRadians(this Vector2 vector, float radians) {
        float cosA = (float)Math.Cos(radians);
        float sinA = (float)Math.Sin(radians);
        return new Vector2(cosA * vector.X - sinA * vector.Y, sinA * vector.X + cosA * vector.Y);
    }

    public static float GetRotationFromDirection(Vector2 direction) {
        float angleRadians;

        float atan2Result = (float)Math.Atan2(direction.X, direction.Y);
        angleRadians = atan2Result / 2;
        if (angleRadians < 0.0f)
            angleRadians += (float)Math.PI;

        angleRadians *= 2;
        return (float)MathUtils.ToDegrees(angleRadians);
    }
    public static Vector2 FullDirectionTo(this Vector2 start, Vector2 end) {
        return new Vector2(
                  end.X - start.X,
                  end.Y - start.Y);
    }
    public static Vector3 FullDirectionTo(this Vector3 start, Vector3 end) {
        return new Vector3(
                  end.X - start.X,
                  end.Y - start.Y,
                  end.Z - start.Z);
    }
    public static Vector2 ToAbsolute(this Vector2 vector) {
        return new Vector2(Math.Abs(vector.X), Math.Abs(vector.Y));
    }
    public static Vector3 ToAbsolute(this Vector3 vector) {
        return new Vector3(Math.Abs(vector.X), Math.Abs(vector.Y), Math.Abs(vector.Z));
    }

    public static Vector2 ToVector2(this Vector3 worldVector) {
        return new Vector2(worldVector.X, worldVector.Z);
    }
    public static Vector3 ToVector3(this Vector2 worldVector) {
        return new Vector3(worldVector.X, 0, worldVector.Y);
    }

    public static Vector3 Add(this Vector3 vector, float amount) {
        return new Vector3(vector.X + amount, vector.Y + amount, vector.Z + amount);
    }

    public static Vector3 Magnitude(float magnitude) {
        return new Vector3(magnitude, magnitude, magnitude);
    }

    public static Vector3 Magnitude(this Vector3 vector, float magnitude) {
        return vector * new Vector3(magnitude, magnitude, magnitude);
    }

    public static bool IsInArea(this Vector2 point, Rect2 area) {
        return !(point.X < area.Position.X ||
            point.X > area.Size.X ||
            point.Y < area.Position.Y ||
            point.Y > area.Size.Y);
    }

    public static Vector3 AddToY(this Vector3 vector, float amount) {
        return new Vector3(vector.X, vector.Y + amount, vector.Z);
    }

    public static Vector3 AddToX(this Vector3 vector, float amount) {
        return new Vector3(vector.X + amount, vector.Y, vector.Z);
    }

    public static Vector3 AddToZ(this Vector3 vector, float amount) {
        return new Vector3(vector.X, vector.Y + amount, vector.Z + amount);
    }

    public static Vector3 WithY(this Vector3 vector, float Y) {
        return new Vector3(vector.X, Y, vector.Z);
    }
}