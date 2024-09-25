## CodecellComponent.Blazor
Custom Components for Blazor

## PersianDatePicker
## Features

- Two-Way Binding
- Dark Mode
- Month Navigation
- Year Navigation
- Date Change Event Callback
- Validation
- Mask Input

## Installation
You can download the latest version of `Codecell.Component.Blazor` from [Github repository](https://github.com/codecellir/Codecell.Blazor.Components).
To install via `nuget`:
```
Install-Package Codecell.Component.Blazor -Version 0.2.5
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

Add style references
``` Razor
@page "/"

<link href="_content/Codecell.Component.Blazor/codecell.css" rel="stylesheet" />
```

Usage Sample
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

Validation Sample
``` Razor
@page "/"
@using System.ComponentModel.DataAnnotations
<PageTitle>Home</PageTitle>

<h1>Hello, world!</h1>

<EditForm Model="@Model" OnValidSubmit="Submit">
    <DataAnnotationsValidator />
    <ValidationSummary/>
    <div class="row" style="direction:rtl">
        <div class="col-md-4">
            <InputText class="form-control" @bind-Value="Model.Name" />
        </div>
        <div class="col-12"></div>
        <div class="col-md-4 mt-3">
            <PersianDatePicker @bind-Date="Model.BirthDate"
                               PlaceHolder="تاریخ تولد"
                               Label="تاریخ تولد:"
                               DarkMode="false"
                               For="()=>Model.BirthDate" 
                               Immediate="true"/>
        </div>
        <div class="col-12 mt-2">
            <button class="btn btn-primary" type="submit">Submit</button>
        </div>
    </div>
</EditForm>


<p class="mt-2">@Model.BirthDate</p>


@code{

    Student Model { get; set; } = new();


    public class Student
    {
        public string Name { get; set; }

         [Required(ErrorMessage ="تاریخ تولد اجباری است")]
        public DateTime? BirthDate { get; set; }
    }

    void Submit()
    {

    }
}


```


## Screenshots
![App Screenshot](https://github.com/codecellir/Codecell.Blazor.Components/blob/master/screenshots/light.png?raw=true)
![App Screenshot](https://github.com/codecellir/Codecell.Blazor.Components/blob/master/screenshots/dark.png?raw=true)
![App Screenshot](https://github.com/codecellir/Codecell.Blazor.Components/blob/master/screenshots/validation.png?raw=true)