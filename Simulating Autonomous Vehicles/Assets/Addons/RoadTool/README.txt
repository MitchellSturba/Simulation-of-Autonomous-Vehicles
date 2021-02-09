# To start making roads:
	
	- Create a large surface, or terrain with which to draw your road.
	- GameObject->Create Other->Road (or Control+Shift+R if you're handy like that).
	- Click on Terrain or other mesh with collider attached.
	- Click again to add points to road.
	- Click "Lock" to stop editing road.
	- Click "Modify" to resume editing road.

# But watch Out For:
	- Meshes without colliders.  Y.A.R.T. need colliders to register mouse clicks (Unity Terrain is okay).
	- Adding points with your Tool set to anything other than 'Move'.  This prevents
		silly errors like adding points when you're just trying to pan around the 
		scene.

# Oh, and some neat features that may interest you:
	- UV2 channel is generated automatically.
	- You can adjust offset and scale by using the "UV Options" foldout.
	- Setting the "Insert at Point" value to 0 will add points at the beginning of 
		your road.  Setting it to '5' with 10 points will put a new point in middle.
		Default is -1, which inserts points at the end of your road.
	- Adjusting the "Ground Offset" value changes how high off the terrain your road
		will sit.  Usually, I just nudge to the point that z-fighting stops.
	- Modifying the "Road Width" value changes how wide the road is (duh).

# If you want to email me to complain:
	- Email Joseph Doe at jdoe@skynet.com

# If you want to email me with thanks and praise:
	- Email Karl at karl@paraboxstudios.com