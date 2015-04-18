//////////////////////
//STALKER DM: KNIVES//
//////////////////////

//////////////
// CONTENTS //
//////////////
//#1. DAMAGE TYPES
//	#1.1 Stalker Knife
//	#1.3 Momentum Knife
//	#1.4 Serrated Knife
//	#1.5 Bloodlust Knife
//	#1.6 Shadow Knife
//	#1.7 Vicious Knife
//#2. PARTICLES AND EMITTERS (removed)
//#3. ITEM DATABLOCKS
//	#3.1 StalkerKnifeItem
//	#3.3 StalkerMomentumKnifeItem
//	#3.4 StalkerSerratedKnifeItem
//	#3.5 StalkerBloodlustKnifeItem
//	#3.6 StalkerShadowKnifeItem
//	#3.7 v2 ViciousKnifeItem
//#4. IMAGE DATABLOCKS
//	#4.1 StalkerKnifeImage
//	#4.3 StalkerMomentumKnifeImage
//	#4.4 StalkerSerratedKnifeImage
//	#4.5 StalkerBloodlustKnifeImage
//	#4.6 StalkerShadowKnifeImage
//	#4.7 v2 ViciousKnifeImage
//#5. FUNCTIONALITY
//	#5.0 Conal Raycasting
//	#5.1 Stalker Knife
//	#5.3 Momentum Knife
//	#5.4 Serrated Knife
//	#5.5 Bloodlust Knife
//	#5.6 Shadow Knife
//	#5.7 v2 Vicious Knife

//#1.
//	#1.1
AddDamageType("StalkerKnife", '<bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_StalkerKnife> %1', '%2 <bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_StalkerKnife> %1',0.2,1);
AddDamageType("StalkerKnifeBackstab", '<bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_StalkerKnife> <bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_Backstab> %1', '%2 <bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_StalkerKnife> <bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_Backstab> %1',0.2,1);
//	#1.3
AddDamageType("StalkerMomentumKnife", '<bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_MomentumKnife> %1', '%2 <bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_MomentumKnife> %1',0.2,1);
AddDamageType("StalkerMomentumKnifeBackstab", '<bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_MomentumKnife>  <bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_Backstab> %1', '%2 <bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_MomentumKnife> <bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_Backstab> %1',0.2,1);
//	#1.4
AddDamageType("StalkerSerratedKnife", '<bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_Serratedknife> %1', '%2 <bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_Serratedknife> %1',0.2,1);
AddDamageType("StalkerSerratedKnifeBackstab", '<bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_Serratedknife> <bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_Backstab> %1', '%2 <bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_Serratedknife> <bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_Backstab> %1',0.2,1);
AddDamageType("Bleed", '<bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_Bleed> %1', '%2 <bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_Bleed> %1',0.2,1);
//	#1.5
AddDamageType("StalkerBloodlustKnife", '<bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_BloodlustKnife> %1', '%2 <bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_BloodlustKnife> %1',0.2,1);
AddDamageType("StalkerBloodlustKnifeBackstab", '<bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_BloodlustKnife> <bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_Backstab> %1', '%2 <bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_BloodlustKnife> <bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_Backstab> %1',0.2,1);
//	#1.6
AddDamageType("StalkerShadowKnife", '<bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_ShadowKnife> %1', '%2 <bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_ShadowKnife> %1',0.2,1);
AddDamageType("StalkerShadowKnifeBackstab", '<bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_ShadowKnife> <bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_Backstab> %1', '%2 <bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_ShadowKnife> <bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_Backstab> %1',0.2,1);
//	#1.7
//AddDamageType("ViciousKnife", '<bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_ShadowKnife> %1', '%2 <bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_ShadowKnife> %1',0.2,1);


