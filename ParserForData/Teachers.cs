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
        private static string[] faculti;
        private static string[][] Kafedra;
        private static string[][][] Teacher;
        private static string[][][] Link;
        private static IWebDriver driver;
        private static IWebElement query,qurf;
        private static IReadOnlyCollection<IWebElement> querry, querryF, querryK, querryT;
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
        public static void SendTeachersAsync()
        {
            int kf = 0, kk = 0, k = 0, ktl = 0; ;
            driver = new ChromeDriver() { Url = "https://rsue.ru/fakultety/" };
            querryF = driver.FindElements(By.CssSelector("div[class=\"col-lg-11 col-md-10 col-sm-10 col-xs-9 col-xs-offset-1 col-sm-offset-0\"]"));
            List<string> flist = querryF.ToList().Select(nm => nm.Text).ToList();
            kf = 0;
            faculti = new string[querryF.Count];
            Kafedra = new string[querryF.Count][];
            Teacher = new string[querryF.Count][][];
            Link = new string[querryF.Count][][];
            k = 0;
            flist.ToList().ForEach(x =>
            {
                //if (k > 0)
                //{
                //    querryF = driver.FindElements(By.CssSelector("div[class=\"col-lg-11 col-md-10 col-sm-10 col-xs-9 col-xs-offset-1 col-sm-offset-0\"]"));
                //}
                if (kf==5)
                {
                    qurf = driver.FindElement(By.Id("accordion6"));
                    faculti[kf] = qurf.Text;
                    Thread.Sleep(1500);
                    qurf.Click();
                    Thread.Sleep(1500);
                }
                else
                {
                    querryF = driver.FindElements(By.CssSelector("div[class=\"col-lg-11 col-md-10 col-sm-10 col-xs-9 col-xs-offset-1 col-sm-offset-0\"]"));
                    faculti[kf] = querryF.ToList()[kf].Text;
                    Thread.Sleep(1500);
                    querryF.ToList()[kf].Click();
                    Thread.Sleep(1500);
                }
               
                
                kk = 0;
                query = driver.FindElement(By.CssSelector("div[class=\"panel-collapse collapse in\"]")).FindElement(By.Id("kafedri"));
                querryK = query.FindElements(By.TagName("a"));
                Kafedra[kf] = new string[querryK.Count];
                Teacher[kf] = new string[querryK.Count][];
                Link[kf] = new string[querryK.Count][];
                List<string> klist = querryK.ToList().Select(nk => nk.Text).ToList();
                k = 0;
                klist.ToList().ForEach(y =>
                {
                    if (kf == 4 && kk >= 6)
                    {

                    }
                    else
                    {


                        if (k > 0)
                        {
                            if (kf == 5)
                            {
                                qurf = driver.FindElement(By.Id("accordion6"));
                                faculti[kf] = qurf.Text;
                                Thread.Sleep(1500);
                                qurf.Click();
                                Thread.Sleep(1500);
                            }
                            else
                            {
                                querryF = driver.FindElements(By.CssSelector("div[class=\"col-lg-11 col-md-10 col-sm-10 col-xs-9 col-xs-offset-1 col-sm-offset-0\"]"));
                                faculti[kf] = querryF.ToList()[kf].Text;
                                Thread.Sleep(1500);
                                querryF.ToList()[kf].Click();
                                Thread.Sleep(1500);
                            }
                            query = driver.FindElement(By.CssSelector("div[class=\"panel-collapse collapse in\"]")).FindElement(By.Id("kafedri"));
                            querryK = query.FindElements(By.TagName("a"));
                        }
                        if (querryK.ToList()[kk].GetAttribute("href") != "https://vk.com/public146692864" && querryK.ToList()[kk].GetAttribute("href") != "https://www.instagram.com/rsue_economics_and_finance/" && querryK.ToList()[kk].GetAttribute("href") != "https://vk.com/finlearn" && querryK.ToList()[kk].GetAttribute("href") != "https://vk.com/public131015726" && querryK.ToList()[kk].GetAttribute("href") != "https://vk.com/bank_delo_rsue" && querryK.ToList()[kk].GetAttribute("href") != "https://www.facebook.com/groups/bank.delo.rsue" && querryK.ToList()[kk].GetAttribute("href") != "https://vk.com/club110200971" && querryK.ToList()[kk].GetAttribute("href") != "https://vk.com/club142650850" && querryK.ToList()[kk].GetAttribute("href") != "https://vk.com/club141978015" && querryK.ToList()[kk].GetAttribute("href") != "https://vk.com/nalogi.rinh" && querryK.ToList()[kk].GetAttribute("href") != "https://vk.com/postupaem_eif")
                        {
                            Kafedra[kf][kk] = querryK.ToList()[kk].Text;
                            driver.Url = querryK.ToList()[kk].GetAttribute("href");
                            //querryK.ToList()[kk].Click();//При некоторых группах(у которых в коде элемента есть после ссылки ==$0 )не работает клик, из-за этого все вылетает, а в целом все работает
                            Thread.Sleep(1500);
                            querryT = driver.FindElements(By.CssSelector("div[class=\"col-lg-5 col-md-5 col-sm-5\"]"));
                            Teacher[kf][kk] = new string[querryT.Count];
                            Link[kf][kk] = new string[querryT.Count];
                            ktl = 0;
                            querryT.ToList().ForEach(z =>
                            {
                                Teacher[kf][kk][ktl] = z.Text;
                                Link[kf][kk][ktl] = driver.FindElement(By.LinkText(z.Text)).GetAttribute("href");
                                ktl++;
                            });
                            kk++;
                            k++;
                            driver.Url = "https://rsue.ru/fakultety/";
                            Thread.Sleep(1500);
                        }
                    }
                });
                kf++;
            });
            driver.Dispose();
        }
    }
}