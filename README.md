## Asp .Net Core 3.x

### Asp .Net Core 

本质是 `一个Server` + `多个中间件（Middleware）组成的管道（Pipline）`。

本身是一个`console application`，经 `Main() > CreateHostBuilder(args).Build()`后成为`web application`。

###  Asp  .Net Core 应用多样性

* MVC：/Home/Index
* Razor Pages: /SomePage
* SignalR: /Hub/Chat

### 1. MVC

#### Create Project 

- New > Project > Asp .Net core web application

#### Program class

```C#
public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
                                  {
                                      webBuilder.UseStartup<Startup>();
                                      //根据不同的环境找不同的Startup类，（StartupDevelopment/StartupProduction/StartupStaging...）
                                      //webBuilder.UseStartup(typeof(Program));

                                      //webBuilder.UseKestrel(); 可以不写，源码默认调用
                                  });
}
```

#### Startup class

```c#
// 不同环境可配置不同类，（StartupDevelopment/StartupProduction/StartupStaging...）
public class Startup
{
    // config service, 不同环境可配置不同方法（ConfigureServicesDevelopment/ConfigureServicesProduction/ConfigureServicesStaging...）
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();
        //services.AddControllers();
        //services.AddMvc();

        //services.AddSingleton<IClock, ChinaClock>();
        services.AddSingleton<IClock, UtcClock>();
    }

    // config middleware, 不同环境可配置不同方法（ConfigureDevelopment/ConfigureProduction/ConfigureStaging...）
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // 判断自定义环境
        //if (env.IsEnvironment("OK"))
        //{
        //    app.UseDeveloperExceptionPage();
        //}

        // featch static files
        //app.UseStaticFiles();

        // redirection http request to https request
        app.UseHttpsRedirection();

        // Authentication
        app.UseAuthentication();

        // routing
        app.UseRouting();

        app.UseEndpoints(endpoints =>
                         {
                             //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});

                //MVC use routing table
                endpoints.MapControllerRoute("default", "{controller=Department}/{action=Index}/{id?}");
                
                //MVC use controller routing attribute
                endpoints.MapControllers();
    }
}
```

#### ASPNETCORE_ENVIRONMENT

- \Properties\launchSettings.json

  ```json
  {
    "profiles": {
      "AspCoreDemo": {
        "commandName": "Project",
        "launchBrowser": true,
        "applicationUrl": "https://localhost:5001;http://localhost:5000",
        "environmentVariables": {
          "ASPNETCORE_ENVIRONMENT": "Development"
        }
      }
    }
  }
  ```

#### bootstrap 

##### a. Using npm package.json file install bootstrap *

- Add > New Item > npm Configuration file : package.json

```json
{
  "version": "1.0.0",
  "name": "asp.net",
  "private": true,
  "devDependencies": {
    "bootstrap": "4.4.1"
  }
}
```

##### b. Using libman Install bootstrap

- Add > Client-Side Library > bootstrap@4.4.1 > Install, libman.json

```json
{
  "version": "1.0",
  "defaultProvider": "unpkg",
  "libraries": [
    {
      "library": "bootstrap@4.4.1",
      "destination": "wwwroot/lib/bootstrap/",
      "files": [
        "dist/css/bootstrap.css"
      ]
    }
  ]
}
```

#### Using BuildBundlerMinifier merge and minify css files

- Add > New Item, bundleconfig.json

```json
[
  {
    "outputFileName": "wwwroot/css/all.min.css",
    "inputFiles": [
      "wwwroot/css/site.css",
      "wwwroot/lib/bootstrap/dist/css/bootstrap.css"
    ]
  },
  {
    "outputFileName": "wwwroot/css/bootstrap.css",
    "inputFiles": [
      "wwwroot/lib/bootstrap/dist/css/bootstrap.css"
    ],
    "minify": {"enabled": true}
  }
]
```

- Manage NuGet packges > BuildBundlerMinifier

#### 使用MVC相关技术

* Controller
* Tag Helper
* Settings
* View Component
* Razor page

#### Models

```c#
public class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public int EmployeeCount { get; set; }
}
```

#### Services

