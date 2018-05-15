using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusingAndReTrySample
{
    class Program
    {
        //***    少搞一点 封装， 少搞一点 控制反转（Ioc）， 少搞一点 AOP， 少搞一些 “声明式”

        //***    不要隐藏太多代码，让 代码 回归 代码

        //***    是 找回 80 年代 写 Basic 的 那种感觉 的 时候了。
        static void Main(string[] args)
        {

            BaiduService baiduService = new BaiduService();

            string str = baiduService.GetBaidu();

            Console.WriteLine(str);

            Console.ReadLine();
        }
    }
}