//#3.
//	#3.1
datablock itemData(StalkerKnifeItem : swordItem)
{
	shapeFile = $StalkerDM::Path @ "/models/stalker_knife.dts";
	image = StalkerKnifeImage;
	uiName = "Stalker Knife";
	iconName = $StalkerDM::Path @ "/icons/icon_Knife";
	doColorShift = false;
	slot = "Knife";
};
//	#3.3
datablock itemData(StalkerMomentumKnifeItem : swordItem)
{
	shapeFile = $StalkerDM::Path @ "/models/slider_knife.dts";
	image = StalkerMomentumKnifeImage;
	uiName = "Momentum Knife";
	iconName = $StalkerDM::Path @ "/icons/icon_MomentumKnife";
	doColorShift = false;
	slot = "Knife";
};
//	#3.4
datablock itemData(StalkerSerratedKnifeItem : swordItem)
{
	shapeFile = $StalkerDM::Path @ "/models/serrated_knife.dts";
	image = StalkerSerratedKnifeImage;
	uiName = "Serrated Knife";
	iconName = $StalkerDM::Path @ "/icons/icon_SerratedKnife";
	doColorShift = false;
	slot = "Knife";
};
//	#3.5
datablock itemData(StalkerBloodlustKnifeItem : swordItem)
{
	shapeFile = $StalkerDM::Path @ "/models/wavy_knife.dts";
	image = StalkerBloodlustKnifeImage;
	uiName = "Bloodlust Knife";
	iconName = $StalkerDM::Path @ "/icons/icon_BloodlustKnife";
	doColorShift = false;
	slot = "Knife";
};
//	#3.6
datablock itemData(StalkerShadowKnifeItem : swordItem)
{
	shapeFile = $StalkerDM::Path @ "/models/shadow_knife.dts";
	image = StalkerShadowKnifeImage;
	uiName = "Shadow Knife";
	iconName = $StalkerDM::Path @ "/icons/icon_ShadowKnife";
	doColorShift = false;
	slot = "Knife";
};
//	#3.7
datablock itemData(ViciousKnifeItem : swordItem)
{
	shapeFile = $StalkerDM::Path @ "/models/stalker_knife.dts";
	image = StalkerShadowKnifeImage;
	uiName = "Vicious Knife";
	iconName = $StalkerDM::Path @ "/icons/icon_Knife";
	doColorShift = false;
	slot = "Knife";
};

//#4.
//	#4.1
datablock shapeBaseImageData(StalkerKnifeImage)
{
	shapeFile = StalkerKnifeItem.shapeFile;
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0;

	correctMuzzleVector = false;

	className = "WeaponImage";

	item = StalkerKnifeItem;
	projectile = "";
	projectileType = Projectile;

	melee = true;
	doRetraction = false;
	armReady = true;

	doColorShift = false;
	colorShiftColor = "0.5 0.5 0.5 1";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.2;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "PreFire";
	stateScript[1] = "onReady";

	stateName[2] = "PreFire";
	stateScript[2] = "onPreFire";
	stateAllowImageChange[2] = false;
	stateTimeoutValue[2] = 0.1;
	stateWaitForTimeout[2] = true;
	stateTransitionOnTimeout[2] = "Fire";

	stateName[3] = "Fire";
	stateTimeoutValue[3]  = 0.4;
	stateWaitForTimeout[3] = true;
	stateTransitionOnTriggerUp[3] = "Ready";
	stateFire[3] = true;
	stateScript[3] = "onFire";
	stateAllowImageChange[3] = false;
};

//	#4.3
datablock shapeBaseImageData(StalkerMomentumKnifeImage)
{
	shapeFile = StalkerMomentumKnifeItem.shapeFile;
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0;

	correctMuzzleVector = false;

	className = "WeaponImage";

	item = StalkerMomentumKnifeItem;
	projectile = "";
	projectileType = Projectile;

	melee = true;
	doRetraction = false;
	armReady = true;
	breaksCloak = true;

	doColorShift = false;
	colorShiftColor = "0.75 0 0 1";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.2;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1] = "Ready";
	stateTimeoutValue[1] = 0.05;
	stateWaitForTimeout[1] = true;
	stateTransitionOnTriggerDown[1] = "PreFire";
	stateScript[1] = "onReady";
	stateAllowImageChange[1] = true;

	stateName[2] = "PreFire";
	stateScript[2] = "onPreFire";
	stateAllowImageChange[2] = false;
	stateTimeoutValue[2] = 0.1;
	stateWaitForTimeout[2] = true;
	stateTransitionOnTimeout[2]  = "Fire";

	stateName[3] = "Fire";
	stateTimeoutValue[3]  = 0.4;
	stateWaitForTimeout[3] = true;
	stateTransitionOnTriggerUp[3] = "Ready";
	stateFire[3] = true;
	stateAllowImageChange[3] = false;
	stateScript[3] = "onFire";
};

