////////////////////
//STALKER DM: MISC//
////////////////////

//////////////
// CONTENTS //
//////////////
//#1. FUNCTION CREATION
//	#1.1 player::Cloak & player::partialCloak
//	#1.2 player::isOnGround
//	#1.3 gameconnection::removeShapename
//	#1.4 gameconnection::inventoryInform
//	#1.5 getVectorAngle
//	#1.6 rotateVector
//	#1.7 flashlight functionality
//	#1.8 ammo replenishment
//	#1.9 bottomprint HUD
//	#1.12 cloak emitter
//	#1.13 serverCmdSetHandicap
//	#1.14 gameConnection::isStatisticallyRelevant
//   #1.15 getTeamFromObject
//#2. PACKAGE
//	#2.1 armor::onTrigger
//	#2.2 player::cloakSched
//	#2.3 player::damage
//	#2.4 Misc achievement-related functions
//	#2.6 servercmdLight
//	#2.7 gameconnection::createplayer
//	#2.8 gameconnection::autoAdminCheck
//	#2.9 CTF overwrites (flag->artifact)
//	#2.10 ctfFlagItem::onPickup
//	#2.11 serverCmdAlarm
//	#2.12 armor::onCollision
//	#2.13 serverCmdDropItem
//	#2.14 anti-typekill
//	#2.15 MinigameSO::pickSpawnPoint
//	#2.16 PistolImage::on(Un)Mount
//	#2.17 ShapeBaseImageData::on(Un)Mount
//	#2.18 player::stun
//	#2.19 minigameCanDamage
//	#2.20 gameConnection::onDrop
//	#2.21 armor::onRemove
//   #2.22 minigameSO::addMember
//   #2.23 minigameSO::removeMember
//   #2.24 gameConnection::applyBodyColors
//   #2.25 gameConnection::applyBodyParts
//   #2.26 cameraData::onTrigger
//#3. SUPPORT SCRIPTS
//	#3.1 InventorySlotsAdjustment package
//	#3.2 CTF Flag -> Artifact model switch

function serverCmdnoxalia(%client)
{
	%client.unlockAchievement("Cheater");
}

new ScriptObject(statsTable)
{
	totalStalkerKills = 0;
	totalHumanKills = 0;
	totalStalkerWins = 0;
	totalHumanWins = 0;
};

function statsTable::Reset(%this)
{
	%this.delete();
	%this = new ScriptObject(statsTable)
	{
		totalStalkerKills = 0;
		totalHumanKills = 0;
		totalStalkerWins = 0;
		totalHumanWins = 0;
	};
	return %this;
}

function statsTable::ReturnData(%this)
{
	%sk = %this.totalStalkerKills;
	%hk = %this.totalHumanKills;
	%sw = %this.totalStalkerWins;
	%hw = %this.totalHumanWins;
	messageAll('', "\c7Kills Balance: " @ %sk @ "/" @ %hk SPC "stalkers/humans");
	messageAll('', "\c7Victory Balance: " @ %sw @ "/" @ %hw SPC "stalkers/humans");
}

function statsTable::ReturnWeaponData(%this)
{
	messageAll('', "\c7Pistol: " @ statsTable.PistolDmg @ "dmg / " @ statsTable.PistolDeaths @ "lvs (" @ (statsTable.PistolDmg / statsTable.PistolDeaths) @ "dpl)");
	messageAll('', "\c7Shotgun: " @ statsTable.ShotgunDmg @ "dmg / " @ statsTable.ShotgunDeaths @ "lvs (" @ (statsTable.ShotgunDmg / statsTable.ShotgunDeaths) @ "dpl)");
	messageAll('', "\c7Assault R.: " @ statsTable.AssaultRifleDmg @ "dmg / " @ statsTable.AssaultRifleDeaths @ "lvs (" @ (statsTable.AssaultRifleDmg / statsTable.AssaultRifleDeaths) @ "dpl)");
	messageAll('', "\c7Flamethrower: " @ statsTable.FlamethrowerDmg @ "dmg / " @ statsTable.FlamethrowerDeaths @ "lvs (" @ (statsTable.FlamethrowerDmg / statsTable.FlamethrowerDeaths) @ "dpl)");
	messageAll('', "\c7Stalker Knife: " @ statsTable.StalkerKnifeDmg @ "dmg / " @ statsTable.StalkerKnifeDeaths @ "lvs (" @ (statsTable.StalkerKnifeDmg / statsTable.StalkerKnifeDeaths) @ "dpl)");
	messageAll('', "\c7Momentum Knife: " @ statsTable.MomentumKnifeDmg @ "dmg / " @ statsTable.MomentumKnifeDeaths @ "lvs (" @ (statsTable.MomentumKnifeDmg / statsTable.MomentumKnifeDeaths) @ "dpl)");
	messageAll('', "\c7Serrated Knife: " @ statsTable.SerratedKnifeDmg @ "dmg / " @ statsTable.SerratedKnifeDeaths @ "lvs (" @ (statsTable.SerratedKnifeDmg / statsTable.SerratedKnifeDeaths) @ "dpl)");
	messageAll('', "\c7Bloodlust Knife: " @ statsTable.BloodlustKnifeDmg @ "dmg / " @ statsTable.BloodlustKnifeDeaths @ "lvs (" @ (statsTable.BloodlustKnifeDmg / statsTable.BloodlustKnifeDeaths) @ "dpl)");
	messageAll('', "\c7Shadow Knife: " @ statsTable.ShadowKnifeDmg @ "dmg / " @ statsTable.ShadowKnifeDeaths @ "lvs (" @ (statsTable.ShadowKnifeDmg / statsTable.ShadowKnifeDeaths) @ "dpl)");
}

function statsTable::Export(%this)
{
}

function servercmdSVHstats(%client)
{
	if(%client.isAdmin)
	{
		statsTable.returnData();
	}
}
function servercmdSVHweaponstats(%client)
{
	if(%client.isAdmin)
	{
		statsTable.returnWeaponData();
	}
}

//#1.
//	#1.1
function Player::Cloak(%this)
{
	if(!%this.isCloaked() && %this.combatEnd < getSimTime() && (%this.lastCloakTime + GlobalStorage.CloakCD) < getSimTime())
	{
		cancel(%this.recloakSched);
		%this.setImageTrigger(0, 0);
		%this.mountImage(CloakingImage, 2);
		%this.setCloaked(1);
		%this.setNodeColor("ALL", "0 0 0 0");
		%this.partialCloak = false;
		if(isObject(%this.ctfFlagSpawnBrick))
		{
			%this.dropCTFflag();
		}
		return;
	}
	if(%this.isCloaked())
	{
		cancel(%this.recloakSched);
		%this.lastCloakTime = getSimTime();
		%this.mountImage(CloakingImage, 2);
		%this.setCloaked(0);
		%this.setNodeColor("ALL", "0 0 0 1");
		%this.partialCloak = false;
	}
}
function Player::PartialCloak(%this, %time, %revert)
{
	if(!%this.isCloaked())
	{
		%this.partialCloak = false;
		return;
	}
	if(!%revert && !isEventPending(%this.recloakSched))
	{
		%this.partialCloak = true;
		%this.setNodeColor("ALL", "0 0 0 0.5");
	}
	else if(%revert)
	{
		%this.partialCloak = false;
		%this.setNodeColor("ALL", "0 0 0 0");
		return;
	}
	cancel(%this.recloakSched);
	%this.recloakSched = %this.schedule(%time, PartialCloak, 0, true);
}

//	#1.2
function Player::isOnGround(%obj)
{
	%pos = %obj.getPosition();
	%end = vectorAdd(%pos, "0 0 -0.1");
	%typemasks = $TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::InteriorObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::VehicleObjectType;
	%ray = containerRayCast(%pos, %end, %typemasks, %obj);
	if(isObject(%hit = firstWord(%ray)))
	{
		if(%hit.getClassName() !$= "FxDTSbrick")
		{
			return true;
		}
		else if(%hit.isColliding())
		{
			return true;
		}
	}
	return false;
}

