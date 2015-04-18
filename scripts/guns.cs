////////////////////
//STALKER DM: GUNS//
////////////////////

////////////
//CONTENTS//
////////////
//#1. GUNS
//	#1.1 Pistol (adjustments)
//	#1.2 Pump Shotgun (modifications)
//	#1.3 Assault Rifle (adjustments)
//	#1.4 ACRA
//	#1.5 Flamethrower

//#1.
//	#1.1
PistolItem.slot = "Gun";

function pistolImage::onFire(%this,%obj,%slot)
{
	%obj.spawnExplosion(TTRecoilProjectile,"1 1 1");
	if(%obj.getDamagePercent() >= 1)
	{
		return;
	}
	if(vectorLen(%obj.getVelocity()) > 0.1)
	{
		%this.raycastSpreadAmt = 0.0018;
		%this.raycastWeaponRange = 85;
	}
	else
	{
		%this.raycastSpreadAmt = 0.0009;
		%this.raycastWeaponRange = 200;
	}
	if(%obj.toolAmmo[%obj.currTool] > 0)
	{
		Parent::onFire(%this,%obj,%slot);
		%obj.toolAmmo[%obj.currTool]--;
		%obj.AmmoSpent[%obj.currTool]++;
	}
	else if(%this.item.maxAmmo == 0)
	{
		Parent::onFire(%this,%obj,%slot);
	}
	%obj.playThread(2, shiftAway);
}

function PistolImage::onReloadStart(%this,%obj,%slot)
{
	if(%obj.client.quantity["9MMrounds"] >= 1)
	{
		%obj.playThread(2, shiftUp);
		serverPlay3D(block_MoveBrick_Sound,%obj.getPosition());
	}
}

function PistolImage::onBounce(%this,%obj,%slot)
{
	%obj.playThread(2, plant);
}

function PistolImage::onReady(%this,%obj,%slot)
{
}

function PistolImage::onReloaded(%this,%obj,%slot)
{
    if(%obj.client.quantity["9MMrounds"] >= 1)
	{
		%obj.playThread(2, plant);
		if(%obj.client.quantity["9MMrounds"] > 12)
		{
			%obj.client.quantity["9MMrounds"] -= %obj.AmmoSpent[%obj.currTool];
			%obj.toolAmmo[%obj.currTool] = %this.item.maxAmmo;
			%obj.AmmoSpent[%obj.currTool] = 0;
			serverPlay3D(Block_PlantBrick_Sound,%obj.getPosition());
			%obj.setImageAmmo(%slot,1);
			return;
		}

		if(%obj.client.quantity["9MMrounds"] <= 12)
		{
			%obj.client.exchangebullets = %obj.client.quantity["9MMrounds"];
			%obj.toolAmmo[%obj.currTool] = %obj.client.exchangebullets;
			%obj.setImageAmmo(%slot,1);
			serverPlay3D(Block_PlantBrick_Sound,%obj.getPosition());
			%obj.client.quantity["9MMrounds"] = 0;
			return;
		}
	}
}

//	#1.2
PumpShotgunItem.slot = "Gun";

