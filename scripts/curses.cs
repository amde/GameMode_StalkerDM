//////////////////////
//STALKER DM: CURSES//
//////////////////////

////////////
//CONTENTS//
////////////
//#1. CURSES
//	#1.1 Toxin
//	#1.2 Wrath
//	#1.3 Rage
//	#1.4 Abyss
//	#1.5 Psychosis

//	#1.1
datablock AudioProfile(curseToxinSound)
{
   filename    = $StalkerDM::Path @ "/sounds/curse_juju.wav";
   description = AudioClose3d;
   preload     = true;
};

datablock ParticleData(ToxinParticle)
{
	dragCoefficient = 0.5;
	windCoefficient = 0.1;
	gravityCoefficient = -0.05;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 3000;
	lifetimeVarianceMS = 500;
	spinSpeed = 0;
	spinRandomMin = -5;
	spinRandomMax = 5;
	useInvAlpha = false;
	textureName = "base/data/particles/cloud";
	colors[0] = "0 0.5 0.1 0";
	colors[1] = "0 0.5 0.3 0.4";
	colors[2] = "0 0.2 0.05 0";
	sizes[0] = 12;
	sizes[1] = 12.1;
	sizes[2] = 12.5;
	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};

datablock ParticleEmitterData(ToxinEmitter)
{
	ejectionPeriodMS = 45;
	periodVarianceMS = 5;
	ejectionVelocity = 0.025;
	velocityVariance = 0.005;
	ejectionOffset = 0.2;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = false;
	particles = "ToxinParticle";
	uiName = "Toxin Curse";
};

datablock ParticleData(ToxinAmbientParticle)
{
	dragCoefficient = 0.05;
	windCoefficient = 0.1;
	gravityCoefficient = -0.1;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1000;
	lifetimeVarianceMS = 200;
	spinSpeed = 0;
	spinRandomMin = -90;
	spinRandomMax = 90;
	useInvAlpha = false;
	textureName = "base/data/particles/cloud";
	colors[0] = "0 0.5 0.1 0";
	colors[1] = "0 0.5 0.3 1";
	colors[2] = "0 0.2 0.05 0";
	sizes[0] = 0.1;
	sizes[1] = 0.4;
	sizes[2] = 0.75;
	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};

datablock ParticleEmitterData(ToxinAmbientEmitter)
{
	ejectionPeriodMS = 100;
	periodVarianceMS = 10;
	ejectionVelocity = 0.15;
	velocityVariance = 0.03;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = false;
	particles = "ToxinAmbientParticle";
	uiName = "Toxin Curse Ambient";
};

datablock ItemData(ToxinItem : DarkMitem)
{
	iconName = $StalkerDM::Path @ "/icons/Icon_Toxin";
	image = ToxinImage;
	uiName = "Toxin";
	slot = "Curse";
};

datablock ShapeBaseImageData(ToxinImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0;
	rotation = "0 0 0";

	correctMuzzleVector = true;

	className = "WeaponImage";
	item = ToxinItem;
	ammo = " ";
	projectile = DarknessProjectile;
	projectileType = Projectile;

	melee = false;
	armReady = true;

	stateName[0]                   = "Activate";
	stateTimeoutValue[0]           = 0.15;
	stateTransitionOnTimeout[0]    = "Ready";

	stateName[1]                   = "Ready";
	stateTransitionOnTriggerDown[1]= "Fire";
	stateAllowImageChange[1]       = true;
	stateSequence[1]               = "Ready";
	stateEmitter[1]                = ToxinAmbientEmitter;
	stateEmitterTime[1]            = 600;

	stateName[2]                   = "Fire";
	stateTransitionOnTimeout[2]    = "Reload";
	stateTimeoutValue[2]           = 1;
	stateFire[2]                   = true;
	stateAllowImageChange[2]       = false;
	stateScript[2]                 = "onFire";
	stateWaitForTimeout[2]         = true;

	stateName[3]                   = "Reload";
	stateTransitionOnTriggerUp[3]  = "Ready";
};

