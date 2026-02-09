# The Player Podiums

- Each player/group receives a console containing all the elements to connect to the system
  - the console is powered by a rasperry pi (zero w? idk it will be wifi connected though)
  - the console opens/unfolds into a sort of mini podium that can be set up on a table
  - the console includes the following I/O
    - button - durable, can be slammed to indicate you want to answer
    - leds 
      - rgb
      - should be large/highly visible to the player and other players
      - will indicate when they have been selected to give their answer
      - could indicate when they are being timed out for pushing button too early
    - ideally, a drawing table of sorts but we see
      - alternative is to pass around ipad but i am a bit worried about it getting damaged in the chaos
    - the box needs a way for people to input text. perhaps the following
      - a qr code that takes you to a site/app where you can type in your answer
      - it is being sent to the main server, but the qr code includes a parameter that tells the server which box it is originating from
    - the above method could also work for drawing in a pinch
    - a display, either lcd or 7 segment (or 8, with a decimal point for games where you earn "dollars") to display the player score
      - should be large enough for other players to read from a distance
    - an inner display that other contestants cant see, just a small monochromatic iic (?) display
      - can be used for displaying info like hints/clues/madgab words to players
      - if large enough it could also display the qr code

# The host console

- ipad webapp?

# The main display

- webapp to display the state of the game

# Ideas

- perhaps devices should be attached to host (or maybe we have a "host session") so they can pull them into whichever session the host goes into
- when moving device from one session to another, try to make it easy to keep player names and whatnot. Not locking it to the device but maybe give some autocomplete options based on pulling data from most recent session
- "auto-host" - some games (e.g. pictionary) can be hosted without human interaction, allowing the would-be host to participate as a player
- "transient" sessions - sessions that are deleted as soon as they become inactive
- allow multiple display devices to be connected (mirrors, as displays are RO)
- admin panel
  - view active sessions - sessions that have at least one client (host, player, display) connected to them
    - view network activity / event stream for a given session
  - view connected devices
  - view all devices
- player view
  - Odyssey logo - visible when connected to the session, opens up a "meta" menu
    - leave session
    - show connection status/errors
    - enable/show on-screen HWIO
    - enable/show split-screen display
  - on screen HWIO (if enabled) - provides HWIO accessible through the browser - buzzer button, plays sounds through device speakers, light strip, etc
  - split-screen display, if enabled. so with just a single device you could play remotely
    - imagine playing while on a discord call
- host view
  - "overrides" section during session for things like quickly fixing player scores
    - for deeper fixes host can directly edit game state
      - for super deep fixes, host can manually edit game state json - maybe we have validation/schemas for this? c# serialization will catch it i suppose
- i think it would be funny to occasionally mix in the "fahhhh" sound for the [X] buzzer in family feud. vine boom sound is also funny
- theme / intro music - able to select different versions (remix, different years, etc), either in game board or application/game settings 
  - user uploaded, along with gameboards to avoid uploading copyrighted stuff to GL
- sound board for host to control - themed to the game or generic. not for the game itself (those can be automatic) but just for arbitrarily playing them
- sound board can include videos or sounds, which will be shown on display
- button to view the rules of the game (players or host) or display the rules on the display (triggered by host).\
- option to add additional "custom" rules section in game board.
- import / export game boards - should probably include a version field, json
- allow renaming of games -> e.g. Domestic Dispute -> Family Feud
- light & dark themes for system
- odyssey, "hybrid" (gameshow colors but muted, odyssey-style), "native" (full on gameshow colors) colorscheme options (in application settings, per game)
- game changer / make some noise / trivia / generic points game (put this text on board, give me the option to have people buzz in for things, let me arbitrarily assign a cuztomizable quanity of points to a given player at any time
- Debug Game for verifying the system before we move to family feud and whatnot

# Games to add

- Family Feud
- Jeopardy!
- Deal or no Deal
- Pictionary
- Boggle
- Mad Gab
- Words on Stream
- heads up
- charades
- price is right