using UnityEngine;
using UnityEngine.UI;

namespace Source.UI
{
public class ButtonToggle : ButtonBase
{
    [SerializeField] Image _icon;
    [SerializeField] Color _colorOn;
    [SerializeField] Color _colorOff;

    public void OnToggleChange_handler(bool enable) => _icon.color = enable ? _colorOn : _colorOff;
}
}