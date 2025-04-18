# cassette player
a better way to make cassette music

## how to use
first download the plugin pack from the GameBanana page, and unzip the content directly to your FMOD project's `Plugin` folder

in FMOD Studio, create an event for your cassette song and add the `sixteenth_note` parameter (according to the Everest documentation). then right-click on the timeline, and add a `Plug-in Instrument > luna-tek > Cassette Player` instrument

place a loop region over this instrument. set the `#Notes` parameter to the number of notes, then automate `Note` linearly based on `sixteenth_note`, and drag in your cassette song's `.wav` file into the drag-and-drop zone. done!

you can test your cassette song by playing the song and  using the `Scripts > luna-tek > Audition Cassette Song` option in the main toolbar, dragging the BPM slider. you can also adjust the portion of the audio file that's considered by using the min/max slider