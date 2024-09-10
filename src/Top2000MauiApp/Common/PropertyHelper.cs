#nullable disable

using System.Runtime.CompilerServices;

namespace Top2000MauiApp.Common;

/// <remarks>
/// All credits for the PropertyBackingDictionary goes to: http://www.codecadwallader.com/2013/04/06/inotifypropertychanged-2-of-3-without-the-backing-fields/
/// </remarks>
public class PropertyHelper
{
    private readonly Dictionary<string, object> propertyBackingDictionary = [];
    private readonly Action<string> notifyOfPropertyChange;
    private readonly Type type;

    private ILookup<string, string> _dependentLookup;

    public PropertyHelper(Action<string> notifyOfPropertyChange, Type type)
    {
        this.notifyOfPropertyChange = notifyOfPropertyChange;
        this.type = type;
    }

    private ILookup<string, string> DependentLookup
    {
        get
        {
            return _dependentLookup ??= (from p in type.GetProperties()
                                         let attrs = p.GetCustomAttributes(typeof(NotifiesOnAttribute), false)
                                         from NotifiesOnAttribute a in attrs
                                         select new { Independent = a.Name, Dependent = p.Name }).ToLookup(i => i.Independent, d => d.Dependent);
        }
    }

    public T GetPropertyValue<T>([CallerMemberName] string propertyName = null)
    {
        _ = propertyName ?? throw new ArgumentNullException(nameof(propertyName));

        if (propertyBackingDictionary.TryGetValue(propertyName, out object value))
        {
            return (T)value;
        }

        return default;
    }

    public bool SetPropertyValue<T>(T newValue, [CallerMemberName] string propertyName = null)
    {
        var set = this.SetPropertyValueSilent(newValue, propertyName);
        if (set)
        {
            this.InvokePropertyChangedEvent(propertyName);
        }

        return set;
    }

    public bool SetPropertyValueSilent<T>(T newValue, string propertyName)
    {
        if (propertyName == null)
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        if (EqualityComparer<T>.Default.Equals(newValue, this.GetPropertyValue<T>(propertyName)))
        {
            return false;
        }

        propertyBackingDictionary[propertyName] = newValue;
        return true;
    }

    protected void InvokePropertyChangedEvent([CallerMemberName] string propertyName = null)
    {
        if (propertyName == null)
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        if (notifyOfPropertyChange != null)
        {
            notifyOfPropertyChange(propertyName);
            foreach (var dependentPropertyName in this.DependentLookup[propertyName])
            {
                this.InvokePropertyChangedEvent(dependentPropertyName);
            }
        }
    }
}
