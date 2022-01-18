using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolyShong.LineBot
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var token = "THGwx6Aq8Qj9K+8UFZfz2z5eM7WUBz/Lapw80QHP1K5loHVwXg3+6RU1E4IgsZLup/ZHSXkA1iqVJZHcn1uq4iXFmgwY6YEsuWWXBpNRvfkHoJ310b/FNKj939A/d0S0sgDaggoojrltq5cvv+7b5gdB04t89/1O/w1cDnyilFU=";

            var who = "U6263208337ed2da1b59ded7c7b90c770";

            isRock.LineBot.Bot bot = new isRock.LineBot.Bot(token);

            for (int i = 0; i < 10; i++)
            {
                bot.PushMessage(who, "hello taiwan !!!" + DateTime.Now);
            }

            bot.PushMessage(who, 446, 1988);
            bot.PushMessage(who, new Uri("https://i.imgur.com/OTac76o.png"));
            Console.ReadLine();
        }
    }
}