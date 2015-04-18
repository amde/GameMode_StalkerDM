$StalkerDM::SaveLocation = "config/server/CraftMods/SvH/";
$Alphanumeric = "abcdefghijklmnopqrstuvwxyz0123456789";

//Save object variables:
//Unlocked[i,itm] - unlocked items
//Unlocked[a,ach] - unlocked achievements
//Loadout[s,itm] - item in slot s
//Opt[p,team] - misc preferences and options
//Points[t] - total achievement points
//Points[a] - unspent achievement points

/////////////////////
//TABLE OF CONTENTS//
/////////////////////
//#0. GlobalStorage Object
//	#0.1 Guest account
//	#0.2 isAlphanumeric
//#1. Account Creation
//	#1.1 serverCmdRegister
//	#1.2 serverCmdLogin
//	#1.3 serverCmdLogout
//	#1.4 serverCmdDeleteFile
//#2. SvhStorage Functionality
//	#2.1 SvhStorage_NewFile
//	#2.2 SvhStorage_LoadFile
//	#2.3 SvhStorage::Read
//	#2.4 SvhStorage::Modify
//	#2.5 SvhStorage::SaveToFile
//	#2.6 SvhStorage::Close
//	#2.7 Automatic saving

//ROUND 2 BALANCE TESTING DATA
//----
//Kills: 2,879 / 1,901 (S/H)
//Victories: 36 / 38 (S/H)
//----
//Pistol: 51,996 / 1,222 (42.5499 dpl)
//Shotgun: 39,896 / 350 (113.989 dpl)
//Assault Rifle: 18,180 / 178 (102.135 dpl)
//Flamethrower: 27,048 / 383 (70.6214 dpl)
//----
//Stalker Knife: 43,002.9 / 918 (46.8441 dpl)
//Momentum Knife: 18,741.9 / 210 (89.2471 dpl)
//Serrated Knife: 32,956.4 / 367 (89.7995 dpl)
//Bloodlust Knife: 81,949 / 614 (133.467 dpl)
//Shadow Knife: 9,241.36 / 124 (74.5271 dpl)

//#0.
if(!isFile($StalkerDM::SaveLocation @ "GlobalStorage.cs"))
{
	new ScriptObject(GlobalStorage)
	{
		class = "SvhStorage";
		readOnly = false;
		nameTaken["GlobalStorage"] = 1;
		nameTaken["Guest"] = 1;
		//Default balance variables
		AbyssCD = 60000;
		AbyssEnergy = 99;
		AbyssRadius = 12;
		AbyssBlindTicks = 5;
		AbyssTotalTicks = 15;
		AbyssTickInterval = 1000;
		ACRAefficiency = 0.3;
		ACRAradius = 16;
		ACRAtickamt = 7.5;
		ARammo = 200;
		ARregen = 40;
		ARsnipeDamage = 15;
		ARsprayDamage = 8;
		BlindCD = 15000;
		BlindEnergy = 30;
		BlindDuration = 6000;
		CloakCD = 1000;
		CloakUsesEnergy = false;
		CombatLength = 5000;
		CurseMinRange = 16;
		CurseMaxRange = 128;
		FlareCD = 45000;
		FlareRadius = 8;
		FTdamage = 5; //!!!
		FTdotstart = 4;
		//FTdotticks = 7;
		FTenergy = 1;
		FThasCD = false;
		FTrange = 7;
		FTsection = 25;
		GSCD = 20000;
		GSEnergy = 75;
		GSLength = 5000;
		KatanaBlocks = true;
		KatanaDamage = 20;
		KatanaRange = 3;
		KatanaSection = 20;
		KnifeBackstab = 70;
		KnifeFacestab = 15;
		KnifeRange = 3.5;
		KnifeSection = 30;
		LightRadius = 3;
		LeechEfficiency = 0.5;
		LeechRadius = 16;
		LustEfficiency = 0.1; //!!!
		LustLossPercent = 0.25; //!!!
		MagicRange = 64;
		MagicSection = 15;
		MomentumKnifeBackstabs = false;
		MomentumKnifeDamage = 8;
		MomentumKnifeSection = 45;
		MomentumKnifeScale = 3;
		ParaCD = 15000;
		ParaEnergy = 75;
		ParaLength = 5000;
		ParaBackfireLength = 2500;
		PistolAmmo = 70;
		PistolDamage = 17.5;
		PistolRegen = 35;
		ProxShockLength = 1500;
		ProxShockRecharge = 20000;
		PsychosisCD = 60000;
		PsychosisEnergy = 50;
		PsychosisRadius = 12;
		PsychosisTotalTicks = 10;
		PsychosisTickInterval = 2000;
		RageCD = 60000;
		RageEnergy = 99;
		RageFalloffTime = 5000;
		RageRadius = 12;
		RageTotalTicks = 15;
		RageTickInterval = 1000;
		RageTickEffect = 0.5;
		RocketBootsCD = 30000;
		RocketBootsDuration = 3000;
		RepulsorCD = 5000;
		RepulsorPower = 20;
		RepulsorRange = 7;
		RepulsorSection = 45;
		SchizophreniaCD = 30000;
		SchizophreniaEnergy = 50;
		SchizoDuration = 8000;
		SerrKnifeDamage = 10;
		SerrKnifeBleed = 2;
		ShadKnifeDamage = 15;
		ShadKnifeBlind = 1500;
		ShadowstepCD = 20000;
		ShadowstepRange = 32;
		ShieldDamage = 15;
		ShieldKnockback = 5;
		ShieldRange = 3;
		ShieldSection = 45;
		ShotgunAmmo = 36;
		ShotgunDamage = 7;
		ShotgunRegen = 8;
		ShotgunShellcount = 7;
		SledgeDamage = 20;
		SledgeDuration = 2000;
		SledgeKnockback = 5;
		SledgeRange = 2.5;
		SledgeSection = 20;
		StalkerCritScale = 2;
		SSCD = 24000;
		SSenergy = 30;
		StimpakCD = 20000;
		StimpakHOT = 25;
		StimpakIH = 25;
		StimpakOverheal = false;
		StimpakTicks = 5;
		TaserCD = 20000;
		TaserDamage = 10;
		TaserMelee = false;
		TaserRange = 10;
		TaserSection = 30;
		TaserShortRange = 3;
		TaserStunLength = 3000;
		TaserShortStunLength = 1500;
		ToxinCD = 60000;
		ToxinEnergy = 99;
		ToxinFalloffTime = 5000;
		ToxinRadius = 12;
		ToxinStunLength = 5000;
		ToxinStunTicks = 5;
		ToxinTotalTicks = 10;
		ToxinTickInterval = 1000;
		TypekillsOff = false;
		TypekillsImmunity = 30000;
		WrathCD = 60000;
		WrathEnergy = 99;
		WrathFalloffTime = 7500;
		WrathRadius = 12;
		WrathTotalTicks = 10;
		WrathTickInterval = 1000;
	};
}
else
{
	exec($StalkerDM::SaveLocation @ "GlobalStorage.cs");
}