//	#4.4
datablock shapeBaseImageData(StalkerSerratedKnifeImage)
{
	shapeFile = StalkerSerratedKnifeItem.shapeFile;
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0;

	correctMuzzleVector = false;

	className = "WeaponImage";

	item = StalkerSerratedKnifeItem;
	projectile = "";
	projectileType = Projectile;

	melee = true;
	doRetraction = false;
	armReady = true;

	doColorShift = false;
	colorShiftColor = "0.5 0.5 0.5 1";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.2;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1] = "Ready";
	stateTimeoutValue[1] = 0.05;
	stateWaitForTimeout[1] = true;
	stateTransitionOnTriggerDown[1] = "PreFire";
	stateScript[1] = "onReady";
	stateAllowImageChange[1] = true;

	stateName[2] = "PreFire";
	stateScript[2] = "onPreFire";
	stateAllowImageChange[2] = false;
	stateTimeoutValue[2] = 0.1;
	stateWaitForTimeout[2] = true;
	stateTransitionOnTimeout[2]  = "Fire";

	stateName[3] = "Fire";
	stateTimeoutValue[3]  = 0.15;
	stateWaitForTimeout[3] = true;
	stateTransitionOnTriggerUp[3] = "Ready";
	stateFire[3] = true;
	stateAllowImageChange[3] = false;
	stateScript[3] = "onFire";
};

//	#4.5
datablock shapeBaseImageData(StalkerBloodlustKnifeImage)
{
	shapeFile = StalkerBloodlustKnifeItem.shapeFile;
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0;

	correctMuzzleVector = false;

	className = "WeaponImage";

	item = StalkerBloodlustKnifeItem;
	projectile = "";
	projectileType = Projectile;

	melee = true;
	doRetraction = false;
	armReady = true;

	doColorShift = false;
	colorShiftColor = "0.75 0 0 1";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.2;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1] = "Ready";
	stateTimeoutValue[1] = 0.05;
	stateWaitForTimeout[1] = true;
	stateTransitionOnTriggerDown[1] = "PreFire";
	stateScript[1] = "onReady";
	stateAllowImageChange[1] = true;

	stateName[2] = "PreFire";
	stateScript[2] = "onPreFire";
	stateAllowImageChange[2] = false;
	stateTimeoutValue[2] = 0.1;
	stateWaitForTimeout[2] = true;
	stateTransitionOnTimeout[2]  = "Fire";

	stateName[3] = "Fire";
	stateTimeoutValue[3]  = 0.25;
	stateWaitForTimeout[3] = true;
	stateTransitionOnTriggerUp[3] = "Ready";
	stateFire[3] = true;
	stateAllowImageChange[3] = false;
	stateScript[3] = "onFire";
};

//	#4.6
datablock shapeBaseImageData(StalkerShadowKnifeImage)
{
	shapeFile = StalkerShadowKnifeItem.shapeFile;
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0;

	correctMuzzleVector = false;

	className = "WeaponImage";

	item = StalkerShadowKnifeItem;
	projectile = "";
	projectileType = Projectile;

	melee = true;
	doRetraction = false;
	armReady = true;
	breaksCloak = false;

	doColorShift = false;
	colorShiftColor = "0 0 0 1";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.2;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1] = "Ready";
	stateTimeoutValue[1] = 0.05;
	stateWaitForTimeout[1] = true;
	stateTransitionOnTriggerDown[1] = "PreFire";
	stateScript[1] = "onReady";
	stateAllowImageChange[1] = true;

	stateName[2] = "PreFire";
	stateScript[2] = "onPreFire";
	stateAllowImageChange[2] = false;
	stateTimeoutValue[2] = 0.1;
	stateWaitForTimeout[2] = true;
	stateTransitionOnTimeout[2]  = "Fire";

	stateName[3] = "Fire";
	stateTimeoutValue[3]  = 0.3;
	stateWaitForTimeout[3] = true;
	stateTransitionOnTriggerUp[3] = "Ready";
	stateFire[3] = true;
	stateAllowImageChange[3] = false;
	stateScript[3] = "onFire";
};

//	#4.7
datablock shapeBaseImageData(ViciousKnifeImage)
{
	shapeFile = ViciousKnifeItem.shapeFile;
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0;

	correctMuzzleVector = false;

	className = "WeaponImage";

	item = ViciousKnifeItem;
	projectile = "";
	projectileType = Projectile;

	melee = true;
	doRetraction = false;
	armReady = true;

	doColorShift = false;
	colorShiftColor = "0.5 0.5 0.5 1";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.2;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "PreFire";
	stateScript[1] = "onReady";

	stateName[2] = "PreFire";
	stateScript[2] = "onPreFire";
	stateAllowImageChange[2] = false;
	stateTimeoutValue[2] = 0.1;
	stateTransitionOnTimeout[2] = "Fire";

	stateName[3] = "Fire";
	stateTimeoutValue[3]  = 0.4;
	stateWaitForTimeout[3] = true;
	stateTransitionOnTriggerUp[3] = "Ready";
	stateFire[3] = true;
	stateScript[3] = "onFire";
	stateAllowImageChange[3] = false;
};

