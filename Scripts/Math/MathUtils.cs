using System;

public static class MathUtils {
    public static double ToRadians(this double degree) {
        return degree * Math.PI / 180;
    }

    public static double ToDegrees(this double val) {
        return val * 180 / Math.PI;
    }

    public static bool IsBetween(this double value, double a, double b) {
        return a < value && value <= b;
    }

    public static bool IsBetween(this float value, float a, float b) {
        return a < value && value <= b;
    }

    public static float LimitToRange(this float value, float a, float b) {
        if (value < a) return a;
        else if (value >= b) return b;
        return value;
    }

    public static float Normalize(this float value) {
        if (value >= 0) return 1;
        else return -1;
    }

}