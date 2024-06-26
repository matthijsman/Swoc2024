// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: player.proto
// </auto-generated>
#pragma warning disable 0414, 1591, 8981, 0612
#region Designer generated code

using grpc = global::Grpc.Core;

namespace PlayerInterface {
  /// <summary>
  /// The greeting service definition.
  /// </summary>
  public static partial class PlayerHost
  {
    static readonly string __ServiceName = "PlayerInterface.PlayerHost";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::PlayerInterface.RegisterRequest> __Marshaller_PlayerInterface_RegisterRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::PlayerInterface.RegisterRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::PlayerInterface.GameSettings> __Marshaller_PlayerInterface_GameSettings = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::PlayerInterface.GameSettings.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::PlayerInterface.SubsribeRequest> __Marshaller_PlayerInterface_SubsribeRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::PlayerInterface.SubsribeRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::PlayerInterface.GameUpdateMessage> __Marshaller_PlayerInterface_GameUpdateMessage = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::PlayerInterface.GameUpdateMessage.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::PlayerInterface.EmptyRequest> __Marshaller_PlayerInterface_EmptyRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::PlayerInterface.EmptyRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::PlayerInterface.GameStateMessage> __Marshaller_PlayerInterface_GameStateMessage = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::PlayerInterface.GameStateMessage.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::PlayerInterface.Move> __Marshaller_PlayerInterface_Move = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::PlayerInterface.Move.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::PlayerInterface.SplitRequest> __Marshaller_PlayerInterface_SplitRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::PlayerInterface.SplitRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::PlayerInterface.ServerUpdateMessage> __Marshaller_PlayerInterface_ServerUpdateMessage = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::PlayerInterface.ServerUpdateMessage.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::PlayerInterface.RegisterRequest, global::PlayerInterface.GameSettings> __Method_Register = new grpc::Method<global::PlayerInterface.RegisterRequest, global::PlayerInterface.GameSettings>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Register",
        __Marshaller_PlayerInterface_RegisterRequest,
        __Marshaller_PlayerInterface_GameSettings);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::PlayerInterface.SubsribeRequest, global::PlayerInterface.GameUpdateMessage> __Method_Subscribe = new grpc::Method<global::PlayerInterface.SubsribeRequest, global::PlayerInterface.GameUpdateMessage>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "Subscribe",
        __Marshaller_PlayerInterface_SubsribeRequest,
        __Marshaller_PlayerInterface_GameUpdateMessage);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::PlayerInterface.EmptyRequest, global::PlayerInterface.GameStateMessage> __Method_GetGameState = new grpc::Method<global::PlayerInterface.EmptyRequest, global::PlayerInterface.GameStateMessage>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetGameState",
        __Marshaller_PlayerInterface_EmptyRequest,
        __Marshaller_PlayerInterface_GameStateMessage);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::PlayerInterface.Move, global::PlayerInterface.EmptyRequest> __Method_MakeMove = new grpc::Method<global::PlayerInterface.Move, global::PlayerInterface.EmptyRequest>(
        grpc::MethodType.Unary,
        __ServiceName,
        "MakeMove",
        __Marshaller_PlayerInterface_Move,
        __Marshaller_PlayerInterface_EmptyRequest);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::PlayerInterface.SplitRequest, global::PlayerInterface.EmptyRequest> __Method_SplitSnake = new grpc::Method<global::PlayerInterface.SplitRequest, global::PlayerInterface.EmptyRequest>(
        grpc::MethodType.Unary,
        __ServiceName,
        "SplitSnake",
        __Marshaller_PlayerInterface_SplitRequest,
        __Marshaller_PlayerInterface_EmptyRequest);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::PlayerInterface.EmptyRequest, global::PlayerInterface.ServerUpdateMessage> __Method_SubscribeToServerEvents = new grpc::Method<global::PlayerInterface.EmptyRequest, global::PlayerInterface.ServerUpdateMessage>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "SubscribeToServerEvents",
        __Marshaller_PlayerInterface_EmptyRequest,
        __Marshaller_PlayerInterface_ServerUpdateMessage);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::PlayerInterface.PlayerReflection.Descriptor.Services[0]; }
    }

    /// <summary>Client for PlayerHost</summary>
    public partial class PlayerHostClient : grpc::ClientBase<PlayerHostClient>
    {
      /// <summary>Creates a new client for PlayerHost</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public PlayerHostClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for PlayerHost that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public PlayerHostClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected PlayerHostClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected PlayerHostClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::PlayerInterface.GameSettings Register(global::PlayerInterface.RegisterRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Register(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::PlayerInterface.GameSettings Register(global::PlayerInterface.RegisterRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Register, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::PlayerInterface.GameSettings> RegisterAsync(global::PlayerInterface.RegisterRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return RegisterAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::PlayerInterface.GameSettings> RegisterAsync(global::PlayerInterface.RegisterRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Register, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncServerStreamingCall<global::PlayerInterface.GameUpdateMessage> Subscribe(global::PlayerInterface.SubsribeRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Subscribe(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncServerStreamingCall<global::PlayerInterface.GameUpdateMessage> Subscribe(global::PlayerInterface.SubsribeRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_Subscribe, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::PlayerInterface.GameStateMessage GetGameState(global::PlayerInterface.EmptyRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetGameState(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::PlayerInterface.GameStateMessage GetGameState(global::PlayerInterface.EmptyRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetGameState, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::PlayerInterface.GameStateMessage> GetGameStateAsync(global::PlayerInterface.EmptyRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetGameStateAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::PlayerInterface.GameStateMessage> GetGameStateAsync(global::PlayerInterface.EmptyRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetGameState, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::PlayerInterface.EmptyRequest MakeMove(global::PlayerInterface.Move request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return MakeMove(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::PlayerInterface.EmptyRequest MakeMove(global::PlayerInterface.Move request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_MakeMove, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::PlayerInterface.EmptyRequest> MakeMoveAsync(global::PlayerInterface.Move request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return MakeMoveAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::PlayerInterface.EmptyRequest> MakeMoveAsync(global::PlayerInterface.Move request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_MakeMove, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::PlayerInterface.EmptyRequest SplitSnake(global::PlayerInterface.SplitRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return SplitSnake(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::PlayerInterface.EmptyRequest SplitSnake(global::PlayerInterface.SplitRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_SplitSnake, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::PlayerInterface.EmptyRequest> SplitSnakeAsync(global::PlayerInterface.SplitRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return SplitSnakeAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::PlayerInterface.EmptyRequest> SplitSnakeAsync(global::PlayerInterface.SplitRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_SplitSnake, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncServerStreamingCall<global::PlayerInterface.ServerUpdateMessage> SubscribeToServerEvents(global::PlayerInterface.EmptyRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return SubscribeToServerEvents(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncServerStreamingCall<global::PlayerInterface.ServerUpdateMessage> SubscribeToServerEvents(global::PlayerInterface.EmptyRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_SubscribeToServerEvents, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override PlayerHostClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new PlayerHostClient(configuration);
      }
    }

  }
}
#endregion