//#5.
//	#5.0
function conalRaycast(%center, %forwardVector, %radius, %angle, %typemasks, %exclude)
{
	InitContainerRadiusSearch(%center, %radius, %typemasks);
	while(%hit = containerSearchNext())
	{
		if(%hit == %exclude)
		{
			continue;
		}
		%vec1 = vectorNormalize(VectorSub(strPos(%hit.getClassName(), "Player") != -1 ? %hit.getHackPosition() : %hit.getPosition(), %center));
		%vec2 = %forwardVector;
		%ang1 = getVectorAngle(%vec2, %vec1);
		if(%ang1 <= %angle)
		{
			if(strPos(%hit.getClassName(), "Player") != -1)
			{
				%d = ((vectorDist(%center, %hit.getPosition()) + vectorDist(%center, %hit.getEyePoint())) / 2) - 1.1 * getWord(%hit.getScale(), 2);
				if(%d - 2 > %radius)
				{
					continue;
				}
			}
			break;
		}
	}
	if(isObject(%hit))
	{
		%raycast = containerRaycast(%center, strPos(%hit.getClassName(), "Player") != -1 ? %hit.getHackPosition() : %hit.getPosition(), %typemasks, %exclude);
	}
	return setWord(%raycast, 0, %hit);
}
function conalRaycastM(%center, %forwardVector, %radius, %angle, %typemasks, %exclude)
{
	InitContainerRadiusSearch(%center, %radius, %typemasks);
	while(%hit = containerSearchNext())
	{
		if(%hit == %exclude)
		{
			continue;
		}
		%vec1 = vectorNormalize(VectorSub(strPos(%hit.getClassName(), "Player") != -1 ? %hit.getHackPosition() : %hit.getPosition(), %center));
		%vec2 = %forwardVector;
		%ang1 = getVectorAngle(%vec2, %vec1);
		if(%ang1 <= %angle)
		{
			if(strPos(%hit.getClassName(), "Player") != -1)
			{
				%d = ((vectorDist(%center, %hit.getPosition()) + vectorDist(%center, %hit.getEyePoint())) / 2) - 1.1 * getWord(%hit.getScale(), 2);
				if(%d - 2 > %radius)
				{
					continue;
				}
			}
			%list = %list SPC %hit;
		}
	}
	return trim(%list);
}

//	#5.1
function StalkerKnifeImage::onReady(%this, %obj, %slot)
{
	%obj.playThread(2, "root");
}
function StalkerKnifeImage::onPreFire(%this, %obj, %slot)
{
	%obj.playThread(2, "shiftUp");
}
function StalkerKnifeImage::onStopFire(%this, %obj, %slot)
{
}
function StalkerKnifeImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(2, "shiftDown");
	%scale = getWord(%obj.getScale(), 2);
	%start = %obj.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%obj.getMuzzleVector(%slot), GlobalStorage.KnifeRange * %scale));
	%typemasks = $Typemasks::PlayerObjectType | $Typemasks::VehicleObjectType | $Typemasks::TerrainObjectType | $Typemasks::InteriorObjectType | $Typemasks::FXbrickObjectType | $TypeMasks::StaticShapeObjectType;
	%raycast = conalRaycast(%start, %obj.getEyeVector(), GlobalStorage.KnifeRange * %scale, GlobalStorage.KnifeSection, $Typemasks::PlayerObjectType, %obj);
	if(!isObject(%col = firstWord(%raycast)))
	{
		%raycast = containerRaycast(%start, %end, %typemasks, %obj);
		%col = firstWord(%raycast);
	}
	if(!isObject(%col))
	{
		if(isObject(KnifeFireSound))
		{
			ServerPlay3D(KnifeFireSound, %obj.getMuzzlePoint(%slot));
		}
		return;
	}
	%pos = posFromRaycast(%raycast);
	%normal = normalFromRaycast(%raycast);
	%p = new Projectile()
	{
		datablock = swordProjectile;
		initialPosition = %pos;
		initialVelocity = %normal;
		sourceObject = 0;
		sourceSlot = 0;
		scale = %scale SPC %scale SPC %scale;
		client = 0;
	};
	%p.explode();
	if(%col.getType() & $Typemasks::PlayerObjectType && minigameCanDamage(%obj, %col) == 1)
	{
		if(isObject(l4bMacheteHitSoundA))
		{
			%r = mFloor(getRandom() + 0.5);
			ServerPlay3D("l4bMacheteHitSound" @ getSubStr("ab", %r, 1), %pos);
		}
		if(vectorDot(%obj.getForwardVector(), %col.getForwardVector()) > 0.25)
		{
			%obj.playThread(2, spearThrow);
			%col.spawnExplosion(critProjectile,%scale);
			if(isObject(%col.client))
			{
				%col.client.play2d(critRecieveSound);
			}
			if(%obj.lastCloakTime + 1500 >= getSimTime())
			{
				%col.damage(%obj, %pos, GlobalStorage.KnifeBackstab, $DamageType::StalkerKnifeBackstab);
				if(%obj.subtleStabs++ >= 3)
				{
					%obj.client.unlockAchievement("Master of Subtlety");
				}
			}
			else
			{
				%col.damage(%obj, %pos, GlobalStorage.KnifeFacestab * GlobalStorage.StalkerCritScale, $DamageType::StalkerKnife);
			}
		}
		else
		{
			%col.damage(%obj, %pos, GlobalStorage.KnifeFacestab, $DamageType::StalkerKnife);
		}
	}
}