```c#
public class DepartmentService : IDepartmentService
{
    private readonly List<Department> _departments = new List<Department>(); 

    public DepartmentService()
    {
        _departments.Add(new Department 
                         {
                             Id = 1,
                             Name = "HR",
                             Location ="ShangHai",
                             EmployeeCount = 10
                         });
        //...
    }

    public Task Add(Department department)
    {
        department.Id = _departments.Max(x => x.Id) + 1;
        _departments.Add(department);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Department>> GetAll()
    {
        return Task.Run(() => _departments.AsEnumerable());
    }

    public Task<Department> GetById(int id)
    {
        return Task.Run(() => _departments.FirstOrDefault(x => x.Id == id));
    }

    public Task<CompanySummary> GetCompanySummary()
    {
        return Task.Run(() => 
                        {
                            return new CompanySummary
                            {
                                EmployeeCount = _departments.Sum(x => x.EmployeeCount),
                                AverageDepartmentEmployeeCount = (int)_departments.Average(x => x.EmployeeCount)
                            };
                        });
    }
}
```

#### Register Services 

- Startup.cs

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllersWithViews();

    services.AddSingleton<IDepartmentService, DepartmentService>();
}
```

#### Controllers

```c#
public class DepartmentController : Controller
{
    private readonly IDepartmentService departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        this.departmentService = departmentService;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Title = "Department Index";
        var departments = await departmentService.GetAll();
        return View(departments);
    }

    public IActionResult Add()
    {
        ViewBag.Title = "Add Department";
        return View(new Department());
    }

    [HttpPost]
    public async Task<IActionResult> Add(Department department)
    {
        if (ModelState.IsValid)
        {
            await departmentService.Add(department);
        }

        return RedirectToAction(nameof(Index));
    }
}
```

#### Views

- Views > _ViewImports.cshtml 全局启用TagHelper: Add > New Item > Razor View Imports

  ```html
  @addTagHelper "*, Microsoft.AspNetCore.Mvc.TagHelpers"
  ```

- Views > Shared > _Layout.cshtml 公共页面 : Add > New Item > Razor Layout

```html
<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>@ViewBag.Title</title>

    <environment include="Development">
        <link rel="stylesheet" asp-href-include="css/*" asp-href-exclude="css/all.main.css" />
    </environment>
    <environment exclude="Development">
        <link rel="stylesheet" asp-href-include="css/all.main.css" />
    </environment>
</head>
<body>
    <div class="container">
        <div class="row">
            <div class="col-md-2">
                <!-- taghelper: asp-append-version 防止图片被缓存 -->
                <img asp-append-version="true" alt="logo" src="~/images/Home.png" style="height: 60px; width: 100px;" />
            </div>
            <div class="col-md-10">
                <span class="h2">@ViewBag.Title</span>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                @RenderBody()
            </div>
        </div>
    </div>
</body>
</html>

```

- Views >  _ViewStart.cshtml start 页面： Add > New Item > Razor View Start

```html
@{
    Layout = "_Layout";
}
```

- Views > Department > Index.cshtml Index页面 :  Add > New Item > Razor View

```html
@using AspCoreDemo.Models
@model IEnumerable<Department>

<div class="row">
    <div class="col-md-10 offset-md-2">
        <table class="table">
            <tr>
                <th>Name</th>
                <th>Location</th>
                <th>EmployeeCount</th>
                <th>Opration</th>
            </tr>
            @Html.DisplayForModel()
        </table>
    </div>
</div>
<div class="row">
    <div class="col-md-4 offset-md-2">
        <a asp-action="Add">Add</a>
    </div>
</div>
```

- Views > Department > DisplayTemplates > Department.cshtml 模板页面: Add > New Item > Razore View

```html
@model AspCoreDemo.Models.Department

<tr>
    <td>@Model.Name</td>
    <td>@Model.Location</td>
    <td>@Model.EmployeeCount</td>
    <td>
        <a asp-controller="Employee" asp-action="Index" asp-route-departmentId="@Model.Id">
            Employees
        </a>
    </td>
</tr>
```

- Views > Department > Add.cshtml Add页面 :  Add > New Item > Razor View

````html
@using AspCoreDemo.Models
@model Department

    <form asp-action="Add">
        <div class="row form-group">
            <div class="col-md-2 offset-md-2">
                <label asp-for="Name"></label>
            </div>
            <div class="col-md-6">
                <input class="form-control" asp-for="Name" />
            </div>
        </div>
        <div class="row form-group">
            <div class="col-md-2 offset-md-2">
                <label asp-for="Location"></label>
            </div>
            <div class="col-md-6">
                <input class="form-control" asp-for="Location" />
            </div>
        </div>
        <div class="row form-group">
            <div class="col-md-2 offset-md-2">
                <label asp-for="EmployeeCount"></label>
            </div>
            <div class="col-md-6">
                <input class="form-control" asp-for="EmployeeCount" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-2 offset-md-2">
                <button type="submit" class="btn btn-primary">Add</button>
            </div>
        </div>
    </form>
````

#### Asp .Net Core 的配置信息源

* appsettings.json

  * appsettings.{Environment}.json

* Secret Manager

* 环境变量

* 命令行参数

  （相同参数，下面的配置覆盖前面配置）

#### appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "AspCoreDemo": {
    "BoldDepartmentEmployeeCount": 30
  }
}
```