function ToxinImage::onFire(%this, %obj, %slot)
{
	if((%obj.CDtoxin) >= getSimTime())
	{
		if(%obj.client)
		{
			%obj.client.centerPrint("Not ready yet!", 1);
		}
		return;
	}
	if(%obj.getEnergyLevel() > GlobalStorage.ToxinEnergy)
	{
		%start = %obj.getEyePoint();
		%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), GlobalStorage.CurseMaxRange));
		%ray = containerRaycast(%start, %end, $Typemasks::TerrainObjectType | $Typemasks::InteriorObjectType | $Typemasks::FXbrickObjectType);
		if(isObject(%hit = firstWord(%ray)))
		{
			if(vectorDist(%start, posFromRaycast(%ray)) < GlobalStorage.CurseMinRange)
			{
				if(isObject(%client = %obj.client))
				{
					%client.centerPrint("You're too close!", 3);
				}
				return;
			}
			%obj.setEnergyLevel(mClamp(%obj.getEnergyLevel() - GlobalStorage.ToxinEnergy, 0, 100));
			%obj.CDtoxin = getSimTime() + GlobalStorage.ToxinCD;
			%cloud = new ParticleEmitterNode()
			{
				dataBlock = GenericEmitterNode;
				emitter = ToxinEmitter;
				position = posFromRaycast(%ray);
				rotation = normalFromRaycast(%ray);
				scale = "25 25 1";
				velocity = 1;
				caster = %obj;
				active = true;
			};
			serverPlay3D(curseToxinSound,%cloud.getPosition());
			ToxinTick(%cloud, GlobalStorage.ToxinTotalTicks);
		}
	}
	else if(isObject(%obj.client))
	{
		%obj.client.centerPrint("Not enough energy!", 1);
	}
}

function ToxinTick(%this, %remain)
{
	if(!isObject(%this))
	{
		return;
	}
	InitContainerRadiusSearch(%this.getPosition(), GlobalStorage.ToxinRadius, $Typemasks::PlayerObjectType);
	while(%hit = containerSearchNext())
	{
		if(minigameCanDamage(%this.caster, %hit) == 1 && %this.caster != %hit)
		{
			if(%hit.toxinStacks++ >= GlobalStorage.ToxinStunTicks && %hit.toxinImmunity <= getSimTime())
			{
				%hit.stun(GlobalStorage.ToxinStunLength, 0, 1, "Toxin");
				if(%this.stuns++ >= 3)
				{
					%this.caster.client.unlockAchievement("Master of Neurotoxins");
				}
				%hit.toxinStacks = 0;
				%hit.toxinImmunity = getSimTime() + 10000;
			}
			if(isObject(%client = %hit.client) && %client.getClassName() $= "GameConnection" && !%hit.stunned)
			{
				%hit.client.centerPrint("\c5A neurotoxin! Run!", 2);
			}
			%hit.schedule(GlobalStorage.ToxinFalloffTime, ToxinTickDecrement);
		}
	}
	if(%remain > 0)
	{
		schedule(GlobalStorage.ToxinTickInterval, 0, ToxinTick, %this, %remain--);
	}
	else
	{
		%this.schedule(GlobalStorage.ToxinTickInterval, delete);
	}
}

function Player::ToxinTickDecrement(%obj)
{
	if(%obj.toxinStacks > 0)
	{
		%obj.toxinStacks--;
	}
}

//	#1.2
datablock AudioProfile(curseWrathSound)
{
   filename    = $StalkerDM::Path @ "/sounds/curse_damage.wav";
   description = AudioClose3d;
   preload = true;
};

datablock ParticleEmitterData(WrathEmitter : BurnEmitterA)
{
	ejectionPeriodMS = 4;
	periodVarianceMS = 1;
	uiName = "Wrath Curse";
};

datablock ParticleData(WrathAmbientParticle)
{
	dragCoefficient = 0.05;
	windCoefficient = 0.1;
	gravityCoefficient = -0.3;
	inheritedVelFactor = 0.25;
	constantAcceleration = 0;
	lifetimeMS = 800;
	lifetimeVarianceMS = 150;
	spinSpeed = 0;
	spinRandomMin = -90;
	spinRandomMax = 90;
	useInvAlpha = false;
	textureName = "base/data/particles/cloud";
	colors[0] = "1 1 0.25 0";
	colors[1] = "1 1 0.25 1";
	colors[2] = "0.6 0 0 0";
	sizes[0] = 0;
	sizes[1] = 0.6;
	sizes[2] = 0.3;
	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};

