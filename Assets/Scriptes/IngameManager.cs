using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;

public class IngameManager : MonoBehaviour {
    public Mesh mesh;
    public Material material;

    private void Start() {
        Assert.IsNotNull(this.mesh);
        Assert.IsNotNull(this.material);

        EntityManager entitieManager = World.Active.EntityManager;
        for (int i = 0; i < 1000; i++) {
            SpawnEntity(entitieManager);
        }
    }

    private void SpawnEntity(EntityManager entitieManager) {
        Entity entity = CreateEntity(entitieManager);
        SetEntityComponentData(entitieManager, 
                               entity, 
                               mesh, 
                               material, 
                               UnityEngine.Random.Range(0.1f, 0.5f), 
                               UnityEngine.Random.Range(0, 10), 
                               new float3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f), 0));
    }
   
    private Entity CreateEntity(EntityManager entitieManager) {
        Entity entity = entitieManager.CreateEntity(
            typeof(Unit),
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(Scale),
            typeof(Translation),
            typeof(MoveSpeed)
        );
        return entity;
    }
    private void SetEntityComponentData(EntityManager entitieManager,
                                        Entity entity, 
                                        Mesh mesh,
                                        Material material,
                                        float scale,
                                        float3 position, 
                                        float3 moveSpeed) {
        entitieManager.SetSharedComponentData<RenderMesh>(entity, new RenderMesh {
            mesh = mesh,
            material = material,
        });
        entitieManager.SetComponentData<Scale>(entity, new Scale {
            Value = scale,
        });
        entitieManager.SetComponentData<Translation>(entity, new Translation {
            Value = position,
        });
        entitieManager.SetComponentData<MoveSpeed>(entity, new MoveSpeed {
            value = moveSpeed,
        });
        
    }
}
