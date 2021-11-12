using Pulumirpc;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using System.Linq;
using System.Threading.Tasks;

public static class Program
{
    class DynamicResourceProviderServicer : ResourceProvider.ResourceProviderBase
    {

// def CheckConfig(self, request, context):
//     context.set_code(grpc.StatusCode.UNIMPLEMENTED)
//     context.set_details("CheckConfig is not implemented by the dynamic provider")
//     raise NotImplementedError("CheckConfig is not implemented by the dynamic provider")
//
// def DiffConfig(self, request, context):
//     context.set_code(grpc.StatusCode.UNIMPLEMENTED)
//     context.set_details("DiffConfig is not implemented by the dynamic provider")
//     raise NotImplementedError("DiffConfig is not implemented by the dynamic provider")
//
// def Invoke(self, request, context):
//     context.set_code(grpc.StatusCode.UNIMPLEMENTED)
//     context.set_details("Invoke is not implemented by the dynamic provider")
//     raise NotImplementedError(
  //  def GetSchema(self, request, context):
  //      context.set_code(grpc.StatusCode.UNIMPLEMENTED)
  //      context.set_details("GetSchema is not implemented by the dynamic provider")
  //      raise NotImplementedError("GetSchema is not implemented by the dynamic provider")f"unknown function {request.token}")



        public override Task<ConfigureResponse> Configure(ConfigureRequest request, ServerCallContext context)
        {
            var response = new ConfigureResponse();
            response.AcceptSecrets = false;
            return Task.FromResult(response);
        }

        public override Task<PluginInfo> GetPluginInfo(Empty request, ServerCallContext context)
        {
            var response = new PluginInfo();
            response.Version = "0.1.0";
            return Task.FromResult(response);
        }

        public override Task<Empty> Cancel(Empty request, ServerCallContext context)
        {
            return Task.FromResult(new Empty());
        }

        public override Task<CreateResponse> Create(CreateRequest request, ServerCallContext context)
        {
            throw new System.Exception("BANG");
            //return base.Create(request, context);
        }

    }

    public static void Main(string[] args)
    {
        var monitor = new DynamicResourceProviderServicer();
        var server = new Server
            {
                Services = { ResourceProvider.BindService(monitor) },
                Ports = { new ServerPort("0.0.0.0", 0, ServerCredentials.Insecure) }
            };

        server.Start();
        var port = server.Ports.First();
        System.Console.Error.WriteLine(port.ToString());

        System.Threading.Tasks.Task? shutdownTask = null;
        var exitEvent = new System.Threading.ManualResetEventSlim();
        System.Console.CancelKeyPress += (System.ConsoleCancelEventHandler)((sender, e) => {
            shutdownTask = server.ShutdownAsync();
            exitEvent.Set();
        });
        exitEvent.Wait();
        shutdownTask!.Wait();
    }
}