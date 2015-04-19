//////////////////////
//STALKER DM: MAGICS//
//////////////////////

////////////
//CONTENTS//
////////////
//#1. MAGICS
//	#1.1 Darkness (modifications)
//	#1.2 Paralysis
//	#1.3 Shadowstep
//	#1.4 Gripping Shadows
//	#1.5 Schizophrenia

//#1.
//	#1.1
DarkMitem.slot = "Magic";
DarkMitem.uiName = "Darkness";

//DarkBlindPlayerImage.shapeFile = "Add-Ons/Weapon_ElementalSpells/ItemShapes/Dark.dts";
//DarkBlindPlayerImage.stateEmitter[0] = "";

function DarkBlindPlayerImage::onMount(%this, %obj, %slot)
{
	serverPlay3d(DarkBlindSound, %obj.getEyePoint());
	parent::onMount(%this, %obj, %slot);
}

function DarkBlindPlayerImage::Dismount(%this, %obj, %slot)
{
}

function DarknessProjectile::Damage(%this, %obj, %col, %fade, %pos, %normal)
{
	if(%col.getType() & $Typemasks::PlayerObjectType)
	{
		if(%col.getMountedImage(0) == nameToID(ShieldImage))
		{
			if(vectorDot(%col.getForwardVector(), %obj.getForwardVector()) >= -0.5)
			{
				%p = new Projectile()
				{
					datablock = darknessProjectile;
					initialPosition = %col.getMuzzlePoint(0);
					initialVelocity = %col.getMuzzleVector(0);
					sourceObject = 0;
					sourceSlot = 0;
					scale = "1 1 1";
					client = 0;
				};
				%p.explode();
				%backfire = 1;
			}
		}
		%obj.setEnergyLevel(%obj.getEnergyLevel() - GlobalStorage.BlindEnergy);
		if(%backfire)
		{
			%obj.mountImage(DarkBlindPlayerImage, 1);
			%obj.schedule(GlobalStorage.BlindDuration, unmountImage, 1);
			%obj.damage(%col, posFromRaycast(%ray), 0, $DamageType::Shield); //for assistkills credit
			return;
		}
		%col.blind(3000, 1);
	}
}

function DarkMImage::onFire(%this, %obj, %slot)
{
	if((%obj.CDdarkness) >= getSimTime())
	{
		if(%obj.client)
		{
			%obj.client.centerPrint("Not ready yet!", 1);
		}
		return;
	}
	if(%obj.getEnergyLevel() >= GlobalStorage.BlindEnergy)
	{
		%start = %obj.getEyePoint();
		%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), GlobalStorage.MagicRange));
		%ray = conalRaycast(%start, %obj.getEyeVector(), GlobalStorage.MagicRange, GlobalStorage.MagicSection, $Typemasks::PlayerObjectType, %obj);
		if(!isObject(%hit = firstWord(%ray)))
		{
			%ray = containerRaycast(%start, %end, $Typemasks::PlayerObjectType, %obj);
			%hit = firstWord(%ray);
		}
		if(isObject(%hit) && minigameCanDamage(%obj, %hit) == 1)
		{
			%obj.CDdarkness = getSimTime() + GlobalStorage.BlindCD;
			DarknessProjectile.damage(%obj, %hit, 0, posFromRaycast(%ray), normalFromRaycast(%ray));
		}
	}
	else if(isObject(%client = %obj.getControllingClient()))
	{
		%client.centerPrint("Not enough energy!", 1);
	}
}

//	#1.2
datablock ParticleData(ParalysisAmbientParticle)
{
	dragCoefficient = 1.75;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1500;
	lifetimeVarianceMS = 500;
	textureName = "base/data/particles/dot";
	spinSpeed = 0;
	spinRandomMin = -300;
	spinRandomMax = 300;
	useInvAlpha = true;

	colors[0] = "0 0 0 1";
	colors[1] = "0 0.05 0.1 0";
	sizes[0] = 0.15;
	sizes[1] = 0.3;
};