datablock ParticleEmitterData(WrathAmbientEmitter)
{
	ejectionPeriodMS = 100;
	periodVarianceMS = 10;
	ejectionVelocity = 0.15;
	velocityVariance = 0.03;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = false;
	particles = "WrathAmbientParticle";
	uiName = "Wrath Curse Ambient";
};

datablock ItemData(WrathItem : DarkMitem)
{
	iconName = $StalkerDM::Path @ "/icons/Icon_Wrath";
	image = WrathImage;
	uiName = "Wrath";
	slot = "Curse";
};

datablock ShapeBaseImageData(WrathImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0;
	rotation = "0 0 0";

	correctMuzzleVector = true;

	className = "WeaponImage";
	item = WrathItem;
	ammo = " ";
	projectile = DarknessProjectile;
	projectileType = Projectile;

	melee = false;
	armReady = true;

	stateName[0]                   = "Activate";
	stateTimeoutValue[0]           = 0.15;
	stateTransitionOnTimeout[0]    = "Ready";

	stateName[1]                   = "Ready";
	stateTransitionOnTriggerDown[1]= "Fire";
	stateAllowImageChange[1]       = true;
	stateSequence[1]               = "Ready";
	stateEmitter[1]                = WrathAmbientEmitter;
	stateEmitterTime[1]            = 600;

	stateName[2]                   = "Fire";
	stateTransitionOnTimeout[2]    = "Reload";
	stateTimeoutValue[2]           = 1;
	stateFire[2]                   = true;
	stateAllowImageChange[2]       = false;
	stateScript[2]                 = "onFire";
	stateWaitForTimeout[2]         = true;

	stateName[3]                   = "Reload";
	stateTransitionOnTriggerUp[3]  = "Ready";
};

function WrathImage::onFire(%this, %obj, %slot)
{
	if((%obj.CDwrath) >= getSimTime())
	{
		if(%obj.client)
		{
			%obj.client.centerPrint("Not ready yet!", 1);
		}
		return;
	}
	if(%obj.getEnergyLevel() > GlobalStorage.WrathEnergy)
	{
		%start = %obj.getEyePoint();
		%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), GlobalStorage.CurseMaxRange));
		%ray = containerRaycast(%start, %end, $Typemasks::TerrainObjectType | $Typemasks::InteriorObjectType | $Typemasks::FXbrickObjectType);
		if(isObject(%hit = firstWord(%ray)))
		{
			if(vectorDist(%start, posFromRaycast(%ray)) < GlobalStorage.CurseMinRange)
			{
				if(isObject(%client = %obj.client))
				{
					%client.centerPrint("That's too close!", 3);
				}
				return;
			}
			%obj.setEnergyLevel(mClamp(%obj.getEnergyLevel() - GlobalStorage.WrathEnergy, 0, 100));
			%obj.CDwrath = getSimTime() + GlobalStorage.WrathCD;
			%cloud = new ParticleEmitterNode()
			{
				datablock = GenericEmitterNode;
				emitter = WrathEmitter;
				position = posFromRaycast(%ray);
				rotation = normalFromRaycast(%ray);
				scale = "25 25 1";
				velocity = 1;
				caster = %obj;
				active = true;
			};
			serverPlay3D(curseWrathSound, %cloud.getPosition());
			WrathTick(%cloud, GlobalStorage.WrathTotalTicks);
		}
	}
	else if(isObject(%obj.client))
	{
		%obj.client.centerPrint("Not enough energy!", 1);
	}
}

function WrathTick(%this, %remain)
{
	if(!isObject(%this))
	{
		return;
	}
	InitContainerRadiusSearch(%this.getPosition(), GlobalStorage.WrathRadius, $Typemasks::PlayerObjectType);
	while(%hit = containerSearchNext())
	{
		if(minigameCanDamage(%this.caster, %hit) == 1 && %this.caster != %hit)
		{
			%hit.damage(%this.caster, %hit.getPosition(), mPow(2, mClamp(%hit.wrathStacks++, 1, 6)), $DamageType::FireM);
			if(%hit.wrathStacks > 3)
			{
				%hit.spawnExplosion(critProjectile, getWord(%hit.getScale(), 2));
			}
			%hit.schedule(GlobalStorage.WrathFalloffTime, WrathTickDecrement);
			if(isObject(%client = %hit.client) && %client.getClassName() $= "GameConnection" && !%hit.stunned)
			{
				%hit.client.centerPrint("\c5Fire! Run!", 2);
			}
			if(%hit.getDamagePercent() >= 1)
			{
				%this.caster.client.unlockAchievement("Master of Hellfire");
			}
		}
	}
	if(%remain > 0)
	{
		schedule(GlobalStorage.WrathTickInterval, 0, WrathTick, %this, %remain--);
	}
	else
	{
		%this.schedule(GlobalStorage.WrathTickInterval, delete);
	}
}