//	#0.1
$GuestAccount = new ScriptObject(GuestAccount)
{
	username = "Guest";
	class = "SvhStorage";
	readOnly = true;
	vPointsT = 0;
	vPointsA = 0;
	vUnlockedI_StalkerKnifeItem = 1;
	vUnlockedI_DarkMitem = 1;
	vUnlockedI_ToxinItem = 1;
	vUnlockedI_PistolItem = 1;
	vUnlockedI_StimpakItem = 1;
	vUnlockedI_L4BKatanItem = 1;
	vLoadout_Curse = "ToxinItem";
	vLoadout_Gun = "PistolItem";
	vLoadout_Knife = "StalkerKnifeItem";
	vLoadout_Magic = "DarkMitem";
	vLoadout_Melee = "L4BKatanaItem";
	vLoadout_Secondary = "StimpakItem";
	vOpt_PrefTeam = -1;
};

//	#0.2
function isAlphanumeric(%str)
{
	%l = strLen(%str);
	for(%i = 0; %i < %l; %i++)
	{
		if(striPos($Alphanumeric, getSubStr(%str, %i, 1)) == -1)
		{
			return false;
		}
	}
	return true;
}

//#1.
//	#1.1
function serverCmdRegister(%client, %user, %a)
{
	if(isObject(%client.file))
	{
		serverCmdLogout(%client, 1);
	}
	if(!strLen(%user) || strLen(%a))
	{
		messageClient(%client, 'MsgError', "You must enter a one-word username.");
		return;
	}
	if(!isAlphanumeric(%user))
	{
		messageClient(%client, 'MsgError', "Your username must only contain characters a-z, A-Z, or 0-9.");
		return;
	}
	%user = strReplace(%user, " ", "");
	if(GlobalStorage.nameTaken[%client.BL_ID @ "_" @ %user])
	{
		messageClient(%client, 'MsgError', "That username has already been taken.");
		return;
	}
	GlobalStorage.nameTaken[%client.BL_ID @ "_" @ %user] = 1;
	%this = SvhStorage_NewFile();
	%this.client = %client;
	%this.username = %user;
	%this.BL_ID = %client.BL_ID;
	%client.file = %this;
	commandToClient(%client, 'SVH_Ping');
	%this.unlockAchievement("The First Achievement");
	%this.SaveToFile();
}

