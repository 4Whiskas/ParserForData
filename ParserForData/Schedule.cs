using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ParserForData
{
    static class Schedule
    {
        static IWebDriver driver;
        static IWebElement query;
        static string faculti, year;
        static IReadOnlyCollection<IWebElement> querry, querryF, querryY, querryG, querryS;
        internal static async  void ParseSchedule()
        {
            driver = new ChromeDriver() { Url = "https://rsue.ru/raspisanie/" };
            Dictionary<string, DayOfWeek> days = new Dictionary<string, DayOfWeek>();
            days.Add("Понедельник", DayOfWeek.Monday);
            days.Add("Вторник", DayOfWeek.Tuesday);
            days.Add("Среда", DayOfWeek.Wednesday);
            days.Add("Четверг", DayOfWeek.Thursday);
            days.Add("Пятница", DayOfWeek.Friday);
            days.Add("Суббота", DayOfWeek.Saturday);
            string message = "";
            int k = 0,kf=0,kk=0;
            query = driver.FindElement(By.Name("f"));
            querryF = query.FindElements(By.TagName("option"));
            List<string> flist = querryF.ToList().Select(nm => nm.Text).ToList();
            int xuinia = 5;
            flist.ToList().ForEach( x =>
            {
                if (querryF.ToList()[kf].Text != "Факультет")
                {
                    faculti = querryF.ToList()[kf].Text;
                    querryF.ToList()[kf].Click();
                    Thread.Sleep(100);
                    query = driver.FindElement(By.Name("k"));
                    querryY = query.FindElements(By.TagName("option"));
                    var flist = querryY.ToList().Select(fk => fk.Text).ToList();
                    
                    flist.ToList().ForEach( y =>
                    {
                        if (k > 0)
                        {
                            query = driver.FindElement(By.Name("f"));
                            querry = query.FindElements(By.TagName("option"));
                            querry.ToList().Find(p => p.Text == faculti).Click();
                            Thread.Sleep(100);
                        }
                        if (querryY.ToList()[kk].Text!="Курс")
                        {
                            year = querryY.ToList()[kk].Text;
                            querryY.ToList()[kk].Click();
                            Thread.Sleep(100);
                            
                            query = driver.FindElement(By.Name("g"));
                            querryG = query.FindElements(By.TagName("option"));
                            var glist = querryG.ToList().Select(mn => mn.Text).ToList();
                            glist.ToList().ForEach( z =>
                            {
                                if (k > 0)
                                {
                                    query = driver.FindElement(By.Name("f"));
                                    querry = query.FindElements(By.TagName("option"));
                                    querry.ToList().Find(p => p.Text == faculti).Click();
                                    Thread.Sleep(100);
                                    query = driver.FindElement(By.Name("k"));
                                    querry = query.FindElements(By.TagName("option"));
                                    querry.ToList().Find(p => p.Text == year).Click();
                                    Thread.Sleep(100);
                                    query = driver.FindElement(By.Name("g"));
                                    querryG = query.FindElements(By.TagName("option"));
                                    
                                }
                                if (querryG.ToList()[k].Text!="Группа")
                                {

                                    querryG.ToList()[k].Click();
                                    Thread.Sleep(100);
                                    
                                    int count = 0;
                                    bool found = false;
                                    querryS = driver.FindElements(By.CssSelector("div[class=\"col-lg-2 col-md-2 col-sm-2\"]"));
                                    DayOfWeek today = DateTime.Now.DayOfWeek != DayOfWeek.Sunday ? DateTime.Now.DayOfWeek : DayOfWeek.Saturday;
                                    
                                    querryS.ToList().ForEach(f =>
                                    {
                                        if (f.Text != "&nbsp;")
                                        {
                                            if (count < 3)
                                            {
                                                //if (days[f.FindElement(By.ClassName("col-lg-12")).Text] == today || found)
                                                //{
                                                //    found = true;
                                                //    message += f.FindElement(By.ClassName("col-lg-12")).Text + Environment.NewLine;
                                                //    count++;
                                                //    f.FindElements(By.CssSelector("div[class=\"col-lg-12 day\"]")).ToList().ForEach(h =>
                                                //    {
                                                //        message += h.FindElement(By.ClassName("time")).Text + " ";
                                                //        message += h.FindElement(By.ClassName("lesson")).Text + " ";
                                                //        message += h.FindElement(By.ClassName("prepod")).Text + " ";
                                                //        //message += String.IsNullOrEmpty(y.FindElements(By.ClassName("aud"))[0].Text)
                                                //          //  ? h.FindElements(By.ClassName("aud"))[0].Text + " "
                                                //            //: "";
                                                //        message += h.FindElements(By.ClassName("aud"))[1].Text + " ";
                                                //        message += h.FindElement(By.CssSelector("span[class=\"type n-type\"]")).Text +
                                                //                   Environment.NewLine;
                                                //    });
                                                //    message += Environment.NewLine + "➖ ➖ ➖ ➖ ➖ ➖\n";
                                                //    Console.WriteLine(message);
                                                //}


                                                ////////////////////////////////////////////////////////////////////////////////
                                                ///
                                                /// 
                                                ///Сделать скачивание строки и парсинг ее через регулярные выражения
                                                ///
                                                /// 
                                                ////////////////////////////////////////////////////////////////////////////////
                                            }


                                        }
                                    });
                                    
                                }
                                k++;

                            });
                            
                        }
                        kk++;
                        
                    });
                }
                kf++;
            });

        }
    }
}
