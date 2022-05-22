using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class IntRangeViewModel : ViewModel
{
    private Vector2Int _range;

    [Binding]
    public int min
    {
        get { return this._range[0]; }
        set
        {
            this._range[0] = value;
            this.OnPropertyChanged("min");
        }
    }

    [Binding]
    public int max
    {
        get { return this._range[1]; }
        set
        {
            this._range[1] = value;
            this.OnPropertyChanged("max");
        }
    }

    [Binding]
    public Vector2Int range
    {
        get { return this._range; }
        set
        {
            this.min = value[0];
            this.max = value[1];
            this.OnPropertyChanged("range");
        }
    }

    IntRangeViewModel(int min, int max)
    {
        this.min = min;
        this.max = max;
    }
}