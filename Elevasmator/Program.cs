namespace Elevasmator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = string.Empty;

            while (input != null && !input.Trim().Equals("q", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("INPUT ==> ");
                input = Console.ReadLine();
            }
        }
    }
}