using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ParserForData
{
    class News
    {
        private static string message;
        private static IWebDriver driver;
        private static string[] date;
        private static string[] title;
        private static string[] content;
        public async static void SendNews()
        {
            IWebDriver driver = new ChromeDriver() { Url = "https://rsue.ru/universitet/novosti/#news" };


            IWebElement query;
            IReadOnlyCollection<IWebElement> querry;
            int i = 0, j = 0, k = 0;
            query = driver.FindElement(By.Id("news-list"));
            querry = query.FindElements(By.Id("news-date"));
            date = new string[querry.Count];
            querry.ToList().ForEach(x =>
            {
                date[i] = x.Text;
                i++;
            });
            querry = query.FindElements(By.Id("news-title"));
            title = new string[querry.Count];
            querry.ToList().ForEach(x =>
            {
                title[j] = x.Text;
                j++;
            });
            querry = query.FindElements(By.Id("news-anons-text"));
            content = new string[querry.Count];
            querry.ToList().ForEach(x =>
            {
                content[k] = x.Text;
                k++;
            });
            for (int h = 0; h < 3; h++)
            {
                message += "&#9203;";
                message += date[h];
                message += "&#9203;\n&#10071;";
                message += title[h];
                message += "&#10071;\n&#128173;";
                message += content[h];
                message += "&#128173;\n\n";
            }
            Console.WriteLine(message);
            driver.Dispose();
        }
    }
}
