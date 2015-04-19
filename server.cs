/////////////////////////
//STALKER DM: SERVER.CS//
/////////////////////////

$StalkerDM::Version = "1.2.0";

//////////////
// CONTENTS //
//////////////
//#1. SETUP
//	#1.1 Pathing
//	#1.2 Required Add-Ons
//	#1.3 Armor Datablocks
//	#1.4 Load scripts
//	#1.5 Saving/loading

//#1.
//	#1.1
$StalkerDM::Path = "Add-Ons/GameMode_StalkerDM";

L4BKatanaItem.uiName = "Katana";

//	#1.2
// %error = 0;
// switch(forceRequiredAddOn(Weapon_Sword))
// {
   // case $Error::AddOn_Disabled: sworditem.uiName = "";
   // case $Error::AddOn_NotFound: echo("\c2CRITICAL ERROR: Weapon_Sword.zip is needed for this add-on!"); %error = 1;
// }
// switch(forceRequiredAddOn(Emote_Critical))
// {
   // case $Error::AddOn_Disabled:
   // case $Error::AddOn_NotFound: echo("\c2ERROR: Emote_Critical.zip missing!");
// }
// switch(forceRequiredAddOn(Particle_Basic))
// {
   // case $Error::AddOn_Disabled:
   // case $Error::AddOn_NotFound: echo("\c2ERROR: Particle_Critical.zip missing!");
// }
// switch(forceRequiredAddOn(Gamemode_TeamDeathmatch))
// {
   // case $Error::AddOn_Disabled:
   // case $Error::AddOn_NotFound: echo("\c2CRITICAL ERROR: Gamemode_TeamDeathmatch.zip is needed for this add-on!"); %error = 1;
// }
// switch(forceRequiredAddOn(Gamemode_CaptureTheFlag))
// {
   // case $Error::AddOn_Disabled:
   // case $Error::AddOn_NotFound: echo("\c2CRITICAL ERROR: Gamemode_CaptureTheFlag.zip is needed for this add-on!"); %error = 1;
// }
// switch(forceRequiredAddOn(Weapon_ElementalSpells))
// {
   // case $Error::AddOn_Disabled:
   // case $Error::AddOn_NotFound: echo("\c2CRITICAL ERROR: Weapon_ElementalSpellpack.zip is needed for this add-on!"); %error = 1;
// }

// switch(forceRequiredAddOn(Weapon_Package_Tier1))
// {
   // case $Error::AddOn_Disabled:
   // case $Error::AddOn_NotFound: echo("\c2CRITICAL ERROR: Weapon_Package_Tier1.zip is needed for this add-on!"); %error = 1;
// }
// switch(forceRequiredAddOn(Weapon_Package_Tier2))
// {
   // case $Error::AddOn_Disabled:
   // case $Error::AddOn_NotFound: echo("\c2CRITICAL ERROR: Weapon_Package_Tier2.zip is needed for this add-on!"); %error = 1;
// }
// switch(forceRequiredAddOn(Weapon_Melee_Extended))
// {
   // case $Error::AddOn_Disabled:
   // case $Error::AddOn_NotFound: echo("\c2CRITICAL ERROR: Weapon_Melee_Extended.zip is needed for this add-on!"); %error = 1;
// }

// if(%error == 1)
// {
	// echo("\c2Critical error(s) reported. Download required add-ons, problems will occur during gameplay.");
// }
// %error = 0;

//	#1.3

datablock PlayerData(PlayerStalkerBloodleechArmor : PlayerStandardArmor)
{
	cameraTilt = 0.1;
	cameraHorizontalOffset = 0.7;
	cameraVerticalOffset = 1.3;
	cameraMaxDist = 1;
	maxForwardSpeed = 7 * 1.5;
	maxForwardCrouchSpeed = 3 * 1.5;
	maxSideSpeed = 6 * 1.5;
	maxSideCrouchSpeed = 2 * 1.5;
	maxBackwardSpeed = 4 * 1.5;
	maxBackwardCrouchSpeed = 2 * 1.5;
	isStalker = true;
	canJet = false;
	showEnergyBar = true;
	rechargeRate = 0.3;
	maxTools = 3;
	maxWeapons = 3;
	jumpForce = 900;
	jumpSound = "";
	minImpactSpeed = 60;
	firstPersonOnly = false;
	uiName = "Stalker Bloodleech";
};
$PlayerStalkerBloodleechArmor = PlayerStalkerBloodleechArmor.getID();

datablock PlayerData(PlayerHumanArmor : PlayerStandardArmor)
{
	cameraTilt = 0.1;
	cameraHorizontalOffset = 0.7;
	cameraVerticalOffset = 1.3;
	cameraMaxDist = 1;
	canJet = false;
	showEnergyBar = true;
	rechargeRate = 0.02;
	maxTools = 3;
	maxWeapons = 3;
	jumpForce = 900;
	firstPersonOnly = false;
	firstPersonOnlyAnalogue = PlayerHumanFpArmor;
	uiName = "Human";
};
datablock PlayerData(PlayerHumanFpArmor : PlayerHumanArmor)
{
	firstPersonOnly = true;
	nonFirstPersonOnlyAnalogue = PlayerHumanArmor;
	uiName = "Human (FP)";
};

datablock PlayerData(PlayerHumanFastArmor : PlayerHumanArmor)
{
	maxForwardSpeed = 7 * 3;
	maxForwardCrouchSpeed = 3 * 3;
	maxSideSpeed = 6 * 3;
	maxSideCrouchSpeed = 2 * 3;
	//maxBackwardSpeed = 4 * 3;
	//maxBackwardCrouchSpeed = 2 * 3;
	airControl = 1;
	jumpForce = 1080;
	runForce = 8640;
	firstPersonOnly = false;
	thirdPersonOnly = true;
	maxTools = 0; //to prevent weapon use while boosting
	uiName = "";
};

//	#1.4
exec($StalkerDM::Path @ "/scripts/saving.cs");
exec($StalkerDM::Path @ "/scripts/achievements.cs");
exec($StalkerDM::Path @ "/scripts/balance.cs");
exec($StalkerDM::Path @ "/scripts/curses.cs");
exec($StalkerDM::Path @ "/scripts/Craftsmen_Debuffs.cs");
exec($StalkerDM::Path @ "/scripts/gadgets.cs");
exec($StalkerDM::Path @ "/scripts/guns.cs");
exec($StalkerDM::Path @ "/scripts/help.cs");
exec($StalkerDM::Path @ "/scripts/knives.cs");
exec($StalkerDM::Path @ "/scripts/magics.cs");
exec($StalkerDM::Path @ "/scripts/melee.cs");
exec($StalkerDM::Path @ "/scripts/misc.cs");
exec($StalkerDM::Path @ "/scripts/notice.cs");
exec($StalkerDM::Path @ "/scripts/shop.cs");
exec($StalkerDM::Path @ "/scripts/sort.cs");
exec($StalkerDM::Path @ "/scripts/voting.cs");
exec($StalkerDM::Path @ "/scripts/DieEierVonBushido.cs");
exec($StalkerDM::Path @ "/scripts/statuseffects.cs");

//	#1.5

// new scriptObject(cl)
// {
	// isAdmin = 1;
	// isSuperAdmin = 1;
	// bl_id = 7;
	// name = cl;
// };