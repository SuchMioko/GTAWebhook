using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAVWebhook.Types
{
   public class EnemyConfiguration
   {

      public void Configuration(Ped npc, string spawnName)
      {
         Function.Call(Hash.SET_PED_CAN_RAGDOLL, npc.Handle, false);
         Function.Call(Hash.SET_PED_CONFIG_FLAG, npc.Handle, 281, true);
         Function.Call(Hash.SET_PED_CONFIG_FLAG, npc.Handle, 145, true);
         Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, npc, 1, false);
         Function.Call(Hash.SET_PED_CONFIG_FLAG, npc.Handle, 2, true);
         Function.Call(Hash.SET_PED_CONFIG_FLAG, npc.Handle, 0, true);
         Function.Call(Hash.SET_PED_CONFIG_FLAG, npc.Handle, 3, true);
         Function.Call(Hash.SET_PED_CONFIG_FLAG, npc.Handle, 5, true);
         Function.Call(Hash.GIVE_PED_HELMET, npc, 2);

         if (spawnName == "Zeus") {
            ((Entity)npc).MaxHealth = 650;
            ((Entity)npc).Health = 650;
            npc.CanRagdoll = false;
            ((Entity)npc).IsExplosionProof = false;
            ((Entity)npc).IsFireProof = true;
         }
         if (spawnName == "Spiderman") {
            ((Entity)npc).MaxHealth = 550;
            ((Entity)npc).Health = 550;
            npc.CanRagdoll = false;
            ((Entity)npc).IsFireProof = true;
            Function.Call(Hash.SET_PED_CONFIG_FLAG, npc.Handle, 118, true);
            Function.Call(Hash.SET_PED_CONFIG_FLAG, npc.Handle, 32, true);
         }
         if (spawnName == "Thanos") {
            ((Entity)npc).MaxHealth = 700;
            ((Entity)npc).Health = 700;
            npc.CanRagdoll = false;
            ((Entity)npc).IsExplosionProof = false;
            ((Entity)npc).IsFireProof = true;
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, npc.Handle, 46, true);
            Function.Call(Hash.SET_PED_CAN_BE_TARGETTED, npc, false);
         }
         if (spawnName == "Transformer") {
            ((Entity)npc).MaxHealth = 800;
            ((Entity)npc).Health = 800;
            npc.CanRagdoll = false;
            ((Entity)npc).IsExplosionProof = false;
            ((Entity)npc).IsFireProof = true;
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, npc.Handle, 46, true);
            Function.Call(Hash.SET_PED_CAN_BE_TARGETTED, npc, false);
         }
         if (spawnName == "Godzilla") {
            ((Entity)npc).MaxHealth = 1000;
            ((Entity)npc).Health = 1000;
            npc.CanRagdoll = false;
            ((Entity)npc).IsFireProof = true;
            ((Entity)npc).IsSteamProof = true;
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, npc.Handle, 46, true);
            Function.Call(Hash.SET_PED_CAN_BE_TARGETTED, npc, false);
         }
         if (spawnName == "Naruto") {
            ((Entity)npc).MaxHealth = 800;
            ((Entity)npc).Health = 800;
            npc.CanRagdoll = false;
            ((Entity)npc).IsExplosionProof = true;
            ((Entity)npc).IsFireProof = true;
         }
         if (spawnName == "Turtle") {
            ((Entity)npc).MaxHealth = 700;
            ((Entity)npc).Health = 700;
            npc.CanRagdoll = false;
            ((Entity)npc).IsExplosionProof = false;
            ((Entity)npc).IsFireProof = true;
            ((Entity)npc).IsMeleeProof = false;
            ((Entity)npc).IsBulletProof = false;
         }
      }
   }
}