function Player::WrathTickDecrement(%obj)
{
	if(%obj.wrathStacks > 0)
	{
		%obj.wrathStacks--;
	}
}

//	#1.3
datablock AudioProfile(curseDebuffSound)
{
   filename    = $StalkerDM::Path @ "/sounds/curse_debuff.wav";
   description = AudioClose3d;
   preload     = true;
};

datablock ParticleData(RageParticle)
{
	dragCoefficient = 0.1;
	windCoefficient = 0.1;
	gravityCoefficient = -0.02;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 2750;
	lifetimeVarianceMS = 250;
	spinSpeed = 0;
	spinRandomMin = -10;
	spinRandomMax = 10;
	useInvAlpha = false;
	textureName = "base/data/particles/cloud";
	colors[0] = "1 0 0 0.1";
	colors[1] = "1 0 0 0.2";
	colors[2] = "1 0 0 0";
	sizes[0] = 5;
	sizes[1] = 7.5;
	sizes[2] = 12.5;
	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};

datablock ParticleEmitterData(RageEmitter)
{
	ejectionPeriodMS = 35;
	periodVarianceMS = 5;
	ejectionVelocity = 0.15;
	velocityVariance = 0.03;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = false;
	particles = "RageParticle";
	uiName = "Rage Curse";
};

datablock ParticleData(RageAmbientParticle)
{
	dragCoefficient = 0.05;
	windCoefficient = 0.1;
	gravityCoefficient = -0.1;
	inheritedVelFactor = 0.25;
	constantAcceleration = 0;
	lifetimeMS = 1500;
	lifetimeVarianceMS = 250;
	spinSpeed = 0;
	spinRandomMin = -90;
	spinRandomMax = 90;
	useInvAlpha = false;
	textureName = "base/data/particles/cloud";
	colors[0] = "1 0 0 0.1";
	colors[1] = "1 0 0 0.3";
	colors[2] = "1 0 0 0";
	sizes[0] = 0.3;
	sizes[1] = 0.5;
	sizes[2] = 0.75;
	times[0] = 0;
	times[1] = 0.2;
	times[2] = 1;
};

datablock ParticleEmitterData(RageAmbientEmitter)
{
	ejectionPeriodMS = 100;
	periodVarianceMS = 10;
	ejectionVelocity = 0.15;
	velocityVariance = 0.03;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = false;
	particles = "RageAmbientParticle";
	uiName = "Rage Curse Ambient";
};

datablock ItemData(RageItem : DarkMitem)
{
	iconName = $StalkerDM::Path @ "/icons/Icon_Rage";
	image = RageImage;
	uiName = "Rage";
	slot = "Curse";
};

datablock ShapeBaseImageData(RageImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0;
	rotation = "0 0 0";

	correctMuzzleVector = true;

	className = "WeaponImage";
	item = RageItem;
	ammo = " ";
	projectile = DarknessProjectile;
	projectileType = Projectile;

	melee = false;
	armReady = true;

	stateName[0]                   = "Activate";
	stateTimeoutValue[0]           = 0.15;
	stateTransitionOnTimeout[0]    = "Ready";

	stateName[1]                   = "Ready";
	stateTransitionOnTriggerDown[1]= "Fire";
	stateAllowImageChange[1]       = true;
	stateSequence[1]               = "Ready";
	stateEmitter[1]                = RageAmbientEmitter;
	stateEmitterTime[1]            = 600;

	stateName[2]                   = "Fire";
	stateTransitionOnTimeout[2]    = "Reload";
	stateTimeoutValue[2]           = 1;
	stateFire[2]                   = true;
	stateAllowImageChange[2]       = false;
	stateScript[2]                 = "onFire";
	stateWaitForTimeout[2]         = true;

	stateName[3]                   = "Reload";
	stateTransitionOnTriggerUp[3]  = "Ready";
};

