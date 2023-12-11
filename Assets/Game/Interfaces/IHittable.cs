using UnityEngine;
using UnityEngine.Events;

public interface IHittable
{
    public void GetHit(Vector3 hitVel);
    public void Stop();
}
