using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
/*
public class Getter : JobComponentSystem
{    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        JobHandle jobHandle = Entities.ForEach((ref Color color) => { }).Schedule(inputDeps);

        return jobHandle;
    }
}
*/