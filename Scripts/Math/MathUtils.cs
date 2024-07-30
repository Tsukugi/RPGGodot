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

}