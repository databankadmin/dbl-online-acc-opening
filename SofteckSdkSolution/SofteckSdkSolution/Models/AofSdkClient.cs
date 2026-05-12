using System;
using System.Threading.Tasks;
using SofteckSdkSolution.SofteckAofSdk;

namespace SofteckSdkSolution.Models
{
    public class AofSdkClient : IDisposable
    {
        public CreateAccountResponse CreateAccount(openClientRequest model)
        {
            using (var client = new ClientOpeningWSClient())
            {
                client.Open();
                var result = client.openClient(model.arg0);

                client.Close();

                return new CreateAccountResponse {Message = result};
            }
        }

        public  Task<CreateAccountResponse> CreateAccountAsync(openClientRequest model)
        {
            return Task.Run(() =>
            {
                var result = CreateAccountTask(model, out var client).Result;
                client.Close();
                
                return new CreateAccountResponse { Message = result.@return };
            });
        }

        private Task<openClientResponse> CreateAccountTask(openClientRequest model, out ClientOpeningWSClient openClient)
        {
            var client = new ClientOpeningWSClient();
            client.Open();
            var task = client.openClientAsync(model.arg0);

            openClient = client;
            return task;
        }

        public void Dispose() { }
    }
}
