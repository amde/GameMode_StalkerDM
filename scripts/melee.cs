/////////////////////
//STALKER DM: MELEE//
/////////////////////

////////////
//CONTENTS//
////////////
//#1. Melee Weapons
//	#1.1 Katana (modifications)
//	#1.2 Taser
//	#1.3 Repulsor
//	#1.4 Riot Shield
//	#1.5 Sledgehammer (modifications)

//	#1.1
L4BKatanaItem.slot = "Melee";
function L4BKatanaImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(2,shiftto);

	if(getRandom(0,1))
	{
		%this.raycastExplosionBrickSound = L4BMacheteHitSoundA;
		%this.raycastExplosionPlayerSound = L4BMacheteHitSoundB;
	}
	else
	{
		%this.raycastExplosionBrickSound = L4BMacheteHitSoundB;
		%this.raycastExplosionPlayerSound = L4BMacheteHitSoundB;
	}

	%scale = getWord(%obj.getScale(), 2);
	%start = %obj.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%obj.getMuzzleVector(%slot), GlobalStorage.KatanaRange * %scale));
	%typemasks = $Typemasks::PlayerObjectType | $Typemasks::VehicleObjectType | $Typemasks::TerrainObjectType | $Typemasks::InteriorObjectType | $Typemasks::FXbrickObjectType | $TypeMasks::StaticShapeObjectType;
	%raycast = conalRaycast(%start, %obj.getEyeVector(), GlobalStorage.KatanaRange * %scale, GlobalStorage.KatanaSection, $Typemasks::PlayerObjectType, %obj);
	if(!isObject(%col = firstWord(%raycast)))
	{
		%raycast = containerRaycast(%start, %end, %typemasks, %obj);
		%col = firstWord(%raycast);
	}
	if(!isObject(%col))
	{
		return;
	}
	%pos = posFromRaycast(%raycast);
	%normal = normalFromRaycast(%raycast);
	if(isObject(L4BMacheteHitSoundA))
	{
		ServerPlay3D(%this.raycastExplosionBrickSound, %pos);
	}
	%p = new Projectile()
	{
		datablock = hammerProjectile;
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
		%col.damage(%obj, %pos, GlobalStorage.KatanaDamage, $DamageType::L4BKatana);
	}
}

//	#1.2
datablock itemData(TaserItem : DarkMItem)
{
	shapeFile = $StalkerDM::Path @ "/models/Taser.dts";
	iconName = $StalkerDM::Path @ "/icons/icon_Taser";
	image = TaserImage;
	uiName = "Taser";
	slot = "Melee";
};

datablock ShapeBaseImageData(TaserImage)
{
	shapeFile = $StalkerDM::Path @ "/models/Taser.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0;
	rotation = "0 0 0";

	correctMuzzleVector = true;

	className = "WeaponImage";
	item = TaserItem;
	ammo = " ";
	//projectile = " ";
	//projectileType = Projectile;

	melee = false;
	armReady = true;

	stateName[0]                   = "Activate";
	stateTimeoutValue[0]           = 0.15;
	stateTransitionOnTimeout[0]    = "Ready";

	stateName[1]                   = "Ready";
	stateTransitionOnTriggerDown[1]= "SetAmmo";
	stateAllowImageChange[1]       = true;
	stateSequence[1]               = "Ready";
	stateScript[1]                 = "onReady";

	stateName[4]                   = "SetAmmo";
	stateScript[4]                 = "SetAmmo";
	stateTransitionOnTimeout[4]    = "AmmoCheck";
	stateTimeoutValue[4]           = 0.01;
	stateWaitForTimeout[4]         = true;

	stateName[5]                   = "AmmoCheck";
	stateTransitionOnAmmo[5]       = "Fire";
	stateTransitionOnNoAmmo[5]     = "NoFire";

	stateName[6]                   = "NoFire";
	stateTransitionOnTimeout[6]    = "Reload";
	stateTimeoutValue[6]           = 0.49;
	stateAllowImageChange[6]       = false;
	stateScript[6]                 = "onFire";
	stateWaitForTimeout[6]         = true;

	stateName[2]                   = "Fire";
	stateTransitionOnTimeout[2]    = "Reload";
	stateTimeoutValue[2]           = 0.49;
	stateFire[2]                   = true;
	stateAllowImageChange[2]       = false;
	stateScript[2]                 = "onFire";
	stateWaitForTimeout[2]         = true;
	stateEmitter[2]                = LightningTrailEmitter;
	stateEmitterTime[2]            = 0.015;

	stateName[3]                   = "Reload";
	stateTransitionOnTriggerUp[3]  = "Ready";
};

