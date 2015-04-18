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
$Notice[$NoticeCount++] = "Humans, it's dangerous to go alone! Stay in a group to deter Stalker attack!";
$Notice[$NoticeCount++] = "Curses are area denial weapons: ineffective on loners, deadly against static groups.";
$Notice[$NoticeCount++] = "Check out the official topic on <a:forum.blockland.us/index.php?topic=162399.0>the forums</a>\c3!";
$Notice[$NoticeCount++] = "You can set a preferred team with \c6/setPreferredTeam <Stalkers/Humans/None>\c3.";
$Notice[$NoticeCount++] = "You can download the StalkerDM client-side mod <a:www.mediafire.com/download.php?v4a2bpdp2041wru>here</a>\c3!";