//	#5.3
function StalkerMomentumKnifeImage::onReady(%this, %obj, %slot)
{
	%obj.playThread(2, "root");
}
function StalkerMomentumKnifeImage::onPreFire(%this, %obj, %slot)
{
	%obj.playThread(2, "shiftUp");
}
function StalkerMomentumKnifeImage::onStopFire(%this, %obj, %slot)
{
}
function StalkerMomentumKnifeImage::onFire(%this, %obj, %slot)
{
	%obj.partialCloak(1000);
	%obj.playThread(2, "shiftDown");
	%scale = getWord(%obj.getScale(), 2);
	%start = %obj.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%obj.getMuzzleVector(%slot), GlobalStorage.KnifeRange * %scale));
	%typemasks = $Typemasks::PlayerObjectType | $Typemasks::VehicleObjectType | $Typemasks::TerrainObjectType | $Typemasks::InteriorObjectType | $Typemasks::FXbrickObjectType | $TypeMasks::StaticShapeObjectType;
	%raycast = conalRaycast(%start, %obj.getEyeVector(), GlobalStorage.KnifeRange * %scale, GlobalStorage.MomentumKnifeSection, $Typemasks::PlayerObjectType, %obj);
	if(!isObject(%col = firstWord(%raycast)))
	{
		%raycast = containerRaycast(%start, %end, %typemasks, %obj);
		%col = firstWord(%raycast);
	}
	if(!isObject(%col))
	{
		if(isObject(KnifeFireSound))
		{
			ServerPlay3D(KnifeFireSound, %obj.getMuzzlePoint(%slot));
		}
		return;
	}
	%pos = posFromRaycast(%raycast);
	%normal = normalFromRaycast(%raycast);
	%p = new Projectile()
	{
		datablock = swordProjectile;
		initialPosition = %pos;
		initialVelocity = %normal;
		sourceObject = 0;
		sourceSlot = 0;
		scale = %scale SPC %scale SPC %scale;
		client = 0;
	};
	%p.explode();
	if(%col.getType() & $Typemasks::PlayerObjectType && minigameCanDamage(%obj, %col) == 1)
	{
		%damage = GlobalStorage.MomentumKnifeDamage + vectorLen(vectorAdd(%obj.getVelocity(), %col.getVelocity())) * GlobalStorage.MomentumKnifeScale;
		if(%damage >= 50)
		{
			%obj.client.unlockAchievement("Master of Speed");
		}
		if($svhdebug)
		{
			announce(%damage);
		}
		if(isObject(l4bMacheteHitSoundA))
		{
			%r = mFloor(getRandom() + 0.5);
			ServerPlay3D("l4bMacheteHitSound" @ getSubStr("ab", %r, 1), %pos);
		}
		if(vectorDot(%obj.getForwardVector(), %col.getForwardVector()) > 0 && GlobalStorage.MomentumKnifeBackstabs)
		{
			%obj.playThread(2, spearThrow);
			%col.spawnExplosion(critProjectile,%scale);
			if(isObject(%col.client))
			{
				%col.client.play2d(critRecieveSound);
			}
			%col.damage(%obj, %pos, %damage * GlobalStorage.StalkerCritScale, $DamageType::StalkerMomentumKnife);
		}
		else
		{
			%col.damage(%obj, %pos, %damage, $DamageType::StalkerMomentumKnife);
		}
	}
}


