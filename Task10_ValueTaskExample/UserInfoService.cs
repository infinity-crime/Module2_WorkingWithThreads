using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task10_ValueTaskExample
{
    public class UserInfoService
    {
        private readonly Dictionary<int, string> _userData = new Dictionary<int, string>();

        public async ValueTask<string> GetDataAsync(int userId)
        {
            var userInfoStr = string.Empty;
            
            if(_userData.TryGetValue(userId, out userInfoStr))
            {
                Console.WriteLine("Запись найдена в кэше");
                return userInfoStr;
            }

            Console.WriteLine("Данной записи нет в кэше -> добавление записи.");

            Console.WriteLine("Загрузка данных с удаленного хранилища...");
            await Task.Delay(1000);
            userInfoStr = $"Данные пользователя {userId}";

            _userData[userId] = userInfoStr;
            Console.WriteLine($"Данные успешно добавлены: userId = {userId}, info = {userInfoStr}");

            return userInfoStr;
        }
    }
}
