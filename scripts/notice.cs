/////////////////////////
//STALKER DM: NOTICE.CS//
/////////////////////////

//////////////
// CONTENTS //
//////////////
//#1. FUNCTION CREATION
//	#1.1 noticeTick
//#2. NOTICE REGISTRATION
//	#2.1 notices

//#1.
//	#1.1
function noticeTick(%delay)
{
	cancel($noticeTick);
	while((%r = getRandom(1, $noticeCount)) == $lastNotice)
	{
	}
	messageAll('', "\c7bot\c5NOTICE\c3: " @ $Notice[%r]);
	$lastNotice = %r;
	$noticetick = schedule(%delay, 0, noticeTick, %delay);
}
schedule(10000, 0, noticeTick, 90000);

//#2.
//	#2.1
$NoticeCount = 0;
$Notice[$NoticeCount++] = "You can purchase new weapons with your Achievement points! /viewstore";
$Notice[$NoticeCount++] = "Check out the official topic on <a:forum.blockland.us/index.php?topic=278041.0>the forums</a>\c3!";
$Notice[$NoticeCount++] = "You can set a preferred team with \c6/setPreferredTeam <Stalkers/Humans/None>\c3.";
$Notice[$NoticeCount++] = "You can download the StalkerDM client-side mod <a:www.mediafire.com/download.php?v4a2bpdp2041wru>here</a>\c3!";
$Notice[$NoticeCount++] = "You can check how many achievement points you have with \c6/checkPoints\c3.";
$Notice[$NoticeCount++] = "You can set a handicap with the command \c6/setHandicap <0.01 .. 0.99>\c3.";
$Notice[$NoticeCount++] = "Humans, did you know you can quickly use your gadget with the jet button?";
$Notice[$NoticeCount++] = "Stalkers, don't run around with your spells out! Everyone can see them!";
$Notice[$NoticeCount++] = "The \"Stalkers\" are the servants of the Aztec god Omacatl. They guard his temple from archaeologists and looters.";
$Notice[$NoticeCount++] = "Humans, don't forget to use your Stimpak! It provides you with a burst of HP. Use your jet button to use it quickly.";