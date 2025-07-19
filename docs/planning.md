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
