using UnityWeld.Binding;
using UnityEngine;

[Binding]
public class DungeonGeneratorViewModel : ViewModel
{
    [SerializeField] private FloatRangeViewModel _dungeonSizeRangeViewModel;
    [SerializeField] private FloatRangeViewModel _roomWidthRangeViewModel;
    [SerializeField] private FloatRangeViewModel _roomHeightRangeViewModel;
    [SerializeField] private IntRangeViewModel _roomCountRangeViewModel;
    [SerializeField] private FloatRangeViewModel _hallwayLengthRangeViewModel;
    [ReadOnly] private bool _randomizeSeed;
    [ReadOnly] private int _randomSeed;

    public Vector2 DungeonSize
    {
        get { return this._dungeonSizeRangeViewModel.range; }
        set { this._dungeonSizeRangeViewModel.range = value; }
    }
    public Vector2 RoomWidthRange
    {
        get { return this._roomWidthRangeViewModel.range; }
        set { this._roomWidthRangeViewModel.range = value; }
    }
    public Vector2 RoomHeightRange
    {
        get { return this._roomHeightRangeViewModel.range; }
        set { this._roomHeightRangeViewModel.range = value; }
    }
    public Vector2Int RoomCountRange
    {
        get { return this._roomCountRangeViewModel.range; }
        set { this._roomCountRangeViewModel.range = value; }
    }
    public Vector2 HallwayLengthRange
    {
        get { return this._hallwayLengthRangeViewModel.range; }
        set { this._hallwayLengthRangeViewModel.range = value; }
    }
    [Binding]
    public int RandomSeed
    {
        get { return this._randomSeed; }
        set
        {
            this._randomSeed = value;
            this.OnPropertyChanged("RandomSeed");
        }
    }
    [Binding]
    public bool RandomizeSeed
    {
        get { return this._randomizeSeed; }
        set
        {
            this._randomizeSeed = value;
            this.OnPropertyChanged("RandomizeSeed");
        }
    }
}
