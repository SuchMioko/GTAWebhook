using System; 
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace GTAVWebhook.Types
{
   public class SpawnnerVehicles
   {
      private List<Vehicle> spawnedVehicles = new List<Vehicle>();
      private Dictionary<Vehicle, string> vehicleNames = new Dictionary<Vehicle, string>();
      private Dictionary<Vehicle, Color> vehicleColors = new Dictionary<Vehicle, Color>();
      private NPCManager npcManager = new NPCManager();
      private List<Ped> spawnedNPCs = new List<Ped>();
      public NPCManager NPCManager => npcManager;

      public void SpawnVehicle(string vehicleName, Vector3 position, string name, PedHash pedName, WeaponHash weaponName)
      {
         if (Enum.TryParse<VehicleHash>(vehicleName, out VehicleHash result))
         {
            // Struktur Kendaraan
            Vehicle val = World.CreateVehicle(new Model(result), position, 0f);
            spawnedVehicles.Add(val);
            vehicleNames[val] = name;

            ((Entity)val).MaxHealth = 700;
            ((Entity)val).Health = 700;
            val.IsEngineRunning = true;

            Function.Call(Hash.SET_VEHICLE_DOORS_LOCKED, val.Handle, 4);
            Function.Call(Hash.SET_VEHICLE_TYRES_CAN_BURST, val.Handle, false);
            Function.Call(Hash.SET_VEHICLE_ENGINE_ON, val.Handle, true, true, false);
            Function.Call(Hash.SET_ENTITY_INVINCIBLE, val.Handle, true);

            // Ped supir
            Model model = new Model(pedName);
            Ped val2 = npcManager.CreatePed(model, ((Entity)val).Position, isDriver: true);
            val2.SetIntoVehicle(val, (VehicleSeat)(-1));
            spawnedNPCs.Add(val2);
            val2.Weapons.Give(weaponName, 250, true, true);
            val2.Task.FightAgainst(Game.Player.Character);

            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, val2.Handle, 46, true);
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, val2.Handle, 3, false);
            Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, val2.Handle, false);

            // Ped penumpang hareup
            Ped val3 = npcManager.CreatePed(model, ((Entity)val).Position);
            val3.SetIntoVehicle(val, (VehicleSeat)0);
            spawnedNPCs.Add(val3);
            val3.Weapons.Give(weaponName, 9999, true, true);
            val3.Task.FightAgainst(Game.Player.Character);

            Function.Call(Hash.SET_ENTITY_HEALTH, val3.Handle, 700);

            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, val3.Handle, 46, true);
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, val3.Handle, 3, false);
            Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, val3.Handle, false);

            // Loop ped jang ngeusi sesa penumpang
            for (int i = 1; i < val.PassengerCapacity; i++)
            {
               Ped val4 = npcManager.CreatePed(model, ((Entity)val).Position);
               val4.SetIntoVehicle(val, (VehicleSeat)i);
               spawnedNPCs.Add(val4);
               val4.Weapons.Give(weaponName, 9999, true, true);
               val4.Task.FightAgainst(Game.Player.Character);

               Function.Call(Hash.SET_ENTITY_HEALTH, val4.Handle, 700);

               Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, val4.Handle, 46, true);
               Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, val4.Handle, 3, false);
               Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, val4.Handle, false);
            }

            // Jiuen hubungan antar supir >= penumpang, + dictionary jang ped
            Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, val2.Handle, Game.GenerateHash("ENEMY"));
            Ped[] passengers = val.Passengers;
            foreach (Ped val5 in passengers)
            {
               Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, val5.Handle, Game.GenerateHash("ENEMY"));
               Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, val3.Handle, Game.GenerateHash("ENEMY"));
            }

            Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, 5, Game.GenerateHash("ENEMY"), Game.GenerateHash("PLAYER"));
            Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, 5, Game.GenerateHash("PLAYER"), Game.GenerateHash("ENEMY"));
            Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, 1, Game.GenerateHash("ENEMY"), Game.GenerateHash("ENEMY"));

            Function.Call(Hash.SET_ENTITY_HEALTH, val2.Handle, 700);
            Function.Call(Hash.SET_PED_RESET_FLAG, val2.Handle, 0f);

            // Bere task (Misi) ped jang nyupir ka arah player
            val2.Task.DriveTo(val, Game.Player.Character.Position, 0f, 40f, DrivingStyle.AvoidTrafficExtremely);
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, val2.Handle, 3, false);

            Ped[] passengers2 = val.Passengers;
            foreach (Ped val6 in passengers2)
            {
               Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, val6.Handle, 3, false);
            }

            Function.Call(Hash.SET_VEHICLE_SIREN, val.Handle, true);
            Function.Call(Hash.SET_VEHICLE_LIGHTS, val.Handle, true);
            Function.Call(Hash.SET_VEHICLE_MAX_SPEED, 10f);

         }
      }

      private Vector3 GetSafeSpawnPosition(Vector3 originalPosition)
      {
          Vector3 val = originalPosition;
          float num = 5f;
          bool flag;
          do
          {
             flag = true;
             foreach (Vehicle item in spawnedVehicles)
             {
                Vector3 position = ((Entity)item).Position;
                if (position.DistanceTo(val) < num)
                {
                   val.X += num;
                   val.Y += num;
                   flag = false;
                   break;
                }
             }
          }
          while (!flag);
          return val;
       }

       public void RemoveSpawnedVehiclesAndNPCs()
       {
          foreach (Vehicle item in spawnedVehicles)
          {
             if ((Entity)(object)item != (Entity)null && item.Exists())
             {
                ((PoolObject)item).Delete();
             }
          }
          spawnedVehicles.Clear();
          vehicleNames.Clear();
          vehicleColors.Clear();
          foreach (Ped spawnedNPC in spawnedNPCs)
          {
             if ((Entity)(object)spawnedNPC != (Entity)null && spawnedNPC.Exists())
             {
                ((PoolObject)spawnedNPC).Delete();
             }
          }
          spawnedNPCs.Clear();
       }

       public void DrawName()
       {
          foreach (KeyValuePair<Vehicle, string> vehicleName in vehicleNames)
          {
             Vehicle key = vehicleName.Key;
             string value = vehicleName.Value;
             if ((Entity)(object)key != (Entity)null && value != null && World.GetDistance(((Entity)key).Position, ((Entity)Game.Player.Character).Position) <= 30f && ((Entity)key).IsOnScreen)
             {
                PointF pointF = Screen.WorldToScreen(((Entity)key).Position, false);
                PointF pointF2 = new PointF(pointF.X, pointF.Y - 20f);
                if (!vehicleColors.ContainsKey(key))
                {
                   Color randomBrightSpecificColor = GetRandomBrightSpecificColor();
                   vehicleColors.Add(key, randomBrightSpecificColor);
                }
                Color color = vehicleColors[key];
                TextElement val = new TextElement(value, pointF2, 1.0f, color, GTA.UI.Font.Pricedown, Alignment.Center);
                val.Outline = true;
                val.Draw();
             }
          }
       }

       private Color GetRandomBrightSpecificColor()
       {
          Random random = new Random();
          int red;
          int green;
          int blue;
          switch (random.Next(1, 6))
          {
          case 1:
             red = random.Next(30, 80);
             green = random.Next(200, 256);
             blue = random.Next(30, 80);
             break;
          case 2:
             red = random.Next(200, 256);
             green = random.Next(20, 50);
             blue = random.Next(20, 50);
             break;
          case 3:
             red = random.Next(0, 50);
             green = random.Next(200, 256);
             blue = random.Next(200, 256);
             break;
          case 4:
             red = random.Next(200, 256);
             green = random.Next(200, 256);
             blue = random.Next(0, 50);
             break;
          case 5:
             red = random.Next(150, 200);
             green = random.Next(50, 100);
             blue = random.Next(150, 200);
             break;
          case 6:
             red = random.Next(200, 255);
             green = random.Next(100, 180);
             blue = random.Next(150, 255);
             break;
          default:
             red = (green = (blue = 255));
             break;
          }
          return Color.FromArgb(red, green, blue);
       }

       public void FlipVehicleIfUpsideDown(Vehicle vehicle)
       {
          if (((Entity)vehicle).IsUpsideDown)
          {
             ((Entity)vehicle).Rotation = new Vector3(((Entity)vehicle).Rotation.X, ((Entity)vehicle).Rotation.Y, ((Entity)vehicle).Rotation.Z + 180f);
          }
       }
   }
}
