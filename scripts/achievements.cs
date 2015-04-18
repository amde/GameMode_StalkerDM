////////////////////////////
//STALKER DM: ACHIEVEMENTS//
////////////////////////////

//////////////
// CONTENTS //
//////////////
//#1. ACHIEVEMENT FUNCTION
//#2. ACHIEVEMENT SERVERCMDS
//#3. ACHIEVEMENT REGISTRY
//	#3.1 UIname Table
//	#3.2 Stalker Achievements
//	#3.3 Human Achievements
//	#3.4 Other Achievements

//#1.
function GameConnection::UnlockAchievement(%client, %achievement)
{
	%achievement = $StalkerDM::Achievement[%achievement];
	%name = strReplace($StalkerDM::Achievement[%achievement, "Name"], " ", "_");
	if(!%client.file.read("UnlockedA_" @ strReplace(%name, " ", "_")) && !%client.file.readOnly)
	{
		%client.file.modify("UnlockedA_" @ strReplace(%name, " ", "_"), 1);
		%client.file.modify("PointsT",  %client.file.read("PointsT") + $StalkerDM::Achievement[%achievement, "Value"]);
		%client.file.modify("PointsA",  %client.file.read("PointsA") + $StalkerDM::Achievement[%achievement, "Value"]);
		%itemname = $StalkerDM::Achievement[%achievement, "Unlock"];
		%itemid = nameToID($StalkerDM::UInameTable[%itemname]);
		%client.file.modify("UnlockedI_" @ ($StalkerDM::UInameTable[%itemname] $= "Everything" ? "Everything" : $StalkerDM::UInameTable[%itemname]), 1);
		messageAll('', "\c3" @ %client.name @ " <color:800000>has unlocked the achievement \c3" @ $StalkerDM::Achievement[%achievement, "Name"] @ "<color:800000> for \c3" @ $StalkerDM::Achievement[%achievement, "Value"] @ "<color:800000> points!");
		if(%itemname !$= "")
		{
			messageAll('', "   \c3" @ %client.name @ " <color:800000>has unlocked the right to use the \c3" @ %itemname @ "<color:800000>!");
			messageClient(%client, '', "\c2You can use this item by entering \c3/setloadout \c0" @ $StalkerDM::UInameTable[%itemname] @ "\c2.");
		}
	}
}

//#2.
function servercmdSetLoadOut(%client, %w1, %w2, %w3)
{
	if(getWordCount(%w1) > 1 || %w2 $= "GUISENT")
	{
		%w = %w1;
		%guisent = 1;
	}
	else
	{
		%w = trim(%w1 SPC %w2 SPC %w3);
	}
	if($StalkerDM::UInameTable[%w] $= "")
	{
		messageClient(%client, 'MsgError', "<color:ff0000>Could not find item \"\c3" @ %w @ "<color:ff0000>\".");
		return;
	}
	%w = $StalkerDM::UInameTable[%w];
	%n = %w.uiName;
	if(!%client.file.read("UnlockedI_" @ %w.getName()) && !%client.file.read("UnlockedI_Everything"))
	{
		messageClient(%client, 'MsgError', "<color:ff0000>You have not unlocked the right to use that item... yet.");
		return;
	}
	if(!%guisent)
	{
		messageClient(%client, '', "\c2You are now using the \c3" @ %n @ "\c2. You will have this item when you next spawn.");
	}
	commandToClient(%client, 'SetLoadoutData', %w.slot, %n);
	%id = %client.bl_id;
	%client.file.modify("Loadout_" @ %w.slot, %w);
	if(isObject(%client.player) && %client.player.spawnTime + 20000 >= getSimTime())
	{
		%client.player.instantRespawn();
	}
}
function serverCmdEquip(%client, %w1, %w2, %w3)
{
	servercmdSetLoadOut(%client, %w1, %w2, %w3);
}
function serverCmdUse(%client, %w1, %w2, %w3)
{
	servercmdSetLoadOut(%client, %w1, %w2, %w3);
}

