using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Expressions;

namespace Codecell.Component.Blazor.Infrastructure;

public class ValidationMessageBase<TValue> : ComponentBase, IDisposable
{

    [CascadingParameter]
    public EditContext EditContext { get; set; }

    [Parameter]
    public Expression<Func<TValue>> For { get; set; }

    [Parameter]
    public EventCallback<bool> OnValidationChanged { get; set; }

    private FieldIdentifier _fieldIdentifier;

    protected IEnumerable<string> ValidationMessages => EditContext.GetValidationMessages(_fieldIdentifier);
    protected override void OnInitialized()
    {
        _fieldIdentifier = FieldIdentifier.Create(For);
        EditContext.OnValidationStateChanged += HandleValidationStateChanged;
    }

    private void HandleValidationStateChanged(object o, ValidationStateChangedEventArgs args)
    {
        var status = EditContext.IsValid(_fieldIdentifier);
        OnValidationChanged.InvokeAsync(status);
        StateHasChanged();
    }

    public void Dispose() => EditContext.OnValidationStateChanged -= HandleValidationStateChanged;

    public void Immediate() => EditContext.Validate();
}
