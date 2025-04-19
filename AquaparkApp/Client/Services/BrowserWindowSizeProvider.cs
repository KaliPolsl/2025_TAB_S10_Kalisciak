// AquaparkApp.Client/Services/BrowserWindowSizeProvider.cs
using Microsoft.JSInterop;

namespace AquaparkApp.Client.Services
{
    public class BrowserWindowSizeProvider : IDisposable
    {
        private readonly IJSRuntime _jsRuntime;
        private DotNetObjectReference<BrowserWindowSizeProvider>? _dotNetObjectRef;

        public bool IsMobile { get; private set; }

        public BrowserWindowSizeProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task Initialize()
        {
            _dotNetObjectRef = DotNetObjectReference.Create(this);
            await _jsRuntime.InvokeVoidAsync("registerResizeHandler", _dotNetObjectRef);
            await UpdateIsMobile();
        }

        [JSInvokable]
        public async Task OnResize()
        {
            await UpdateIsMobile();
        }

        private async Task UpdateIsMobile()
        {
            IsMobile = await _jsRuntime.InvokeAsync<bool>("isMobileDevice");
        }

        public void Dispose()
        {
            _dotNetObjectRef?.Dispose();
        }
    }
}