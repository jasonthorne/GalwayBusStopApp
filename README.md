#Mobile Applications Project.#

###Jason Thorne. G00317349.###

This is an app designed for giving realtime information on all Bus Éireann bus routes within Galway. It uses Bing Maps to display to the user's location upon opening, along with the locations of every bus stop within the city. 

When a bus stop is selected, an API call is sent to [Galway Bus RTPI API](https://github.com/appsandwich/galwaybus#galway-bus-rtpi-api). This retreives the information on all busses scheduled to arrive at that stop, and displays the relevent details to the user through a message dialog. 

There is also a menu bar on the top left of the app which displays all scheduled bus routes. When one of these routes is selected, a line is drawn on the map showing every stop on that particular route. 

In order to run the app, simply clone this repository or download a zipped copy. Then with Visual Studio 2015 installed, open the **'MobileAppProj'** directory, and run **'MobileAppProj.sln'**. 



