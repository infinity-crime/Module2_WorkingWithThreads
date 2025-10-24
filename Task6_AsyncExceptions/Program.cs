using System.Threading.Tasks;

namespace Task6_AsyncExceptions
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                await PrintAsync("Hello!");

                await PrintAsync("Hello"); // вот тут вылетит исключение (эвейтом выбросим наружу)
            }
            catch(ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // если использовать async void -> исключение нельзя будет отловить!
        static async Task PrintAsync(string message)
        {
            if (!message.EndsWith('!'))
                throw new ArgumentException("В конце сообщения должен быть \'!\'");

            await Task.Delay(1000); // имитация долгой работы
            Console.WriteLine(message);
        }
    }
}
