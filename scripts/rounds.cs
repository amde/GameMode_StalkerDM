$StalkersImmune = false;
$HumansImmune = false;

function MinigameSO::EndRound(%mini)
{
	for(%i = 0; %i < %mini.numMembers; %i++)
	{
		// if(isObject(%player = %mini.member[%i].player))
		// {
			// %player.schedule(2500, instantRespawn); //for some reason players sometimes weren't respawning normally
		// }
		%points[%mini.member[%i].tdmTeam]+= %mini.member[%i].score;
		%mini.member[%i].score = 0;
	}
	if(%points[0] > %points[1])
	{
		if(%mini.numMembers >= 6)
		{
			statsTable.totalStalkerWins++;
		}
		%mini.messageAll('', "\c5The \c3Stalkers\c5 are victorious!");
		$StalkersImmune = true;
		//global psychosis curse
		
	}
	else if(%points[0] < %points[1])
	{
		if(%mini.numMembers >= 6)
		{
			statsTable.totalHumanWins++;
		}
		%mini.messageAll('', "\c5The \c3Humans\c5 are victorious!");
		$HumansImmune = true;
	}
	else
	{
		%mini.messageAll('', "\c5It's a tie! Everyone loses!");
	}
}

function MinigameSO::Reset(%mini)
{
	$StalkersImmune = false;
	$HumansImmune = false;
	%mini.endStalkerGamemodeVote();
	if(getRandom() > 0.5)
	{
		%mini.sortPlayers();
	}
	//apply gamemode
	switch$(%mode = %mini.nextGamemode)
	{
		case "Last Man Standing":
			%mini.schedule(0, removeCTFBricks, %client);
			%mini.gamemode = gameModeTitleToID("Team Deathmatch");
			%mini.resetSched = %mini.schedule(5 * 60000, endRound);
			%mini.schedule(0, setPointsLimit, 0);
			%mini.schedule(0, setLivesLimit, 1);
		case "Deathmatch":
			%mini.schedule(0, removeCTFBricks, %client);
			%mini.gamemode = gameModeTitleToID("Team Deathmatch");
			%mini.schedule(0, setTimeLimit, 10);
			%mini.schedule(0, setPointsLimit, mClamp(%mini.numMembers * 3, 10, 35));
			%mini.schedule(0, setLivesLimit, 0);
		case "Capture The Artifact":
			%mini.gamemode = gameModeTitleToID("Capture the Flag");
			%mini.schedule(0, setTimeLimit, 5);
			%mini.schedule(0, setPointsLimit, mClamp(%mini.numMembers * 3, 10, 35));
			%mini.schedule(0, setLivesLimit, 0);
			%mini.tdmModeRule0 = 5; //Points per flag
			%mini.tdmModeRule2 = 10; //Flag respawn time
			%mini.tdmModeRule3 = true; //Flags can be recovered
			%mini.tdmModeRule4 = true; //Can capture without own flag
	}
	parent::reset(%mini, %client);
	%mini.schedule(3000, messageAll, '', "\c5This match's gamemode is \c3" @ %mode @ "\c5.");
	%mini.schedule(15000, startStalkerGamemodeVote);
}