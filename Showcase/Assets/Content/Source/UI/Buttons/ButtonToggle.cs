using UnityEngine;
using UnityEngine.UI;

namespace Source.UI
{
public class ButtonToggle : ButtonBase
{
#region Fields

    [SerializeField] Image _icon;
    [SerializeField] Color _colorOn;
    [SerializeField] Color _colorOff;

#endregion

    public void OnToggleChange_handler(bool enable) => _icon.color = enable ? _colorOn : _colorOff;
}
}