//	#5.4
function StalkerSerratedKnifeImage::onReady(%this, %obj, %slot)
{
	%obj.playThread(2, "root");
}
function StalkerSerratedKnifeImage::onPreFire(%this, %obj, %slot)
{
	%obj.playThread(2, "shiftUp");
}
function StalkerSerratedKnifeImage::onStopFire(%this, %obj, %slot)
{
}
function StalkerSerratedKnifeImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(2, "shiftDown");
	%scale = getWord(%obj.getScale(), 2);
	%start = %obj.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%obj.getMuzzleVector(%slot), GlobalStorage.KnifeRange * %scale));
	%typemasks = $Typemasks::PlayerObjectType | $Typemasks::VehicleObjectType | $Typemasks::TerrainObjectType | $Typemasks::InteriorObjectType | $Typemasks::FXbrickObjectType | $TypeMasks::StaticShapeObjectType;
	%raycast = conalRaycast(%start, %obj.getEyeVector(), GlobalStorage.KnifeRange * %scale, GlobalStorage.KnifeSection, $Typemasks::PlayerObjectType, %obj);
	if(!isObject(%col = firstWord(%raycast)))
	{
		%raycast = containerRaycast(%start, %end, %typemasks, %obj);
		%col = firstWord(%raycast);
	}
	if(!isObject(%col))
	{
		if(isObject(KnifeFireSound))
		{
			ServerPlay3D(KnifeFireSound, %obj.getMuzzlePoint(%slot));
		}
		return;
	}
	%pos = posFromRaycast(%raycast);
	%normal = normalFromRaycast(%raycast);
	%p = new Projectile()
	{
		datablock = swordProjectile;
		initialPosition = %pos;
		initialVelocity = %normal;
		sourceObject = 0;
		sourceSlot = 0;
		scale = %scale SPC %scale SPC %scale;
		client = 0;
	};
	%p.explode();
	if(%col.getType() & $Typemasks::PlayerObjectType && minigameCanDamage(%obj, %col) == 1)
	{
		if(isObject(l4bMacheteHitSoundA))
		{
			%r = mFloor(getRandom() + 0.5);
			ServerPlay3D("l4bMacheteHitSound" @ getSubStr("ab", %r, 1), %pos);
		}
		if(vectorDot(%obj.getForwardVector(), %col.getForwardVector()) > 0.25)
		{
			%col.spawnExplosion(critProjectile,%scale);
			if(isObject(%col.client))
			{
				%col.client.play2d(critRecieveSound);
			}
			%col.damage(%obj, %pos, GlobalStorage.SerrKnifeDamage * GlobalStorage.StalkerCritScale, $DamageType::StalkerSerratedKnifeBackstab);
		}
		else
		{
			%col.damage(%obj, %pos, GlobalStorage.SerrKnifeDamage, $DamageType::StalkerSerratedKnife);
		}
		%col.schedule(1000, DoBleedDamage, %obj, 5);
	}
}
function Player::DoBleedDamage(%this, %sObj, %amt)
{
	%this.damage(%sObj, %this.getPosition(), GlobalStorage.SerrKnifeBleed, $DamageType::Bleed);
	if(%amt-- > 0)
	{
		%this.schedule(3000, DoBleedDamage, %sObj, %amt);
	}
	if(%this.getDamagePercent() >= 1)
	{
		if(isObject(%sObj.client))
		{
			%sObj.client.unlockAchievement("Master of Slicing");
		}
	}
}

