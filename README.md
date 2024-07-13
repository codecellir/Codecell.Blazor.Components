## CodecellComponent.Blazor
Custom Components for Blazor

## PersianDatePicker
## Features

- Two-Way Binding
- Dark Mode
- Month Navigation
- Year Navigation
- Date Change Event Callback

## Installation
You can download the latest version of `Codecell.Component.Blazor` from [Github repository](https://github.com/codecellir/Codecell.Blazor.Components).
To install via `nuget`:
```
Install-Package Codecell.Component.Blazor -Version 0.0.1
```
Install from [Nuget](https://www.nuget.org/packages/Codecell.Component.Blazor) directly.

## How to use
Register Codecell Persian DatePicker Control to project container in `Program.cs` file:
``` C#
using Codecell.Component.Blazor;

builder.Services.AddCodecellBlazor();
```

Add using to _imports.razor
``` C#
@using Codecell.Component.Blazor.Components.PersianDatePickerComponent
```

``` Razor
@page "/"

Add style references
``` Html
<link href="_content/Codecell.Component.Blazor/codecell.css" rel="stylesheet" />
```

``` Razor
@page "/"

<PageTitle>Home</PageTitle>

<h1>Hello, world!</h1>


<div class="row" style="direction:rtl">
    <div class="col-md-4 mt-3">
        <PersianDatePicker @bind-Date="myDate"
                           PlaceHolder="تاریخ تولد"
                           Label="تولد شما:"
                           ValueChanged="DateChanged"
                           DarkMode="false"/>
    </div>
</div>


<p class="mt-2">@myDate</p>


@code{

    DateTime? myDate;

    void DateChanged(DateTime? value)
    {
        Console.WriteLine($"Date Changed: {value}");
    }
}

```


## Screenshots
![App Screenshot](https://github.com/codecellir/Codecell.Blazor.Components/blob/master/screenshots/light.png?raw=true)
![App Screenshot](https://github.com/codecellir/Codecell.Blazor.Components/blob/master/screenshots/dark.png?raw=true)