function serverCmdGetAchievementsData(%client)
{
	if(%client.scd > getSimTime())
	{
		return;
	}
	%client.scd = getSimTime() + 100;
	for(%i = 0; %i < $StalkerDM::AchievementCount; %i++)
	{
		%name = $StalkerDM::Achievement[%i, "Name"];
		%desc = $StalkerDM::Achievement[%i, "Description"];
		%unlock = $StalkerDM::Achievement[%i, "Unlock"];
		%value = $StalkerDM::Achievement[%i, "Value"];
		%unlocked = %client.file.read("UnlockedA_" @ %name);
		commandToClient(%client, 'PopulateAchievementsList', %i, %name, %desc, %unlock, %value, %unlocked);
	}
}
function serverCmdGetAchievementsProgress(%client)
{
	if(%client.scd > getSimTime())
	{
		return;
	}
	%client.scd = getSimTime() + 100;
	for(%i = 0; %i < $StalkerDM::AchievementCount; %i++)
	{
		%unlocked = %client.file.read("UnlockedA_" @ $StalkerDM::Achievement[%i, "Name"]);
		commandToClient(%client, 'UnlockedAchievement', %i, %unlocked);
	}
}
function serverCmdUnlockedItems(%client)
{
	if(%client.scd > getSimTime())
	{
		return;
	}
	%client.scd = getSimTime() + 100;
	//list items the player has unlocked
}
function serverCmdGetLoadout(%client)
{
	if(%client.scd > getSimTime())
	{
		return;
	}
	%client.scd = getSimTime() + 100;
	messageClient(%client, '', "\c2Knife: " @ %client.file.read("Loadout_Knife").uiName @ "   Spell: " @ %client.file.read("Loadout_Magic").uiName @ "    Curse: " @ %client.file.read("Loadout_Curse").uiName);
	messageClient(%client, '', "\c2Gun: " @ %client.file.read("Loadout_Gun").uiName @ "   Gadget: " @ %client.file.read("Loadout_Secondary").uiName @ "    Melee: " @ %client.file.read("Loadout_Melee").uiName);
}

function serverCmdSVH_Pong(%client)
{
	%client.unlockAchievement("StalkerDM Fan");
	if(%client.scd > getSimTime())
	{
		return;
	}
	%client.scd = getSimTime() + 2000;
	%n = 10;
	for(%i = 0; %i < $ItemList::Count; %i++)
	{
		%obj = $ItemList::Object[%i];
		%unlocked = %client.file.read("UnlockedI_" @ %obj.getName()) || %client.file.read("UnlockedI_Everything");
		switch$(%obj.slot)
		{
			case "Magic": %slot = "Spell";
			case "Secondary": %slot = "Gadget";
			default: %slot = %obj.slot;
		}
		schedule(%n+= 10, 0, commandToClient, %client, 'ItemUnlocked', %slot, %obj.num, %unlocked);
	}
	schedule(%n+= 10, 0, commandToClient, %client, 'SetLoadoutData', "Knife", %client.file.read("Loadout_Knife").num);
	schedule(%n+= 10, 0, commandToClient, %client, 'SetLoadoutData', "Spell", %client.file.read("Loadout_Magic").num);
	schedule(%n+= 10, 0, commandToClient, %client, 'SetLoadoutData', "Curse", %client.file.read("Loadout_Curse").num);
	schedule(%n+= 10, 0, commandToClient, %client, 'SetLoadoutData', "Gun", %client.file.read("Loadout_Gun").num);
	schedule(%n+= 10, 0, commandToClient, %client, 'SetLoadoutData', "Gadget", %client.file.read("Loadout_Secondary").num);
	schedule(%n+= 10, 0, commandToClient, %client, 'SetLoadoutData', "Melee", %client.file.read("Loadout_Melee").num);
	schedule(%n+= 10, 0, commandToClient, %client, 'SetPreferredTeam', %client.file.read("Opt_PrefTeam"));
}

//#3.
//	#3.1
$StalkerDM::UInameTable["Fucking Everything"] = "Everything";

//	#3.2
$StalkerDM::AchievementCount = -1;

function RegisterAchievement(%name, %desc, %value, %unlock)
{
	$StalkerDM::Achievement[$StalkerDM::AchievementCount++, "Name"] = %name;
	$StalkerDM::Achievement[$StalkerDM::AchievementCount, "Description"] = %desc;
	$StalkerDM::Achievement[$StalkerDM::AchievementCount, "Value"] = %value;
	if(strLen(%unlock))
	{
		$StalkerDM::Achievement[$StalkerDM::AchievementCount, "Unlock"] = %unlock;
	}
	$StalkerDM::Achievement[$StalkerDM::Achievement[$StalkerDM::AchievementCount, "Name"]] = $StalkerDM::AchievementCount;
}

