using System;
using UnityEngine.Rendering;

namespace Source
{
[Serializable]
public struct VolumeByState
{
    public EVolumeState State;
    public Volume Volume;
}
}