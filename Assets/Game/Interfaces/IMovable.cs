using UnityEngine;
using UnityEngine.Events;

public interface IMovable
{
    public void GetHit(Vector3 hitVel);
    public void Stop();
}