//	#1.3
function GameConnection::RemoveShapename(%c)
{
	if(!isObject(%oo = %c.player))
	{
		return;
	}
	%data = %oo.getDatablock();
	%hp = %oo.getDamageLevel();
	%energy = %oo.getEnergyLevel();
	%no = new player()
	{
		position = %oo.getposition();
		rotation = rotfromtransform(%oo.gettransform());
		client = %c;
		datablock = %oo.getdatablock();
		spawnTime = getSimTime();
	};
	%no.setTransform(%oo.getTransform());
	%no.setDamageLevel(%hp);
	%no.setEnergyLevel(%energy);
	%c.player = %no;
	for(%i = 0; %i < %oo.getDatablock().maxTools; %i++)
	{
		%no.tool[%i] = %oo.tool[%i];
	}
	%oo.delete();
	%c.setcontrolobject(%no);
	%c.applybodycolors();
	%c.applybodyparts();
}

//	#1.4
function GameConnection::InventoryInform(%client)
{
	if(isObject(%obj = %this.player))
	{
		%c = %obj.getDatablock().maxTools;
		for(%i = 0; %i < %c; %i++)
		{
			messageClient(%client, 'MsgItemPickup', "", %i, %obj.tool[%i]);
		}
	}
}

//	#1.5
function getVectorAngle(%vec1, %vec2)
{
	return mRadToDeg(mACos(vectorDot(vectorNormalize(%vec1), vectorNormalize(%vec2))));
}

//	#1.6
function RotateVector(%sourceVector, %angle)
{
	%x = getWord(%sourceVector, 0);
	%y = getWord(%sourceVector, 1);
	%nx = %x * mCos(%angle) - %y * mSin(%angle);
	%ny = %x * mSin(%angle) - %y * mCos(%angle);
	setWord(%sourceVector, 0, %nx);
	setWord(%sourceVector, 1, %ny);
	return %sourceVector;
}

//	#1.7

//This emitter is a modified form of TinyLightEmitter by Devastator
datablock ParticleData(FlashlightParticle)
{
	lifetimeMS = 20;
	lifetimeVarianceMS = 0;
	textureName = "base/data/particles/dot";
	dragCoefficient = 0;
	gravityCoefficient = 0;
	inheritedVelFactor = 0;
	windCoefficient = 0;
	constantAcceleration = 0;
	spinSpeed = 0;
	spinRandomMin = 0;
	spinRandomMax = 0;
	useInvAlpha = false;
	colors[0] = "1 1 0.5 0.2";
	colors[1] = "1 1 0.5 0.1";
	colors[2] = "1 1 0.5 0";
	sizes[0] = 0.1;
	sizes[1] = 1.5;
	sizes[2] = 3;
	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};

datablock ParticleEmitterData(FlashlightEmitter)
{
	ejectionVelocity = 250;
	particles = FlashlightParticle;
	uiName = "Flashlight";
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionOffset = 0;
	velocityVariance = 0;
	thetaMin = 0;
	thetaMax = 0;
	phiReferenceVel = 0;
	phiVariance = 0;
	overrideAdvance = false;
	useEmitterColors = false;
};

datablock fxLightData(FlashlightLight)
{
	uiName = "";

	LightOn = true;
	radius = 4;
	brightness = 2;
	color = "1 1 0.5 1";

	flareOn = false;
	flarebitmap = "base/lighting/corona";
	NearSize = 2;
	FarSize = 1;
};

datablock ShapeBaseImageData(FlashlightImageR)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 1.25 0.13";
	eyeOffset = 0;
	rotation = "0 0 0";

	correctMuzzleVector = true;

	//className = "WeaponImage";
	item = "";
	ammo = " ";
	projectile = "";
	projectileType = "";

	melee = false;
	armReady = false;
	firstPersonParticles = false;

	stateName[0]                = "state0";
	stateTimeoutValue[0]        = 0.04;
	stateTransitionOnTimeout[0] = "state0";
	stateScript[0]              = "onFire";
	stateFire[0]                = true;
	stateEmitter[0]             = FlashlightEmitter;
	stateEmitterTime[0]         = 600;
};
datablock ShapeBaseImageData(FlashlightImageL : FlashlightImageR)
{
	shapeFile = "Add-Ons/Weapon_Package_Tier1/handgun.2.dts";
	doColorShift = true;
	colorShiftColor = "0.73 0.73 0.73 1";
	offset = "0 0 0";
	mountPoint = 1;
	armReady = true;
	stateEmitterNode[0] = "Light";
};

function FlashlightImageR::onFire(%this, %obj, %slot)
{
	if(%obj.getEnergyLevel() < 0.15 || %obj.getDamagePercent() >= 1)
	{
		%obj.schedule(0, unmountImage, %slot);
		return;
	}
	%start = %obj.getMuzzlePoint(%slot);
	%vec = %obj.getMuzzleVector(%slot);
	%end = vectorAdd(%start, vectorScale(%vec, 200));
	%typemasks = $Typemasks::PlayerObjectType | $Typemasks::VehicleObjectType | $Typemasks::TerrainObjectType | $Typemasks::InteriorObjectType | $Typemasks::FXbrickObjectType | $TypeMasks::StaticShapeObjectType;
	%ray = containerRaycast(%start, %end, %typemasks, %obj);
	%ePos = posFromRaycast(%ray);
	%eNorm = normalFromRaycast(%ray);
	if(!isObject(%obj.flashlight))
	{
		%light = new FxLight()
		{
			datablock = FlashlightLight;
			position = vectorAdd(%ePos, vectorScale(%eNorm, 0.5));
			scale = "1 1 1";
			obj = %obj;
		};
		MissionCleanup.add(%light);
		%obj.flashlight = %light;
	}
	%obj.flashlight.setTransform(vectorAdd(%ePos, vectorScale(%eNorm, 0.5)) SPC "1 0 0 0");
	%obj.flashlight.reset();
	InitContainerRadiusSearch(%ePos, GlobalStorage.LightRadius, $Typemasks::PlayerObjectType);
	while(%hit = containerSearchNext())
	{
		if(%hit.isCloaked())
		{
			%hit.partialCloak(35);
		}
	}
	%obj.setEnergyLevel(%obj.getEnergyLevel() - 0.15);
}
function FlashlightImageL::onFire(%this, %obj, %slot)
{
	FlashlightImageR::onFire(%this, %obj, %slot);
}

function FlashlightImageR::onUnmount(%this, %obj, %slot)
{
	if(isObject(%obj.flashlight))
	{
		%obj.flashlight.delete();
	}
	return parent::onUnmount(%this, %obj, %slot);
}
function FlashlightImageL::onUnmount(%this, %obj, %slot)
{
	if(isObject(%obj.flashlight))
	{
		%obj.flashlight.delete();
	}
	return parent::onUnmount(%this, %obj, %slot);
}

//	#1.8
function player::AmmoRegenTick(%this)
{
	if(%this.rate <= 0)
	{
		switch$(%this.tool[0].getName())
		{
			case "PistolItem": %rate = GlobalStorage.PistolRegen;
				%max = GlobalStorage.PistolAmmo;
			case "PumpShotgunItem": %rate = GlobalStorage.ShotgunRegen;
				%max = GlobalStorage.ShotgunAmmo;
			case "TAssaultRifleItem": %rate = GlobalStorage.ARregen;
				%max = GlobalStorage.ARammo;
			default: return;
		}
		%this.rate = mFloatLength(60 / %rate, 3) * 1000;
		%this.max = %max;
		%this.toolAmmo[0] = %max;
	}
	if(%this.toolAmmo[0] < %this.max)
	{
		%this.toolAmmo[0]++;
		%this.setImageAmmo(0, true);
	}
	%this.schedule(%this.rate, AmmoRegenTick);
}

