
public static class Utils
{
    private static Random rng = new Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public static string getAccessToken(string headers)
    {
        if (string.IsNullOrEmpty(headers))
            throw new ArgumentNullException(nameof(headers));
        string[] splitToken = headers.Split("Bearer");
        return splitToken[1].Trim();
    }
}