datablock ParticleEmitterData(ParalysisAmbientEmitter)
{
	ejectionPeriodMS = 15;
	periodVarianceMS = 0;
	ejectionVelocity = -0.3;
	velocityVariance = 0.15;
	ejectionOffset = 0.4;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = false;
	particles = ParalysisAmbientParticle;

	uiName = "Paralysis - Ambient";
};

datablock ExplosionData(ParalysisExplosion : DarkMExplosion)
{
	particleEmitter = ParalysisAmbientEmitter;
	particleRadius = 0.25;
	emitter[1] = ParalysisAmbientEmitter;
	uiName = "Paralysis";
};

datablock ProjectileData(ParalysisProjectile : DarknessProjectile)
{
	explosion = ParalysisExplosion;
	particleEmitter = ParalysisAmbientEmitter;
	uiName = "Paralysis";
};

datablock ItemData(ParalysisMitem : DarkMItem)
{
	shapeFile = $StalkerDM::Path @ "/models/Paralysis.dts";
	iconName = $StalkerDM::Path @ "/icons/Icon_Paralysis";
	image = ParalysisMimage;
	uiName = "Paralysis";
	slot = "Magic";
};

datablock ShapeBaseImageData(ParalysisMImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0;
	rotation = "0 0 0";

	correctMuzzleVector = true;

	className = "WeaponImage";
	item = ParalysisMItem;
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
	stateEmitter[1]                = ParalysisAmbientEmitter;
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

function ParalysisMimage::onFire(%this, %obj, %slot)
{
	if((%obj.CDparalysis) >= getSimTime())
	{
		if(%obj.client)
		{
			%obj.client.centerPrint("Not ready yet!", 1);
		}
		return;
	}
	if(%obj.getEnergyLevel() > GlobalStorage.ParaEnergy)
	{
		%start = %obj.getEyePoint();
		%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), GlobalStorage.MagicRange));
		%ray = conalRaycast(%start, %obj.getEyeVector(), GlobalStorage.MagicRange, GlobalStorage.MagicSection, $Typemasks::PlayerObjectType, %obj);
		if(!isObject(%hit = firstWord(%ray)))
		{
			%ray = containerRaycast(%start, %end, $Typemasks::PlayerObjectType, %obj);
			%hit = firstWord(%ray);
		}
		if(isObject(%hit) && minigameCanDamage(%obj, %hit) == 1)
		{
			if(%hit.getMountedImage(0) == nameToID(ShieldImage))
			{
				if(vectorDot(%hit.getForwardVector(), %obj.getForwardVector()) >= -0.5)
				{
					%p = new Projectile()
					{
						datablock = paralysisProjectile;
						initialPosition = %hit.getMuzzlePoint(0);
						initialVelocity = %hit.getMuzzleVector(0);
						sourceObject = 0;
						sourceSlot = 0;
						scale = "1 1 1";
						client = 0;
					};
					%p.explode();
					%backfire = 1;
				}
			}
			%obj.setEnergyLevel(mClamp(%obj.getEnergyLevel() - GlobalStorage.ParaEnergy, 0, 100));
			%obj.CDparalysis = getSimTime() + GlobalStorage.ParaCD;
			if(%backfire)
			{
				%obj.stun(GlobalStorage.ParaLength, 0, 1);
				%obj.damage(%obj, posFromRaycast(%ray), 0, $DamageType::Shield);
				%obj.spawnExplosion(ParalysisProjectile, 2);
				return;
			}
			%hit.stun(GlobalStorage.ParaLength, 0, 1);
			%hit.damage(%obj, posFromRaycast(%ray), 0, $DamageType::FireM); //for assistkills credit
			%hit.spawnExplosion(ParalysisProjectile, 2);
			%hit.paraData = %obj SPC getSimTime();
			%obj.stun(GlobalStorage.ParaBackfireLength, 0, 1);
			%obj.spawnExplosion(ParalysisProjectile, 2);
		}
	}
	else if(isObject(%client = %obj.client) && %client.getClassName() $= "GameConnection")
	{
		%client.centerPrint("Not enough energy!", 1);
	}
}

