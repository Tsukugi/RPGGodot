using Godot;

public static class NodeUtils {
    public static T TryFindParentNodeOfType<T>(this Node child) {
        Node currentNode = child;
        while (true) {
            currentNode = currentNode.GetParent();
            if (currentNode == null)  // We went to the root node
                throw new System.Exception("[TryFindParentNodeOfType] Could not find a direct or indirect parent of this node with the specified type");
            if (currentNode is T typedNode) return typedNode;
        }
    }
}