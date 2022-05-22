using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class FloatRangeViewModel : ViewModel
{
    private Vector2 _range;

    [Binding]
    public float min
    {
        get { return this._range[0]; }
        set
        {
            this._range[0] = value;
            this.OnPropertyChanged("min");
        }
    }

    [Binding]
    public float max
    {
        get { return this._range[1]; }
        set
        {
            this._range[1] = value;
            this.OnPropertyChanged("max");
        }
    }

    [Binding]
    public Vector2 range
    {
        get { return this._range; }
        set
        {
            this.min = value[0];
            this.max = value[1];
            this.OnPropertyChanged("range");
        }
    }

    FloatRangeViewModel(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}