RegisterAchievement("10 Human Kills", "Kill 10 Humans", 25);
RegisterAchievement("25 Human Kills", "Kill 25 Humans", 50);
RegisterAchievement("50 Human Kills", "Kill 50 Humans", 100);
RegisterAchievement("100 Human Kills", "Kill 100 Humans", 200);
RegisterAchievement("Group Hysteria", "Kill 5 humans within 2 minutes.", 20);
RegisterAchievement("Forgetful", "Attack the same human twice with a 30 second gap between the attacks.", 10);
RegisterAchievement("Dominatrix", "Kill the same human 3 times in one life.", 10);
RegisterAchievement("Batman", "Kill 3 humans while in midair in one life.", 15);
RegisterAchievement("Master of Subtlety", "Deal 3 \"subtle\" backstabs in one life with the Stalker Knife.", 15);
RegisterAchievement("Master of Blood", "Power up a Bloodlust Knife enough to deal 100 damage without a backstab.", 15);
RegisterAchievement("Master of Speed", "Deal 50 damage in one hit with the Momentum Knife.", 15);
RegisterAchievement("Master of Slicing", "Kill a human with the Serrated Knife's bleed effect.", 15);
RegisterAchievement("Master of Shadow", "Kill a human with the Shadow Knife while cloaked.", 15);
RegisterAchievement("Master of Darkness", "Blind 5 humans in one life.", 15);
RegisterAchievement("Master of Paralysis", "Get an assisted kill with Paralysis.", 15);
RegisterAchievement("Master of Shadowstep", "Kill a human within 5 seconds of Shadowstepping them.", 15);
RegisterAchievement("Master of Neurotoxins", "Stun 3 humans with a single Toxin curse.", 15);
RegisterAchievement("Master of Hellfire", "Kill a human with the Wrath curse.", 15);
RegisterAchievement("Master of Fury", "Attack a human afflicted by at least 100% vulnerability from the Rage curse.", 15);
RegisterAchievement("Master of Neurosis", "Kill a human affected by Psychosis or Schizophrenia.", 15);

//	#3.3
RegisterAchievement("10 Stalker Kills", "Kill 10 stalkers.", 25);
RegisterAchievement("25 Stalker Kills", "Kill 25 stalkers.", 50);
RegisterAchievement("50 Stalker Kills", "Kill 50 stalkers.", 100);
RegisterAchievement("100 Stalker Kills", "Kill 100 stalkers.", 200);
RegisterAchievement("Assault Specialist", "Kill 3 stalkers from above in one life.", 15);
RegisterAchievement("Pistol Specialist", "Inflict 100 damage in 3 seconds with the pistol.", 15);
RegisterAchievement("Shotgun Specialist", "Hit a Stalker with every bullet from a Shotgun blast.", 15);
RegisterAchievement("Medical Specialist", "Heal over 200 points of damage in one life with the ACRA or Stimpak.", 15);
RegisterAchievement("Pyrotechnician", "Deal 300 damage in one minute with the Flamethrower.", 15);
RegisterAchievement("Electrician", "Kill a Stalker with the taser.", 15);
RegisterAchievement("Eye of the Tiger", "Shoot 3 cloaked stalkers in one life.", 10);
RegisterAchievement("Stabproof Vest", "Take 200 damage in one life as a human.", 10);
RegisterAchievement("Pierce the Heavens", "Kill a cloaked stalker who's above you.", 10);
RegisterAchievement("Duck Hunt", "Kill a Stalker while it is in midair.", 10);
RegisterAchievement("Shot in the Dark", "Hit a stalker while blinded.", 10);

//	#3.4
RegisterAchievement("The First Achievement", "Create an account.", 50);
RegisterAchievement("Paraplegic", "Be stunned 3 times in one life.", 10);
RegisterAchievement("Cheater", "Enter the secret code given to Craftsmen playtesters.", 0, "Everything");
RegisterAchievement("Shitty Player", "Die, having done no damage, 3 times.", -10, "poop nife");
RegisterAchievement("StalkerDM Fan", "<a:www.mediafire.com/download.php?v4a2bpdp2041wru>Download</a>\c2 and use the StalkerDM client-side mod.", 10);
RegisterAchievement("Donator", "Donate to the Craftsmen (<a:wbjah.com/cmen/donate.htm>http://wbjah.com/cmen/donate.htm</a>\c2).", 10);