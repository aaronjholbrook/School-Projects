Create Domain uniqueID char(22);
Create Domain nameOf char(40) not null;
Create Domain numOf int default 0;
Create Domain unchecked bool default false;

Create Table Users
(
userID			uniqueID,
username		nameOf,
stars			numOf,
fans			numOf,
userJoined		date,	
usefulVote		numOf,
funnyVote		numOf,
coolVote		numOf,
tipCount		numOf,
totalTipLikes	numOf,
latitude		int,
longitude		int,
Primary Key(userID)
);

Create Table Business
(
businessID		uniqueID,
businessName	nameOf,
theState		char(2) not null,
city			char(40),
postalCode		int,
address			char(50),
distance 		int default 0,
numOfCheckins	numOf,
numOfTips		numOf,
hours			char(20),
Primary Key(businessID)
);

Create Table Price
(
price1		unchecked,
price2		unchecked,
price3		unchecked,
price4		unchecked
);

Create Table Attributes
(
acceptCard 		unchecked,
takeReserve		unchecked,
wheelAccess		unchecked,
outdoorSeat		unchecked,
goodForKids		unchecked,
goodForGroups	unchecked,
Delivery		unchecked,
TakeOut			unchecked,
Wifi			unchecked,
bikePark		unchecked
);

Create Table Meal
(
breakfast		unchecked,
lunch			unchecked,
brunch			unchecked,
dinner			unchecked,
dessert			unchecked,
lateNight		unchecked
);

Create Table Tips
(
tipUserID		uniqueID,
tipUsername		nameOf,
tiplikes		numOf,
tipTimeStamp	date,
tip				text not null,
Primary Key (tipUSerID, tipTimeStamp)
);

Create Table Friends
(
friendID		uniqueID,
hasFriend 		uniqueID,
isFriend		uniqueID,
primary key(friendID),
Foreign key(hasFriend) references Users(userID),
Foreign key(isFriend) references Users(userID)
);

Create Table FriendsLatestTips
(
tipFriendName	nameOf,
tipBusinessName	nameOf,	
tipCityName		nameOf,
latestTip		text not null
	
);