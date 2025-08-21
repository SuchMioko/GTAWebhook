using System;
using System.Collections.Generic;
using System.Linq;
using GTA;
using GTA.Native;

public class NPCCleaner : Script
{
	private const float Radius = 500f;

	private const int DeleteDelay = 20000;

	private const int EmptyVehicleDelay = 25000;

	private Dictionary<Ped, DateTime> deadPeds = new Dictionary<Ped, DateTime>();

	private Dictionary<Vehicle, DateTime> destroyedVehicles = new Dictionary<Vehicle, DateTime>();

	private Dictionary<Vehicle, DateTime> emptyVehicles = new Dictionary<Vehicle, DateTime>();

	private DateTime lastCleanTime = DateTime.Now;

	private const int CleanInterval = 1000;

	private readonly HashSet<Model> weaponModels = new HashSet<Model>
	{
      new Model(WeaponHash.AssaultRifle),           // -1074790547
      new Model(WeaponHash.SniperRifle),            // 100416529
      new Model(WeaponHash.CarbineRifle),           // -2084633992 (mungkin mirip -2067956739)
      new Model(WeaponHash.PumpShotgun),            // 487013001
      new Model(WeaponHash.APPistol),               // 584646201 (mungkin mendekati 1317494643)
      new Model(WeaponHash.Pistol),                 // 453432689
      new Model(WeaponHash.MicroSMG),               // 324215364
      new Model(WeaponHash.CombatPistol),           // 1593441988 (mirip -1312131151?)
      new Model(WeaponHash.MG),                     // -1660422300 (mungkin mirip 1834241177)
      new Model(WeaponHash.AdvancedRifle),          // -1357824103 (bisa jadi 1672152130)
      new Model(WeaponHash.SMG),                    // 736523883
      new Model(WeaponHash.SawnOffShotgun),         // 2017895192 (bisa jadi 1119849093)
      new Model(WeaponHash.HeavySniper),            // -952879014 (mungkin mirip -581044007)
      new Model(WeaponHash.Gusenberg),              // 1627465347 (mirip -1238556825?)
      new Model(WeaponHash.Pistol50),               // -1716589765 (mungkin -1716189206)
      new Model(WeaponHash.MachinePistol)           // 213834749
																	 //
	};

	public NPCCleaner()
	{
		((Script)this).Tick += OnTick;
	}

	private void OnTick(object sender, EventArgs e)
	{
		if ((DateTime.Now - lastCleanTime).TotalMilliseconds >= 1000.0)
		{
			CleanDeadPeds();
			CleanDestroyedVehicles();
			CleanDroppedWeapons();
			CleanEmptyVehicles();
			lastCleanTime = DateTime.Now;
		}
	}

	private void CleanDeadPeds()
	{
		Ped[] nearbyPeds = World.GetNearbyPeds(((Entity)Game.Player.Character).Position, 500f, Array.Empty<Model>());
		Ped[] array = nearbyPeds;
		foreach (Ped val in array)
		{
			if (((Entity)val).IsDead && !val.IsPlayer && !deadPeds.ContainsKey(val))
			{
				deadPeds[val] = DateTime.Now;
			}
		}
		List<Ped> list = (from entry in deadPeds
			where (DateTime.Now - entry.Value).TotalMilliseconds >= 20000.0
			select entry.Key).ToList();
		foreach (Ped item in list)
		{
			((PoolObject)item).Delete();
			deadPeds.Remove(item);
		}
	}

	private void CleanDestroyedVehicles()
	{
		Vehicle[] nearbyVehicles = World.GetNearbyVehicles(((Entity)Game.Player.Character).Position, 500f, Array.Empty<Model>());
		Vehicle[] array = nearbyVehicles;
		foreach (Vehicle val in array)
		{
			if (((Entity)val).IsDead && !destroyedVehicles.ContainsKey(val))
			{
				destroyedVehicles[val] = DateTime.Now;
			}
		}
		List<Vehicle> list = (from entry in destroyedVehicles
			where (DateTime.Now - entry.Value).TotalMilliseconds >= 20000.0
			select entry.Key).ToList();
		foreach (Vehicle item in list)
		{
			((PoolObject)item).Delete();
			destroyedVehicles.Remove(item);
		}
	}

	private void CleanDroppedWeapons()
	{
		Prop[] nearbyProps = World.GetNearbyProps(((Entity)Game.Player.Character).Position, 250f, Array.Empty<Model>());
		Prop[] array = nearbyProps;
		foreach (Prop val in array)
		{
			if ((Entity)(object)val != (Entity)null && val.Exists() && weaponModels.Contains(((Entity)val).Model))
			{
				((PoolObject)val).Delete();
			}
		}
		Ped[] allPeds = World.GetAllPeds(Array.Empty<Model>());
		foreach (Ped val2 in allPeds)
		{
			if (((Entity)val2).IsDead && !val2.IsPlayer)
			{
            Function.Call(Hash.REMOVE_ALL_PED_WEAPONS, val2.Handle, true);
            }
        }
	}

	private void CleanEmptyVehicles()
	{
		Vehicle[] nearbyVehicles = World.GetNearbyVehicles(((Entity)Game.Player.Character).Position, 500f, Array.Empty<Model>());
		Vehicle[] array = nearbyVehicles;
		foreach (Vehicle val in array)
		{
			if ((Entity)(object)val != (Entity)null && val.Exists() && !((Entity)val).IsDead && val.PassengerCount == 0 && (Entity)(object)val.Driver == (Entity)null && !emptyVehicles.ContainsKey(val))
			{
				emptyVehicles[val] = DateTime.Now;
			}
		}
		List<Vehicle> list = (from entry in emptyVehicles
			where (DateTime.Now - entry.Value).TotalMilliseconds >= 25000.0
			select entry.Key).ToList();
		foreach (Vehicle item in list)
		{
			((PoolObject)item).Delete();
			emptyVehicles.Remove(item);
		}
	}
}
