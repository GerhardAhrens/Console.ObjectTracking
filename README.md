# Console.ObjectTracking

![NET](https://img.shields.io/badge/NET-8.0-green.svg)
![License](https://img.shields.io/badge/License-MIT-blue.svg)
![VS2022](https://img.shields.io/badge/Visual%20Studio-2022-white.svg)
![Version](https://img.shields.io/badge/Version-1.0.2025.0-yellow.svg)]

Mit den beiden Interface *IChangeTracking* und *IRevertibleChangeTracking* werden Methoden implementiert um Änderungen an einem Objekt verfolgen zu können.
Speziell das Interface *IRevertibleChangeTracking* ermöglicht auch die Implementierung ein Zurücksetzten *RejectChanges()* der Änderungen. 

Beispiel mit dem Interface *IRevertibleChangeTracking*
```csharp
public class ViewItemTrackingV2 : TrackingBase, IRevertibleChangeTracking
{
    #region Properties
    //  
    #endregion Properties

    public void RejectChanges()
    {
        foreach (KeyValuePair<string,object> property in this.OriginalValues)
        {
            this.GetType().GetRuntimeProperty(property.Key).SetValue(this, property.Value);
        }

        this.AcceptChanges();
    }

    public void AcceptChanges()
    {
        base.ResetChanged();
    }
}
```

Basisklasse *TrackingBase*
```csharp
public abstract class TrackingBase
{
    private readonly ConcurrentDictionary<string, object> originalValues = new ConcurrentDictionary<string, object>();

    public int CountChanges { get { return this.originalValues.Count(); } }

    public ConcurrentDictionary<string, object> OriginalValues { get { return this.originalValues; } }

    public bool IsChanged { get; private set; }

    public void AddOriginalValues(string key, object originalValue)
    {
        if (this.originalValues.ContainsKey(key) == false)
        {
            this.originalValues[key] = originalValue;
            this.IsChanged = true;
        }
    }

    public void ResetChanged()
    {
        this.originalValues.Clear();
        this.IsChanged = false;
    }
}
```

