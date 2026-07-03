using System.Runtime.InteropServices;

[assembly:Guid("5612a06f-00bc-458b-b17c-0f52c7c8482d")]
namespace RemoteConsole
{
    class Program
    {
        static void Main()
        {
            PinFun.ServiceContainer.Host.Run(x =>
            {
                x
                    .WithDisplayName("品杰远程网页日志查看服务")
                    .WithName("PJ.Web.Log")
                    .WithDescriptions("品杰远程网页日志查看服务");
            });
        }
    }
}