//	#1.3
datablock ParticleData(ShadowstepParticle : ParalysisAmbientParticle)
{
	dragCoefficient = 0;
	gravityCoefficient = -0.02;
	colors[0] = "0 0 0 1";
	colors[1] = "0.1 0 0.07 0";
	sizes[0] = 0.3;
	sizes[1] = 0.2;
};

datablock ParticleEmitterData(ShadowstepEmitter : ParalysisAmbientEmitter)
{
	ejectionVelocity = 0;
	velocityVariance = 0;
	ejectionOffset = 0.3;
	particles = ShadowstepParticle;

	uiName = "Shadowstep - Ambient";
};

datablock ItemData(ShadowstepItem : DarkMItem)
{
	iconName = $StalkerDM::Path @ "/icons/Icon_Shadowstep";
	image = ShadowstepImage;
	uiName = "Shadowstep";
	slot = "Magic";
};

datablock ShapeBaseImageData(ShadowstepImage : ParalysisMImage)
{
	item = ShadowstepItem;
	breaksCloak = false;
	stateEmitter[1] = ShadowstepEmitter;
};

function ShadowstepImage::onFire(%this, %obj, %slot)
{
	if((%obj.CDshadowstep) >= getSimTime())
	{
		if(isObject(%client = %obj.client) && %client.getClassName() $= "GameConnection")
		{
			%client.centerPrint("Not ready yet!", 1);
		}
		return;
	}
	if(%obj.getEnergyLevel() > GlobalStorage.SSEnergy)
	{
		%start = %obj.getEyePoint();
		%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), GlobalStorage.MagicRange));
		%ray = conalRaycast(%start, %obj.getEyeVector(), GlobalStorage.MagicRange, GlobalStorage.MagicSection, $Typemasks::PlayerObjectType, %obj);
		if(!isObject(%hit = firstWord(%ray)))
		{
			%ray = containerRaycast(%start, %end, $Typemasks::PlayerObjectType, %obj);
			%hit = firstWord(%ray);
		}
		if(isObject(%hit) && minigameCanDamage(%obj, %hit) == 1)
		{
			if(%hit.getMountedImage(0) == nameToID(ShieldImage))
			{
				if(vectorDot(%hit.getForwardVector(), %obj.getForwardVector()) >= -0.5)
				{
					%p = new Projectile()
					{
						datablock = paralysisProjectile;
						initialPosition = %hit.getMuzzlePoint(0);
						initialVelocity = %hit.getMuzzleVector(0);
						sourceObject = 0;
						sourceSlot = 0;
						scale = "1 1 1";
						client = 0;
					};
					%p.explode();
					%backfire = 1;
				}
			}
			if(%backfire)
			{
				%tpos = vectorAdd(vectorSub(%hit.getPosition(), vectorScale(%hit.getForwardVector(), -5)), "0 0 0.1");
				if(%obj.isCloaked())
				{
					%obj.cloak();
				}
			}
			else
			{
				%tpos = vectorAdd(vectorSub(%hit.getPosition(), vectorScale(%hit.getForwardVector(), 2)), "0 0 0.1");
			}
			%typemasks = $Typemasks::PlayerObjectType | $Typemasks::VehicleObjectType | $Typemasks::TerrainObjectType | $Typemasks::InteriorObjectType | $Typemasks::FXbrickObjectType | $TypeMasks::StaticShapeObjectType;
			%ray2 = containerRaycast(%tpos, %hit.getHackPosition(), %typemasks, %hit);
			%ray3 = containerRaycast(vectorAdd(%tpos, "0 0 3"), %tpos, %typemasks);
			if(isObject(firstWord(%ray2)) || isObject(firstWord(%ray3)))
			{
				if(isObject(%client = %obj.client) && %client.getClassName() $= "GameConnection")
				{
					%client.centerPrint("Not enough space!", 1);
				}
				return;
			}
			%obj.mountImage(%obj.tool[0].image, 0);
			%obj.spawnExplosion(ParalysisProjectile, 2);
			%obj.setEnergyLevel(mClamp(%obj.getEnergyLevel() - GlobalStorage.SSEnergy, 0, 100));
			%obj.CDshadowstep = getSimTime() + GlobalStorage.SSCD;
			%obj.setTransform(%tpos SPC rotFromTransform(%hit.getTransform()));
			%obj.setVelocity("0 0 0");
			%obj.SS[%hit] = getSimTime();
		}
	}
	else if(isObject(%client = %obj.client) && %client.getClassName() $= "GameConnection")
	{
		%client.centerPrint("Not enough energy!", 1);
	}
}

