using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ParserForData
{
    static class Schedule
    {
        static Regex regex;
        static IWebDriver driver;
        static IWebElement query;
        static string faculti, year;
        static string []fac;
        static string[][] kurs;
        static string[][][] group;
        static IReadOnlyCollection<IWebElement> querry, querryF, querryY, querryG, querryS;
        internal static IWebElement TryParse(By by,IWebElement web)
        {
            List<IWebElement> elements = web.FindElements(by).ToList();
            if (elements.Count>0)
            {
                return web.FindElement(by);
            }
            else
            {
                return null;
            }
            
        }
        internal static void ParseSchedule()
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
            int k = 0,kf=0,kk=0,kg=0;
            query = driver.FindElement(By.Name("f"));
            querryF = query.FindElements(By.TagName("option"));
            fac = new string[querryF.Count-1];
            kurs = new string[querryF.Count-1][];
            group = new string[querryF.Count-1][][];
            List<string> flist = querryF.ToList().Select(nm => nm.Text).ToList();
            flist.ToList().ForEach( x =>
            {
                if (k>0)
                {
                    query = driver.FindElement(By.Name("f"));
                    querryF = query.FindElements(By.TagName("option"));
                }
                if (querryF.ToList()[kf].Text != "Факультет")
                {
                    fac[kf] = querryF.ToList()[kf].Text;
                    faculti = querryF.ToList()[kf].Text;
                    querryF.ToList()[kf].Click();
                    Thread.Sleep(500);
                    query = driver.FindElement(By.Name("k"));
                    querryY = query.FindElements(By.TagName("option"));
                    var flist = querryY.ToList().Select(fk => fk.Text).ToList();
                    kurs[kf] = new string[querryY.Count - 1];
                    group[kf] = new string[querryY.Count - 1][];
                    kk = 0;
                    flist.ToList().ForEach( y =>
                    {
                        if (k > 0)
                        {
                            query = driver.FindElement(By.Name("f"));
                            querry = query.FindElements(By.TagName("option"));
                            querry.ToList().Find(p => p.Text == faculti).Click();
                            Thread.Sleep(500);
                            query = driver.FindElement(By.Name("k"));
                            querryY = query.FindElements(By.TagName("option"));
                        }
                        if (querryY.ToList()[kk].Text!="Курс")
                        {
                            kurs[kf][kk] = querryY.ToList()[kk].Text;
                            year = querryY.ToList()[kk].Text;
                            querryY.ToList()[kk].Click();
                            Thread.Sleep(500);
                            
                            query = driver.FindElement(By.Name("g"));
                            querryG = query.FindElements(By.TagName("option"));
                            group[kf][kk] = new string[querryG.Count-1];
                            var glist = querryG.ToList().Select(mn => mn.Text).ToList();
                            kg = 0;
                            glist.ToList().ForEach( z =>
                            {
                                if (k > 0)
                                {
                                    query = driver.FindElement(By.Name("f"));
                                    querry = query.FindElements(By.TagName("option"));
                                    querry.ToList().Find(p => p.Text == faculti).Click();
                                    Thread.Sleep(500);
                                    query = driver.FindElement(By.Name("k"));
                                    querry = query.FindElements(By.TagName("option"));
                                    querry.ToList().Find(p => p.Text == year).Click();
                                    Thread.Sleep(500);
                                    query = driver.FindElement(By.Name("g"));
                                    querryG = query.FindElements(By.TagName("option"));
                                    
                                }
                                if (querryG.ToList()[kg].Text!="Группа")
                                {
                                    group[kf][kk][kg] = querryG.ToList()[kg].Text;
                                    querryG.ToList()[kg].Click();
                                    Thread.Sleep(500);
                                    
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
                                                IWebElement temp = TryParse(By.CssSelector("div[class=\"col-lg-12\"]"), f);
                                                if (temp!=null)
                                                {
                                                    if (days[f.FindElement(By.CssSelector("div[class=\"col-lg-12\"]")).Text] == today || found)
                                                    {
                                                        found = true;
                                                        message += f.FindElement(By.CssSelector("div[class=\"col-lg-12\"]")).Text + Environment.NewLine;
                                                        count++;
                                                        f.FindElements(By.CssSelector("div[class=\"col-lg-12 day\"]")).ToList().ForEach(h =>
                                                        {
                                                            message += h.FindElement(By.ClassName("time")).Text + " ";
                                                            message += h.FindElement(By.ClassName("lesson")).Text + " ";
                                                            message += h.FindElement(By.ClassName("prepod")).Text + " ";
                                                            message += String.IsNullOrEmpty(h.FindElements(By.ClassName("aud"))[0].Text)
                                                              ? h.FindElements(By.ClassName("aud"))[0].Text + " "
                                                            : "";
                                                            message += h.FindElements(By.ClassName("aud"))[1].Text + " ";
                                                            message += h.FindElement(By.CssSelector("span[class=\"type n-type\"]")).Text +
                                                                       Environment.NewLine;
                                                        });
                                                        
                                                        
                                                    }
                                                }                                                
                                                
                                                Thread.Sleep(500);
                                            }


                                        }
                                    });
                                    
                                }
                                kg++;
                                k++;

                            });
                            
                        }
                        kk++;
                        
                    });
                }
                kf++;
            });
            driver.Dispose();

        }
    }
}