#### Startup.cs 注入Config，映射为options类

```c#
private readonly IConfiguration configuration;

public Startup(IConfiguration configuration)
{
    this.configuration = configuration;
}

// register service
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllersWithViews();

    services.AddSingleton<IDepartmentService, DepartmentService>();
    services.AddSingleton<IEmployeeService, EmpolyeeService>();

    services.Configure<AspCoreDemoOptions>(configuration.GetSection("AspCoreDemo"));
}
```

#### Controller 注入options

```c#
private readonly IDepartmentService departmentService;
private readonly IOptions<AspCoreDemoOptions> options;

public DepartmentController(IDepartmentService departmentService, IOptions<AspCoreDemoOptions> options)
{
    this.departmentService = departmentService;
    this.options = options;
}
```

#### .cshtml注入options

```html
@using AspCoreDemo.Models
@using Microsoft.Extensions.Options
@model AspCoreDemo.Models.Department
@inject IOptions<AspCoreDemoOptions> options

    <tr>
        @if (Model.EmployeeCount > options.Value.BoldDepartmentEmployeeCount)
        {
            <td><strong>@Model.Name</strong></td>
        }
        else 
        {
            <td>@Model.Name</td>
        }
        <td>@Model.Location</td>
        <td>@Model.EmployeeCount</td>
        <td>
            <a asp-controller="Employee" asp-action="Index" asp-route-departmentId="@Model.Id">
                Employees
            </a>
        </td>
    </tr>
```

#### ViewComponet 可重用组件

- ViewComponents > CompanySummaryViewComponent.cs component类

```c#
public class CompanySummaryViewComponent : ViewComponent
{
    private readonly IDepartmentService departmentService;

    public CompanySummaryViewComponent(IDepartmentService departmentService)
    {
    	this.departmentService = departmentService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string title)
    {
        ViewBag.Title = title;
        var companySummary = await departmentService.GetCompanySummary();
        return View(companySummary);
    }
}
```

- Views\Shared\Components\CompanySummary\Default.cshtml component页面

```html
@model AspCoreDemo.Models.CompanySummary

    <div class="small">
        <div class="row h6">@ViewBag.Title</div>
        <div class="row">
            <div class="col-md-10">Employee Count:</div>
            <div class="col-md-2">@Model.EmployeeCount</div>
        </div>
        <div class="row">
            <div class="col-md-10">Average Count :</div>
            <div class="col-md-2">@Model.AverageDepartmentEmployeeCount</div>
        </div>
    </div>
```

- Views\\_ViewImports.cshtml 引入本项目程序集

  ```html
  @addTagHelper "*, Microsoft.AspNetCore.Mvc.TagHelpers"
  @addTagHelper "*, AspCoreDemo"
  ```

- Views\Department\Index.cshtml 使用ViewComponent

```html
<div class="row">
    <div class="col-md-2">
        @await Component.InvokeAsync("CompanySummary", new { title = "Summary of Company" })
        <vc:company-summary title="Summary"></vc:company-summary>
    </div>

    <div class="col-md-4">
        <a asp-action="Add">Add</a>
    </div>
</div>
```

### 2. Razor Page

#### MVC

* Model: 数据
* View: Html, Razor, TagHelpers
* Controller: 逻辑

#### Razor Page

* 数据
* Html, Razor, TagHelpers
* 逻辑

#### Create Project 

- New > Project > Asp .Net core web application

#### Startup.cs

```c#
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
                         {
                             endpoints.MapRazorPages();
                         });
    }
}
```

#### Create Models, Service and Register services

- Startup.cs

  ```c#
  public void ConfigureServices(IServiceCollection services)
  {
      services.AddRazorPages();
  
      services.AddSingleton<IDepartmentService, DepartmentService>();
      services.AddSingleton<IEmployeeService, EmpolyeeService>();
  }
  ```

