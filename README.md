# valor-project
## v0.4 4/2/23
**LOTS ADDED**
- pvp mode
- usernames
- respawns
- m16, m9, mp5 all added and works great however mp5 recoil is not perfectly adjusted
- new map (more to come)
- server messages: player kills, player joins/leaves

**Game is now perfectly playable with gameplay loop!!!**

**to fix**
- sounds aren't replicated to other clients
- lag on networking possibly due to code
- zombie mode is all hold until pvp is working well with more content

**whats needed**
- maps
- more guns / unlockable guns
- stats
- player models
- game modes: first to x amount of kills, king of the hill, capture the flag
- teams?

## v0.2 3/27/23

**added/fixed**

- zombie model and ragdoll (temporary model, put in framework needed for higher quality model and animations)
- zombies stop infront of destination rather than clippin through it
- cleaned up the map
- players cannot join in middle of round but only during intermission
- lots more to add and fix (not a lot of time to work on project during weekdays + making first character model took some time)

**bugs/fixes**

- barriers aren't buffered still (simple fix just havent gotten to it)
- working on "host as left match - everyone must be kicked" when masterClient leaves
- "player leaves room in middle of match resulting in loss replication"(not masterclient) should not be an issue but further testing required
- room list active however new instance is created everytime player leaves or joins a room

**needs**

- player username
- player list
- player death
- view barrier health

**to add**

- remodel on weapons
- player avatar
- item shops
- powerups
- experience/ reward system
- replicate sounds

## v0.1 3/26/23
**bugs/fixes**
- enemy physiccs aren't replicated
- ~~player can join room in middle of match~~ barriers aren't buffered and possibly more issues
- player leaves room in middle of match resulting in loss replication
- ~~needs room list~~ room list needs to be polished and cleaned up
- player usernames
- player list within room
- death screen isn't properly implemented with respawn system
- physical way to view barrier health
- remodel shotgun/pistol + combine shotgun script into GunController script
- fix vending machines
- add powerups
- add experience/reward system
- add items to purchase outside of match (persistent items for players to use in all matches ex: skins, weapons, cosmetics)
- create proper player model
- replicate and destroy sounds correctly
