// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Microsoft.AspNetCore.Components.Web
{
    internal class VirtualizeJsInterop : IAsyncDisposable
    {
        private const string JsFunctionsPrefix = "Blazor._internal.Virtualize";

        private readonly IVirtualizeJsCallbacks _owner;

        private readonly IJSRuntime _jsRuntime;

        private DotNetObjectReference<VirtualizeJsInterop>? _selfReference;

        public VirtualizeJsInterop(IVirtualizeJsCallbacks owner, IJSRuntime jsRuntime)
        {
            _owner = owner;
            _jsRuntime = jsRuntime;
        }

        public async ValueTask InitializeAsync(ElementReference spacerBefore, ElementReference spacerAfter)
        {
            _selfReference = DotNetObjectReference.Create(this);
            await _jsRuntime.InvokeVoidAsync($"{JsFunctionsPrefix}.init", _selfReference, spacerBefore, spacerAfter);
        }

        [JSInvokable]
        public void OnSpacerBeforeVisible(float spacerSize, float containerSize)
        {
            _owner.OnBeforeSpacerVisible(spacerSize, containerSize);
        }

        [JSInvokable]
        public void OnSpacerAfterVisible(float spacerSize, float containerSize)
        {
            _owner.OnAfterSpacerVisible(spacerSize, containerSize);
        }

        public async ValueTask DisposeAsync()
        {
            if (_selfReference != null)
            {
                await _jsRuntime.InvokeVoidAsync($"{JsFunctionsPrefix}.dispose", _selfReference);
            }
        }
    }
}
