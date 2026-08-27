using Unity.Cinemachine;

namespace Source
{
public struct StateDrivenCameraData
{
    public CinemachineVirtualCameraBase Camera { get; }
    public int StateHash { get; }
    
    public StateDrivenCameraData(CinemachineVirtualCameraBase camera, int stateHash)
    {
        Camera = camera;
        StateHash = stateHash;
    }
}
}