//	#1.9
function Player::BottomPrintHUDupdate(%this)
{
	if(!isObject(%this.client))
	{
		return;
	}
	%data = %this.getDatablock();
	%time = getSimTime();
	%start = %this.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%this.getEyeVector(), 32));
	%raycast = containerRaycast(%start, %end, $Typemasks::PlayerObjectType, %this);
	if(isObject(%col = firstWord(%raycast)))
	{
		if(%col.client.tdmteam == %this.client.tdmteam)
		{
			%targetname = "\c2" @ %col.client.name @ "(" @ (%col.getDatablock().maxDamage - mFloatLength(%col.getDamageLevel(), 1)) @ "/" @ %col.getDatablock().maxDamage @ ")";
		}
		else if(!%col.isCloaked())
		{
			%targetname = "<color:ff0000>" @ %col.client.name;
		}
	}
	%myMaxHealth = %data.maxDamage;
	%myHealth = (%myMaxHealth + %this.tempHealth) - mClampF(%this.neurotic ? %this.getDamageLevel() + %this.neurosis : %this.getDamageLevel(), 0, 100);
	%center = "          \c2HP: " @ mFloatLength(%myHealth, 1) @ "/" @ %myMaxHealth @ "    \c3Target: " @ (%targetname !$= "" ? %targetname : "\c0<None>");
	if(%this.client.tdmTeam == 0)
	{
		if(%this.isCloaked())
		{
			%cloakstatus = "\c3Cloak: " @ (%this.partialCloak ? "\c0Partial" : "\c2Cloaked");
		}
		else
		{
			%cloakstatus = "\c3Cloak: " @ (%this.combatEnd <= %time ? "\c2Ready" : "\c0In Combat");
		}
		%left = %cloakstatus;
	}
	else
	{
		switch$(%this.tool[0].uiName)
		{
			case "Flamethrower":
				%ammo = "\c6Gasoline: \c3" @ mFloatLength(%this.getEnergyLevel(), 0) @ "/" @ %data.maxEnergy;
			case "A.C.R.A.":
				%totalCurHealth = 0;
				%totalMaxHealth = 0;
				InitContainerRadiusSearch(%this.getPosition(), GlobalStorage.ACRAradius, $Typemasks::PlayerObjectType);
				while(%hit = containerSearchNext())
				{
					if(%hit.client.minigame == %this.client.minigame && (minigameCanDamage(%this, %hit) == 0) || %this == %hit)
					{
						%totalCurHealth+= %hit.getDatablock().maxDamage - %hit.getDamageLevel();
						%totalMaxHealth+= %hit.getDatablock().maxDamage;
					}
				}
				%ammo = "\c6Health pool: \c2" @ mFloatLength(%totalCurHealth, 1) @ "/" @ %totalMaxHealth;
			default:
				%ammo = "\c6Ammo: \c3" @ %this.toolAmmo[0] @ "/" @ %this.max;
		}
		%left = %ammo;
	}
	for(%i = 0; %i < 3; %i++)
	{
		%cd[%i] = mClamp(mFloatLength((%this.CD[strLwr(strReplace(%this.tool[%i].uiName, " ", ""))] - %time) / 1000, 0), 0, 600);
		%cd[%i] = (%cd[%i] > 0 ? "\c0" : "\c2") @ %cd[%i];
	}
	%cds = "\c6Cooldowns: " @ %cd[0] @ "\c3/" @ %cd[1] @ "\c3/" @ %cd[2];
	%right = "<just:right>" @ %cds;
	%this.client.bottomPrint(%left @ %center @ %right, 1);
	%this.schedule(200, BottomPrintHUDupdate);
}

//	#1.12
datablock ParticleData(CloakingParticle)
{
	dragCoefficient = 2;
	windCoefficient = 0;
	gravityCoefficient = -0.25;
	inheritedVelFactor = 1;
	constantAcceleration = 0;
	lifetimeMS = 750;
	lifetimeVarianceMS = 250;
	spinSpeed = 0;
	spinRandomMin = -150;
	spinRandomMax = 150;
	useInvAlpha = true;
	textureName = "base/data/particles/cloud";
	colors[0] = "0 0 0 0.1";
	colors[1] = "0 0 0 0.7";
	colors[2] = "0 0 0 0";
	sizes[0] = 1;
	sizes[1] = 1;
	sizes[2] = 1;
	times[0] = 0;
	times[1] = 0.5;
	times[2] = 1;
};

datablock ParticleEmitterData(CloakingEmitter)
{
	className = ParticleEmitterData;
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = -3;
	velocityVariance = 1;
	ejectionOffset = 1.5;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	particles = "CloakingParticle";
};

datablock ShapeBaseImageData(CloakingImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = false;

	mountPoint = $BackSlot;
	rotation = "1 0 0 -90";
	offset = "0 0 -0.5";
	eyeOffset = "0 -5 0";

	stateName[0]					= "Ready";
	stateTransitionOnTimeout[0]		= "FireA";
	stateTimeoutValue[0]			= 0.01;

	stateName[1]					= "FireA";
	stateTransitionOnTimeout[1]		= "Done";
	stateWaitForTimeout[1]			= true;
	stateTimeoutValue[1]			= 0.15;
	stateEmitter[1]					= CloakingEmitter;
	stateEmitterTime[1]				= 0.15;

	stateName[2]					= "Done";
	stateScript[2]					= "onDone";
};
function CloakingImage::onDone(%this,%obj,%slot)
{
	%obj.unMountImage(%slot);
}

//	#1.13
function serverCmdSetHandicap(%this, %amt)
{
	if(%amt > 0 && %amt <= 1 && !%this.file.readOnly)
	{
		%this.file.handicap = %amt;
		messageAll('', "\c3" @ %this.name @ " is now playing at " @ mFloatLength(%amt * 100, 0) @ "% strength.");
	}
}

//	#1.14
function gameConnection::isStatisticallyRelevant(%this) //for various reasons I want to exclude guests and new players from statistics collection
{
	if(!%this.file.readOnly)
	{
		if(%this.file.read("PointsT") >= 25)
		{
			return true;
		}
	}
	return false;
}

// #1.15
function getTeamFromObject(%obj)
{
	if(!isObject(%obj))
	{
		return -1;
	}
	switch$(%obj.getClassName())
	{
		case "Projectile": return getTeamFromObject(%obj.sourceObject);
		case "Player": return %obj.client.tdmTeam;
		case "GameConnection": return %obj.tdmTeam;
		case "AIPlayer": return 1;
		case "FxDtsBrick": return -1;
		case "Item": return -1;
		default: announce("getTeamFromObject(" @ %obj.getClassName() @ ") ???"); //!!!
	}
}

