namespace Vm167Box.Behaviors; 

public class EntryLostFocus : Behavior<Entry>
{
    public static readonly BindableProperty TextProperty = BindableProperty.CreateAttached(
        "Text",
        typeof(string),
        typeof(EntryLostFocus),
        null,
        BindingMode.TwoWay,
        propertyChanged: TextPropertyChanged);

    public static string GetText(Entry entry) => (string)entry.GetValue(TextProperty);
    public static void SetText(Entry entry, string value) => entry.SetValue(TextProperty, value);

    private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var entry = (Entry)bindable;
        entry.Text = newValue as string;
    }

    protected override void OnAttachedTo(Entry bindable)
    {
        base.OnAttachedTo(bindable);
        bindable.Unfocused += Entry_Unfocused;
    }

    protected override void OnDetachingFrom(Entry bindable)
    {
        base.OnDetachingFrom(bindable);
        bindable.Unfocused -= Entry_Unfocused;
    }

    private void Entry_Unfocused(object? sender, FocusEventArgs e)
    {
        var entry = (Entry)sender!;
        SetText(entry, entry.Text);
    }
}