function RageImage::onFire(%this, %obj, %slot)
{
	if((%obj.CDrage) >= getSimTime())
	{
		if(%obj.client)
		{
			%obj.client.centerPrint("Not ready yet!", 1);
		}
		return;
	}
	if(%obj.getEnergyLevel() > GlobalStorage.RageEnergy)
	{
		%start = %obj.getEyePoint();
		%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), GlobalStorage.CurseMaxRange));
		%ray = containerRaycast(%start, %end, $Typemasks::TerrainObjectType | $Typemasks::InteriorObjectType | $Typemasks::FXbrickObjectType);
		if(isObject(%hit = firstWord(%ray)))
		{
			if(vectorDist(%start, posFromRaycast(%ray)) < GlobalStorage.CurseMinRange)
			{
				if(isObject(%client = %obj.client))
				{
					%client.centerPrint("You're too close!", 1);
				}
				return;
			}
			%obj.setEnergyLevel(mClamp(%obj.getEnergyLevel() - GlobalStorage.RageEnergy, 0, 100));
			%obj.CDrage = getSimTime() + GlobalStorage.RageCD;
			%cloud = new ParticleEmitterNode()
			{
				datablock = GenericEmitterNode;
				emitter = RageEmitter;
				position = posFromRaycast(%ray);
				rotation = normalFromRaycast(%ray);
				scale = "25 25 1";
				velocity = 1;
				caster = %obj;
				active = true;
			};
			serverPlay3D(curseDebuffSound, %cloud.getPosition());
			RageTick(%cloud, GlobalStorage.RageTotalTicks);
		}
	}
	else if(isObject(%obj.client))
	{
		%obj.client.centerPrint("Not enough energy!", 1);
	}
}

function RageTick(%this, %remain)
{
	if(!isObject(%this))
	{
		return;
	}
	InitContainerRadiusSearch(%this.getPosition(), GlobalStorage.RageRadius, $Typemasks::PlayerObjectType);
	while(%hit = containerSearchNext())
	{
		if(minigameCanDamage(%this.caster, %hit) && %this.caster != %hit)
		{
			%hit.RageCursed+= GlobalStorage.RageTickEffect;
			cancel(%hit.RageEnd);
			%hit.RageEnd = %hit.schedule(GlobalStorage.RageFalloffTime, RageTickDecrement);
			if(isObject(%client = %hit.client) && %client.getClassName() $= "GameConnection" && !%hit.stunned)
			{
				%hit.client.centerPrint("\c5You feel yourself becoming vulnerable - Run!", 2);
			}
		}
	}
	if(%remain > 0)
	{
		schedule(GlobalStorage.RageTickInterval, 0, RageTick, %this, %remain--);
	}
	else
	{
		%this.schedule(GlobalStorage.RageTickInterval, delete);
	}
}

function Player::RageTickDecrement(%obj)
{
	if(%obj.RageCursed > GlobalStorage.RageTickEffect)
	{
		%obj.RageCursed-= GlobalStorage.RageTickEffect;
		%obj.RageEnd = %obj.schedule(1000, RageTickDecrement);
	}
	else
	{
		%obj.RageCursed = 0;
	}
}

//	#1.4
datablock ParticleData(AbyssParticle)
{
	dragCoefficient = 1;
	windCoefficient = 0;
	gravityCoefficient = -0.25;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1750;
	lifetimeVarianceMS = 0;
	spinSpeed = 0;
	spinRandomMin = -10;
	spinRandomMax = 10;
	useInvAlpha = true;
	textureName = "base/data/particles/dot";
	colors[0] = "0 0 0 0.7";
	colors[1] = "0 0 0 0.35";
	colors[2] = "0 0 0 0";
	sizes[0] = 3;
	sizes[1] = 2.5;
	sizes[2] = 2;
	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};

datablock ParticleEmitterData(AbyssEmitter)
{
	ejectionPeriodMS = 10;
	periodVarianceMS = 0;
	ejectionVelocity = 0.1;
	velocityVariance = 0.02;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = false;
	particles = "AbyssParticle";
	uiName = "Abyss Curse";
};