//#2.
package StalkerDM
{
	//	#2.1
	function armor::OnTrigger(%this, %obj, %slot, %val)
	{
		%obj.immune = 0;
		cancel(%obj.EndImmuneSched);
		if(%slot == 3)
		{
			%obj.crouching = %val;
		}
		if(%val && %this.isStalker)
		{
			if(%slot == 4)
			{
				%obj.cloakSched(true);
			}
			else if(%slot == 0)
			{
				if(isObject(%obj.getMountedImage(0)))
				{
					if(%obj.isCloaked())
					{
						if(%obj.getMountedImage(0).breaksCloak !$= "0")
						{
							%obj.CloakSched(true);
						}
						else
						{
							%obj.setEnergyLevel(%obj.getEnergyLevel() - 10);
						}
					}
				}
			}
		}
		if(%slot == 4 && %val)
		{
			if(%obj.getMountedImage(0) == StimpakImage.getID())
			{
				if(%obj.CDstimpak > getSimTime())
				{
					if(isObject(%obj.client) && %obj.client.getClassName() $= "GameConnection")
					{
						%obj.client.centerPrint("Not ready yet!", 1);
					}
				}
				else
				{
					%start = %obj.getEyePoint();
					%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 10));
					%typemasks = $Typemasks::PlayerObjectType | $Typemasks::VehicleObjectType | $Typemasks::TerrainObjectType | $Typemasks::InteriorObjectType | $Typemasks::FXbrickObjectType | $TypeMasks::StaticShapeObjectType;
					%raycast = containerRaycast(%start, %end, %typemasks, %obj);
					if(isObject(%col = firstWord(%raycast)) && %col.getType() & $Typemasks::PlayerObjectType)
					{
						if(%col.client.tdmTeam == 1)
						{
							if(%col.getDamageLevel() > 0 || GlobalStorage.StimpakOverheal)
							{
								%col.stimpakHeal(%obj);
								%obj.CDstimpak = getSimTime() + GlobalStorage.StimpakCD;
								if(isObject(%obj.client) && %obj.client.getClassName() $= "GameConnection")
								{
									if(strLen(%col.client.name))
									{
										%obj.client.centerPrint("\c2Healed " @ %col.client.name, 3);
									}
								}
							}
							else
							{
								if(isObject(%obj.client) && %obj.client.getClassName() $= "GameConnection")
								{
									%obj.client.centerPrint("That player has full health!", 2);
								}
							}
						}
					}
				}
			}
			else if(%obj.client.tdmTeam == 1)
			{
				if(isObject(%obj.tool1.image))
				{
					%obj.tool1.image.onFire(%obj, 0);
				}
			}
		}
		return parent::OnTrigger(%this, %obj, %slot, %val);
	}
	function armor::onRemove(%this, %obj)
	{
		if(isObject(%obj.client) && !isObject(%obj.client.getControlObject()))
		{
			%obj.client.setControlObject(%obj.client.camera);
		}
		if(isObject(%obj.flashlight))
		{
			%obj.flashlight.delete();
		}
		return parent::onRemove(%this, %obj);
	}
	//	#2.2
	function Player::CloakSched(%this, %val)
	{
		if(%val)
		{
			if(%this.isCloaked())
			{
				%this.cloak();
				return;
			}
			if(%this.getEnergyLevel() >= 5)
			{
				%this.cloak();
			}
			else
			{
				return;
			}
		}
		if(!isObject(%this) || !%this.isCloaked())
		{
			return;
		}
		if(%this.tool[0] == nameToID(StalkerShadowKnifeItem) || GlobalStorage.CloakUsesEnergy)
		{
			if(%v = vectorLen(%this.getVelocity()))
			{
				if((%lev = mClamp(%this.getEnergyLevel() - %v / 5, 0, 100)) == 0)
				{
					%this.setEnergyLevel(0);
					%this.cloak();
					return;
				}
				%this.setEnergyLevel(%lev);
			}
		}
		cancel(%this.cloakSched);
		%this.cloakSched = %this.schedule(100, cloakSched);
	}
	//	#2.3
	function Player::Damage(%this, %sObj, %pos, %amt, %type)
	{
		if(%this.immune && vectorLen(%this.getVelocity()) == 0)
		{
			return;
		}
		if(%this.dummy)
		{
			%this.target.neurotic = false;
			%this.mountImage(CloakingImage, 2);
			%this.schedule(100, delete);
		}
		if($StalkersImmune && %this.client.tdmTeam == 0)
		{
			return;
		}
		else if($HumansImmune && %this.client.tdmTeam == 1)
		{
			return;
		}
		if(isObject(%this))
		{
			%maxHealing = %this.getDatablock().maxDamage - %this.getDamageLevel();
		}
		if(%this.usingACRA == true)
		{
			%amt*= 2;
		}
		if(%this.RageCursed > 0)
		{
			%amt*= (1 + %this.RageCursed);
		}
		if(%this.getMountedImage(1) == nameToID(DarkBlindPlayerImage))
		{
			%this.unmountImage(1);
		}
		if(%this.isCloaked())
		{
			%this.setEnergyLevel(mClamp(%this.getEnergyLevel() - %amt, 0, 100));
			%this.partialCloak(GlobalStorage.CombatLength);
			%flarecredit = 1;
		}
		if(isObject(%sObj))
		{
			switch$(%sObj.getClassName())
			{
				case "Player": %obj = %sObj;
				case "Projectile": %obj = %sObj.sourceObject;
				case "GameConnection": %obj = %sObj.player;
				case "AIconnection": %obj = isObject(%sObj.player) ? %sObj.player : %sObj.bot;
			}
			if(isObject(%obj))
			{
				if(%obj.client.file.handicap != 0)
				{
					%amt*= %obj.client.file.handicap;
				}
				%obj.immune = 0;
				cancel(%obj.endImmuneSched);
				if(%this.getMountedImage(0) == nameToID(ShieldImage))
				{
					if(vectorDot(%this.getForwardVector(), %obj.getForwardVector()) >= -0.5 && vectorDist(%this.getPosition(), %obj.getPosition()) <= GlobalStorage.KnifeRange && isObject(%sObj) && %sObj != %this)
					{
						%p = new Projectile()
						{
							datablock = hammerProjectile;
							initialPosition = %this.getMuzzlePoint(0);
							initialVelocity = %this.getMuzzleVector(0);
							sourceObject = 0;
							sourceSlot = 0;
							scale = "1 1 1";
							client = 0;
						};
						%p.explode();
						return;
					}
				}
				if(%amt > 0)
				{
					%obj.dealtDamage = true;
				}
				%time = getSimTime();
				if(%obj.client.TDMteam == 0)
				{
					if(%this.tool[2] == nameToID(ArmorItem) && %amt > 0)
					{
						%vec1 = vectorNormalize(VectorSub(setWord(%obj.getPosition(), 2, 0), setWord(%this.getPosition, 2, 0)));
						%vec2 = %this.getForwardVector();
						%ang1 = getVectorAngle(%vec2, %vec1);
						if(%ang1 > 135)
						{
							%amt*= 0.8;
						}
						else if(%ang1 < 45)
						{
							%amt*= 0.5;
						}
						else
						{
							%amt*= 0.65;
						}
					}
					if(%this.tempHealth > 0 && %amt > 0)
					{
						%temp = %this.tempHealth;
						%this.tempHealth = mClampF(%temp - %amt, 0, 100);
						%amt = mClampF(%amt - %temp, 0, %amt);
					}
					if(%this.attack[%obj] > 0 && %this.attack[%obj] + 30000 <= %time)
					{
						%obj.client.unlockAchievement("Forgetful");
					}
					%this.attack[%obj] = %time;
					if(%type != $DamageType::FireM && %type != $DamageType::Bleed) //wrath/bleeds should not put the caster in combat
					{
						%obj.combatEnd = %time + GlobalStorage.CombatLength;
					}
					if(%obj.getDatablock() == $PlayerStalkerBloodleechArmor && minigameCanDamage(%this, %obj) == 1 && %this != %obj && %type != $DamageType::FireM && %amt > 0)
					{
						if(%obj.getDamageLevel() > 0)
						{
							%obj.addHealth(mClamp(%amt, 0, %maxHealing) * (GlobalStorage.LeechEfficiency / 2));
							%obj.emote(HealImage,1);
						}
						InitContainerRadiusSearch(%pos, GlobalStorage.LeechRadius, $Typemasks::PlayerObjectType);
						while(%hit = containerSearchNext())
						{
							if(%hit.client.tdmTeam == 0)
							{
								if(!%hit.isCloaked() && %hit.getDamageLevel() > 0)
								{
									%hit.emote(HealImage,1);
								}
								%hit.addHealth(mClamp(%amt, 0, %maxHealing) * (GlobalStorage.LeechEfficiency / 2));
							}
						}
					}
				}
				else if(%obj.client.TDMteam == 1)
				{
					if(%obj.getMountedImage(1) == nameToID(DarkBlindPlayerImage))
					{
						%obj.client.unlockAchievement("Shot in the Dark");
					}
					%this.combatEnd = %time + GlobalStorage.CombatLength;
					%this.BLKdamage = mClamp(%this.BLKdamage - %amt * GlobalStorage.LustLossPercent, 0, 100);
					if($DamageType_Array[%type] $= "L4Pistol")
					{
						if((%obj.SHLFdamage+= %amt) >= 100)
						{
							%obj.client.unlockAchievement("Pistol Specialist");
						}
						schedule(3000, 0, eval, %obj @ ".SHLFdamage-= " @ %amt @ ";");
					}
					if($DamageType_Array[%type] $= "PumpShotgun")
					{
						if((%obj.PSSdamage+= GlobalStorage.ShotgunDamage) >= (GlobalStorage.ShotgunDamage * GlobalStorage.ShotgunShellcount))
						{
							%obj.client.unlockAchievement("Shotgun Specialist");
						}
						schedule(100, 0, eval, %obj @ ".PSSdamage-= " @ GlobalStorage.ShotgunDamage @ ";");
					}
					if(%flarecredit && %obj.lastFlareCredit + 5000 <= %time)
					{
						%obj.lastFlareCredit = %time;
						if(%obj.flarecredit++ >= 3)
						{
							%obj.client.unlockAchievement("Eye of the Tiger");
						}
					}
				}
			}
		}
		%p = parent::Damage(%this, %sObj, %pos, %amt, %type);
		if(!isObject(%obj))
		{
			return;
		}
		if(%this != %obj && %amt > 0)
		{
			if(isObject(%this.client) && isObject(%obj.client) && %this.client.isStatisticallyRelevant() && %obj.client.isStatisticallyRelevant())
			{
				switch$($DamageType_Array[%type])
				{
					case "StalkerKnife": statsTable.StalkerKnifeDmg+= %amt;
					case "StalkerKnifeBackstab": statsTable.StalkerKnifeDmg+= %amt;
					case "StalkerMomentumKnife": statsTable.MomentumKnifeDmg+= %amt;
					case "StalkerMomentumKnifeBackstab": statsTable.MomentumKnifeDmg+= %amt;
					case "StalkerSerratedKnife": statsTable.SerratedKnifeDmg+= %amt;
					case "StalkerSerratedKnifeBackstab": statsTable.SerratedKnifeDmg+= %amt;
					case "Bleed": statsTable.SerratedKnifeDmg+= %amt;
					case "StalkerBloodlustKnife": statsTable.BloodlustKnifeDmg+= %amt;
					case "StalkerBloodlustKnifeBackstab": statsTable.BloodlustKnifeDmg+= %amt;
					case "StalkerShadowKnife": statsTable.ShadowKnifeDmg+= %amt;
					case "StalkerShadowKnifeBackstab": statsTable.ShadowKnifeDmg+= %amt;
					case "Flamethrower": statsTable.FlamethrowerDmg+= %amt;
					case "PumpShotgun": statsTable.ShotgunDmg+= %amt;
					case "L4Pistol": statsTable.PistolDmg+= %amt;
					case "ARifle": statsTable.AssaultRifleDmg+= %amt;
				}
			}
		}
		if(%this.getDamagePercent() >= 1)
		{
			if(isObject(%obj.client)
			{
				%obj.client.incScore(1);
			}
			if(%this.stunned)
			{
				%this.unstun();
			}
			if(%this.dealtdamage !$= "1")
			{
				if(%this.client.shitcred++ > 2)
				{
					%this.client.unlockAchievement("Shitty Player");
				}
			}
			if(%obj.client.tdmTeam == 0)
			{
				if(%obj.SS[%this] >= %time - 5000)
				{
					%obj.client.unlockAchievement("Master of Shadowstep");
				}
				if(%this.paraData)
				{
					%caster = getWord(%this.paraData, 0);
					%time = getWord(%this.paraData, 1);
					if(%time > %time - GlobalStorage.ParaLength)
					{
						%caster.client.unlockAchievement("Master of Paralysis");
					}
				}
				if(%this.neurotic)
				{
					%obj.client.unlockAchievement("Master of Neurosis");
				}
				if(%this.RageCursed >= 1)
				{
					%obj.client.unlockAchievement("Master of Fury");
				}
				if(%obj.client.humankills++ >= 10)
				{
					%obj.client.unlockAchievement("10 Human Kills");
				}
				else if(%obj.client.humankills >= 25)
				{
					%obj.client.unlockAchievement("25 Human Kills");
				}
				else if(%obj.client.humankills >= 50)
				{
					%obj.client.unlockAchievement("50 Human Kills");
				}
				else if(%obj.client.humankills >= 100)
				{
					%obj.client.unlockAchievement("100 Human Kills");
				}
				if(%obj.client.minigame.numMembers >= 6)
				{
					statsTable.totalStalkerKills++;
				}
				if(!%obj.isOnGround())
				{
					if(%obj.midairKills++ >= 3)
					{
						%obj.client.unlockAchievement("Batman");
					}
				}
				schedule(120000, 0, eval, %obj @ ".GHkills--;");
				if(%obj.GHkills++ >= 5)
				{
					%obj.client.unlockAchievement("Group Hysteria");
				}
				if(%obj.kills[%this.client.bl_id]++ >= 3)
				{
					%obj.client.unlockAchievement("Dominatrix");
				}
			}
			else if(%obj.client.tdmTeam == 1)
			{
				if(%obj.client.stalkerkills++ >= 10)
				{
					%obj.client.unlockAchievement("10 Stalker Kills");
				}
				if(%obj.client.stalkerkills >= 25)
				{
					%obj.client.unlockAchievement("25 Stalker Kills");
				}
				if(%obj.client.stalkerkills >= 50)
				{
					%obj.client.unlockAchievement("50 Stalker Kills");
				}
				if(%obj.client.stalkerkills >= 100)
				{
					%obj.client.unlockAchievement("100 Stalker Kills");
				}
				if(%obj.client.minigame.numMembers >= 6)
				{
					statsTable.totalHumanKills++;
				}
				if($DamageType_Array[%type] $= "L4Pistol" && getWord(%this.getPosition(), 2) + 3 < getWord(%obj.getPosition(), 2))
				{
					if(%obj.AOSkills++ >= 3)
					{
						%obj.client.unlockAchievement("Assault Specialist");
					}
				}
				if(%flarecredit && getWord(%pos, 2) - 3 > getWord(%obj.getPosition(), 2))
				{
					%obj.client.unlockAchievement("Pierce the Heavens");
				}
				if(!%this.isOnGround())
				{
					%obj.client.unlockAchievement("Duck Hunt");
				}
			}
		}
		else
		{
			if((%this.damageTaken+= %amt) >= 200 && %this.client.tdmTeam == 1)
			{
				%this.client.unlockAchievement("Stabproof Vest");
			}
		}
		return %p;
	}
	//	#2.6
	function serverCmdLight(%client)
	{
		if(%client.tdmTeam == 0 && isObject(%client.player) && %client.player.getDamagePercent() < 1)
		{
			%center = %client.player.getEyePoint();
			%radius = 50;
			%closest = 100;
			InitContainerRadiusSearch(%center, %radius, $Typemasks::PlayerObjectType);
			while(%hit = containerSearchNext())
			{
				if(%hit.client.tdmTeam == 0 || !%hit.client)
				{
					continue;
				}
				%ct++;
				%dist = vectorLen(vectorSub(%center, %hit.getPosition()));
				%closest = %dist < %closest ? %dist : %closest;
				%co = %hit;
			}
			if(isObject(%co))
			{
				%vec1 = vectorNormalize(VectorSub(setWord(%co.getPosition(), 2, 0), setWord(%center, 2, 0)));
				%vec2 = %client.player.getForwardVector();
				%ang1 = getVectorAngle(%vec2, %vec1);
				if(%ang1 > 135)
				{
					%dir = "to the back";
				}
				else if(%ang1 < 45)
				{
					%dir = "to the front";
				}
				else
				{
					%vec1 = vectorNormalize(VectorSub(setWord(%co.getPosition(), 2, 0), setWord(%center, 2, 0)));
					%vec2 = RotateVector(%client.player.getForwardVector(), $pi / 4);
					%ang1 = getVectorAngle(%vec2, %vec1);
					if(%ang1 < 45)
					{
						%dir = "to the right";
					}
					else
					{
						%dir = "to the left";
					}
				}
				%zDiff = getWord(%center, 2) - getWord(%co.getPosition(), 2);
				if(%zDiff > 3)
				{
					%dir = %dir SPC "and below";
				}
				else if(%zdiff < -3)
				{
					%dir = %dir SPC "and above";
				}
			}
			switch(mFloor(%closest / 10))
			{
				case 0: %d = ", right next to you.";
				case 1: %d = ", close to you.";
				case 2: %d = ", nearby.";
				case 3: %d = ", a stone's throw away.";
				default: %d = ", a ways away.";
			}
			switch(%ct)
			{
				case 0: %n = "no humans"; %d = ".";
				case 1: %n = "a lone human";
				case 2: %n = "a couple of humans";
				case 3: %n = "a few humans";
				case 4: %n = "several humans";
				default: %n = "many humans";
			}
			%client.centerprint("\c3You can smell " @ %n @ %d @ (isObject(%co) ? "\n<color:800000>Aprox. Location of nearest human: " @ %dir : ""), 3);
		}
		else if(%client.tdmTeam == 1 && isObject(%client.player) && %client.player.getDamagePercent() < 1)
		{
			if(%client.player.tool[0] == PistolItem.getID())
			{
				%obj = %client.player;
				if(%obj.getMountedImage(1) == FlashlightImageR.getID() || %obj.getMountedImage(1) == FlashlightImageL.getID())
				{
					%obj.unmountImage(1);
					%obj.fixArms();
				}
				else if(%obj.getEnergyLevel() > 0.1 && %obj.getMountedImage(1) != NameToID(DarkBlindPlayerImage))
				{
					if(%obj.getMountedImage(0) == nameToID(PistolImage))
					{
						%obj.mountImage(FlashlightImageR, 1);
					}
					else
					{
						%obj.mountImage(FlashlightImageL, 1);
						%obj.fixArms();
					}
				}
			}
			else
			{
				%client.centerPrint("\c3Only the pistol has a flashlight on it!", 4);
			}
		}
		else
		{
			return parent::serverCmdLight(%client);
		}
	}
	//	#2.7
	function GameConnection::SpawnPlayer(%client)
	{
		%p = parent::SpawnPlayer(%client);
		%client.removeShapename();
		%id = %client.bl_id;
		%obj = %client.player;
		if(%client.TDMteam == 0)
		{
			//stalker
			commandToClient(%client, 'NightVision', 1);
			%obj.schedule(0, setDatablock, PlayerStalkerBloodleechArmor);
			%obj.tool[0] = nameToID(%client.file.read("Loadout_Knife"));
			%obj.tool[1] = nameToID(%client.file.read("Loadout_Magic"));
			%obj.tool[2] = nameToID(%client.file.read("Loadout_Curse"));
			if(%client.isStatisticallyRelevant())
			{
				switch$(%obj.client.file.read("Loadout_Knife"))
				{
					case "StalkerKnifeItem": statsTable.StalkerKnifeDeaths++;
					case "StalkerMomentumKnifeItem": statsTable.MomentumKnifeDeaths++;
					case "StalkerSerratedKnifeItem": statsTable.SerratedKnifeDeaths++;
					case "StalkerBloodlustKnifeItem": statsTable.BloodlustKnifeDeaths++;
					case "StalkerShadowKnifeItem": statsTable.ShadowKnifeDeaths++;
				}
			}
		}
		else
		{
			//human
			commandToClient(%client, 'NightVision', 0);
			%obj.schedule(0, setDatablock, PlayerHumanArmor);
			%obj.tool[0] = nameToID(%client.file.read("Loadout_Gun"));
			if(%obj.tool[0] == nameToID(ACRAitem))
			{
				%obj.mountImage(ACRAbothImage, 2);
			}
			%obj.schedule(1000, ammoRegenTick);
			%obj.tool[1] = nameToID(%client.file.read("Loadout_Secondary"));
			if(%obj.tool[1] == nameToID(ProxShockItem))
			{
				%obj.mountImage(ProxShockImage, 3);
			}
			%obj.tool[2] = nameToID(%client.file.read("Loadout_Melee"));
			if(%client.isStatisticallyRelevant())
			{
				switch$(%obj.client.file.read("Loadout_Gun"))
				{
					case "PistolItem": statsTable.PistolDeaths++;
					case "PumpShotgunItem": statsTable.ShotgunDeaths++;
					case "TAssaultRifleItem": statsTable.AssaultRifleDeaths++;
					case "FlamethrowerItem": statsTable.FlamethrowerDeaths++;
				}
			}
		}
		schedule(0, 0, commandToClient, %client, 'ShowEnergyBar', true);
		%obj.schedule(1000, BottomPrintHUDupdate);
		%client.schedule(0, InventoryInform);
		return %p;
	}
	//	#2.8
	function GameConnection::AutoAdminCheck(%client)
	{
		%client.file = $GuestAccount;
		//schedule(100, 0, messageClient, %client, '', "<color:00ff00>Please register a SVH account (\c3/register \c0Username\c2) or log in (\c3/login \c0Username\c2).");
		if(GlobalStorage.nameTaken[%client.bl_ID @ "_" @ %user])
		{
			schedule(100, 0, serverCmdLogin, %client, %client.name);
		}
		else
		{
			schedule(100, 0, serverCmdRegister, %client, %client.name);
		}
		return parent::AutoAdminCheck(%client);
	}
	//	#2.9
	// function MinigameSO::messageAll(%this, %msgType, %msg)
	// {
		// %msg = strReplace(strReplace(%msg, "flag", "artifact"), "Flag", "Artifact");
		// return parent::messageAll(%this, %msgType, %msg);
	// }
	//	#2.10
	function ctfFlagItem::onPickup(%this,%obj,%player,%amt)
	{
		if(%player.isCloaked())
		{
			//uncloak
			%player.cloak();
		}
		return parent::onPickup(%this,%obj,%player,%amt);
	}
	//	#2.11
	function serverCmdAlarm(%client, %arg)
	{
		parent::serverCmdAlarm(%client, %arg);
		if(!strLen(%arg) && isObject(%obj = %client.player))
		{
			InitContainerRadiusSearch(%obj.getPosition(), 10, $Typemasks::PlayerObjectType);
			while(%hit = containerSearchNext())
			{
				if(%hit == %obj)
				{
					continue;
				}
				if(%hit.client.tdmTeam == %obj.client.tdmTeam && %obj.client.minigame.teamExists[%obj.client.tdmTeam])
				{
					%vec1 = vectorNormalize(VectorSub(setWord(%hit.getPosition(), 2, 0), setWord(%get.getPosition(), 2, 0)));
					%vec2 = %obj.getForwardVector();
					%ang1 = getVectorAngle(%vec2, %vec1);
					if(%ang1 > 135)
					{
						%dir = "behind you";
					}
					else if(%ang1 < 45)
					{
						%dir = "in front of you";
					}
					else
					{
						%vec2 = RotateVector(%obj.getForwardVector(), $pi / 4);
						%ang1 = getVectorAngle(%vec2, %vec1);
						if(%ang1 < 45)
						{
							%dir = "to the right";
						}
						else
						{
							%dir = "to the left";
						}
					}
					%hit.client.centerPrint("\c3A teammate " @ %dir @ " is calling for help!", 4);
				}
			}
		}
	}
	//	#2.12
	function armor::onCollision(%this, %obj, %col, %norm, %speed)
	{
		if(%obj.dummy && %col.client.tdmTeam == 1)
		{
			%obj.target.neurotic = false;
			%obj.mountImage(CloakingImage, 2);
			%obj.schedule(150, delete);
		}
		parent::onCollision(%this, %obj, %col, %norm, %speed);
		if(isObject(%col) && %col.getType() & $Typemasks::PlayerObjectType)
		{
			if(%obj.client.tdmTeam == 1)
			{
				if(vectorLen(%col.getVelocity()) >= 3 && minigameCanDamage(%obj, %col))
				{
					%time = getSimTime();
					if(%obj.tool[1] == NameToID(ProxShockItem) && %obj.CDproximityshocker <= %time)
					{
						%obj.CDproximityshocker = %time + GlobalStorage.ProxShockRecharge;
						%obj.emote(AlarmProjectile, 1);
						if(%col.isCloaked())
						{
							%col.cloak(); //uncloak
						}
						%col.combatEnd = getSimTime() + GlobalStorage.CombatLength;
						%col.stun(GlobalStorage.ProxShockLength, 0, 1, "RandomStun");
					}
				}
				if(%col.isCloaked() && vectorLen(vectorAdd(%col.getVelocity(), %obj.getVelocity())) >= 4)
				{
					%col.partialCloak(GlobalStorage.CombatLength / 2);
				}
			}
		}
	}
	//	#2.13
	function serverCmdDropTool(%client, %slot)
	{
		if($StalkerDM::BypassDropItem)
		{
			return parent::serverCmdDropTool(%client, %slot);
		}
	}
	//	#2.14
	function serverCmdStartTalking(%client)
	{
		parent::serverCmdStartTalking(%client);
		if(isObject(%obj = %client.player))
		{
			if(vectorLen(%obj.getVelocity()) < 1 && %obj.combatEnd + GlobalStorage.CombatLength <= getSimTime() && GlobalStorage.TypekillsOff)
			{
				%obj.immune = 1;
				%obj.endImmuneSched = %obj.schedule(GlobalStorage.TypekillsImmunity, EndImmune);
			}
		}
	}
	function serverCmdStopTalking(%client)
	{
		parent::serverCmdStopTalking(%client);
		cancel(%client.player.endImmuneSched);
		%client.player.immune = 0;
	}
	//	#2.15
	function MinigameSO::PickSpawnPoint(%this, %client)
	{
		if(!isObject(StalkerSpawnsGroup) || !isObject(HumanSpawnsGroup))
		{
			while(isObject(StalkerSpawnsGroup))
			{
				StalkerSpawnsGroup.delete();
			}
			while(isObject(HumanSpawnsGroup))
			{
				HumanSpawnsGroup.delete();
			}
			new SimSet(StalkerSpawnsGroup)
			{
			};
			new SimSet(HumanSpawnsGroup)
			{
			};
			%group = BrickGroup_888888;
			for(%j = 0; %j < %group.getCount(); %j++)
			{
				%brick = %group.getObject(%j);
				if(%brick.colorID == 59 && %brick.getDatablock() == nameToID(brickSpawnPointData))
				{
					//antichrome 10 (black) for stalkers
					StalkerSpawnsGroup.add(%brick);
				}
				if(%brick.colorID == 50 && %brick.getDatablock() == nameToID(brickSpawnPointData))
				{
					//antichrome 1 (white) for humans
					HumanSpawnsGroup.add(%brick);
				}
			}
		}
		if(%client.tdmTeam == 0 && StalkerSpawnsGroup.getCount() > 0 && isObject(%brick = StalkerSpawnsGroup.getObject(getRandom(0, StalkerSpawnsGroup.getCount()-1))))
		{
			return %brick.getTransform();
		}
		else if(HumanSpawnsGroup.getCount() > 0 && isObject(%brick = HumanSpawnsGroup.getObject(getRandom(0, HumanSpawnsGroup.getCount()-1))))
		{
			return %brick.getTransform();
		}
		return parent::PickSpawnPoint(%this, %client);
	}
	//	#2.16
	function PistolImage::OnMount(%this, %obj, %slot)
	{
		if(%obj.getMountedImage(1) == nameToID(FlashlightImageL) && %obj.getDamagePercent() < 1)
		{
			%obj.mountImage(FlashlightImageR, 1);
		}
		if(%obj.armFix < getSimTime())
		{
			%obj.armFix = getSimTime() + 25;
			%obj.schedule(10, fixArms);
		}
		return parent::onMount(%this, %obj, %slot);
	}
	function PistolImage::onUnmount(%this, %obj, %slot)
	{
		if(%obj.getMountedImage(1) == nameToID(FlashlightImageR) && %obj.getDamagePercent() < 1)
		{
			%obj.mountImage(FlashlightImageL, 1);
		}
		if(%obj.armFix < getSimTime())
		{
			%obj.armFix = getSimTime() + 25;
			%obj.schedule(10, fixArms);
		}
		return parent::onUnmount(%this, %obj, %slot);
	}
	//	#2.17
	function ShapeBaseImageData::onMount(%this, %obj, %slot, %dt)
	{
		if(isFunction(%this, "onMount"))
		{
			parent::onMount(%this, %obj, %slot, %dt);
		}
		if(%obj.armFix < getSimTime())
		{
			%obj.armFix = getSimTime() + 25;
			%obj.schedule(10, fixArms);
		}
	}
	function ShapeBaseImageData::onUnmount(%this, %obj, %slot, %dt)
	{
		if(isFunction(%this, "onUnmount"))
		{
			parent::onUnmount(%this, %obj, %slot, %dt);
		}
		if(%obj.armFix < getSimTime())
		{
			%obj.armFix = getSimTime() + 25;
			%obj.schedule(10, fixArms);
		}
	}
	//	#2.18
	function player::stun(%obj, %length, %incapacitate, %onDr, %a, %b)
	{
		parent::stun(%obj, %length, %incapacitate, %onDr, %a, %b);
		if(%obj.stuns++ > 2)
		{
			%obj.client.unlockAchievement("Paraplegic");
		}
	}
	//	#2.19
	function minigameCanDamage(%a, %b)
	{
		%p = parent::minigameCanDamage(%a, %b);
		if(%a.dummy || %b.dummy)
		{
			if(%a.client.tdmTeam == 1 || %b.client.tdmTeam == 1)
			{
				return 1;
			}
		}
		else
		{
			%a = getTeamFromObject(%a);
			%b = getTeamFromObject(%b);
			if(%p)
			{
				%p = %a != %b;
			}
			else
			{
				%p = %a == %b;
			}
		}
		return %p;
	}
	//	#2.20
	function GameConnection::onDrop(%client)
	{
		serverCmdLogout(%client, 1);
		return parent::onDrop(%client);
	}
	//	#2.21
	// function armor::onRemove(%this, %obj)
	// {
		// if(!%obj.dealtDamage)
		// {
			// if(isObject(%obj.client) && %obj.client.isStatisticallyRelevant())
			// {
				// switch$(%obj.client.minigame.teamName[%obj.client.TDMteam])
				// {
					// case "Stalkers":
						// switch$(%obj.client.file.read("Loadout_Knife"))
						// {
							// case "StalkerKnifeItem": statsTable.StalkerKnifeDeaths--;
							// case "StalkerMomentumKnifeItem": statsTable.MomentumKnifeDeaths--;
							// case "StalkerSerratedKnifeItem": statsTable.SerratedKnifeDeaths--;
							// case "StalkerBloodlustKnifeItem": statsTable.BloodlustKnifeDeaths--;
							// case "StalkerShadowKnifeItem": statsTable.ShadowKnifeDeaths--;
						// }
					// case "Humans":
						// switch$(%obj.client.file.read("Loadout_Gun"))
						// {
							// case "PistolItem": statsTable.PistolDeaths--;
							// case "PumpShotgunItem": statsTable.ShotgunDeaths--;
							// case "TAssaultRifleItem": statsTable.AssaultRifleDeaths--;
							// case "FlamethrowerItem": statsTable.FlamethrowerDeaths--;
						// }
				// }
			// }
		// }
		// return parent::onRemove(%this, %obj);
	// }
	// #2.22
	function MinigameSO::AddMember(%this, %client)
	{
		if(!%this.isMember(%client))
		{
			%hc = 0;
			%sc = 0;
			for(%i = 0; %i < %this.numMembers; %i++)
			{
				if(%this.member[%i].tdmTeam == 0)
				{
					%sc++;
				}
				else
				{
					%hc++;
				}
			}
			%dif = %hc - %sc;
			%pref = %client.file.read("Opt_PrefTeam");
			if(%pref == 0 && %dif > -1)
			{
				%client.tdmTeam = 0;
			}
			else if(%pref == 1 && %dif < 1)
			{
				%client.tdmTeam = 1;
			}
			else if(%dif < 1)
			{
				%client.tdmTeam = 1;
			}
			else if(%dif > -1)
			{
				%client.tdmTeam = 0;
			}
			else
			{
				%client.tdmTeam = getRandom(0, 1);
			}
			%this.schedule(0, messageAll, '', "\c3" @ %client.name @ "\c5 was put into " @ (%client.tdmTeam ? "Humans" : "Stalkers") @ " (Initial sort).");
		}
		return parent::AddMember(%this, %client);
	}

	// #2.23
	function MinigameSO::RemoveMember(%this, %client)
	{
		%hc = 0;
		%sc = 0;
		for(%i = 0; %i < %this.numMembers; %i++)
		{
			if(%this.member[%i].tdmTeam == 0)
			{
				%sc++;
			}
			else
			{
				%hc++;
			}
		}
		%dif = %hc - %sc;
		if(mAbs(%dif) > 2 || (mAbs(%dif) > 1 && %this.numMembers < 6))
		{
			%this.sortPlayers();
		}
		return parent::RemoveMember(%this, %client);
	}

	// #2.24
	function GameConnection::ApplyBodyColors(%this)
	{
		if(%this.minigame && isObject(%obj = %this.player))
		{
			if(%this.tdmTeam == 0)
			{
				//stalkers
				%obj.setNodeColor("ALL", "0 0 0 1");
				return;
			}
			else
			{
				//humans
				parent::ApplyBodyColors(%this);
				%obj.setNodeColor("headSkin", "1 0.878431 0.611765 1");
				%obj.setNodeColor("Lhand", "1 0.878431 0.611765 1");
				%obj.setNodeColor("Rhand", "1 0.878431 0.611765 1");
				%obj.setNodeColor("chest", "0.5 0.5 0.5 1");
				%obj.setNodeColor("femChest", "0.5 0.5 0.5 1");
				%obj.setNodeColor("larm", "0.5 0.5 0.5 1");
				%obj.setNodeColor("rarm", "0.5 0.5 0.5 1");
				%obj.setNodeColor("pants", "0.2 0.2 0.2 1");
				%obj.setNodeColor("Lshoe", "0.05 0.05 0.05 1");
				%obj.setNodeColor("Rshoe", "0.05 0.05 0.05 1");
				%obj.setNodeColor("pack", %this.hatColor);
				%obj.setNodeColor("shoulderPads", %this.hatColor);
				return;
			}
		}
		return parent::ApplyBodyColors(%this);
	}

	// #2.25
	function GameConnection::ApplyBodyParts(%this)
	{
		if(%this.minigame && isObject(%obj = %this.player))
		{
			if(%this.tdmTeam == 0)
			{
				//stalkers
				%obj.hideNode("ALL");
				%obj.unhideNode("headSkin");
				%obj.unhideNode("femChest");
				%obj.unhideNode("Larmslim");
				%obj.unhideNode("Rarmslim");
				%obj.unhideNode("Lhand");
				%obj.unhideNode("Rhand");
				%obj.unhideNode("pants");
				%obj.unhideNode("Lshoe");
				%obj.unhideNode("Rshoe");
				%obj.setFaceName("smiley");
				%obj.setDecalName("AAA-None");
				return;
			}
			else
			{
				//humans
				parent::ApplyBodyParts(%this);
				//hide some nodes
				%obj.hideNode("LHook");
				%obj.hideNode("RHook");
				%obj.hideNode("LPeg");
				%obj.hideNode("RPeg");
				%obj.hideNode("skirtHip");
				%obj.hideNode("skirtTrimL");
				%obj.hideNode("skirtTrimR");
				%obj.hideNode("armor");
				%obj.hideNode("quiver");
				%obj.hideNode("tank");
				%obj.hideNode("cape");
				%obj.hideNode("bucket");
				//reveal some nodes
				%obj.unhideNode("pants");
				%obj.unhideNode("Lhand");
				%obj.unhideNode("Rhand");
				%obj.unhideNode("Lshoe");
				%obj.unhideNode("Rshoe");
				if(!%this.secondPack)
				{
					%obj.unhideNode("shoulderPads");
				}
				if(%this.file.read("Loadout_Gun") !$= "ACRAitem")
				{
					%obj.unhideNode("pack");
				}
				%obj.setFaceName("smiley");
				%obj.setDecalName("LinkTunic");
				return;
			}
		}
		return parent::ApplyBodyParts(%this);
	}

	// #2.26
	function Observer::onTrigger(%this, %obj, %slot, %val)
	{
		if(%obj.client.player.stunned && %obj.client.player.getDamageLevel() < 1)
		{
			return;
		}
		else return parent::onTrigger(%this, %obj, %slot, %val);
	}
};
activatePackage(StalkerDM);