//	#1.4
datablock ParticleData(GSAmbientParticle)
{
	dragCoefficient = 1.75;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1500;
	lifetimeVarianceMS = 500;
	textureName = "base/data/particles/dot";
	spinSpeed = 0;
	spinRandomMin = -300;
	spinRandomMax = 300;
	useInvAlpha = true;

	colors[0] = "0 0 0 1";
	colors[1] = "0.15 0 0 0";
	sizes[0] = 0.15;
	sizes[1] = 0.3;
};

datablock ParticleEmitterData(GSAmbientEmitter)
{
	ejectionPeriodMS = 15;
	periodVarianceMS = 0;
	ejectionVelocity = -0.3;
	velocityVariance = 0.15;
	ejectionOffset = 0.4;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = false;
	particles = GSAmbientParticle;

	uiName = "Gripping Shadows Ambient";
};

datablock ParticleData(GSParticle)
{
	dragCoefficient = 1.75;
	windCoefficient = 0;
	gravityCoefficient = -0.05;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1500;
	lifetimeVarianceMS = 500;
	textureName = "base/data/particles/dot";
	spinSpeed = 0;
	spinRandomMin = -300;
	spinRandomMax = 300;
	useInvAlpha = true;

	colors[0] = "0.15 0 0 0.1";
	colors[1] = "0 0 0 1";
	sizes[0] = 0.4;
	sizes[1] = 0.2;
};

datablock ParticleEmitterData(GSemitter)
{
	ejectionPeriodMS = 15;
	periodVarianceMS = 0;
	ejectionVelocity = -1.5;
	velocityVariance = 0.3;
	ejectionOffset = 1;
	thetaMin = 85;
	thetaMax = 95;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = false;
	particles = GSParticle;

	uiName = "Gripping Shadows";
};

datablock ExplosionData(GSExplosion : DarkMExplosion)
{
	lifeTimeMS = GlobalStorage.GSLength;

	particleEmitter = GSemitter;
	particleDensity = 50;
	particleRadius = 1.5;

	emitter[0] = GSemitter;

	faceViewer = true;
	explosionScale = "1 1 1";

	shakeCamera = false;
	camShakeFreq = "30 30 30";
	camShakeAmp = "7 2 7";
	camShakeDuration = 0.6;
	camShakeRadius = 2.5;

	lightStartRadius = 2;
	lightEndRadius = 0;
	lightStartColor = "0.5 0 0";
	lightEndColor = "0 0 0";

	uiName = "Gripping Shadows";
};

datablock ProjectileData(GSProjectile : DarknessProjectile)
{
	explosion = GSExplosion;
	particleEmitter = "";
	uiName = "Gripping Shadows";
};

datablock ItemData(GSitem : DarkMItem)
{
	iconName = $StalkerDM::Path @ "/icons/Icon_GrippingShadows";
	image = GSimage;
	uiName = "Gripping Shadows";
	slot = "Magic";
};