function PumpShotgunImage::onFire(%this,%obj,%slot)
{
	if(%obj.toolAmmo[%obj.currTool] > 0)
	{
		%fvec = %obj.getForwardVector();
		%fX = getWord(%fvec,0);
		%fY = getWord(%fvec,1);

		%evec = %obj.getEyeVector();
		%eX = getWord(%evec,0);
		%eY = getWord(%evec,1);
		%eZ = getWord(%evec,2);

		%eXY = mSqrt(%eX*%eX+%eY*%eY);

		%aimVec = %fX*%eXY SPC %fY*%eXY SPC %eZ;
		serverPlay3D(PumpShotgunfireSound,%obj.getPosition());
		%obj.playThread(2, activate);
		%obj.toolAmmo[%obj.currTool]--;

		%obj.spawnExplosion(TTBigRecoilProjectile,"1 1 1");

		%projectile = %this.projectile;
		%spread = 0.0028;
		%shellcount = GlobalStorage.ShotgunShellcount;

		for(%shell=0; %shell<%shellcount; %shell++)
		{
			%vector = %obj.getMuzzleVector(%slot);
			%objectVelocity = %obj.getVelocity();
			%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
			%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
			%velocity = VectorAdd(%vector1,%vector2);
			%x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%mat = MatrixCreateFromEuler(%x SPC %y SPC %z);
			%velocity = MatrixMulVector(%mat, %velocity);

			%p = new (%this.projectileType)()
			{
				dataBlock = %projectile;
				initialVelocity = %velocity;
				initialPosition = %obj.getMuzzlePoint(%slot);
				sourceObject = %obj;
				sourceSlot = %slot;
				client = %obj.client;
			};
			%p.schedule(1000, delete);
		}

		%projectile = "shotgunBlastProjectile";

		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);


		%p = new (%this.projectileType)()
		{
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %obj.getMuzzlePoint(%slot);
			sourceObject = %obj;
			sourceSlot = %slot;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
		return %p;

		%projectile = "shotgunFlashProjectile";

		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);


		%p = new (%this.projectileType)()
		{
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %obj.getMuzzlePoint(%slot);
			sourceObject = %obj;
			sourceSlot = %slot;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
		return %p;
	}
	else
	{
		serverPlay3D(PumpShotgunJamSound,%obj.getPosition());
	}
}

function PumpShotgunImage::onEject(%this,%obj,%slot)
{
	%this.onLoadCheck(%obj,%slot);
	%obj.playThread(2, plant);
}

function PumpShotgunImage::onReloadStart(%this,%obj,%slot)
{
	if(%obj.client.quantity["shotgunrounds"] >= 1)
	{
		%obj.playThread(2, shiftto);
		serverPlay3D(block_MoveBrick_Sound,%obj.getPosition());
	}
}

function PumpShotgunImage::onReloaded(%this,%obj,%slot)
{
	%this.onLoadCheck(%obj,%slot);
	if(%obj.client.quantity["shotgunrounds"] >= 1)
	{
		%obj.client.quantity["shotgunrounds"]--;
		%obj.toolAmmo[%obj.currTool]++;
	}
}

//	#1.3
TAssaultRifleItem.slot = "Gun";

TAssaultRifleImage.stateTimeoutValue[10] = 0.1;

function TAssaultRifleImage::onFire(%this,%obj,%slot)
{
	if(vectorLen(%obj.getVelocity()) < 0.1 && (getSimTime() - %obj.lastShotTime) > 500)
	{
		%projectile = TAssaultRifleProjectile2;
		%spread = 0.0003;
	}
	else
	{
		%projectile = TAssaultRifleProjectile1;
		%spread = 0.0006;
	}
	%obj.lastShotTime = getSimTime();
	%shellcount = 1;
	%obj.playThread(2, plant);
	%shellcount = 1;
	%obj.toolAmmo[%obj.currTool]--;
	%obj.AmmoSpent[%obj.currTool]++;
	%obj.spawnExplosion(TTRecoilProjectile,"1 1 1");
	for(%shell=0; %shell<%shellcount; %shell++)
	{
		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);
		%x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
		%velocity = MatrixMulVector(%mat, %velocity);

		%p = new Projectile()
		{
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %obj.getMuzzlePoint(%slot);
			sourceObject = %obj;
			sourceSlot = %slot;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
	}
	return %p;
}

function TAssaultRifleImage::onReloadStart(%this,%obj,%slot)
{
	if(%obj.client.quantity["556rounds"] >= 1)
	{
		%obj.playThread(2, shiftRight);
		serverPlay3D(block_MoveBrick_Sound,%obj.getPosition());
	}
}

function TAssaultRifleImage::onReloadWait(%this,%obj,%slot)
{
	if(%obj.client.quantity["556rounds"] >= 1)
	{
		%obj.playThread(2, plant);
		serverPlay3D(magazineOutSound,%obj.getPosition());
	}
}