datablock ParticleData(AbyssAmbientParticle : AbyssParticle)
{
	gravityCoefficient = -0.2;
	lifetimeMS = 1000;
	sizes[0] = 0.7;
	sizes[1] = 0.6;
	sizes[2] = 0.3;
};

datablock ParticleEmitterData(AbyssAmbientEmitter : AbyssEmitter)
{
	ejectionPeriodMS = 50;
	ejectionVelocity = 0.3;
	velocityVariance = 0.05;
	particles = "AbyssAmbientParticle";
	uiName = "Abyss Curse Ambient";
};

datablock ItemData(AbyssItem : DarkMitem)
{
	image = AbyssImage;
	uiName = "Abyss";
	slot = "Curse";
};

datablock ShapeBaseImageData(AbyssImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0;
	rotation = "0 0 0";

	correctMuzzleVector = true;

	className = "WeaponImage";
	item = AbyssItem;
	ammo = " ";
	projectile = DarknessProjectile;
	projectileType = Projectile;

	melee = false;
	armReady = true;

	stateName[0]                   = "Activate";
	stateTimeoutValue[0]           = 0.15;
	stateTransitionOnTimeout[0]    = "Ready";

	stateName[1]                   = "Ready";
	stateTransitionOnTriggerDown[1]= "Fire";
	stateAllowImageChange[1]       = true;
	stateSequence[1]               = "Ready";
	stateEmitter[1]                = AbyssAmbientEmitter;
	stateEmitterTime[1]            = 600;

	stateName[2]                   = "Fire";
	stateTransitionOnTimeout[2]    = "Reload";
	stateTimeoutValue[2]           = 1;
	stateFire[2]                   = true;
	stateAllowImageChange[2]       = false;
	stateScript[2]                 = "onFire";
	stateWaitForTimeout[2]         = true;

	stateName[3]                   = "Reload";
	stateTransitionOnTriggerUp[3]  = "Ready";
};

function AbyssImage::onFire(%this, %obj, %slot)
{
	if((%obj.CDAbyss) >= getSimTime())
	{
		if(%obj.client)
		{
			%obj.client.centerPrint("Not ready yet!", 1);
		}
		return;
	}
	if(%obj.getEnergyLevel() > GlobalStorage.AbyssEnergy)
	{
		%start = %obj.getEyePoint();
		%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), GlobalStorage.CurseMaxRange));
		%ray = containerRaycast(%start, %end, $Typemasks::TerrainObjectType | $Typemasks::InteriorObjectType | $Typemasks::FXbrickObjectType);
		if(isObject(%hit = firstWord(%ray)))
		{
			if(vectorDist(%start, posFromRaycast(%ray)) < GlobalStorage.CurseMinRange)
			{
				if(isObject(%client = %obj.client))
				{
					%client.centerPrint("You're too close!", 1);
				}
				return;
			}
			%obj.setEnergyLevel(mClamp(%obj.getEnergyLevel() - GlobalStorage.AbyssEnergy, 0, 100));
			%obj.CDAbyss = getSimTime() + GlobalStorage.AbyssCD;
			%cloud = new ParticleEmitterNode()
			{
				datablock = GenericEmitterNode;
				emitter = AbyssEmitter;
				position = posFromRaycast(%ray);
				rotation = normalFromRaycast(%ray);
				scale = "25 25 1";
				velocity = 1;
				caster = %obj;
				active = true;
			};
			serverPlay3D(curseDebuffSound, %cloud.getPosition());
			AbyssTick(%cloud, GlobalStorage.AbyssTotalTicks);
		}
	}
	else if(isObject(%obj.client))
	{
		%obj.client.centerPrint("Not enough energy!", 1);
	}
}