datablock ShapeBaseImageData(GSImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0;
	rotation = "0 0 0";

	correctMuzzleVector = true;

	className = "WeaponImage";
	item = GSItem;
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
	stateEmitter[1]                = GSAmbientEmitter;
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

function GSimage::onFire(%this, %obj, %slot)
{
	if((%obj.CDgrippingshadows) >= getSimTime())
	{
		if(%obj.client)
		{
			%obj.client.centerPrint("Not ready yet!", 1);
		}
		return;
	}
	if(%obj.getEnergyLevel() > GlobalStorage.GSenergy)
	{
		%start = %obj.getEyePoint();
		%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), GlobalStorage.MagicRange));
		%ray = conalRaycast(%start, %obj.getEyeVector(), GlobalStorage.MagicRange, GlobalStorage.MagicSection, $Typemasks::PlayerObjectType, %obj);
		if(!isObject(%hit = firstWord(%ray)))
		{
			%ray = containerRaycast(%start, %end, $Typemasks::PlayerObjectType, %obj);
			%hit = firstWord(%ray);
		}
		if(isObject(%hit) && minigameCanDamage(%obj, %hit) == 1)
		{
			if(%hit.getMountedImage(0) == nameToID(ShieldImage))
			{
				if(vectorDot(%hit.getForwardVector(), %obj.getForwardVector()) >= -0.5)
				{
					%p = new Projectile()
					{
						datablock = paralysisProjectile;
						initialPosition = %hit.getMuzzlePoint(0);
						initialVelocity = %hit.getMuzzleVector(0);
						sourceObject = 0;
						sourceSlot = 0;
						scale = "1 1 1";
						client = 0;
					};
					%p.explode();
					%backfire = 1;
				}
			}
			%obj.setEnergyLevel(mClamp(%obj.getEnergyLevel() - GlobalStorage.GSEnergy, 0, 100));
			%obj.CDgrippingshadows = getSimTime() + GlobalStorage.GSCD;
			if(!%backfire)
			{
				%hit.slow(1, GlobalStorage.GSLength);
				%proj = new Projectile()
				{
					datablock = GSprojectile;
					initialPosition = %hit.getPosition();
					initialVelocity = "0 0 0";
					scale = %hit.getScale();
				};
				%proj.schedule(1, explode);
				%hit.damage(%obj, posFromRaycast(%ray), 0, $DamageType::FireM); //for assistkills credit
			}
			else
			{
				%obj.slow(1, GlobalStorage.GSLength);
				%proj = new Projectile()
				{
					datablock = GSprojectile;
					initialPosition = %obj.getPosition();
					initialVelocity = "0 0 0";
					scale = %obj.getScale();
				};
				%proj.schedule(1, explode);
				%obj.damage(%hit, posFromRaycast(%ray), 0, $DamageType::Shield);
			}
		}
	}
	else if(isObject(%client = %obj.client) && %client.getClassName() $= "GameConnection")
	{
		%client.centerPrint("Not enough energy!", 1);
	}
}

//	#1.5
datablock ParticleData(SchizophreniaAmbientParticle)
{
	dragCoefficient = 1.75;
	windCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	constantAcceleration = 0;
	lifetimeMS = 1500;
	lifetimeVarianceMS = 500;
	spinSpeed = 0;
	spinRandomMin = -10;
	spinRandomMax = 10;
	useInvAlpha = true;
	textureName = "base/data/particles/thinring";
	colors[0] = "1 0 0 0.3";
	colors[1] = "1 1 0 0.3";
	colors[2] = "0 1 1 0.3";
	colors[3] = "1 0 1 0.3";
	sizes[0] = 0.1;
	sizes[1] = 0.16;
	sizes[2] = 0.23;
	sizes[3] = 0.3;
	times[0] = 0;
	times[1] = 0.33;
	times[2] = 0.67;
	times[3] = 1;
};

datablock ParticleEmitterData(SchizophreniaAmbientEmitter)
{
	ejectionPeriodMS = 15;
	periodVarianceMS = 0;
	ejectionVelocity = 1.5;
	velocityVariance = 1;
	ejectionOffset = 0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = false;
	particles = SchizophreniaAmbientParticle;
	uiName = "Schizophrenia";
};

