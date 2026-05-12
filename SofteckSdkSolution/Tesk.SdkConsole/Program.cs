using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SofteckSdkSolution.Models;
using SofteckSdkSolution.SofteckAofSdk;

namespace Tesk.SdkConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new AofSdkClient())
            {
                var result = client.CreateAccount(
                    new openClientRequest
                    {
                        arg0 = new clientAofApiBean { }
                    });//.Result;

                Console.WriteLine(result.Message);
                Console.ReadLine();
            }
        }
    }
}
