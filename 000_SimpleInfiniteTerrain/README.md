## Simple Infinite Terrain

---

Creates an endless flat plane that loads in tiles as the player moves around the landscape. Somewhat boring to look at by itself but is useful as a starting 'sandbox' for other projects. 

---

1. Create an empty GameObject (Cmd-Shift-N), name it “Landscape", and attach the “InfiniteTerrainGenerator.cs” script.

2. Create a new Plane from the GameObject -> 3D Object menu, name it “SmartPlane”, and attach the “GenerateTerrain.cs” script.

3. Go to Assets -> Import Package -> Characters. Deselect the ThirdPersonCharacter and RollerBall options.  
Import the FPSCharacter and drag the FPSController prefab into the scene.

4. Edit the camera within the FPS Character to increase the Far clipping plane (maybe 2000 - 5000).

5. Set the public variables on Landscape to your preferred values (suggested: SCALE = 5, RADIUS = 10, PLANE_SIZE_IN_QUADS = 20). 

> SCALE: larger values spread the world out at no rendering cost. Smaller values let you fine tune the details of near features.\
> RADIUS: number of concentric square ‘rings’ of tiles to load around the tile containing the player. Larger numbers will cause exponentially worse performance issues.\
> PLANE_SIZE_IN_QUADS: number of vertices, minus 1, along one edge of each tile. Max is about 250 which may cause performance issues.
  
