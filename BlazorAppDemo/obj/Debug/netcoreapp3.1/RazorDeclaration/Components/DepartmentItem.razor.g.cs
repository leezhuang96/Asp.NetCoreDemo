#pragma checksum "D:\repos\C#\Asp.NetCoreDemo\BlazorAppDemo\Components\DepartmentItem.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "26c5af6359e36e2a97aadf9584deb8f801d32f54"
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace BlazorAppDemo.Components
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "D:\repos\C#\Asp.NetCoreDemo\BlazorAppDemo\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\repos\C#\Asp.NetCoreDemo\BlazorAppDemo\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\repos\C#\Asp.NetCoreDemo\BlazorAppDemo\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\repos\C#\Asp.NetCoreDemo\BlazorAppDemo\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\repos\C#\Asp.NetCoreDemo\BlazorAppDemo\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\repos\C#\Asp.NetCoreDemo\BlazorAppDemo\_Imports.razor"
using BlazorAppDemo.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "D:\repos\C#\Asp.NetCoreDemo\BlazorAppDemo\_Imports.razor"
using BlazorAppDemo.Services;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\repos\C#\Asp.NetCoreDemo\BlazorAppDemo\_Imports.razor"
using BlazorAppDemo.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "D:\repos\C#\Asp.NetCoreDemo\BlazorAppDemo\_Imports.razor"
using BlazorAppDemo.Components;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "D:\repos\C#\Asp.NetCoreDemo\BlazorAppDemo\Components\DepartmentItem.razor"
using Microsoft.Extensions.Options;

#line default
#line hidden
#nullable disable
    public partial class DepartmentItem : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 22 "D:\repos\C#\Asp.NetCoreDemo\BlazorAppDemo\Components\DepartmentItem.razor"
       
    [Parameter]
    public Department Department { get; set; }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IOptions<BlazorAppDemoOptions> options { get; set; }
    }
}
#pragma warning restore 1591
