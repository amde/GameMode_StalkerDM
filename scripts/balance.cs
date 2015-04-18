///////////////////////
//STALKER DM: BALANCE//
///////////////////////

//////////////
// CONTENTS //
//////////////
//#1. ADJUSTMENTS
//	#1.1 StalkerDM_Balance function

//	#1.1
function StalkerDM_Balance()
{
	L4BKatanaImage.raycastDirectDamage = GlobalStorage.KatanaDamage;
	PistolItem.canReload = false;
	PistolItem.maxAmmo = GlobalStorage.PistolAmmo;
	PistolItem.raycastDirectDamage = GlobalStorage.PistolDamage;
	PumpShotgunItem.canReload = false;
	PumpShotgunItem.maxAmmo = GlobalStorage.ShotgunAmmo;
	PumpShotgunProjectile.directDamage = GlobalStorage.ShotgunDamage;
	TAssaultRifleItem.canReload = false;
	TAssaultRifleItem.maxAmmo = GlobalStorage.ARammo;
	TAssaultRifleProjectile1.directDamage = GlobalStorage.ARsprayDamage;
	TAssaultRifleProjectile2.directDamage = GlobalStorage.ARsnipeDamage;
}
StalkerDM_Balance();