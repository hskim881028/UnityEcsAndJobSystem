using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class MoveSystem : JobComponentSystem {
    
    [RequireComponentTag(typeof(Unit))]
    [BurstCompile]
    struct MoveSystemJob : IJobForEachWithEntity<Translation, MoveSpeed> {
        [DeallocateOnJobCompletion] public NativeArray<Entity> units;
        public void Execute(Entity entity, 
                            int index, 
                            ref Translation translation, 
                            [ReadOnly] ref MoveSpeed moveSpeed) {
            float distance = math.distance(translation.Value, Vector3.zero);
            if (distance >= 10) {
                translation.Value = 0;
            }
            translation.Value += moveSpeed.value;
        }
    }
    private EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBufferSystem;
    protected override void OnCreate() {
        this.endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        base.OnCreate();
    }
    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        EntityQuery entityQuery = GetEntityQuery(typeof(Unit));
        NativeArray<Entity> units = entityQuery.ToEntityArray(Allocator.TempJob);
        MoveSystemJob moveJob = new MoveSystemJob {
            units = units
        };
        JobHandle job = moveJob.Schedule(this, inputDependencies);
        this.endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(job);
        return job;
    }
}