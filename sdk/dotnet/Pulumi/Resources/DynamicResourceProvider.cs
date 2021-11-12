// Copyright 2016-2019, Pulumi Corporation

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Pulumi
{
    public abstract class DynamicResourceProvider
    {
        public virtual Task<(string, IDictionary<string, object?>)> Create(ImmutableDictionary<string, object?> properties)
        {
            throw new NotImplementedException();
        }
    }
}
