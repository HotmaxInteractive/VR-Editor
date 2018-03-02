using UnityEngine;

public interface IHittable
{
    void receiveHit(RaycastHit hit);
}