//	#1.2
function serverCmdLogin(%client, %user)
{
	if(isObject(%client.file))
	{
		serverCmdLogout(%client, 1);
	}
	if(!GlobalStorage.nameTaken[%client.bl_ID @ "_" @ %user])
	{
		messageClient(%client, 'MsgError', "Username not found / password incorrect.");
		return;
	}
	SvhStorage_LoadFile($StalkerDM::SaveLocation @ %client.BL_ID @ "_" @ %user @ ".cs", %client);
	messageClient(%client, '', "<color:00ff00>Welcome back, " @ %client.file.username @ ".");
	commandToClient(%client, 'SVH_Ping');
}

//	#1.3
function serverCmdLogout(%client, %nomsg)
{
	if(isObject(%client.file) || %client.file.username $= "Guest")
	{
		%client.file.close();
		if(!%nomsg)
		{
			messageClient(%client, '', "\c2You have been logged out.");
		}
		%client.file = $GuestAccount;
	}
	else
	{
		messageClient(%client, 'MsgError', "You aren't logged in.");
	}
}

//	#1.4
function serverCmdDeleteFile(%client)
{
	if(isObject(%client.file) && %client.file.username !$= "Guest")
	{
		GlobalStorage.nameTaken[%client.BL_ID @ "_" @ %client.file.username] = 0;
		fileDelete($StalkerDM::SaveLocation @ %client.BL_ID @ "_" @ %client.file.username @ ".cs");
		%client.file.delete();
		%client.file = $GuestAccount;
		messageClient(%client, '', "<color:00ff00>Your file has been deleted.");
	}
	else
	{
		messageClient(%client, 'MsgError', "You aren't logged in.");
	}
}

//#2.
//	#2.1
function SvhStorage_NewFile()
{
	%this = new ScriptObject()
	{
		class = "SvhStorage";
		readOnly = false;
		//default options
		vPointsT = 0;
		vPointsA = 0;
		vUnlockedI_StalkerKnifeItem = 1;
		vUnlockedI_DarkMitem = 1;
		vUnlockedI_ToxinItem = 1;
		vUnlockedI_PistolItem = 1;
		vUnlockedI_StimpakItem = 1;
		vUnlockedI_L4BKatanItem = 1;
		vLoadout_Curse = "ToxinItem";
		vLoadout_Gun = "PistolItem";
		vLoadout_Knife = "StalkerKnifeItem";
		vLoadout_Magic = "DarkMitem";
		vLoadout_Melee = "L4BKatanaItem";
		vLoadout_Secondary = "StimpakItem";
		vOpt_PrefTeam = -1;
	};
	return %this;
}

//	#2.2
function SvhStorage_LoadFile(%file, %client)
{
	if(!isFile(%file))
	{
		error("Error loading file " @ %file);
		return;
	}
	exec(%file);
	if(!isObject(NewlyLoaded))
	{
		error("Loading failed for " @ %file);
		return -1;
	}
	%this = NewlyLoaded.getID();
	%this.setName("");
	%this.client = %client;
	%client.file = %this;
	return %this;
}

//	#2.3
function SvhStorage::Read(%this, %var)
{
	return %this.v[%var];
}

//	#2.4
function SvhStorage::Modify(%this, %var, %val)
{
	if(!%this.readOnly)
	{
		%this.v[%var] = %val;
		return 1;
	}
	return 0;
}

//	#2.5
function SvhStorage::SaveToFile(%this)
{
	if(%this.username $= "Guest")
	{
		return;
	}
	%this.setName("NewlyLoaded");
	%this.save($StalkerDM::SaveLocation @ %this.BL_ID @ "_" @ %this.username @ ".cs");
	%this.setName("");
}

//	#2.6
function SvhStorage::Close(%this)
{
	if(!%this.username $= "Guest")
	{
		%this.SaveToFile();
		%this.delete();
	}
}

//	#2.7
function StalkerDM_Autosave()
{
	cancel($StalkerDM::AutoSaveSched);
	GlobalStorage.save($StalkerDM::SaveLocation @ "GlobalStorage.cs");
	for(%i = 0; %i < clientGroup.getCount(); %i++)
	{
		%this = clientGroup.getObject(%i).file;
		if(isObject(%this) && %this.username !$= "Guest")
		{
			%this.SaveToFile();
		}
	}
	$StalkerDM::AutoSaveSched = schedule(60000, 0, StalkerDM_Autosave);
}
$StalkerDM::AutoSaveSched = schedule(60000, 0, StalkerDM_Autosave);