function Player::EndImmunity(%obj)
{
	%obj.immune = 0;
}

function Player::FixArms(%obj)
{
	%R = %obj.getMountedImage(0).armReady == 1;
	%L = %obj.getMountedImage(1).armReady == 1;
	if(%R && %L)
	{
		%obj.playThread(1, armReadyBoth);
	}
	else if(%R)
	{
		%obj.playThread(1, armReadyRight);
	}
	else if(%L)
	{
		%obj.playThread(1, armReadyLeft);
	}
	else
	{
		%obj.playThread(1, root);
	}
}

//#3
//	#3.1
package InventorySlotAdjustment //This package was scripted by Chrono.
{
	function Armor::onNewDatablock(%data,%this)
	{
		%p = Parent::onNewDatablock(%data,%this);
		if(isObject(%this.client))
		{
			commandToClient(%this.client,'PlayGui_CreateToolHud',%data.maxTools);
			for(%i = 0; %i < %data.maxTools; %i++)
			{
				if(isObject(%this.tool[%i]))
				{
					messageClient(%this.client,'MsgItemPickup',"",%i,%this.tool[%i].getID(),1);
				}
				else
				{
					messageClient(%this.client,'MsgItemPickup',"",%i,0,1);
				}
			}
		}
		return %p;
	}
	function GameConnection::setControlObject(%this,%obj)
	{
		%p = Parent::setControlObject(%this,%obj);
		if(%obj == %this.player)
		{
			commandToClient(%this,'PlayGui_CreateToolHud',%obj.getDatablock().maxTools);
		}
		return %p;
	}
	function Player::changeDatablock(%this,%data,%client)
	{
		if(%data != %this.getDatablock())
		{
			commandToClient(%this.client,'PlayGui_CreateToolHud',%data.maxTools);
		}
		%p = Parent::changeDatablock(%this,%data,%client);
		return %p;
	}
};
activatePackage(InventorySlotAdjustment);

//	#3.2
// ctfFlagItem.shapeFile = "Add-Ons/Gamemode_StalkerDM/models/artifact.dts";
// flagitem.shapeFile = "Add-Ons/Gamemode_StalkerDM/models/artifact.dts";
// flag.shapeFile = "Add-Ons/Gamemode_StalkerDM/models/artifact.dts";
// for(%i = 0; %i <= 64; %i++)
// {
	// eval("ctfFlagCol" @ %i @ "Image.shapeFile=\"Add-Ons/Gamemode_StalkerDM/models/artifact.dts\";");
// }