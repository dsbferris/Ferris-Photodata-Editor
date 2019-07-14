# Ferris-Photodata-Editor
##Adjusts Photodates from iCloud

If you are moving from Apple to Android, I strongly recommend you to have a Computer next to you. Otherwise things will get difficult or chaotic.

If you also miss your creation dates in Photos Metadata, this could be the solution for you.
It reads the date from iCloud in yyyyMMdd-HHmmss format from parent folder and overrides it to Photos Metadata.

##Instruction to use:
-
-Get a Mac and login via iCloud

-Go into Photos app, wait till download of your Photos is finished and in Headbar: Photos -> Preferences -> General : Show in Finder

-Rightclick -> Show Package Contents

-Copy over the contained Master folder to your Windows maschine

-Run "Ferris Photo(data) Editor", select Master folder in first box, select an output directory in second

-Now click change

-If all your photos were contained in format like this (//YYYY//MM//DD//YYYYMMDD-HHmmss//IMG_XXX.JPG) everything should work fine

-For video files I made set the file creation and last access/write date to this, because it is currently no implemented to change Video metadata too


Hope you enjoyed it ;)
