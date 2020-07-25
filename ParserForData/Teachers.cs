using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ParserForData
{
    class Teachers
    {
        public enum TCP
        {
            Start,
            Faculti,
            SwitchDekKaf,
            Dekanat,
            Kafedra,
        }

        static IWebDriver driver;
        static Dictionary<long, IWebDriver> PvsB = new Dictionary<long, IWebDriver>();
        private static string mes;
        static IWebElement GetElement(By locator)
        {
            List<IWebElement> elements = driver.FindElements(locator).ToList();
            if (elements.Count > 0)
            {
                return elements[0];
            }
            else
            {
                return null;
            }
        }
        public static async Task SendTeachersAsync(string message, long userID, bool tg, TCP sd)
        {
            string[] buttons = new string[1];

            if (!PvsB.ContainsKey(userID))
            {
                PvsB.Add(userID, new ChromeDriver() { Url = "https://rsue.ru/fakultety/" });
                driver = PvsB[userID];
            }
            else
            {
                driver = PvsB[userID];
            }

            IWebElement query;
            IReadOnlyCollection<IWebElement> querry, querrY;
            switch (sd)
            {
                case TCP.Start:
                    int k = 0;
                    querry = driver.FindElements(By.CssSelector("div[class=\"col-lg-11 col-md-10 col-sm-10 col-xs-9 col-xs-offset-1 col-sm-offset-0\"]"));
                    buttons = new string[querry.Count];
                    querry.ToList().ForEach(x =>
                    {
                        buttons[k] = x.Text;
                        k++;
                    });
                    Program.sAuto.SendMessage(buttons, "Выберите факультет", userID, tg);
                    Program.users[userID].Tcp = TCP.Faculti;
                    break;
                case TCP.Faculti:
                    KeyboardBuilder keyboard = new KeyboardBuilder();
                    keyboard.AddButton("Деканат", "Деканат", KeyboardButtonColor.Positive);
                    keyboard.AddLine();
                    keyboard.AddButton("Кафедра", "Кафедра", KeyboardButtonColor.Positive);
                    keyboard.AddLine();
                    keyboard.AddButton("Меню", "Меню", KeyboardButtonColor.Positive);
                    var key = keyboard.Build();
                    query = driver.FindElement(By.LinkText(message));
                    query.Click();
                    mes = message;
                    await Task.Delay(3000);
                    Program.sAuto.SendMessage(key, "Выберите", userID, tg);
                    Program.users[userID].Tcp = TCP.SwitchDekKaf;
                    break;

                case TCP.SwitchDekKaf:
                    switch (message)
                    {
                        case "Деканат":
                            query = driver.FindElement(By.CssSelector("div[class=\"panel-collapse collapse in\"]"));
                            query = query.FindElement(By.LinkText("Подробнее о факультете..."));
                            query.Click();
                            await Task.Delay(3000);
                            switch (mes)
                            {
                                case "Факультет Менеджмента и предпринимательства":
                                    await Task.Delay(3000);
                                    query = driver.FindElement(By.LinkText("Структура факультета"));
                                    query.Click();
                                    await Task.Delay(3000);
                                    query = driver.FindElement(By.Id("content")).FindElement(By.CssSelector("div[class=\"container\"]"));
                                    querry = query.FindElements(By.TagName("h2"));
                                    message = "";
                                    querry.ToList().ForEach(x =>
                                    {
                                        message += x.Text;
                                        message += "\n";
                                    });
                                    driver.Dispose();
                                    PvsB.Remove(userID);
                                    Program.sAuto.SendMessage(message, userID, tg);
                                    Program.users[userID].Tcp = TCP.Start;
                                    Program.users[userID].shtype = ShedType.Start;
                                    Program.sAuto.SendMessage(Program.buttons, "Выберете интерсующую информацию", userID, tg);
                                    break;
                                case "ФМИП":
                                    await Task.Delay(3000);
                                    query = driver.FindElement(By.LinkText("Структура факультета"));
                                    query.Click();
                                    await Task.Delay(3000);
                                    query = driver.FindElement(By.Id("content")).FindElement(By.CssSelector("div[class=\"container\"]"));
                                    querry = query.FindElements(By.TagName("h2"));
                                    message = "";
                                    querry.ToList().ForEach(x =>
                                    {
                                        message += x.Text;
                                        message += "\n";
                                    });
                                    driver.Dispose();
                                    PvsB.Remove(userID);
                                    Program.sAuto.SendMessage(message, userID, tg);
                                    Program.users[userID].Tcp = TCP.Start;
                                    Program.users[userID].shtype = ShedType.Start;
                                    Program.sAuto.SendMessage(Program.buttons, "Выберете интерсующую информацию", userID, tg);
                                    break;
                                case "Факультет Торгового дела":
                                    await Task.Delay(3000);
                                    int sch = 1;
                                    int pisNepis = 0;
                                    int sche = 0;
                                    query = driver.FindElement(By.Id("deaneryTD"));
                                    query = query.FindElement(By.TagName("tbody"));
                                    querry = query.FindElements(By.TagName("td"));
                                    buttons = new string[querry.Count];
                                    message = "";
                                    querry.ToList().ForEach(x =>
                                    {
                                        if (sche == 14)
                                        {
                                            pisNepis = 0;
                                            if (pisNepis < 2)
                                            {
                                                message += x.Text;
                                                if (sch % 2 != 0)
                                                {
                                                    message += " ";
                                                }
                                                else
                                                {
                                                    message += "\n";
                                                }
                                                sch++;
                                                pisNepis++;
                                            }
                                            else
                                            {
                                                pisNepis = 0;
                                            }
                                        }
                                        else
                                        {
                                            if (pisNepis < 2)
                                            {
                                                message += x.Text;
                                                if (sch % 2 != 0)
                                                {
                                                    message += " ";
                                                }
                                                else
                                                {
                                                    message += "\n";
                                                }
                                                sch++;
                                                pisNepis++;
                                            }
                                            else
                                            {
                                                pisNepis = 0;
                                            }
                                        }
                                        sche++;

                                    });
                                    driver.Dispose();
                                    PvsB.Remove(userID);
                                    Program.sAuto.SendMessage(message, userID, tg);
                                    Program.users[userID].Tcp = TCP.Start;
                                    Program.users[userID].shtype = ShedType.Start;
                                    Program.sAuto.SendMessage(Program.buttons, "Выберете интерсующую информацию", userID, tg);
                                    break;
                                case "Факультет Компьютерных технологий и информационной безопасности":
                                    await Task.Delay(3000);
                                    query = driver.FindElement(By.CssSelector("div[class=\"col-lg-5 col-md-5 col-sm-4 col-xs-12 col-sm-offset-1 col-md-offset-0\"]"));
                                    query = driver.FindElement(By.TagName("h2"));
                                    message = "";
                                    message += query.Text;
                                    message += "\n";
                                    query = driver.FindElement(By.Id("sotrudnik"));
                                    querry = query.FindElements(By.TagName("b"));
                                    querry.ToList().ForEach(x =>
                                    {
                                        message += x.Text;
                                        message += "\n";
                                    });
                                    //message = "Тищенко Евгений Николаевич\nМисиченко Надежда Юрьевна\nБогачев Тарас Викторович\nЛозина Екатерина Николаевна";
                                    driver.Dispose();
                                    PvsB.Remove(userID);
                                    Program.sAuto.SendMessage(message, userID, tg);
                                    Program.users[userID].Tcp = TCP.Start;
                                    Program.users[userID].shtype = ShedType.Start;
                                    Program.sAuto.SendMessage(Program.buttons, "Выберете интерсующую информацию", userID, tg);
                                    break;
                                case "ФКТИИБ":
                                    await Task.Delay(3000);
                                    query = driver.FindElement(By.CssSelector("div[class=\"col-lg-5 col-md-5 col-sm-4 col-xs-12 col-sm-offset-1 col-md-offset-0\"]"));
                                    query = query.FindElement(By.TagName("h2"));
                                    message = "";
                                    message += query.Text;
                                    message += "\n";
                                    query = driver.FindElement(By.Id("sotrudnik"));
                                    querry = query.FindElements(By.TagName("b"));
                                    querry.ToList().ForEach(x =>
                                    {
                                        message += x.Text;
                                        message += "\n";
                                    });
                                    //message = "Тищенко Евгений Николаевич\nМисиченко Надежда Юрьевна\nБогачев Тарас Викторович\nЛозина Екатерина Николаевна";
                                    driver.Dispose();
                                    PvsB.Remove(userID);
                                    Program.sAuto.SendMessage(message, userID, tg);
                                    Program.users[userID].Tcp = TCP.Start;
                                    Program.users[userID].shtype = ShedType.Start;
                                    Program.sAuto.SendMessage(Program.buttons, "Выберете интерсующую информацию", userID, tg);
                                    break;
                                case "Учетно-экономический факультет":
                                    await Task.Delay(3000);
                                    query = driver.FindElement(By.Id("content")).FindElement(By.CssSelector("div[class=\"container\"]"));
                                    querry = query.FindElements(By.TagName("h2"));
                                    message = "";
                                    querry.ToList().ForEach(x =>
                                    {
                                        message += x.Text;
                                        message += "\n";
                                    });                                    
                                    PvsB.Remove(userID);
                                    Program.sAuto.SendMessage(message, userID, tg);
                                    Program.users[userID].Tcp = TCP.Start;
                                    Program.users[userID].shtype = ShedType.Start;
                                    Program.sAuto.SendMessage(Program.buttons, "Выберете интерсующую информацию", userID, tg);
                                    break;
                                case "Факультет Экономики и финансов":
                                    message = "Молчанов Евгений Григорьевич\nБогославцева Людмила Викторовна\nТерентьева Вера Викторовна\nАлифанова Елена Николаевна\nКоликова Екатерина Михайловна";                                                      
                                    driver.Dispose();
                                    PvsB.Remove(userID);
                                    Program.sAuto.SendMessage(message, userID, tg);
                                    Program.users[userID].Tcp = TCP.Start;
                                    Program.users[userID].shtype = ShedType.Start;
                                    Program.sAuto.SendMessage(Program.buttons, "Выберете интерсующую информацию", userID, tg);
                                    break;
                                case "Юридический факультет":
                                    driver.Dispose();
                                    PvsB.Remove(userID);
                                    Program.sAuto.SendMessage(message, userID, tg);
                                    Program.users[userID].Tcp = TCP.Start;
                                    Program.users[userID].shtype = ShedType.Start;
                                    Program.sAuto.SendMessage(Program.buttons, "Выберете интерсующую информацию", userID, tg);
                                    break;
                                case "Факультет Лингвистики и журналистики":
                                    message = "Евсюкова Татьяна Всеволодовна\nАгабабян С.Р.\nВолодина О.В.\nДорохина И.В.";
                                    message += "";
                                    driver.Dispose();
                                    PvsB.Remove(userID);
                                    Program.sAuto.SendMessage(message, userID, tg);
                                    Program.users[userID].Tcp = TCP.Start;
                                    Program.users[userID].shtype = ShedType.Start;
                                    Program.sAuto.SendMessage(Program.buttons, "Выберете интерсующую информацию", userID, tg);
                                    break;

                            }
                            break;
                        case "Кафедра":
                            int i = 0;
                            query = driver.FindElement(By.CssSelector("div[class=\"panel-collapse collapse in\"]"));
                            query = query.FindElement(By.Id("kafedri"));
                            querry = query.FindElements(By.TagName("a"));
                            buttons = new string[querry.Count];
                            querry.ToList().ForEach(x =>
                            {
                                buttons[i] = x.Text;
                                i++;
                            });
                            Program.sAuto.SendMessage(buttons, "Выберите кафедру", userID, tg);
                            Program.users[userID].Tcp = TCP.Kafedra;
                            break;
                    }
                    break;
                case TCP.Kafedra:
                    int j = 0;
                    query = driver.FindElement(By.LinkText(message));
                    query.Click();
                    await Task.Delay(1500);
                    querry = driver.FindElements(By.CssSelector("div[class=\"col-lg-5 col-md-5 col-sm-5\"]"));
                    message = "";

                    querry.ToList().ForEach(x =>
                    {
                        message += x.Text;
                        message += "\n";
                        var href = driver.FindElement(By.LinkText(x.Text)).GetAttribute("href");
                        message += href;
                        message += "\n";
                    });
                    query = driver.FindElement(By.Id("content"));
                    query.Click();
                    query = GetElement(By.LinkText("следующая"));
                    if (query != null)
                    {
                        query.Click();
                        await Task.Delay(3000);
                        querrY = driver.FindElements(By.CssSelector("div[class=\"col-lg-5 col-md-5 col-sm-5\"]"));
                        querrY.ToList().ForEach(x =>
                        {
                            message += x.Text;
                            message += "\n";
                            //message += x.GetAttribute("href");
                            var href = driver.FindElement(By.LinkText(x.Text)).GetAttribute("href");
                            message += href;
                            message += "\n";
                        });
                    }
                    driver.Dispose();
                    PvsB.Remove(userID);
                    Program.sAuto.SendMessage(message, userID, tg);
                    Program.users[userID].Tcp = TCP.Start;
                    Program.users[userID].shtype = ShedType.Start;
                    Program.sAuto.SendMessage(Program.buttons, "Выберете интерсующую информацию", userID, tg);
                    break;


            }
        }
    }
}
    }
}
