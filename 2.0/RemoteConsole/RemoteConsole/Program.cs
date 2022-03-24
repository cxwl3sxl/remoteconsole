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