function TAssaultRifleImage::onReloaded(%this,%obj,%slot)
{
	if(%obj.client.quantity["556rounds"] >= 1)
	{
		if(%obj.client.quantity["556rounds"] > %this.item.maxAmmo)
		{
			%obj.client.quantity["556rounds"] -= %obj.AmmoSpent[%obj.currTool];
			%obj.toolAmmo[%obj.currTool] = %this.item.maxAmmo;
			%obj.AmmoSpent[%obj.currTool] = 0;
			%obj.setImageAmmo(%slot,1);
			return;
		}

		if(%obj.client.quantity["556rounds"] <= %this.item.maxAmmo)
		{
			%obj.client.exchangebullets = %obj.client.quantity["556rounds"];
			%obj.toolAmmo[%obj.currTool] = %obj.client.exchangebullets;
			%obj.setImageAmmo(%slot,1);
			%obj.client.quantity["556rounds"] = 0;
			return;
		}
	}
}

function TAssaultRifleImage::onMount(%this,%obj,%slot)
{
	Parent::onMount(%this,%obj,%slot);
}

//	#1.4
datablock AudioProfile(AcraOnSound)
{
   filename    = $StalkerDM::Path @ "/sounds/heal_mechanical.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock itemData(ACRAitem : DarkMItem)
{
	shapeFile = $StalkerDM::Path @ "/models/acraboth.dts";
	iconName = $StalkerDM::Path @ "/icons/icon_acra";
	image = ACRAimage;
	uiName = "A.C.R.A.";
	slot = "Gun";
};

datablock ShapeBaseImageData(ACRAbothimage)
{
	shapeFile = $StalkerDM::Path @ "/models/acraboth.dts";
	emap = true;

	mountPoint = 7;
	offset = "0 -0.3 0.75";
	eyeOffset = "0 -5 0";
	rotation = "0 0 0";

	correctMuzzleVector = true;

	className = "WeaponImage";
	item = ACRAitem;
	ammo = " ";
	//projectile = " ";
	//projectileType = Projectile;

	melee = false;
	armReady = false;

	stateName[0]                   = "Activate";
	stateTimeoutValue[0]           = 0.15;
	stateTransitionOnTimeout[0]    = "Ready";

	stateName[1]                   = "Ready";
	stateAllowImageChange[1]       = true;
};

datablock ShapeBaseImageData(ACRAimage)
{
	shapeFile = $StalkerDM::Path @ "/models/acragun.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0;
	rotation = "0 0 0";

	correctMuzzleVector = true;

	className = "WeaponImage";
	item = ACRAitem;
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
	stateScript[1]                 = "onReady";

	stateName[2]                   = "Fire";
	stateTransitionOnTimeout[2]    = "Reload";
	stateTimeoutValue[2]           = 1;
	stateSound[2]                  = ACRAOnSound;
	stateFire[2]                   = true;
	stateAllowImageChange[2]       = false;
	stateScript[2]                 = "onFire";
	stateWaitForTimeout[2]         = true;
	stateEmitter[2]                = isObject(HealEmitter) ? HealEmitter : "";
	stateEmitterTime[2]            = 0.175;

	stateName[3]                   = "Reload";
	stateTransitionOnTriggerDown[3]= "Fire";
	stateTransitionOnTriggerUp[3]  = "Ready";
};

datablock ShapeBaseImageData(ACRApackImage)
{
	shapeFile = $StalkerDM::Path @ "/models/acrapack.dts";
	emap = true;

	mountPoint = 7;
	offset = "0 -0.3 0.75";
	eyeOffset = "0 -5 0";
	rotation = "0 0 0";

	correctMuzzleVector = true;

	className = "WeaponImage";
	item = ACRAitem;
	ammo = " ";
	//projectile = " ";
	//projectileType = Projectile;

	melee = false;
	armReady = false;

	stateName[0]                   = "Activate";
	stateTimeoutValue[0]           = 0.15;
	stateTransitionOnTimeout[0]    = "Ready";

	stateName[1]                   = "Ready";
	stateAllowImageChange[1]       = true;
};

function ACRApackImage::onMount(%this, %obj, %slot)
{
	%obj.hideNode("pack");
}
function ACRAbothImage::onMount(%this, %obj, %slot)
{
	%obj.hideNode("pack");
}

function ACRAimage::onReady(%this, %obj, %slot)
{
	%obj.usingACRA = false;
	if(isObject(%obj.ACRAlight))
	{
		%obj.ACRAlight.delete();
	}
}

function ACRAimage::onMount(%this, %obj, %slot)
{
	%obj.hideNode("pack");
	%obj.unmountImage(2);
	%obj.mountImage(ACRApackImage, 2);
}
function ACRAimage::onUnmount(%this, %obj, %slot)
{
	%obj.hideNode("pack");
	if(%obj.getMountedImage(2) == nameToID(ACRApackImage))
	{
		%obj.unmountImage(2);
	}
	%obj.mountImage(ACRAbothImage, 2);
}

function ACRAimage::onFire(%this, %obj, %slot)
{
	%obj.usingACRA = true;
	if(!isObject(%obj.ACRAlight) && isObject(GreenDimAmbientLight))
	{
		%obj.ACRAlight = new FxLight()
		{
			datablock = GreenDimAmbientLight;
			position = "0 0 0";
			scale = "1 1 1";
		};
		%obj.ACRAlight.attachToObject(%obj);
		%obj.ACRAlight.reset();
	}
	cancel(%obj.ACRAlight.deleteSched);
	%obj.ACRAlight.deleteSched = %obj.ACRAlight.schedule(1500, delete);
	%mini = %obj.client.minigame;
	%totalCurHealth = 0;
	%totalMaxHealth = 0;
	%totalAmt = 0;
	InitContainerRadiusSearch(%obj.getPosition(), GlobalStorage.ACRAradius, $Typemasks::PlayerObjectType);
	while(%hit = containerSearchNext())
	{
		if(%hit.client.minigame == %mini && (minigameCanDamage(%obj, %hit) == 0) || %obj == %hit)
		{
			if(%hit.getDamageLevel() > 0 && %obj.getEnergyLevel() > (GlobalStorage.ACRAefficiency * GlobalStorage.ACRAtickamt))
			{
				if(%obj.client.healing+= mClamp(GlobalStorage.ACRAtickamt, 0, %hit.getDamageLevel()) >= 200)
				{
					%sObj.client.unlockAchievement("Medical Specialist");
				}
				%hit.addHealth(GlobalStorage.ACRAtickamt);
				%hit.lastTF2Healer = %obj; //Healing credit for assistkills
				%hit.emote(HealImage, getWord(%hit.getScale(), 2));
				%obj.setEnergyLevel(%obj.getEnergyLevel() - (GlobalStorage.ACRAefficiency * %amt));
			}
		}
	}
}

//	#1.5
datablock AudioProfile(flamethrowerLoopSound)
{
   filename    = $StalkerDM::Path @ "/sounds/flamethrower_e.wav";
   description = AudioClose3d;
   preload = true;
};

datablock AudioProfile(flamethrowerIgniteSound)
{
   filename    = $StalkerDM::Path @ "/sounds/flamethrower_ignite.wav";
   description = AudioClose3d;
   preload = true;
};

datablock AudioProfile(flamethrowerOnSound)
{
   filename    = $StalkerDM::Path @ "/sounds/flamethrower_activate.wav";
   description = AudioClose3d;
   preload = true;
};

datablock ParticleData(FTparticle)
{
	dragCoefficient      = 6;
	gravityCoefficient   = -3.5;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 225;
	lifetimeVarianceMS   = 55;
	textureName          = "base/data/particles/cloud";
	spinSpeed		= 16.0;
	spinRandomMin		= -500.0;
	spinRandomMax		= 500.0;
	colors[0]     = "0.8 0.8 0.8 0.1";
	colors[1]     = "0.9 0.6 0.3 0.7";
	colors[2]     = "0.9 0.3 0.1 0.4";
	colors[3]     = "0.9 0.3 0.1 0.2";
	sizes[0]      = 0.35;
	sizes[1]      = 0.4;
	sizes[2]      = 1.05;
	sizes[3]      = 2.9;
   times[0] = 0.0;
   times[1] = 0.3;
   times[2] = 0.6;
   times[3] = 1.0;

	useInvAlpha = false;
};
datablock ParticleEmitterData(FTemitter)
{   ejectionPeriodMS = 3;
   periodVarianceMS = 0;
   ejectionVelocity = 70;
   velocityVariance = 0.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 5;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   orientOnVelocity = true;
   particles = "FTParticle";

   uiName = "Flamethrower Flame";
};

datablock ParticleData(FTLightParticle)
{
	dragCoefficient      = 8;
	gravityCoefficient   = -3.9;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 100;
	lifetimeVarianceMS   = 0;
	textureName          = "base/data/particles/cloud";
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	colors[0]     = "0.6 0.6 1 0.2";
	colors[1]     = "1 0.4 0.1 0.2";
	colors[2]     = "1 0.4 0.1 0.0";
	sizes[0]      = 0.03;
	sizes[1]      = 0.2;
	sizes[2]      = 0.3;

	useInvAlpha = false;
};
datablock ParticleEmitterData(FTLightEmitter)
{
	lifeTimeMS = 50;

	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = -0.2;
	velocityVariance = 0.0;
	ejectionOffset   = 0.0;
	thetaMin         = 89;
	thetaMax         = 90;
	phiReferenceVel  = 0;
	phiVariance      = 0;
	overrideAdvance = false;
	particles = "FTLightParticle";
};
datablock ParticleData(FTIgniteParticle)
{
	textureName				= "base/data/particles/cloud";
	dragCoefficient			= 0.0;
	gravityCoefficient		= -5.0;
	inheritedVelFactor		= 0.5;
	windCoefficient			= 0;
	constantAcceleration	= 3.0;
	lifetimeMS				= 400;
	lifetimeVarianceMS		= 200;
	spinSpeed				= 0;
	spinRandomMin			= -90.0;
	spinRandomMax			=  90.0;
	useInvAlpha				= false;
	
	colors[0]	= "1 1 1 0.1";
	colors[1]	= "1.0 1.0 0.3 0.3";
	colors[2]	= "0.6 0.0 0.0 0.0";
	
	sizes[0]	= 4.0;
	sizes[1]	= 1.7;
	sizes[2]	= 0.8;
	
	times[0]	= 0.0;
	times[1]	= 0.2;
	times[2]	= 1.0;
};

datablock ParticleEmitterData(FTIgniteEmitter)
{
	ejectionPeriodMS	= 5;
	periodVarianceMS	= 4;
	ejectionVelocity	= 0;
	ejectionOffset		= 0.50;
	velocityVariance	= 0.0;
	thetaMin			= 89;
	thetaMax			= 90;
	phiReferenceVel		= 0;
	phiVariance			= 360;
	overrideAdvance		= false;
	
	particles = FTIgniteParticle;  
};

datablock AudioProfile(flamethrowerBurnSound)
{
   filename    = $StalkerDM::Path @ "/sounds/flamethrower_burn.wav";
   description = AudioClose3d;
   preload = true;
};

datablock ShapeBaseImageData(FTburnImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;

	mountPoint = 7;
	offset = "0 0 -0.5";
	eyeOffset = 0;
	rotation = "0 0 0";

	correctMuzzleVector = true;

	className = "WeaponImage";
	item = FlamethrowerItem;
	ammo = " ";
	//projectile = " ";
	//projectileType = Projectile;

	melee = false;
	armReady = false;

	stateName[0]                = "Activate";
	stateEmitter[0]             = FTIgniteEmitter;
	stateEmitterTime[0]         = 10;
	stateSound[0]               = flamethrowerBurnSound;
	stateTimeoutValue[0]        = 0.2;
	stateTransitionOnTimeout[0] = "Activate";
};

datablock itemData(FlamethrowerItem : DarkMItem)
{
	shapeFile = $StalkerDM::Path @ "/models/FLAMETHROWER_2.dts";
	iconName = $StalkerDM::Path @ "/icons/icon_flamethrower";
	image = FlamethrowerImage;
	uiName = "Flamethrower";
	slot = "Gun";
};

AddDamageType("Flamethrower", '<bitmap:Add-Ons/Weapon_ElementalSpells/Icons/CI_Fire> %1', '%2 <bitmap:Add-Ons/Weapon_ElementalSpells/Icons/CI_Fire> %1', 0.25, 1);

datablock ShapeBaseImageData(FlamethrowerImage)
{
	shapeFile = $StalkerDM::Path @ "/models/FLAMETHROWER_2.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0;
	rotation = "0 0 0";

	correctMuzzleVector = true;

	className = "WeaponImage";
	item = FlamethrowerItem;
	ammo = " ";
	//projectile = " ";
	//projectileType = Projectile;

	melee = false;
	armReady = true;

	stateName[0]                   = "Activate";
	stateTimeoutValue[0]           = 0.15;
	stateTransitionOnTimeout[0]    = "Ready";
	stateSound[0]                  = flamethrowerOnSound;

	stateName[1]                   = "Ready";
	stateTransitionOnTriggerDown[1]= "Check";
	stateAllowImageChange[1]       = true;
	stateSequence[1]               = "Ready";
	stateScript[1]                 = "onReady";
	stateEmitter[1]                = FTLightEmitter;
	stateEmitterTime[1]            = 600;

	stateName[5]                   = "Check";
	stateTimeoutValue[5]           = 0.01;
	stateWaitForTimeout[5]         = true;
	stateTransitionOnTimeout[5]    = "TriggerCheck";
	stateScript[5]                 = "Check";

	stateName[2]                   = "TriggerCheck";
	stateTransitionOnTriggerDown[2]= "AmmoCheck";
	stateTransitionOnTriggerUp[2]  = "Ready";

	stateName[3]                   = "AmmoCheck";
	stateTransitionOnAmmo[3]       = "Fire";
	stateTransitionOnNoAmmo[3]     = "Ready";

	stateName[4]                   = "Fire";
	stateFire[4]                   = true;
	stateScript[4]                 = "onFire";
	stateEmitter[4]                = FTemitter;
	stateEmitterTime[4]            = 0.51;
	stateTimeoutValue[4]           = 0.1;
	stateTransitionOnTimeout[4]    = "Fire2";
	stateWaitForTimeout[4]         = true;
	stateAllowImageChange[4]       = false;
	stateSound[4]                  = flamethrowerLoopSound;

	stateName[6]                   = "Fire2";
	stateFire[6]                   = true;
	stateScript[6]                 = "onFire";
	stateTimeoutValue[6]           = 0.1;
	stateTransitionOnTimeout[6]    = "Fire3";
	stateWaitForTimeout[6]         = true;
	stateAllowImageChange[6]       = false;
	stateSound[6]                  = flamethrowerLoopSound;

	stateName[7]                   = "Fire3";
	stateFire[7]                   = true;
	stateScript[7]                 = "onFire";
	stateTimeoutValue[7]           = 0.1;
	stateTransitionOnTimeout[7]    = "Fire4";
	stateWaitForTimeout[7]         = true;
	stateAllowImageChange[7]       = false;
	stateSound[7]                  = flamethrowerLoopSound;

	stateName[8]                   = "Fire4";
	stateFire[8]                   = true;
	stateScript[8]                 = "onFire";
	stateTimeoutValue[8]           = 0.1;
	stateTransitionOnTimeout[8]    = "Fire5";
	stateWaitForTimeout[8]         = true;
	stateAllowImageChange[8]       = false;
	stateSound[8]                  = flamethrowerLoopSound;

	stateName[9]                   = "Fire5";
	stateFire[9]                   = true;
	stateScript[9]                 = "onFire";
	stateTimeoutValue[9]           = 0.1;
	stateTransitionOnTimeout[9]    = "Check";
	stateWaitForTimeout[9]         = true;
	stateAllowImageChange[9]       = false;
	stateSound[9]                  = flamethrowerLoopSound;
};

function FlamethrowerImage::onReady(%this, %obj, %slot)
{
	if(%obj.FTblength > 0)
	{
		//%obj.CDflamethrower = getSimTime() + mClamp(%obj.FTblength * 1000, 2000, 6000);
		%obj.CDflamethrower = getSimTime() + 2000;
		%obj.FTblength = 0;
	}
}

function FlamethrowerImage::Check(%this, %obj, %slot)
{
	%ammo = %obj.getEnergyLevel() >= (1.0124 * GlobalStorage.FTenergy * 5) && %obj.CDflamethrower <= getSimTime();
	if(!%ammo && isObject(%client = %obj.client) && %client.getClassName() $= "GameConnection")
	{
		if(%obj.getEnergyLevel() < (1.0124 * GlobalStorage.FTenergy * 5))
		{
			%client.centerPrint("Not enough energy!", 2);
		}
		else
		{
			%client.centerPrint("Not ready yet!", 1);
		}
	}
	%obj.setImageAmmo(%slot, %ammo);
}

function FlamethrowerImage::onFire(%this, %obj, %slot)
{
	%obj.FTblength+= 0.1;
	%obj.setEnergyLevel(%obj.getEnergyLevel() - (1.0124 * GlobalStorage.FTenergy));
	%list = conalRaycastM(%obj.getEyePoint(), %obj.getEyeVector(), GlobalStorage.FTrange, GlobalStorage.FTsection, $Typemasks::PlayerObjectType, %obj);
	for(%i = 0; %i < getWordCount(%list); %i++)
	{
		%col = getWord(%list, %i);
		if(minigameCanDamage(%obj, %col) == 1 && !isEventPending(%col.recloakSched) && !%col.isCloaked())
		{
			%col.damage(%obj, %col.getHackPosition(), GlobalStorage.FTdamage, $DamageType::Flamethrower);
			if(%obj.client.FTdamage+= GlobalStorage.FTdamage >= 300)
			{
				%obj.client.unlockAchievement("Pyrotechnician");
			}
			schedule(60000, 0, eval, %obj.client @ ".FTdamage-= " @ GlobalStorage.FTdotstart @ ";");
			%col.FTticks = GlobalStorage.FTdotticks;
			if(!isEventPending(%col.FTdotSched))
			{
				%col.FTdot(%obj);
			}
		}
	}
}

function Player::FTdot(%obj, %sObj)
{
	if(%obj.getMountedImage(2) != nameToID(FTburnImage))
	{
		%obj.mountImage(FTburnImage, 2);
	}
	if(%obj.crouching)
	{
		if(%obj.SDR++ >= 2)
		{
			%obj.unmountImage(2);
			%obj.SDR = 0;
			return; //cancel dot
		}
	}
	else
	{
		%obj.SDR = mClamp(%obj.SDR - 1, 0, 2);
	}
	if(%obj.isCloaked)
	{
		%obj.combatEnd = getSimTime() + GlobalStorage.CombatLength;
		%obj.cloak();
	}
	if(isObject(%client = %obj.client) && %client.getClassName() $= "GameConnection")
	{
		%client.centerPrint("\c3You're on fire! Crouch!", 1);
	}
	%obj.damage(%sObj, %obj.getHackPosition(), GlobalStorage.FTdotstart, $DamageType::Flamethrower);
	if(%sObj.client.FTdamage+= GlobalStorage.FTdotstart >= 300)
	{
		%sObj.client.unlockAchievement("Pyrotechnician");
	}
	schedule(60000, 0, eval, %sObj.client @ ".FTdamage-= " @ GlobalStorage.FTdotstart @ ";");
	if(%obj.FTticks-- > 0)
	{
		%obj.FTdotSched = %obj.schedule(1000, FTdot, %sObj);
	}
	else
	{
		%obj.unmountImage(2);
	}
}