#### Config配置信息源

- appsettings.json

  ```json
  {
    "Logging": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Information"
      }
    },
    "AllowedHosts": "*",
    "AspCoreRazorDemo": {
      "BoldDepartmentEmployeeC: 30
    }
  }
  ```

#### Startup.cs 注入Config，映射为options类

```c#
private readonly IConfiguration configuration;

public Startup(IConfiguration configuration)
{
	this.configuration = configuration;
}

public void ConfigureServices(IServiceCollection services)
{
    services.AddRazorPages();

    services.AddSingleton<IDepartmentService, DepartmentService>();
    services.AddSingleton<IEmployeeService, EmpolyeeService>();

    services.Configure<AspCoreRazorDemoOptions>(configuration.GetSection("AspCoreRazorDemo"));
}
```

#### Using libman Install bootstrap

- Add > Client-Side Library > bootstrap@4.4.1 > Install, libman.json

  ```json
  {
    "version": "1.0",
    "defaultProvider": "unpkg",
    "libraries": [
      {
        "library": "bootstrap@4.4.1",
        "destination": "wwwroot/lib/bootstrap/",
        "files": [
          "dist/css/bootstrap.css"
        ]
      }
    ]
  }
  ```

#### Using BuildBundlerMinifier Merge and Minify css files

- Add > New Item, bundleconfig.json 

  ```json
  [
    {
      "outputFileName": "wwwroot/css/all.min.css",
      "inputFiles": [
        "wwwroot/css/site.css",
        "wwwroot/lib/bootstrap/dist/css/bootstrap.css"
      ]
    },
    {
      "outputFileName": "wwwroot/css/bootstrap.css",
      "inputFiles": [
        "wwwroot/lib/bootstrap/dist/css/bootstrap.css"
      ],
      "minify": {"enabled": true}
    }
  ]
  ```

- Manage NuGet packges > BuildBundlerMinifier

#### ASPNETCORE_ENVIRONMENT

- \Properties\launchSettings.json

  ```json
  {
    "profiles": {
      "AspCoreRazorDemo": {
        "commandName": "Project",
        "launchBrowser": true,
        "applicationUrl": "https://localhost:5001;http://localhost:5000",
        "environmentVariables": {
          "ASPNETCORE_ENVIRONMENT": "Development"
        }
      }
    }
  }
  ```

#### Views

- Views  >  _ViewImports.cshtml 全局启用TagHelper : Add > New Item > Razor View Imports

  ```html
  @using AspCoreRazorDemo
  @namespace AspCoreRazorDemo.Pages
  @addTagHelper "*, Microsoft.AspNetCore.Mvc.TagHelpers"
  ```

- Views > Shared > _Layout.cshtml 公共页面 : Add > New Item > Razor Layout

  ```html
  <!DOCTYPE html>
  
  <html>
  <head>
      <meta name="viewport" content="width=device-width" />
      <title>@ViewBag.Title</title>
  
      <environment include="Development">
          <link rel="stylesheet" asp-href-include="css/*" asp-href-exclude="css/all.main.css" />
      </environment>
      <environment exclude="Development">
          <link rel="stylesheet" asp-href-include="css/all.main.css" />
      </environment>
  </head>
  <body>
      <div class="container">
          <div class="row">
              <div class="col-md-2">
                  <!-- taghelper: asp-append-version 防止图片被缓存 -->
                  <img asp-append-version="true"
                       alt="logo"
                       src="~/images/Home.png"
                       style="height: 60px; width: 100px;" />
              </div>
              <div class="col-md-10">
                  <span class="h2">@ViewBag.Title</span>
              </div>
          </div>
          <div class="row">
              <div class="col-md-12">
                  @RenderBody()
              </div>
          </div>
      </div>
  </body>
  </html>
  
  ```

- Views > _ViewStart.cshtml start 页面： Add > New Item > Razor View Start

  ```html
  @{
      Layout = "_Layout";
  }
  ```

- Pages > Index.cshtml : Add > NewItem > Razor View

  ```html
  @page
  @using AspCoreRazorDemo.Models
  @using AspCoreRazorDemo.Services
  @inject IDepartmentService departmentService
  
  <div class="row">
      <div class="col-md-10 offset-md-2">
          <table class="table">
              <tr>
                  <th>Name</th>
                  <th>Location</th>
                  <th>EmployeeCount</th>
                  <th>Opration</th>
              </tr>
              @Html.DisplayFor(x => x.Departments)
          </table>
      </div>
  </div>
  <div class="row">
      <div class="col-md-4">
          <a asp-page="Department/AddDepartment">Add</a>
      </div>
  </div>
  
  @functions
  {
      public IEnumerable<Department> Departments { get; set; }
  
      public async Task OnGetAsync()
      {
          Departments = await departmentService.GetAll();
      }
  
  }
  ```