//	#5.5
function StalkerBloodlustKnifeImage::onReady(%this, %obj, %slot)
{
	%obj.playThread(2, "root");
}
function StalkerBloodlustKnifeImage::onPreFire(%this, %obj, %slot)
{
	%obj.playThread(2, "shiftUp");
}
function StalkerBloodlustKnifeImage::onStopFire(%this, %obj, %slot)
{
}
function StalkerBloodlustKnifeImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(2, "shiftDown");
	%scale = getWord(%obj.getScale(), 2);
	%start = %obj.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%obj.getMuzzleVector(%slot), GlobalStorage.KnifeRange * %scale));
	%typemasks = $Typemasks::PlayerObjectType | $Typemasks::VehicleObjectType | $Typemasks::TerrainObjectType | $Typemasks::InteriorObjectType | $Typemasks::FXbrickObjectType | $TypeMasks::StaticShapeObjectType;
	%raycast = conalRaycast(%start, %obj.getEyeVector(), GlobalStorage.KnifeRange * %scale, GlobalStorage.KnifeSection, $Typemasks::PlayerObjectType, %obj);
	if(!isObject(%col = firstWord(%raycast)))
	{
		%raycast = containerRaycast(%start, %end, %typemasks, %obj);
		%col = firstWord(%raycast);
	}
	if(!isObject(%col))
	{
		if(isObject(KnifeFireSound))
		{
			ServerPlay3D(KnifeFireSound, %obj.getMuzzlePoint(%slot));
		}
		return;
	}
	%pos = posFromRaycast(%raycast);
	%normal = normalFromRaycast(%raycast);
	%p = new Projectile()
	{
		datablock = swordProjectile;
		initialPosition = %pos;
		initialVelocity = %normal;
		sourceObject = 0;
		sourceSlot = 0;
		scale = %scale SPC %scale SPC %scale;
		client = 0;
	};
	%p.explode();
	if(%col.getType() & $Typemasks::PlayerObjectType && minigameCanDamage(%obj, %col) == 1)
	{
		if(isObject(l4bMacheteHitSoundA))
		{
			%r = mFloor(getRandom() + 0.5);
			ServerPlay3D("l4bMacheteHitSound" @ getSubStr("ab", %r, 1), %pos);
		}
		if(vectorDot(%obj.getForwardVector(), %col.getForwardVector()) > 0.25)
		{
			%col.spawnExplosion(critProjectile,%scale);
			if(isObject(%col.client))
			{
				%col.client.play2d(critRecieveSound);
			}
			%col.damage(%obj, %pos, (GlobalStorage.KnifeFacestab + (GlobalStorage.LustEfficiency * %obj.BLKdamage)) * GlobalStorage.StalkerCritScale, $DamageType::StalkerBloodlustKnifeBackstab);
			%obj.BLKdamage+= GlobalStorage.KnifeFacestab * GlobalStorage.StalkerCritScale + (GlobalStorage.LustEfficiency * %obj.BLKdamage);
		}
		else
		{
			%col.damage(%obj, %pos, GlobalStorage.KnifeFacestab + (GlobalStorage.LustEfficiency * %obj.BLKdamage), $DamageType::StalkerBloodlustKnife);
			%obj.BLKdamage+= GlobalStorage.KnifeFacestab + (GlobalStorage.LustEfficiency * %obj.BLKdamage);
		}
		if((GlobalStorage.KnifeFacestab + (%obj.BLKdamage * GlobalStorage.LustEfficiency)) >= 100)
		{
			%obj.client.unlockAchievement("Master of Blood");
		}
	}
}

