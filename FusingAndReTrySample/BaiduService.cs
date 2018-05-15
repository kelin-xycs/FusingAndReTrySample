using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

namespace FusingAndReTrySample
{
    class BaiduService
    {
        //***    少搞一点 封装， 少搞一点 控制反转（Ioc）， 少搞一点 AOP， 少搞一些 “声明式”

        //***    不要隐藏太多代码，让 代码 回归 代码

        //***    是 找回 80 年代 写 Basic 的 那种感觉 的 时候了。
     
        public string GetBaidu()
        {
            try
            {
                return DoGetBaidu();
            }
            catch(Exception ex)
            {
                //  记录 Log
                return ReTryGetBaidu();
            }
        }

        public string ReTryGetBaidu()
        {
            int retryCount = 3;
            Exception lastEx = null;

            for(int i=0; i<retryCount; i++)
            {
                try
                {
                    return DoGetBaidu();
                }
                catch (Exception ex)
                {
                    //  记录 Log 什么的
                    lastEx = ex;
                }
            }

            if (lastEx != null)
                throw new Exception("最后一次异常", lastEx);
            else
                throw new Exception("没有进入到 ReTry 的循环中。");
        }

        public string DoGetBaidu()
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://www.baidu.com");

            //  设置 超时时间 就是 熔断
            request.Timeout = 5000;               //  建立连接的 超时时间
            request.ReadWriteTimeout = 5000;      //  下载内容的 超时时间


            byte[] bytes;
            IList<byte[]> bytesList = new List<byte[]>();
            
            int readCount;
            int readCountTotal = 0;

            int offset;
            int remainLength;
            int byteLength = 100;
           
            using(WebResponse response = request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {

                    bytes = new byte[byteLength];

                    offset = 0;
                    remainLength = bytes.Length;
                    

                    while (true)
                    {

                        readCount = stream.Read(bytes, offset, remainLength);

                        if (readCount == 0)
                        {
                            bytesList.Add(bytes);
                            break;
                        }


                        remainLength = remainLength - readCount;
                        offset += readCount;

                        if (remainLength == 0)
                        {
                            bytesList.Add(bytes);

                            bytes = new byte[byteLength];

                            offset = 0;
                            remainLength = bytes.Length;
                        }

                        readCountTotal += readCount;

                    }
                }

            }

            byte[] bytesTotal = new byte[readCountTotal];

            int i = 0;

            foreach (byte[] b in bytesList)
            {
                for (int j = 0; j < b.Length; j++)
                {
                    
                    bytesTotal[i] = b[j];

                    if (i == (readCountTotal - 1))
                        break;

                    i++;
                }
            }

            string str = Encoding.UTF8.GetString(bytesTotal, 0, bytesTotal.Length);

            return str;
            
        }
    }
}
