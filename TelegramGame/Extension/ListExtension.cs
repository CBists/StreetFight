namespace TelegramGame.Extension;

public static class ListExtension
{
    public static List<T> ListOf<T>(this T item)
    {
        return new List<T> { item };
    }
}