- Pages > DisplayTemplates > Department.cshtml: Add > NewItem > Razor View

  ```html
  @using AspCoreRazorDemo.Models
  @using Microsoft.Extensions.Options
  @model AspCoreRazorDemo.Models.Department
  @inject IOptions<AspCoreRazorDemoOptions> options
  
      <tr>
          @if (Model.EmployeeCount > options.Value.BoldDepartmentEmployeeCount)
          {
              <td><strong>@Model.Name</strong></td>
          }
          else
          {
              <td>@Model.Name</td>
          }
          <td>@Model.Location</td>
          <td>@Model.EmployeeCount</td>
      </tr>
  ```

- Pages > Department> AddDepartment.cshtml: Add > New Item > Razor Page

  ```html
  @page
  @model AspCoreRazorDemo.Pages.Department.AddDepartmentModel
  
  <form method="post">
      <div class="row form-group">
          <div class="col-md-2 offset-md-2">
              <label asp-for="Department.Name"></label>
          </div>
          <div class="col-md-6">
              <input class="form-control" asp-for="Department.Name" />
          </div>
      </div>
      <div class="row form-group">
          <div class="col-md-2 offset-md-2">
              <label asp-for="Department.Location"></label>
          </div>
          <div class="col-md-6">
              <input class="form-control" asp-for="Department.Location" />
          </div>
      </div>
      <div class="row form-group">
          <div class="col-md-2 offset-md-2">
              <label asp-for="Department.EmployeeCount"></label>
          </div>
          <div class="col-md-6">
              <input class="form-control" asp-for="Department.EmployeeCount" />
          </div>
      </div>
      <div class="row">
          <div class="col-md-2 offset-md-2">
              <button type="submit" class="btn btn-primary">Add</button>
          </div>
      </div>
  </form>
  ```

- Pages > Department> AddDepartment.cshtml.cs

  ```c#
  public class AddDepartmentModel : PageModel
  {
      private readonly IDepartmentService departmentService;
  
      [BindProperty]
      public AspCoreRazorDemo.Models.Department Department { get; set; }
  
      public AddDepartmentModel(IDepartmentService departmentService)
      {
      	this.departmentService = departmentService;
      }
  
      public async Task<IActionResult> OnPostAsync()
      {
          if (ModelState.IsValid)
          {
              await departmentService.Add(Department);
              return RedirectToPage("/Index");
          }
          return Page();
      }
  }
  ```

#### ViewComponet 可重用组件

- Pages> Shared> Components> CompanySummary> Default.cshtml

- ViewComponents > CompanySummaryViewComponent.cs

- Views\\_ViewImports.cshtml 引入本项目程序集

  ```html
  @using AspCoreRazorDemo
  @namespace AspCoreRazorDemo.Pages
  @addTagHelper "*, Microsoft.AspNetCore.Mvc.TagHelpers"
  @addTagHelper "*, AspCoreRazorDemo"
  ```

- Pages\Index.cshtml 使用ViewComponent

  ```html
  <div class="row">
      <div class="col-md-2">
          @await Component.InvokeAsync("CompanySummary", new { title = "Summary of Company" })
          <vc:company-summary title="Summary"></vc:company-summary>
      </div>
  
      <div class="col-md-4">
          <a asp-page="Department/AddDepartment">Add</a>
      </di>
  </div>
  ```


### 3. SignalR

实时web应用技术：默认采用回落机制

- webSocket
- server sent enevts (SSE)
- long polling

SignalR采用RPC范式进行客户端与服务端的通信：RPC（Remote procedure call）可以像调用本地方法一样调用远程服务。

Hub: SignalR的一个组件，运行再Asp .net core 应用里的一个服务端的类，进行服务端与客户端通信。Hub支持Json和MessagePack协议

#### Create Project 

- New > Project > Asp .Net core web application

#### Startup.cs

```c#
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSignalR();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
        	app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
```

#### ASPNETCORE_ENVIRONMENT

