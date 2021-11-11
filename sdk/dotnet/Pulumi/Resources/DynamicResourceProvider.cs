// Copyright 2016-2019, Pulumi Corporation

using System;
using System.Collections.Generic;

namespace Pulumi
{
    public abstract class DynamicResourceProvider
    {
        public virtual (string, IDictionary<string, object?>) Create(IDictionary<string, object?> properties)
        {
            throw new NotImplementedException();
        }
    }
}
