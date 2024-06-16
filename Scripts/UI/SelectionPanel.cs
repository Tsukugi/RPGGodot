using Godot;
using System;

public partial class SelectionPanel : Control {
    public void ApplySelectionTransform(Vector2 start, Vector2 end) {
        Vector2 selectionSize = GetSize(start, end);
        Vector2 startPosition = GetStartPosition(start, end);
        Position = startPosition;
        Size = selectionSize;
    }

    public static Vector2 GetSize(Vector2 start, Vector2 end) {
        return new Vector2(
              Math.Abs(start.X - end.X),
              Math.Abs(start.Y - end.Y));
    }
    public static Vector3 GetSize(Vector3 start, Vector3 end) {
        return new Vector3(
              Math.Abs(start.X - end.X),
              Math.Abs(start.Y - end.Y),
              Math.Abs(start.Z - end.Z));
    }
    public static Vector2 GetStartPosition(Vector2 start, Vector2 end) {
        return new Vector2(
            Math.Min(start.X, end.X),
            Math.Min(start.Y, end.Y));
    }
    public static Vector3 GetStartPosition(Vector3 start, Vector3 end) {
        return new Vector3(
            Math.Min(start.X, end.X),
            Math.Min(start.Y, end.Y),
            Math.Min(start.Z, end.Z));
    }

    public void ResetPosition() {
        Position = Vector2.Zero;
        Size = Vector2.Zero;
    }
}
