namespace Encrypt_POS_Script_Password;

public static class StackExtensions
{
    public static Stack<T> Clone1<T>(this IEnumerable<T> original)
    {
        return new Stack<T>(new Stack<T>(original));
    }

    public static Stack<T> Clone2<T>(this IEnumerable<T> original)
    {
        return new Stack<T>(original.Reverse());
    }

    public static Stack<T> Clone3<T>(this Stack<T> original)
    {
        var arr = original.ToArray();
        Array.Reverse(arr);
        return new Stack<T>(arr);
    }

    public static Stack<T> Clone4<T>(this Stack<T> original)
    {
        var arr = new T[original.Count];
        original.CopyTo(arr, 0);
        Array.Reverse(arr);
        return new Stack<T>(arr);
    }
}