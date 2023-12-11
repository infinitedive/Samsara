using UnityEngine;
using UnityEngine.Events;

public interface IPlayable
{
    public void GetHit(Vector3 hitVel);
    public void Stop();
}