function AbyssTick(%this, %remain)
{
	if(!isObject(%this))
	{
		return;
	}
	InitContainerRadiusSearch(%this.getPosition(), GlobalStorage.AbyssRadius, $Typemasks::PlayerObjectType);
	while(%hit = containerSearchNext())
	{
		if(minigameCanDamage(%this.caster, %hit) && %this.caster != %hit)
		{
			if(%hit.AbyssCursed++ > GlobalStorage.AbyssBlindTicks)
			{
				%hit.blind(2000);
			}
			%hit.slow(0.5, 2000);
			cancel(%hit.AbyssEnd);
			%hit.AbyssEnd = %hit.schedule(GlobalStorage.AbyssFalloffTime, AbyssTickDecrement);
			if(isObject(%client = %hit.client) && %client.getClassName() $= "GameConnection" && !%hit.stunned)
			{
				%hit.client.centerPrint("\c5Your eyes and legs are failing - Run!", 2);
			}
		}
	}
	if(%remain > 0)
	{
		schedule(GlobalStorage.AbyssTickInterval, 0, AbyssTick, %this, %remain--);
	}
	else
	{
		%this.schedule(GlobalStorage.AbyssTickInterval, delete);
	}
}

function Player::AbyssTickDecrement(%obj)
{
	if(%obj.getDatablock() == nameToID(PlayerHumanSnaredArmor))
	{
		%obj.popDatablock(PlayerHumanSnaredArmor);
	}
	if(%obj.AbyssCursed > 1)
	{
		%obj.AbyssCursed-= 1;
		%obj.AbyssEnd = %obj.schedule(GlobalStorage.AbyssTickInterval, AbyssTickDecrement);
	}
	else
	{
		%obj.AbyssCursed = 0;
	}
}

//	#1.5
datablock ParticleData(PsychosisParticle)
{
	dragCoefficient = 1;
	windCoefficient = 0;
	gravityCoefficient = -0.25;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 500;
	lifetimeVarianceMS = 0;
	spinSpeed = 0;
	spinRandomMin = -10;
	spinRandomMax = 10;
	useInvAlpha = true;
	textureName = "base/data/particles/thinring";
	colors[0] = "1 0 0 0.3";
	colors[1] = "1 1 0 0.3";
	colors[2] = "0 1 1 0.3";
	colors[3] = "1 0 1 0.3";
	sizes[0] = 0.3;
	sizes[1] = 0.3;
	sizes[2] = 0.3;
	sizes[3] = 0.3;
	times[0] = 0;
	times[1] = 0.33;
	times[2] = 0.67;
	times[3] = 1;
};

datablock ParticleEmitterData(PsychosisEmitter)
{
	ejectionPeriodMS = 7;
	periodVarianceMS = 0;
	ejectionVelocity = 0.1;
	velocityVariance = 0.02;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = false;
	particles = "PsychosisParticle";
	uiName = "Psychosis Curse";
};

datablock ParticleData(PsychosisAmbientParticle : PsychosisParticle)
{
	gravityCoefficient = -0.2;
	lifetimeMS = 500;
	sizes[0] = 0.5;
	sizes[1] = 0.4;
	sizes[2] = 0.3;
	sizes[3] = 0.2;
};

datablock ParticleEmitterData(PsychosisAmbientEmitter : PsychosisEmitter)
{
	ejectionPeriodMS = 30;
	ejectionVelocity = 0.3;
	velocityVariance = 0.05;
	particles = "PsychosisAmbientParticle";
	uiName = "Psychosis Curse Ambient";
};

datablock ItemData(PsychosisItem : DarkMitem)
{
	iconName = $StalkerDM::Path @ "/icons/Icon_Psychosis";
	image = PsychosisImage;
	uiName = "Psychosis";
	slot = "Curse";
};

datablock ShapeBaseImageData(PsychosisImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0;
	rotation = "0 0 0";

	correctMuzzleVector = true;

	className = "WeaponImage";
	item = PsychosisItem;
	ammo = " ";
	projectile = DarknessProjectile;
	projectileType = Projectile;

	melee = false;
	armReady = true;

	stateName[0]                   = "Activate";
	stateTimeoutValue[0]           = 0.15;
	stateTransitionOnTimeout[0]    = "Ready";

	stateName[1]                   = "Ready";
	stateTransitionOnTriggerDown[1]= "Fire";
	stateAllowImageChange[1]       = true;
	stateSequence[1]               = "Ready";
	stateEmitter[1]                = PsychosisAmbientEmitter;
	stateEmitterTime[1]            = 600;

	stateName[2]                   = "Fire";
	stateTransitionOnTimeout[2]    = "Reload";
	stateTimeoutValue[2]           = 1;
	stateFire[2]                   = true;
	stateAllowImageChange[2]       = false;
	stateScript[2]                 = "onFire";
	stateWaitForTimeout[2]         = true;

	stateName[3]                   = "Reload";
	stateTransitionOnTriggerUp[3]  = "Ready";
};

