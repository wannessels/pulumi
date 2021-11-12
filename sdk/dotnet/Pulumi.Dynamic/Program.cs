using Pulumirpc;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

public static class Program
{
    class DynamicResourceProviderServicer : ResourceProvider.ResourceProviderBase
    {
        public override Task<CheckResponse> CheckConfig(CheckRequest request, ServerCallContext context)
        {
            throw new RpcException(new Status(StatusCode.Unimplemented, "CheckConfig is not implemented by the dynamic provider"));
        }

        public override Task<DiffResponse> DiffConfig(DiffRequest request, ServerCallContext context)
        {
            throw new RpcException(new Status(StatusCode.Unimplemented, "DiffConfig is not implemented by the dynamic provider"));
        }

        public override Task<InvokeResponse> Invoke(InvokeRequest request, ServerCallContext context)
        {
            throw new RpcException(new Status(StatusCode.Unimplemented, "Invoke is not implemented by the dynamic provider"));
        }

        public override Task<GetSchemaResponse> GetSchema(GetSchemaRequest request, ServerCallContext context)
        {
            throw new RpcException(new Status(StatusCode.Unimplemented, "GetSchema is not implemented by the dynamic provider"));
        }

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
            var response = new CreateResponse();
            response.Id = "some_id";
            var result = new Struct();
            result.Fields.Add("val", Value.ForString("AQIDBAUG"));
            response.Properties = result;
            return Task.FromResult(response);
        }

        public override Task<ReadResponse> Read(ReadRequest request, ServerCallContext context)
        {
            var id = request.Id;
            var props = request.Properties;

            var response = new ReadResponse();
            response.Id = id;
            response.Properties = props;
            return Task.FromResult(response);
        }

        public override Task<CheckResponse> Check(CheckRequest request, ServerCallContext context)
        {
            var response = new CheckResponse();
            response.Inputs = request.News;
            return Task.FromResult(response);
        }

        public override Task<DiffResponse> Diff(DiffRequest request, ServerCallContext context)
        {
            var response = new DiffResponse();
            return Task.FromResult(response);

            //fields = {}
            //if result.changes is not None:
            //    if result.changes:
            //        fields["changes"] = proto.DiffResponse.DIFF_SOME # pylint: disable=no-member
            //    else:
            //        fields["changes"] = proto.DiffResponse.DIFF_NONE # pylint: disable=no-member
            //else:
            //    fields["changes"] = proto.DiffResponse.DIFF_UNKNOWN # pylint: disable=no-member
            //if result.replaces is not None:
            //    fields["replaces"] = result.replaces
            //if result.delete_before_replace is not None:
            //    fields["deleteBeforeReplace"] = result.delete_before_replace
        }

        public override Task<UpdateResponse> Update(UpdateRequest request, ServerCallContext context)
        {
            var response = new UpdateResponse();
            response.Properties = request.News;
            return Task.FromResult(response);
        }

        public override Task<Empty> Delete(DeleteRequest request, ServerCallContext context)
        {
            return Task.FromResult(new Empty());
        }
    }

    public static void Main(string[] args)
    {
        var monitor = new DynamicResourceProviderServicer();
        // maxRpcMessageSize raises the gRPC Max Message size from `4194304` (4mb) to `419430400` (400mb)
        var maxRpcMessageSize = 400 * 1024 * 1024;
        var grpcChannelOptions = new List<ChannelOption> { new ChannelOption(ChannelOptions.MaxReceiveMessageLength, maxRpcMessageSize)};
        var server = new Server(grpcChannelOptions)
            {
                Services = { ResourceProvider.BindService(monitor) },
                Ports = { new ServerPort("0.0.0.0", 0, ServerCredentials.Insecure) }
            };

        server.Start();
        var port = server.Ports.First();
        System.Console.WriteLine(port.BoundPort.ToString());

        Task? shutdownTask = null;
        var exitEvent = new System.Threading.ManualResetEventSlim();
        System.Console.CancelKeyPress += (System.ConsoleCancelEventHandler)((sender, e) => {
            shutdownTask = server.ShutdownAsync();
            exitEvent.Set();
        });
        exitEvent.Wait();
        shutdownTask!.Wait();
    }
}