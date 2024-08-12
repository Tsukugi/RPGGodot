
using System;
using System.Collections.Generic;

public static class ListUtils {
    private static Random rng = new Random();

    public static void Shuffle<T>(this List<T> list) {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static List<T> ShuffleNew<T>(this List<T> list) {
        var newList = new List<T>(list);
        int n = newList.Count;
        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            T value = newList[k];
            newList[k] = newList[n];
            newList[n] = value;
        }
        return list;
    }
}