//	#5.6
function StalkerShadowKnifeImage::onReady(%this, %obj, %slot)
{
	%obj.playThread(2, "root");
}
function StalkerShadowKnifeImage::onPreFire(%this, %obj, %slot)
{
	%obj.playThread(2, "shiftUp");
}
function StalkerShadowKnifeImage::onStopFire(%this, %obj, %slot)
{
}
function StalkerShadowKnifeImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(2, "shiftDown");
	%scale = getWord(%obj.getScale(), 2);
	%start = %obj.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%obj.getMuzzleVector(%slot), GlobalStorage.KnifeRange * %scale));
	%typemasks = $Typemasks::PlayerObjectType | $Typemasks::VehicleObjectType | $Typemasks::TerrainObjectType | $Typemasks::InteriorObjectType | $Typemasks::FXbrickObjectType | $TypeMasks::StaticShapeObjectType;
	%raycast = conalRaycast(%start, %obj.getEyeVector(), GlobalStorage.KnifeRange * %scale, GlobalStorage.KnifeSection, $Typemasks::PlayerObjectType, %obj);
	if(!isObject(%col = firstWord(%raycast)))
	{
		%raycast = containerRaycast(%start, %end, %typemasks, %obj);
		%col = firstWord(%raycast);
	}
	if(!isObject(%col))
	{
		if(isObject(KnifeFireSound))
		{
			ServerPlay3D(KnifeFireSound, %obj.getMuzzlePoint(%slot));
		}
		return;
	}
	%pos = posFromRaycast(%raycast);
	%normal = normalFromRaycast(%raycast);
	%p = new Projectile()
	{
		datablock = swordProjectile;
		initialPosition = %pos;
		initialVelocity = %normal;
		sourceObject = 0;
		sourceSlot = 0;
		scale = %scale SPC %scale SPC %scale;
		client = 0;
	};
	%p.explode();
	if(%col.getType() & $Typemasks::PlayerObjectType && minigameCanDamage(%obj, %col))
	{
		if(isObject(l4bMacheteHitSoundA))
		{
			%r = mFloor(getRandom() + 0.5);
			ServerPlay3D("l4bMacheteHitSound" @ getSubStr("ab", %r, 1), %pos);
		}
		if(vectorDot(%obj.getForwardVector(), %col.getForwardVector()) > 0.25)
		{
			%col.spawnExplosion(critProjectile,%scale);
			if(isObject(%col.client))
			{
				%col.client.play2d(critRecieveSound);
			}
			%col.damage(%obj, %pos, GlobalStorage.ShadKnifeDamage * GlobalStorage.StalkerCritScale, $DamageType::StalkerShadowKnifeBackstab);
		}
		else
		{
			%col.damage(%obj, %pos, GlobalStorage.ShadKnifeDamage, $DamageType::StalkerShadowKnife);
		}
		if(%obj.isCloaked())
		{
			%col.mountImage(DarkBlindPlayerImage, 1);
			%col.schedule(GlobalStorage.ShadKnifeBlind, unmountImage, 1);
			if(%col.getDamagePercent() >= 1)
			{
				%obj.client.unlockAchievement("Master of Shadow");
			}
		}
	}
}

//	#5.7
function ViciousKnifeImage::onReady(%this, %obj, %slot)
{
	%obj.playThread(2, "root");
}
function ViciousKnifeImage::onPreFire(%this, %obj, %slot)
{
	%obj.playThread(2, "shiftUp");
}
function ViciousKnifeImage::onStopFire(%this, %obj, %slot)
{
}
function ViciousKnifeImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(2, "shiftDown");
	%scale = getWord(%obj.getScale(), 2);
	%start = %obj.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%obj.getMuzzleVector(%slot), GlobalStorage.KnifeRange * %scale));
	%typemasks = $Typemasks::PlayerObjectType | $Typemasks::VehicleObjectType | $Typemasks::TerrainObjectType | $Typemasks::InteriorObjectType | $Typemasks::FXbrickObjectType | $TypeMasks::StaticShapeObjectType;
	%raycast = conalRaycast(%start, %obj.getEyeVector(), GlobalStorage.KnifeRange * %scale, GlobalStorage.KnifeSection, $Typemasks::PlayerObjectType, %obj);
	if(!isObject(%col = firstWord(%raycast)))
	{
		%raycast = containerRaycast(%start, %end, %typemasks, %obj);
		%col = firstWord(%raycast);
	}
	if(!isObject(%col))
	{
		if(isObject(KnifeFireSound))
		{
			ServerPlay3D(KnifeFireSound, %obj.getMuzzlePoint(%slot));
		}
		return;
	}
	%pos = posFromRaycast(%raycast);
	%normal = normalFromRaycast(%raycast);
	%p = new Projectile()
	{
		datablock = swordProjectile;
		initialPosition = %pos;
		initialVelocity = %normal;
		sourceObject = 0;
		sourceSlot = 0;
		scale = %scale SPC %scale SPC %scale;
		client = 0;
	};
	%p.explode();
	if(%col.getType() & $Typemasks::PlayerObjectType && minigameCanDamage(%obj, %col))
	{
		if(isObject(l4bMacheteHitSoundA))
		{
			%r = mFloor(getRandom() + 0.5);
			ServerPlay3D("l4bMacheteHitSound" @ getSubStr("ab", %r, 1), %pos);
		}
		if(vectorDot(%obj.getForwardVector(), %col.getForwardVector()) > 0.25)
		{
			%obj.playThread(2, spearThrow);
			%col.spawnExplosion(critProjectile,%scale);
			if(isObject(%col.client))
			{
				%col.client.play2d(critRecieveSound);
			}
			%col.damage(%obj, %pos, GlobalStorage.KnifeFacestab * GlobalStorage.StalkerCritScale, $DamageType::ViciousKnife);
		}
		else
		{
			%col.damage(%obj, %pos, GlobalStorage.KnifeFacestab, $DamageType::ViciousKnife);
		}
	}
}