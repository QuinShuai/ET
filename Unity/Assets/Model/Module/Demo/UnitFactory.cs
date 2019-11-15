using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ETModel {
    public static class UnitFactory {
        public static async ETTask<Unit> Create(long id) {
            UnitComponent unitComponent = Game.Scene.GetComponent<UnitComponent>();

            var go = await Addressables.InstantiateAsync("Unit/Skeleton.prefab").Task;
            Unit unit = ComponentFactory.CreateWithId<Unit, GameObject>(id, go);

            unit.AddComponent<AnimatorComponent>();
            unit.AddComponent<MoveComponent>();
            unit.AddComponent<TurnComponent>();
            unit.AddComponent<UnitPathComponent>();

            unitComponent.Add(unit);
            return unit;
        }
    }
}