// Copyright 2016-2019, Pulumi Corporation

using System;

namespace Pulumi
{
    public class DynamicResource : CustomResource
    {
      private static string GetTypeName(Resource resource)
      {
          var type = resource.GetType();
          var typeName = string.IsNullOrEmpty(type.Namespace) ? $"dynamic:{type.Name}" : $"dynamic/{type.Namespace}:{type.Name}";;
          return $"pulumi-dotnet:{typeName}";
      }

      private static ResourceArgs SerializeProvider(DynamicResourceProvider provider, ResourceArgs? args)
      {
          return args!;
//
//  if PROVIDER_KEY in props:
//      raise  Exception("A dynamic resource must not define the __provider key")
//
//  props = cast(dict, props)
//  props[PROVIDER_KEY] = serialize_provider(provider)
//
//  super().__init__(f"pulumi-python:{self._resource_type_name}", name, props, opts)
//
      }


#pragma warning disable RS0022 // Constructor make noninheritable base class inheritable
        public DynamicResource(DynamicResourceProvider provider, string name, ResourceArgs? args, CustomResourceOptions? options = null)
            : base((Func<Resource, string>)GetTypeName, name, SerializeProvider(provider, args), options)
#pragma warning restore RS0022 // Constructor make noninheritable base class inheritable
        {
        }
    }
}
