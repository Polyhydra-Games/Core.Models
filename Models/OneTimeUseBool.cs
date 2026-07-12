namespace PolyhydraGames.Core.Models;

public struct OneTimeUseBool
{
    private bool _value = true;

    public OneTimeUseBool()
    {
    }

    public bool Value
    {
        get
        {
            var original = _value;
            _value = false;
            return original;
        }
    }
}