function TaserImage::onReady(%this, %obj, %slot)
{
	%obj.setImageAmmo(%slot, true);
}

function TaserImage::SetAmmo(%this, %obj, %slot)
{
	%res = false;
	if(GlobalStorage.TaserMelee || %obj.CDtaser < getSimTime())
	{
		%res = true;
	}
	%obj.setImageAmmo(%slot, %res);
}

function TaserImage::onFire(%this, %obj, %slot)
{
	%mode = %obj.CDtaser >= getSimTime();
	if(!%mode)
	{
		%obj.CDtaser = getSimTime() + GlobalStorage.TaserCD;
	}
	else if(!GlobalStorage.TaserMelee)
	{
		if(isObject(%client = %obj.client) && %client.getClassName() $= "GameConnection")
		{
			%client.centerPrint("Not ready yet!", 1);
		}
		return;
	}
	%scale = getWord(%obj.getScale(), 2);
	%start = %obj.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%obj.getMuzzleVector(%slot), (%mode ? GlobalStorage.TaserShortRange : GlobalStorage.TaserRange) * %scale));
	%typemasks = $Typemasks::PlayerObjectType | $Typemasks::VehicleObjectType | $Typemasks::TerrainObjectType | $Typemasks::InteriorObjectType | $Typemasks::FXbrickObjectType | $TypeMasks::StaticShapeObjectType;
	%raycast = ConalRaycast(%start, %obj.getEyeVector(), (%mode ? GlobalStorage.TaserShortRange : GlobalStorage.TaserRange) * %scale, GlobalStorage.TaserSection, $Typemasks::PlayerObjectType, %obj);
	if(!isObject(%col = firstWord(%raycast)))
	{
		%raycast = containerRaycast(%start, %end, %typemasks, %obj);
		%col = firstWord(%raycast);
	}
	if(isObject(%col))
	{
		if(%col.getType() & $Typemasks::PlayerObjectType && minigameCanDamage(%obj, %col) == 1)
		{
			%col.spawnExplosion(LightningBolt, getWord(%col.getScale(), 2));
			if(%col.isCloaked())
			{
				%col.combatEnd = getSimTime() + GlobalStorage.CombatLength;
				%col.cloak();
			}
			if(!%mode)
			{
				%col.damage(%obj, posFromRaycast(%raycast), GlobalStorage.TaserDamage, $DamageType::LightningD);
				if(%col.getDamagePercent() >= 1)
				{
					%obj.client.unlockAchievement("Electrician");
				}
			}
			%col.stun((%mode ? GlobalStorage.TaserShortStunLength : GlobalStorage.TaserStunLength), 1, 1);
			%col.playThread(2, talk);
			%col.schedule(GlobalStorage.TaserStunLength - 1, playThread, 2, root);
		}
	}
}

function Player::CDtasernote(%this)
{
	if(isObject(%client = %this.client) && %client.getClassName() $= "GameConnection")
	{
		%client.centerPrint("\c3Taser cartridge ready!", 3);
	}
}

//	#1.3
AddDamageType("Repulsor", '<bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_windcannon> %1', '%2 <bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_windcannon> %1',1,1);

datablock itemData(RepulsorItem : DarkMItem)
{
	shapeFile = $StalkerDM::Path @ "/models/windcannon.dts";
	iconName = $StalkerDM::Path @ "/icons/Icon_windcannon";
	image = RepulsorImage;
	uiName = "Repulsor";
	slot = "Melee";
};