- \Properties\launchSettings.json

  ```json
  {
    "profiles": {
      "SignalRDemo": {
        "commandName": "Project",
        "launchBrowser": true,
        "applicationUrl": "https://localhost:5001;http://localhost:5000",
        "environmentVariables": {
          "ASPNETCORE_ENVIRONMENT": "Development"
        }
      }
    }
  }
  ```

#### Services > ICountService.cs & CountService.cs

```c#
public interface ICountService
{
	public Task<int> GetCount();
}

public class CountService : ICountService
{
    private int _count;

    public Task<int> GetCount()
    {
    	return Task.Run(() => _count++);
    }
}
```

#### CountHub.cs

```c#
//[Authorize]
public class CountHub : Hub
{
    private readonly ICountService countService;

    public CountHub(ICountService countService)
    {
        this.countService = countService;
    }

    public async Task GetLatestCount(string maxValue)
    {
        //var userName = Context.User.Identity.Name;

        int count;
        do
        {
            count = await countService.GetCount();
            Thread.Sleep(1000);

            //incoke all clients method.
            await Clients.All.SendAsync("ReciveUpdate", count);
        }
        while (count < int.Parse(maxValue));

        //incoke all clients method.
        await Clients.All.SendAsync("Finsihed");
    }

    public async override Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        var client = Clients.Client(connectionId);

        //incoke client<connectionId> method.
        await client.SendAsync("someFunc", new { random= "Init" });
        //incoke except client<connectionId> method.
        await Clients.AllExcept(connectionId).SendAsync("someFunc");

        await Groups.AddToGroupAsync(connectionId, "MyGroup");
        await Groups.RemoveFromGroupAsync(connectionId, "MyGroup");

        await Clients.Group("MyGroup").SendAsync("someFunc");
    }
}
```

#### Controllers > CountController.cs

```c#
[Route("api/count")]
public class CountController : Controller
{
    private readonly IHubContext<CountHub> countHub;

    public CountController(IHubContext<CountHub> countHub)
    {
    	this.countHub = countHub;
    }

    [HttpPost]
    public async Task<IActionResult> Post(string random)
    {
        await countHub.Clients.All.SendAsync("someFunc", new { random = "Start" });

        return Accepted(10);
    }
}
```

#### Using libman Install bootstrap

- Add > Client-Side Library > @aspnet/signalr@1.1.4 > Install, libman.json

  ```josn
  {
    "version": "1.0",
    "defaultProvider": "unpkg",
    "libraries": [
      {
        "library": "@aspnet/signalr@1.1.4",
        "destination": "wwwroot/lib/@aspnet/signalr/",
        "files": [
          "dist/browser/signalr.js"
        ]
      }
    ]
  }
  ```

#### Client

- wwwroot > index.html 客户端html

  ```html
  <!DOCTYPE html>
  <html>
  <head>
      <meta charset="utf-8" />
      <title></title>
  </head>
  <body>
      <button id="submit">Submit</button>
      <div id="result" style="color: green; font-weight: bold; font-size: 24px;"></div>
  
      <script src="/lib/@aspnet/signalr/dist/browser/signalr.js"></script>
      <script src="index.js"></script>
  </body>
  </html>
  ```

- wwwroot > index.js 客户端js

  ```javascript
  let connection = null;
  
  setupConnection = () => {
      //设置使用longPolling
      //connection = new signalR.HubConnectionBuilder()
      //    .withUrl("/counthub", signalR.HttpTransportType.LongPolling)
      //    .build();
  
      connection = new signalR.HubConnectionBuilder()
          .withUrl("/counthub")
          .build();
  
      connection.on("ReciveUpdate", (update) => {
          const resultDiv = document.getElementById("result");
          resultDiv.innerHTML = update;
      });
  
      connection.on("someFunc", function (obj) {
          const resultDiv = document.getElementById("result");
          resultDiv.innerHTML = "Someone called, parametes: " + obj.random;
      });
  
      connection.on("Finsihed", function (obj) {
          const resultDiv = document.getElementById("result");
          resultDiv.innerHTML = "Finsihed";
      });
  
      connection.start()
          .catch(err => console.error(err.toString()));
  }
  
  setupConnection();
  
  document.getElementById("submit").addEventListener("click", e => {
      e.preventDefault();
  
      fetch("/api/count",
          {
              method: "POST",
              headers: {
                  'content-type': 'application/json'
              }
          })
          .then(response => response.text())
          .then(maxValue => connection.invoke("GetLatestCount", maxValue));
  })
  ```

  