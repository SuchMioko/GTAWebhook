using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAVWebhook.Types
{
    public class NPCManager
    {
        private List<Ped> spawnedPeds = new List<Ped>();

        public Ped CreatePed(Model model, Vector3 position, bool isDriver = false)
        {
            Ped val = World.CreatePed(model, position, 0f);
            ConfigurePed(val, isDriver);
            return val;
        }

        private void ConfigurePed(Ped ped, bool isDriver)
        {
            Function.Call(Hash.SET_PED_CONFIG_FLAG, ((PoolObject)ped).Handle, true);
            Function.Call(Hash.GIVE_WEAPON_TO_PED, ((PoolObject)ped).Handle, 9999);
            Function.Call(Hash.SET_PED_COMBAT_ABILITY, ((PoolObject)ped).Handle, 2, true);
            Function.Call(Hash.SET_PED_CAN_BE_TARGETTED, ((PoolObject)ped).Handle, 5, true);
            Function.Call(Hash.SET_PED_CONFIG_FLAG, ((PoolObject)ped).Handle, 46, true);
            Function.Call(Hash.SET_PED_CONFIG_FLAG, ((PoolObject)ped).Handle, 35, true);
            Function.Call(Hash.SET_PED_ACCURACY, ((PoolObject)ped).Handle, 100);
            Function.Call(Hash.SET_PED_CONFIG_FLAG, ((PoolObject)ped).Handle, 1, true);
            Function.Call(Hash.SET_PED_CONFIG_FLAG, ((PoolObject)ped).Handle, 2, true);
            Function.Call(Hash.SET_PED_CONFIG_FLAG, ((PoolObject)ped).Handle, 3, true);
            Function.Call(Hash.SET_PED_CONFIG_FLAG, ((PoolObject)ped).Handle, 21, true);
            Function.Call(Hash.SET_PED_CONFIG_FLAG, ((PoolObject)ped).Handle, 23, true);
            Function.Call(Hash.SET_PED_CONFIG_FLAG, ((PoolObject)ped).Handle, 62, true);

            ped.CanRagdoll = false;
            int num = Function.Call<int>(Hash.GET_HASH_KEY, "GANG_1");
            Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped.Handle, num);

            int num2 = Function.Call<int>(Hash.GET_HASH_KEY, "PLAYER");
            Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, 5, num, num2);
            Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, 5, num2, num);

            if (!isDriver)
            {
                Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, ped.Handle, 324215364, 9999, true, true);
                Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped.Handle, true);
                Function.Call(Hash.TASK_AIM_GUN_AT_ENTITY, ped.Handle, Game.Player.Character, 0, 16);

                ped.CanRagdoll = false;
            }
        }

        public void RemoveSpawnedPeds()
        {
            foreach (Ped spawnedPed in spawnedPeds)
            {
                if ((Entity)(object)spawnedPed != (Entity)null && spawnedPed.Exists())
                {
                    ((PoolObject)spawnedPed).Delete();
                }
            }
            spawnedPeds.Clear();
        }
    }
}