datablock ShapeBaseImageData(RepulsorImage)
{
	shapeFile = $StalkerDM::Path @ "/models/windcannon.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0;
	rotation = "0 0 0";

	correctMuzzleVector = true;

	className = "WeaponImage";
	item = RepulsorItem;
	ammo = " ";
	//projectile = " ";
	//projectileType = Projectile;

	melee = false;
	armReady = true;

	stateName[0]                   = "Activate";
	stateTimeoutValue[0]           = 0.15;
	stateTransitionOnTimeout[0]    = "Ready";

	stateName[1]                   = "Ready";
	stateTransitionOnTriggerDown[1]= "Fire";
	stateAllowImageChange[1]       = true;
	stateSequence[1]               = "Ready";

	stateName[2]                   = "Fire";
	stateTransitionOnTimeout[2]    = "Reload";
	stateTimeoutValue[2]           = 0.5;
	stateFire[2]                   = true;
	stateAllowImageChange[2]       = false;
	stateScript[2]                 = "onFire";
	stateWaitForTimeout[2]         = true;

	stateName[3]                   = "Reload";
	stateTransitionOnTriggerUp[3]  = "Ready";
};

function RepulsorImage::onFire(%this, %obj, %slot)
{
	%time = getSimTime();
	if(%obj.CDrepulsor > %time)
	{
		if(isObject(%client = %obj.client) && %client.getClassName() $= "GameConnection")
		{
			%client.centerPrint("Not ready yet!", 1);
		}
		return;
	}
	%obj.CDrepulsor = %time + GlobalStorage.RepulsorCD;
	%list = ConalRaycastM(%obj.getEyePoint(), %obj.getEyeVector(), GlobalStorage.RepulsorRange, GlobalStorage.RepulsorSection, $Typemasks::PlayerObjectType, %obj);
	for(%i = 0; %i < getWordCount(%list); %i++)
	{
		%col = getWord(%list, %i);
		if(minigameCanDamage(%obj, %col) == 1)
		{
			%col.damage(%obj, %col.getHackPosition(), 0, $DamageType::Repulsor); //for assistkills credit
			%v = vectorNormalize(vectorAdd(vectorSub(%col.getPosition(), %obj.getPosition()), "0 0 7.5"));
			%v = vectorScale(%v, GlobalStorage.RepulsorPower);
			%col.setVelocity(%v);
			%col.lastPusher = %obj;
			%col.lastPushTime = getSimTime();
		}
	}
}

//	#1.4
AddDamageType("Shield", '<bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_Shield> %1', '%2 <bitmap:Add-Ons/Gamemode_StalkerDM/icons/CI_Shield> %1',1,1);

datablock itemData(ShieldItem : DarkMItem)
{
	shapeFile = $StalkerDM::Path @ "/models/wooden_shield.dts";
	//iconName = $StalkerDM::Path @ "/icons/Icon_windcannon";
	image = ShieldImage;
	uiName = "Shield";
	slot = "Melee";
};

datablock ShapeBaseImageData(ShieldImage)
{
	shapeFile = $StalkerDM::Path @ "/models/wooden_shield.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0;
	rotation = "0 0 0";

	correctMuzzleVector = true;

	className = "WeaponImage";
	item = ShieldItem;
	ammo = " ";
	//projectile = " ";
	//projectileType = Projectile;

	melee = false;
	armReady = true;

	stateName[0]                   = "Activate";
	stateTimeoutValue[0]           = 0.15;
	stateTransitionOnTimeout[0]    = "Ready";

	stateName[1]                   = "Ready";
	stateTransitionOnTriggerDown[1]= "Fire";
	stateAllowImageChange[1]       = true;
	stateSequence[1]               = "Ready";

	stateName[2]                   = "Fire";
	stateTransitionOnTimeout[2]    = "Reload";
	stateTimeoutValue[2]           = 0.5;
	stateFire[2]                   = true;
	stateAllowImageChange[2]       = false;
	stateScript[2]                 = "onFire";
	stateWaitForTimeout[2]         = true;

	stateName[3]                   = "Reload";
	stateTransitionOnTriggerUp[3]  = "Ready";
};

function ShieldImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(2, shiftto);

	%r = mFloor(getRandom() + 0.5);
	%Sound = "L4BsledgehammerHitSound" @ getSubStr("AB", %r, %r + 1);

	%scale = getWord(%obj.getScale(), 2);
	%start = %obj.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%obj.getMuzzleVector(%slot), GlobalStorage.ShieldRange * %scale));
	%typemasks = $Typemasks::PlayerObjectType | $Typemasks::VehicleObjectType | $Typemasks::TerrainObjectType | $Typemasks::InteriorObjectType | $Typemasks::FXbrickObjectType | $TypeMasks::StaticShapeObjectType;
	%raycast = conalRaycast(%start, %obj.getEyeVector(), GlobalStorage.ShieldRange * %scale, GlobalStorage.ShieldSection, $Typemasks::PlayerObjectType, %obj);
	if(!isObject(%col = firstWord(%raycast)))
	{
		%raycast = containerRaycast(%start, %end, %typemasks, %obj);
		%col = firstWord(%raycast);
	}
	if(!isObject(%col))
	{
		return;
	}
	%pos = posFromRaycast(%raycast);
	%normal = normalFromRaycast(%raycast);
	if(isObject(l4bsledgehammerHitSoundB))
	{
		ServerPlay3D(%Sound, %pos);
	}
	%p = new Projectile()
	{
		datablock = hammerProjectile;
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
		%col.damage(%obj, %pos, GlobalStorage.ShieldDamage, $DamageType::Shield);
	}
}

//	#1.5
SledgehammerItem.slot = "Melee";

function SledgehammerImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(2,shiftto);
	%obj.slow(0.2, 1);

	if(getRandom(0,1))
	{
		%this.raycastExplosionBrickSound = l4bsledgehammerHitSoundA;
		%this.raycastExplosionPlayerSound = l4bsledgehammerHitSoundB;
	}
	else
	{
		%this.raycastExplosionBrickSound = l4bsledgehammerHitSoundB;
		%this.raycastExplosionPlayerSound = l4bsledgehammerHitSoundB;
	}

	%scale = getWord(%obj.getScale(), 2);
	%start = %obj.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%obj.getMuzzleVector(%slot), GlobalStorage.SledgeRange * %scale));
	%typemasks = $Typemasks::PlayerObjectType | $Typemasks::VehicleObjectType | $Typemasks::TerrainObjectType | $Typemasks::InteriorObjectType | $Typemasks::FXbrickObjectType | $TypeMasks::StaticShapeObjectType;
	%raycast = conalRaycast(%start, %obj.getEyeVector(), GlobalStorage.SledgeRange * %scale, GlobalStorage.SledgeSection, $Typemasks::PlayerObjectType, %obj);
	if(!isObject(%col = firstWord(%raycast)))
	{
		%raycast = containerRaycast(%start, %end, %typemasks, %obj);
		%col = firstWord(%raycast);
	}
	if(!isObject(%col))
	{
		return;
	}
	%pos = posFromRaycast(%raycast);
	%normal = normalFromRaycast(%raycast);
	if(isObject(l4bsledgehammerHitSoundB))
	{
		ServerPlay3D(%this.raycastExplosionBrickSound, %pos);
	}
	%p = new Projectile()
	{
		datablock = hammerProjectile;
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
		%col.damage(%obj, %pos, GlobalStorage.SledgeDamage, $DamageType::sledgehammer);
		%col.slow(0.5, GlobalStorage.SledgeDuration);
		%col.setVelocity(vectorScale(vectorAdd(%obj.getForwardVector(), "0 0 0.5"), GlobalStorage.SledgeKnockback));
		cancel(%col.undazeSched);
	}
}