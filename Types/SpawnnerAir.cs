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
   public class SpawnnerAir
   {
      private List<Vehicle> spawnedVehicles = new List<Vehicle>();
      private Dictionary<Vehicle, string> vehicleNames = new Dictionary<Vehicle, string>();
      private Dictionary<Vehicle, Color> vehicleColors = new Dictionary<Vehicle, Color>();
      private NPCManager npcManager = new NPCManager();
      private List<Ped> spawnedNPCs = new List<Ped>();
      public NPCManager NPCManager => npcManager;

      public void SpawnAir(string heliName, Vector3 position, string name)
      {
         if (Enum.TryParse(heliName, out VehicleHash result))
         {
            // Spawn kendarann udara
            Vector3 spawnPos = GetSafeSpawnPosition(position + new Vector3(0, 0, 30f));
            Vehicle heli = World.CreateVehicle(new Model(result), spawnPos, 0f);
            spawnedVehicles.Add(heli);
            vehicleNames[heli] = name;

            heli.MaxHealth = 1500;
            heli.Health = 1500;
            heli.IsEngineRunning = true;

            Function.Call(Hash.SET_VEHICLE_DOORS_LOCKED, heli.Handle, 4);
            Function.Call(Hash.SET_VEHICLE_ENGINE_ON, heli.Handle, true, true, false);

            // Jieun pilot + nemak make senjata helikopter na
            Ped pilot = npcManager.CreatePed(new Model(PedHash.Marine03SMY), heli.Position, true);
            pilot.SetIntoVehicle(heli, VehicleSeat.Driver);
            spawnedNPCs.Add(pilot);

            pilot.Task.FightAgainst(Game.Player.Character);

            Vector3 patrolHeli = Game.Player.Character.Position + new Vector3(10f, 0, 35f);
            Function.Call(Hash.TASK_HELI_MISSION,
              pilot, heli, Game.Player.Character, 0, 0,
              patrolHeli.X + 20.0f,
              patrolHeli.Y,
              patrolHeli.Z + 10.0f,
              19,
              50.0f,
              50.0f,
              100.0f,
              0f,
              16
            );

            Function.Call(Hash.SET_ENTITY_HEALTH, pilot.Handle, 700);

            Function.Call(Hash.SET_PED_SHOOT_RATE, pilot, 500);
            Function.Call(Hash.SET_PED_ACCURACY, pilot, 60);
            Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, pilot, true);
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, pilot, 5, true);
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, pilot, 24, true);
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, pilot, 46, true);
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, pilot, 52, true);

            pilot.Task.VehicleChase(Game.Player.Character);

            // Jieun penumpang hareup
            Ped frontPassenger = npcManager.CreatePed(new Model(PedHash.Marine03SMY), heli.Position);
            frontPassenger.SetIntoVehicle(heli, VehicleSeat.Passenger);
            spawnedNPCs.Add(frontPassenger);

            frontPassenger.Weapons.Give(WeaponHash.CombatMG, 9999, true, true);
            frontPassenger.Task.FightAgainst(Game.Player.Character);

            Function.Call(Hash.SET_ENTITY_HEALTH, frontPassenger.Handle, 700);
            Function.Call(Hash.SET_PED_CONFIG_FLAG, frontPassenger.Handle, 46, true);
            Function.Call(Hash.SET_PED_CONFIG_FLAG, frontPassenger.Handle, 3, false);

            // Jieun sesa penumpang na
            for (int i = 1; i < heli.PassengerCapacity; i++)
            {
               Ped passenger = npcManager.CreatePed(new Model(PedHash.Marine03SMY), heli.Position);
               passenger.SetIntoVehicle(heli, (VehicleSeat)i);
               spawnedNPCs.Add(passenger);
               passenger.Weapons.Give(WeaponHash.CombatMG, 9999, true, true);
               passenger.Task.FightAgainst(Game.Player.Character);

               Function.Call(Hash.SET_ENTITY_HEALTH, passenger.Handle, 700);
               Function.Call(Hash.SET_PED_CONFIG_FLAG, passenger.Handle, 46, true);
               Function.Call(Hash.SET_PED_CONFIG_FLAG, passenger.Handle, 3, false);
            }

            // Jieun hubungan antar penumpang jeung pilot
            Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, pilot.Handle, Game.GenerateHash("ENEMY"));
            Ped[] passangers = heli.Passengers;
            foreach (Ped passanger in passangers)
            {
               Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, passanger.Handle, Game.GenerateHash("ENEMY"));
            }
            Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, 5, Game.GenerateHash("ENEMY"), Game.GenerateHash("PLAYER"));
            Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, 5, Game.GenerateHash("PLAYER"), Game.GenerateHash("ENEMY"));
            Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, 1, Game.GenerateHash("ENEMY"), Game.GenerateHash("PLAYER"));

            Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, pilot.Handle, false);
            foreach (Ped passanger in heli.Passengers)
            {
               Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, passanger.Handle, false);
            }

            Function.Call(Hash.SET_VEHICLE_UNDRIVEABLE, heli.Handle, false);

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
                TextElement val = new TextElement(value, pointF2, 0.8f, color, GTA.UI.Font.Pricedown, Alignment.Center);
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