function PsychosisImage::onFire(%this, %obj, %slot)
{
	if((%obj.CDPsychosis) >= getSimTime())
	{
		if(%obj.client)
		{
			%obj.client.centerPrint("Not ready yet!", 1);
		}
		return;
	}
	if(%obj.getEnergyLevel() > GlobalStorage.PsychosisEnergy)
	{
		%start = %obj.getEyePoint();
		%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), GlobalStorage.CurseMaxRange));
		%ray = containerRaycast(%start, %end, $Typemasks::TerrainObjectType | $Typemasks::InteriorObjectType | $Typemasks::FXbrickObjectType);
		if(isObject(%hit = firstWord(%ray)))
		{
			if(vectorDist(%start, posFromRaycast(%ray)) < GlobalStorage.CurseMinRange)
			{
				if(isObject(%client = %obj.client))
				{
					%client.centerPrint("You're too close!", 1);
				}
				return;
			}
			%obj.setEnergyLevel(mClamp(%obj.getEnergyLevel() - GlobalStorage.PsychosisEnergy, 0, 100));
			%obj.CDPsychosis = getSimTime() + GlobalStorage.PsychosisCD;
			%cloud = new ParticleEmitterNode()
			{
				datablock = GenericEmitterNode;
				emitter = PsychosisEmitter;
				position = posFromRaycast(%ray);
				rotation = normalFromRaycast(%ray);
				scale = "25 25 1";
				velocity = 1;
				caster = %obj;
				active = true;
			};
			//serverPlay3D(curseDebuffSound, %cloud.getPosition());
			PsychosisTick(%cloud, GlobalStorage.PsychosisTotalTicks);
		}
	}
	else if(isObject(%obj.client))
	{
		%obj.client.centerPrint("Not enough energy!", 1);
	}
}

function PsychosisTick(%this, %remain)
{
	if(!isObject(%this))
	{
		return;
	}
	InitContainerRadiusSearch(%this.getPosition(), GlobalStorage.PsychosisRadius, $Typemasks::PlayerObjectType);
	while(%hit = containerSearchNext())
	{
		if(minigameCanDamage(%this.caster, %hit) && %this.caster != %hit && !%hit.dummy)
		{
			%hit.neurotic = true;
			%pos = %hit.getPosition();
			%xy = getRandom(-16, 16) SPC getRandom(-16, 16);
			%start = vectorAdd(%pos, %xy SPC 5);
			%end = vectorAdd(%pos, %xy SPC -100);
			%ray = containerRaycast(%start, %end, $Typemasks::TerrainObjectType | $Typemasks::InteriorObjectType | $Typemasks::FXbrickObjectType);
			%dummy = new AiPlayer()
			{
				datablock = PlayerStalkerBloodleechArmor;
				position = posFromRaycast(%ray);
				maxPitchSpeed = 100;
				maxYawSpeed = 100;
				dummy = true;
				target = %hit;
			};
			%dummy.hideNode("Larm");
			%dummy.hideNode("Rarm");
			%dummy.unhideNode("Larmslim");
			%dummy.unhideNode("Rarmslim");
			%dummy.setNodeColor("ALL", "0.078 0.078 0.078 1");
			%dummy.mountImage(CloakingImage, 2);
			%dummy.mountImage($ItemList::Object["Knife", getRandom(0, $ItemList::Count["Knife"] - 1)].image, 0);
			%dummy.fixArms();
			%dummy.setAimObject(%hit);
			%dummy.setMoveX(getRandom() * getRandom(-1, 1));
			%dummy.setMoveY(1);
			%dummy.schedule(10000, mountImage, CloakingImage, 2);
			%dummy.schedule(10150, delete);
		}
	}
	if(%remain > 0)
	{
		schedule(GlobalStorage.PsychosisTickInterval, 0, PsychosisTick, %this, %remain--);
	}
	else
	{
		%this.schedule(GlobalStorage.PsychosisTickInterval, delete);
	}
}