datablock ItemData(SchizophreniaItem : DarkMItem)
{
	iconName = $StalkerDM::Path @ "/icons/Icon_Psychosis";
	image = SchizophreniaImage;
	uiName = "Schizophrenia";
	slot = "Magic";
};

datablock ShapeBaseImageData(SchizophreniaImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0;
	rotation = "0 0 0";

	correctMuzzleVector = true;

	className = "WeaponImage";
	item = SchizophreniaItem;
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
	stateEmitter[1]                = SchizophreniaAmbientEmitter;
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

function SchizophreniaImage::onFire(%this, %obj, %slot)
{
	if((%obj.CDschizophrenia) >= getSimTime())
	{
		if(%obj.client)
		{
			%obj.client.centerPrint("Not ready yet!", 1);
		}
		return;
	}
	if(%obj.getEnergyLevel() > GlobalStorage.SchizophreniaEnergy)
	{
		%start = %obj.getEyePoint();
		%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), GlobalStorage.MagicRange));
		%ray = conalRaycast(%start, %obj.getEyeVector(), GlobalStorage.MagicRange, GlobalStorage.MagicSection, $Typemasks::PlayerObjectType, %obj);
		if(!isObject(%hit = firstWord(%ray)))
		{
			%ray = containerRaycast(%start, %end, $Typemasks::PlayerObjectType, %obj);
			%hit = firstWord(%ray);
		}
		if(isObject(%hit) && minigameCanDamage(%obj, %hit) == 1)
		{
			if(%hit.getMountedImage(0) == nameToID(ShieldImage))
			{
				if(vectorDot(%hit.getForwardVector(), %obj.getForwardVector()) >= -0.5)
				{
					%backfire = 1;
				}
			}		
			%obj.setEnergyLevel(mClamp(%obj.getEnergyLevel() - GlobalStorage.SchizophreniaEnergy, 0, 100));
			%obj.CDschizophrenia = getSimTime() + GlobalStorage.SchizophreniaCD;
			if(!%backfire)
			{
				%hit.SchizophreniaEnd = getSimTime() + GlobalStorage.SchizophreniaDuration;
				%hit.SchizophreniaTick();
				%hit.damage(%obj, posFromRaycast(%ray), 0, $DamageType::FireM); //for assistkills credit
			}
			else
			{
				%obj.SchizophreniaEnd = getSimTime() + GlobalStorage.SchizoDuration;
				%obj.SchizophreniaTick();
				%obj.damage(%hit, posFromRaycast(%ray), 0, $DamageType::Shield); //for assistkills credit
			}
		}
	}
	else if(isObject(%client = %obj.client) && %client.getClassName() $= "GameConnection")
	{
		%client.centerPrint("Not enough energy!", 1);
	}
}

function Player::SchizophreniaTick(%obj)
{
	%r = mFloor(getRandom() + 0.5);
	%obj.setDamageFlash(0.4 + getRandom() * 0.3);
	//%obj.emote("Pain" @ getWord("Low Mid", %r) @ "Image");
	if(isObject(%client = %obj.client) && %client.getClassName() $= "Gameconnection")
	{
		%client.play2d(PainCrySound);
		if(%obj.client.tdmTeam == 0)
		{
			//stalker
			%sounds = "PistolFireSound PumpShotgunFireSound";
			%client.play2d(getWord(%sounds, %r));
		}
		else
		{
			//human
			%client.play2d("l4bMacheteHitSound" @ getSubStr("AB", %r, %r + 1));
		}
	}
	if(getSimTime() > %obj.SchizophreniaEnd)
	{
		%obj.neurotic = false;
		%obj.neurosis = 0;
		return;
	}
	else
	{
		%obj.neurotic = true;
		%obj.neurosis+= getRandom(7, 20);
	}
	%obj.SchizophreniaSched = %obj.schedule(getRandom(750, 2000), SchizophreniaTick);
}