<a name="readme-top"></a>
<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->


<!-- PROJECT LOGO -->

<p align="center">
<img width="300" height="125" src="https://i.imgur.com/w5hcUtR.png">
</p>

<div align="center">

<h3 align="center">2D Procedural Generation</h3>

  <p align="center">
    A versatile 2D procedural generation for the Unity game engine. 
    <br />
    <br />
    <a href="https://github.com/ryan-io/procedural-generator/issues">Report Bug</a>
    ·
    <a href="https://github.com/ryan-io/procedural-generator/issues">Request Feature</a>
  </p>
</div>

---
<!-- TABLE OF CONTENTS -->

<details align="center">
  <summary>Table of Contents</summary>
  <ol>
  <li>
      <a href="#overview">Overview</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites-and-dependencies">Prerequisites and Dependencies</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage & Examples</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments-and-credit">Acknowledgments</a></li>
  </ol>
</details>

---

<!-- ABOUT THE PROJECT -->

# Overview
This procedural generator started as a hobby project to learn about various procedural algorithms (cellular automata, marching squares and Bresenham's line algorithm to name a few). Inspiration was drawn from Sebastian Lague's video series on 'Procedural Cave Generation'.

<p align="center">
<img  src="https://i.imgur.com/RW2xyFI.png" width="600">
</p>

##### Features
<ol>
<li>
Create a 2-dimensional square or rectangular map
</li>
<li>
 Generate colliders for use with 2D & 3D physics
</li>
<li>
Generate sprite shape borders
</li>
<li>
Integrates with the Astar Pathfinding Project and generates pathfinding with its API
</li>
<li>
Generation occurs asynchronously and concurrently.
</li>
<li>
Optionally supports integration with Unity's tilemap & tile system
</li>
<li>
Define segments for "randomly" placing game objects
</li>
<li>
Utilizes Unity's Burst compiler and the Jobs systems for performance
</li>
<li>
Serialize and deserialize map, mesh, pathfinding, collider and sprite shape data
</li>
<li>
Integration with Odin Inspector
</li>
</ol>

![](https://github.com/Your_Repository_Name/Your_GIF_Name.gif](https://github.com/ryan-io/procedural-generator/blob/main/proc-gen-demo-resized.gif)

<p align="center">
<img  src="https://i.imgur.com/X4xsYEv.gifv" width="600">
</p>

<p align="right">(<a href="#readme-top">back to top</a>)</p>

# Built With
- Unity Game Engine
- JetBrains Rider
- Odin Inspector

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- GETTING STARTED -->
# Getting Started
This project can be used as a new, standalone Unity project, installed into an existing project using UPM, or manually imported into a Unity project. 

It needs to be stated that this generator is opinionated in how it is setup within a Unity scene. See the 'Usage' section for more information regarding this.

The generator is setup to run out of the box. You are more than welcome to define your own generation process describe in the 'Usage' section. 

It is opinionated. One of the primary goals was to create a new scene, import the generator and have the scene functionally ready within a few minutes. As such, I opted to implore the use of some outside projects that are defined in the 'Prerequisites and Dependencies' section.
<p align="right">(<a href="#readme-top">back to top</a>)</p>


# Prerequisites and Dependencies
##### All dependencies are internal Unity systems or Unity projects that can be found on the asset store.

**Newer versions of these packages should be compatible.**

- Unity 2022.3.9.f1
	- This project was originally created with Unity 2020.3.5. 2022.3 is required due to improvements in the Burst, Jobs, SpriteShape and TileMap systems
* Unity Burst 1.6.5
* Unity Collections 1.2.3
* Unity Jobs 0.50.0-preview.9
* UniTask 2.2.5
* Unity TileMap 1.0.0
* Unity Sprites 1.0.0
* Unity 2D Tilemap Extras 1.6.0
* Universal Render Pipeline 12.1.6
	* This is not a hard requirement. For demonstration using a 2D environment, URP was selected.
* Unity Addressable 1.19.19
* A* Pathfinding Project
	* This is a paid asset. It is the most opinionated sub-system to this generator. Aron does *terrific* work with his asset and A* Pathfinding Project is simply a great asset to use.
	* *This technically is NOT required*
* EasyWallCollider
	* This is a paid asset. It is required if you want to generate primitive 3D colliders in a 2D environment. This is used for 2D games that want to use 3D physics with "2D colliders".

##### Please feel free to contact me with any issues or concerns in regards to the dependencies defined above. We can work around the majority of them if needed.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

# Installation
> The generator and many dependencies can be installed via the UPM, a package manifest, or the asset store.


### UPM Installation
> To install a package via UPM, open or create a project in Unity and open the Package Manager. Click "Add packge from git URL"

<p align="center">
<img  src="https://i.imgur.com/VO1LgrH.png" width="200">
</p>

> Copy & paste the GIT url for the package you are installing and click "Add". You will need to navigate to "Packages/manifest.json" in your file explorer.

### Manifest Installation
> To install a package via a package manifest, open or create a project in Unity. 
> Navigate to Packages/manifest.json in your file explorer

<p align="center">
<img  src="https://i.imgur.com/GCzGPou.png" width="500">
</p>

> Open your manifest JSON file; you will need to add the appropriate query to this file
> An example manifest.json is given below. 
> **NOTE**: do not simply copy and paste this example into your own manifest.json. This is just an example to show demonstrate how to add a dependency package.

``` ExampleManifest
{  
  "scopedRegistries": [  
    {  
      "name": "A* Pathfinding Project",  
      "url": "PROJECT_URL_TO_YOUR_VERSION",  
      "scopes": [  
        "com.arongranberg.astar"  
      ]  
    }  
  ],  
  "dependencies": {  
    "com.arongranberg.astar": "4.3.60",  
    "com.cysharp.unitask": https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask",  
    "com.unity.adaptiveperformance": "4.0.1",  
    "com.unity.addressables": "1.21.17",  
    "com.unity.burst": "1.8.4",  
    "com.unity.collab-proxy": "2.0.7",  
    "com.unity.collections": "2.2.0",  
    "com.unity.feature.2d": "2.0.0",  
    "com.unity.ide.rider": "3.0.25",  
    "com.unity.ide.visualstudio": "2.0.18",  
    "com.unity.inputsystem": "1.7.0",  
    "com.unity.profiling.core": "1.0.2",  
    "com.unity.render-pipelines.universal": "14.0.8",  
    "com.unity.textmeshpro": "3.0.6",  
    "com.unity.timeline": "1.7.5",  
    "com.unity.ugui": "1.0.0",  
    "com.unity.visualscripting": "1.9.0",  
    "com.unity.modules.androidjni": "1.0.0",  
    "com.unity.modules.animation": "1.0.0",  
    "com.unity.modules.assetbundle": "1.0.0",  
    "com.unity.modules.audio": "1.0.0",  
    "com.unity.modules.cloth": "1.0.0",  
    "com.unity.modules.director": "1.0.0",  
    "com.unity.modules.imageconversion": "1.0.0",  
    "com.unity.modules.imgui": "1.0.0",  
    "com.unity.modules.jsonserialize": "1.0.0",  
    "com.unity.modules.particlesystem": "1.0.0",  
    "com.unity.modules.physics": "1.0.0",  
    "com.unity.modules.physics2d": "1.0.0",  
    "com.unity.modules.screencapture": "1.0.0",  
    "com.unity.modules.terrain": "1.0.0",  
    "com.unity.modules.terrainphysics": "1.0.0",  
    "com.unity.modules.tilemap": "1.0.0",  
    "com.unity.modules.ui": "1.0.0",  
    "com.unity.modules.uielements": "1.0.0",  
    "com.unity.modules.umbra": "1.0.0",  
    "com.unity.modules.unityanalytics": "1.0.0",  
    "com.unity.modules.unitywebrequest": "1.0.0",  
    "com.unity.modules.unitywebrequestassetbundle": "1.0.0",  
    "com.unity.modules.unitywebrequestaudio": "1.0.0",  
    "com.unity.modules.unitywebrequesttexture": "1.0.0",  
    "com.unity.modules.unitywebrequestwww": "1.0.0",  
    "com.unity.modules.vehicles": "1.0.0",  
    "com.unity.modules.video": "1.0.0",  
    "com.unity.modules.vr": "1.0.0",  
    "com.unity.modules.wind": "1.0.0",  
    "com.unity.modules.xr": "1.0.0"  
  }  
}
```

### UniTask
```UPM
 https://github.com/Cysharp/UniTask
```

```Manifest
"com.cysharp.unitask": "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask"
```

### A* Pathfinding Project
```UPM
Please visit this link to install via UPM:
https://www.arongranberg.com/astar/download_upm
```

```Manifest
"com.arongranberg.astar": "4.3.60"

"scopedRegistries": [  
  {  
    "name": "A* Pathfinding Project",  
    "url": "https://arongranberg.com/packages/eb088f6cecf90e6a2451cf15241e7b955e94e8ec3fe14/",  
    "scopes": [  
      "com.arongranberg.astar"  
    ]  
  }  
]
```

### EasyWallCollider
```AssetStore
https://assetstore.unity.com/packages/tools/physics/easy-wall-collider-158206
```

### Addressables
```
"com.unity.addressables": "1.19.19",
```

### Burst
```
"com.unity.burst": "1.6.5",
```

### URP
```
"com.unity.render-pipelines.universal": "12.1.6",
```

### 2D Sprites
```
"com.unity.feature.2d": "2.0.0",
```

### Collections
```
"com.unity.collections": "1.2.3",
```

### Generator
1. Clone or fork this repository to your PC
	1. This repository is an entire project
	2. You can use the project as a template or simply rip out anything you don't need

```Shell
git clone https://github.com/ryan-io/procedural-generator
```

2. Install via Unity UPM
	1. See "UPM Installation" for how-to

```Manifest
"com.ryanio.procgen": "https://github.com/ryan-io/procedural-generator"
```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- USAGE EXAMPLES -->
# Usage

### Scene Setup
##### Manually
1. Create an empty game object; add a 'Procedural Generator' monobehavior
	1. width="300" height="125"
2. The monobehavior component will look like this

<p align="center">
<img  src="https://i.imgur.com/u1sUljZ.png" width="500"/>
</p>

##### Using Included Prefab
1. If you are using the project repository (forked or cloned the entire Git repo), then you can also simply add the included Procedural Generator Prefab to a scene.
2. The prefab can be found at 

```Directory
Packages\ryan-io-procedural-generator\Assets\Prefabs\procedural-generator
```

   within the Unity Editor.

<p align="center">
<img  src="https://i.imgur.com/vlJnN5K.png" width="400"/>
</p>
### Map Anatomy

<p align="center">
<img  src="https://i.imgur.com/YIHZHeY.png" width="600"/>
</p>
- 'Pink' background is a simple material to show the procedurally generated mesh.
	- Behind this mesh are 'red' pathfinding nodes. These nodes are NOT walkable/searchable
- The 'blue' are pathfinding nodes that ARE walkable/searchable.
- The 'green' gizmo are the generated colliders. These act as the map boundary.
### Settings

Settings are divided into two groups: 'Map Configuration' and 'SpriteShape Configuration'. 'Map Configuration' contains settings relevant to the map generation, serialization, deserialization, tiles, pathfinding, colliders, events and more. 'SpriteShape Configuration' contains settings relevent to sprite shape generation.

These configurations are isolated from each other for serialization purposes.

<p align="center">
<img  src="https://i.imgur.com/Zo29Hqy.png" width="500"/>
</p>

##### Setup

> Setup is intended for high level configuration of the generator. This is the entry point for this package.

<p align="center">
<img  src="https://i.imgur.com/tcauKhp.png" width="500"/>
</p>

- State
	- Map Name/Id
		- An identifier for the map. This is used in instantiated game objects and prefabs. All serialized data will append this ID to the file names.
	- IsBuild
		- Typically this should be left to 'No'
		- There are optimizations for 'Yes' in place, but this setting is for runtime generation (not recommended).
	- Run
		- 'Create Map' will run the generator
		- 'Deserialize Map' will deserialize generated data and re-instantiate monobehaviors from this data. 
- Seed
	- These settings allow for a more deterministic approach to procedural generation. Specifying your own seed allows for map generation to be more predictable. If you want "truly" randomization of each map, select 'Use Random Seed'. Otherwise, define your own (one will be generated if not specified).
- UseRandomSeed
		- If 'Yes', a seed will be provided for you. If 'No', create your own or use a previously defined seed value
	- Seed
		- The value seed into the generated. Under the hood, is really just a hash code defined in an extension method.
	- LastSeed
		- ReadOnly; the last seed used by the generator
	- LastIteration
		- Readonly; if using the same seed, the iterator will simply increase its value by '1' each time you reuse a seed. This is to prevent overwriting data. Will always be '0' if using random seeds

<p align="center">
<img  src="https://i.imgur.com/8GrwA68.gif" width="600"/>
</p>

- Monobehaviors
	- Any required monobehaviors are defined here.
	- At this point in time, the only required monobehavior is the Pathfinder component from the A* Pathfinding Project
	- If this is not set, one will be (try) created. Otherwise, no pathfinding will be calculated
	- Pathfinder
		- Scene reference to a game object that contains the Pathfinder component

<p align="center">
<img  src="https://i.imgur.com/Khho7Fy.png" width="600"/>
</p>

##### Serialize & Deserialize
> This grouping is for saving and loading generated data. Elements that can be serialized are: mesh, pathfinding, map prefab(EXPENSIVE), sprite shape and colliders.
> *** Generating and serializing a map prefab will utilize a non-negligible amount of storage on your hard drive. Take care if you are repeatedly generating maps.

<p align="center">
<img  src="https://i.imgur.com/8fBZ22g.png"/>
</p>

- Iteration Tracker
	- Name Seed Iteration
		- This is a drop down that contains a list of all maps that have been generated AND serialized. If you do not serialize a map, the name seed iteration will not be added to this collection.
		- Selecting a value from this drop down will allow you to deserialize or delete the data
- Serialization
	- Select 'Yes' do serialize data when you generate a map. Select 'No' for the elements you do NOT want data serialized for.
- Deserialization
	- These settings work identical to the serialization settings
- Delete Selected Serialized Button
	- The value of NameSeedIteration will have all of its data deleted.

##### Map 

> These settings control the characteristics of your maps. Each setting will have a short written description, followed by two pictures contrasting the difference between values at the low and high end.

##### Default Settings
<p align="center">
<img  src="https://i.imgur.com/1kN6iVv.png" width="600"/>
</p>

- Columns
	- The number of columns the map should generate
	- Colums = 50

| Columns=50                                                                              |
| --------------------------------------------------------------------------------------- |
| <p align="center"><br><img  src="https://i.imgur.com/tVIHgul.png" width="225"/><br></p> |
| **Columns=200**                                                                         |
| <p align="center"><br><img  src="https://i.imgur.com/kaBeVSu.png" width="600"/><br></p>                                                                                        |

- Rows
	- The number of rows the map should generate

| Rows=50                                                                              |
| --------------------------------------------------------------------------------------- |
| <p align="center"><br><img  src="https://i.imgur.com/tVIHgul.png" width="225"/><br></p> |
| **Rows=200**                                                                         |
| <p align="center"><br><img  src="https://i.imgur.com/5skUyEb.png" width="225"/><br></p>                                                                                        |

- CellSize
	- THIS IS NOT A SCALAR FACTOR
	- The larger this value, the more nodes will be generated; no nodes will be scaled.
	- **Setting this to a value other than '1' make a few algorithms run with a quadratic time complexity

| CellSize=1 & CellSize=2 |
| ---- |
| <p align="center"><br><img  src="https://i.imgur.com/LlcrrXV.gif" width="600"/><br></p> |

> CellSize * Rows MUST be <= 1024 for pathfinding to generate
> CellSize * Columns MUST be <= 1024 for pathfinding to generate

- **Columns X Rows = TotalNumberOfCells

- BorderSize
	- An optional border around the map.
	- If a portion of your map is being generated 'outside' the bounds, setting this to a higher value will solve this issue.
- SmoothingIterations
	- How many times to run all algorithms on a generated map
	- This setting is more trial-and-error than anything. Setting this value to 1 is typically too low. Setting this value very high will not have any different effect than most lower values. The trick is to find how many smoothing iterations results in a 'clean' looking map. This value is typically between 5-20.

| SmoothingIterations=1 | SmoothingIterations=5 | SmoothingIteratins=20 |
| ---- | ---- | ---- |
| <p align="center"><br><img  src="https://i.imgur.com/i4H0RHS.png" width="200"/><br></p> | <p align="center"><br><img  src="https://i.imgur.com/eYMPGtx.png" width="200"/><br></p> | <p align="center"><br><img  src="https://i.imgur.com/D9nz4PI.png" width="200"/><br></p> |

*** There is variance once SmoothingIterations reaches it critical value. This has to do with the 'randomness' baked into the generator.

- There are diminishing returns for high values, and not post-processing for lower values.
	- 'Correct' vales can give a more a map with a more purposeful layout
	- This setting has diminishing returns when working with smaller maps, high WallRemovalThreshold or high RoomRemovalThreshold values.
- WallRemovalThreshold
	- How many cells can be in a cluster that do NOT enclose open space (think a room).

<p align="center">
<img  src="https://i.imgur.com/urRNijf.png" width="400"/>
</p>

   - The larger the number, the less stand alone walls there will be. The small the number, the more there will be. This can make maps feel and appear more dense. 
   - Lower numbers are great if you want to a lot of obstacles in your map(s).

| WallRemovalThreshold=1 | WallRemovalThreshold=100 | WallRemovalThreshold=500 |
| ---- | ---- | ---- |
| <p align="center"><br><img  src="https://i.imgur.com/2ZJcvvI.png" width="200"/><br></p> | <p align="center"><br><img  src="https://i.imgur.com/DSQyy9B.png" width="200"/><br></p> | <p align="center"><br><img  src="https://i.imgur.com/TIluEM0.png" width="200"/><br></p> |

- RoomRemovalThreshold
	- Analogous to WallRemovalThreshold, but for rooms
	- A room is defined as open/free map space that is enclose by a wall.
	- An example of a room:

<p align="center">
<img  src="https://i.imgur.com/MsQNy0m.png" width="500"/>
</p>

   - Notice the area circle in 'red' is not closed off. This is an example of a passage
   - **Rooms are not entirely isolated from other rooms. One of the generator's constraints is to create maps where all rooms are connected.

| RoomRemovalThreshold=1 | RoomRemovalThreshold=100 | RoomRemovalThreshold=500 |
| ---- | ---- | ---- |
| <p align="center"><br><img  src="https://i.imgur.com/2ZJcvvI.png" width="200"/><br></p> | <p align="center"><br><img  src="https://i.imgur.com/2ZJcvvI.png" width="200"/><br></p> | <p align="center"><br><img  src="https://i.imgur.com/2ZJcvvI.png" width="200"/><br></p> |

- LowerNeighborLimit
	- The minimum number of cells that are generated as a cluster.
	- This setting has DRASTIC consequences for a value too lower or too high
	- Play around with it; I've used it as a simple customization option during my testing.
	- **A value of '4' is appropriate for MOST maps
- UpperNeighborLimit
	- The maximum number of cells that are generated as a cluster.
	- This setting has DRASTIC consequences for a value too lower or too high
	- Play around with it; I've used it as a simple customization option during my testing.
	- **A value of 6 is appropriate for MOST maps.
- WallFillPercentage
	- How dense the map is.
	- Higher values will yield less rooms generated
	- Lower values will yield more rooms genereated. Too low of a value and the map will appear empty.
	- Values are limited between 40 & 55. The generation algorithm is very sensitive to fluctuations of this value. **47 is the default**.

<p><b><i>WallFillPercentage = 47</i></b></p>

<p align="center">
<img  src="https://i.imgur.com/yvdADGD.png" width="600"/>
</p>

<p><b><i>WallFillPercentage  = 45</i></b></p> 

<p align="center">
<img  src="https://i.imgur.com/yo5kYrk.png" width="600"/>
</p>

<p><b><i>WallFillPercentage = 40</i></b></p> 

<p align="center">
<img  src="https://i.imgur.com/C4dG53v.png" width="600"/>
</p>

<p><b><i>WallFillPercentage = 49</i></b></p> 

<p align="center">
<img  src="https://i.imgur.com/jp8o0Uf.png" width="600"/>
</p>

<p><b><i>WallFillPercentage = 55</i></b></p> 

<p align="center">
<img  src="https://i.imgur.com/M3wDv9v.png" width="600"/>
</p>

- CorridorWidth
	- How wide or narrow each connected room passageway is
	- Below is an example with CorridorWidth => {3, 5}
		- SOME passageways are circled in 'red'.

<p align="center">
<img  src="https://i.imgur.com/cdhfyEz.png" width="600"/>
</p>

- GroundLayerMask
	- What layer mask the 'ground' should be on.
- ObstacleLayerMask
	- What layer mask 'obstacles' should be on.
- BoundaryLayerMask
	- What layer ask the 'boundary' of the map should be on.
- MeshMaterial
	- Optional: the material to apply to the generated mesh
	- The example below shows a 'salmon' colored (very generic) material applied to a generated mesh

<p align="center">
<img  src="https://i.imgur.com/61Foupt.png" width="600"/>
</p>

##### Tiles

> The generator can also utilize the internal tile mapper. This mapper utilizes Unity's [Rule Tiles](https://docs.unity3d.com/Packages/com.unity.2d.tilemap.extras@1.6/manual/RuleTile.html) & [Tilemaps](https://docs.unity3d.com/Manual/Tilemap.html) systems. By default, this system is disabled and should be used sparingly. This is one of the systems I need to implement optimizations for. It does work, but will add on significant time to the generation process. The larger the map and/or the larger the cell size, the less performant this system becomes

<p align="center">
<img  src="https://i.imgur.com/kDEQizA.png" width="500"/>
</p>

- RenderTilemaps
	- The generator can choose to calculate where tiles should go, but to not render them. 
	- 'Yes' will still run the tile mapper. All tile map renderers will be disabled at the end of the generation process
- CreateTileLabels
	- If yes, debug labels will be displayed to indicate the tile dictionary key
- CreateTileAngles
	- A small optimization; selecting yes will allow rule tiles to calculate in up to 8 directions instead of 4. 
- TileDictionary
	- A pre-populated dictionary with keys for each boundary direction as well as the ground. Values should be assigned appropriate tiles (TileBase, Tile, RuleTile, etc.).

##### Pathfinding

> The generator integrates terrifically with the [A* Pathfinding Project](https://arongranberg.com/astar/). At its core, the A* Pathfinding Project uses the A* pathfinding algorithm. But it is much more than that. This generator integrates with the following systems: 2D pathfinding API, A* heuristics (Euclidean, Manhattan, & Diagonal Manhattan), 2D & 3D physics (collisions) and Erosion.
> I highly recommend you read through the A* Pathfinding Project [documentation](https://arongranberg.com/astar/documentation/stable/) to learning more about these topics.
> This generator exclusively uses [Grid](https://arongranberg.com/astar/documentation/dev_4_3_87_ed9ba20c0/graphtypes.html) graphs.

<p align="center">
<img  src="https://i.imgur.com/74tcSso.png" width="500"/>
</p>

- Astar
	>Visit [here](https://arongranberg.com/astar/documentation/4_2_7_0b5deb87/getstarted.html) for a terrific overview on the A* package
	
	- GeneratedColliderType
		- These are the colliders that are generated around the map border as well as the boundary of generated rooms.
		- Edge
			- A 2D collider that has a single AABB. This means if the collider is stationary (not moving), then only one edge needs to be check for collisions. On the contrary, a box collider has an AABB when all four edges need to be checked for collision. Use this if an EdgeCollider2D is appropriate for your setup
			- [Unity documentation for EdgeCollider2D](https://docs.unity3d.com/Manual/class-EdgeCollider2D.html)
		- Box
			- Colliders are generated using BoxCollider2D
			- [Unity documentation for BoxCollider2D](https://docs.unity3d.com/ScriptReference/BoxCollider2D.html)
		- PrimitiveCombo
			- FOR USE WITH 3D PHYSICS ONLY
			- This option uses the [EasyWallCollider](https://assetstore.unity.com/packages/tools/physics/easy-wall-collider-158206) from Pepijn Willekens
			- At its core, it uses 3D primitive colliders (Box & Capusle)
			- The use case are for 2D games that utilize 3D physics
	- NavGraphCollisionType
		- This determines how collisions are calculated and is mostly used in the A* Pathfinding Project to check for collision on grid graphs.
		- The default is a capsule shape. A sphere shape 
	- NavGraphcollisionDetectionDiameter
		- Sets the diameter for CollisionType.Sphere, the width for CollisionType.Capsule and is the length of CollisionType.Ray
	- NavGraphCollisionDetectionHeight
		- This is only used to set the height of CollisionType.Capsule. It is not used for CollisionType.Sphere or CollisionType.Ray.
	- NavGraphNodeSize
		- How large the A* grid node size is. This is the most performant and critical pathfinding setting. A much lower value results in many smaller nodes, but with better granularity and resolution for pathfinding. The large the value, the larger the grid node and the less "accurate" pathfinding becomes
- Erosion
	>[Erosion](https://arongranberg.com/astar/docs/gridgraph.html) is a way to make a node signify a penalty without making that node unwalkable. Eroding a nodes is simply assigning it a tag. From this tag, you can assign a penalty for selecting this node when calculating a path or make it unwalkable for a specific or group of game objects. 
		- I like to think of Erosion as being analogous to the Unity Layer system. You define a set of layers. You can then assign a game object a layer. Knowing the layer a game object is on can help with physics interactions, transform manipulation and many other elements. Erosion is like this; only in the context of pathfinding. 
		- Each game object in your game can utilize the seeker component. This component will allow you to choose what erosion node tags are traversable.

	- ErodePathfindingGrid
		- Is 'Yes', erosion around obstacles and unwalkable nodes will be eroded
		- There is a great description that Aron (A* Pathfinding Project developer) gave for erosion:
		-<p align="center">
<img  src="https://i.imgur.com/U2pf32e.png" width="600"/>
</p>
	- NodesToErodeAtBoundaries
		- How many nodes to erode starting around the boundary of the map
	- StartingNodeIndexToErode
		- This number represents the number of nodes that become unwalkable before being assigned a tag value.
		- This number must be less than or equal to NodesToErodeAtBoundaries, otherwise all erosion nodes will be set to unwalkable
		- For example, if NodesToErodeAtBoundaries = 5 and StartingNodeIndexToErode = 3, then boundaryNode + 1 = unwalkable, boundaryNode + 2 = unwalkable, boundaryNode + 3 = erosionTag1, boundaryNode + 4 = erosionTag2.
	- DrawNodePositions
		- For debugging
		- Will create a gizmo for each node.transform.position
	- DrawNodePositionsShifted
		- For debugging
		- Will create a gizmo for each nodeShifted.transform.position
			- nodeShifted is the final representation of an "accurate" pathfinding node location. This setting isn't really pertinent to usage of the project, but I left it in just in case.

##### Colliders

> Collider solvers calculate and optimize collider location, geometry, and type. ~~Two~~ One can be used right out of the box: [Edge]() ~~& [Box]()~~. ~~Both of these are~~ This is built into Unity and only works with 2D physics.
> If you want a more hybrid approach that utilizes 3D primitive colliders & works with 3D physics, there is an optional solver type: Primitive Combo. THIS SETTING IS DEPEDNENT ON EASYWALLCOLLIDER. This setting uses a combination of box and capsule colliders to generate boundaries that functionally are analogous to it's 2D alternatives. 

- SolverType
	- Edge
		- Tells the generator to use the EdgeCollider2DSolver
	- ~~Box~~
		- ~~Tells the generator to use the BoxCollider2DSolver~~
	- PrimitiveCombo
		- Requires EasyWallCollider
		- Tells the generator use PrimitiveComboColliderSolver
- EdgeSolver
	- EdgeColliderRadius
		- Per Unity documentation: "Set a value that forms a radius around the edge of the Collider. This results in a larger Collider 2D with rounded convex corners".
	- EdgeColliderOfset
		- Sets the local offset values of the Collider 2D geometry. This setting helps aligning your boundary with the map.
		- Default is 0,0
- ~~BoxSolver~~
	- ~~This option utilizes very thin BoxCollider2D components. A single collider is instantiated per each boundary segment.~~
	- ~~BoxColliderPrefab~~
		- ~~OPTIONAL~~
		- ~~If this is NOT null, a prefab that SHOULD contain a box collider will be instantiated at each calculated boundary segments~~
	- ~~BoxColliderSkinWidth~~
		- ~~The width (and depth) of the box collider (think of this as a 2D cross-section with square geometry)~~
- PrimitiveCombo
	- PrimitiveColliderRadius
		- When the algorithm determines a capsule collider should be used at a control point, this setting defines the radius of the capsule.
	- PrimitiveColliderSkinWidth
		- When the algorithm determines a box collider should be used at a control point, this setting defines the length, width and depth of the box.
	- NodeCullDistance
		- If the algorithm determines two points are too close together, then it will skip adding that point as a control point
		- This setting is the minimum distance required between two points.

##### Events

> The inspector exposes a container of serialized events that are invoked at specific points in the generation process. These are optional and you are NOT required to use this system.
> This container functionality works like a [UnityEvent](https://docs.unity3d.com/ScriptReference/Events.UnityEvent.html).
> You can add a listener to any of the following pre-defined generation checkpoints

- SerializedEvent process
	- Cleaning
	- Initializing
	- Running
	- Completing
	- Disposing
	- Error

##### SpriteShapeConfiguration

> This system utilizes its own configuration process and is also isolated from the standard ProceduralGenerationConfiguration for serialization purposes. Please read Unity's documentation on [2D Sprite Shapes](https://docs.unity3d.com/Packages/com.unity.2d.spriteshape@3.0/manual/index.html).
> Some of these settings will not make sense until you are familiar with the SpriteShape API.

- PPU
	- Pixels per unit
	- How many pixels a sprite corresponds to in a single world unit
	- The higher the value, the 'larger' pixels will appear, but can appear jagged
	- The lower the value, the 'smaller' pixels will appear. This generally gives the appearance of increased quality.
	- This setting should be the same the PPU used in creating the sprites for your boundary.
- ScaleModifier
	- Applies a scalar multiplier to the transform.scale of the SpriteBorder
- OrderInLayer
	- What order in the 'boundary' layer should the SpriteBorder be.
	- This setting will allow for sprites in the 'boundary' layer to render on top or below the generated SpriteShape.
- CornerThreshold
	- This setting defines when a point should be defined as a corner. The value represents an angle. The larger this value, the larger the angle between two control points must be in order for them to be considered a corner.
	- This setting has a great effect on what sprite in the sprite shape profile is used relative to the angle between two control points
	- See Unity's documentation on [SpriteShapeController](https://docs.unity3d.com/Packages/com.unity.2d.spriteshape@10.0/manual/SSController.html). for more detail documentation.
- IsSplineAdaptive
	- This is enabled by default and is an intrinsic parameter to the SpriteShapeController
	- Per Unity documentation: "When enabled, Unity attempts to seamlessly tile the Sprites along the Sprite Shape path by deforming the Sprites to between Control Points. Disable this property to tile Sprites with no deformation and at their exact width. Sprites may appear cutoff if the space between Control Points is shorter than the width of the Sprite."
- Open/Closed
	- If open, both ends of the sprite shape will join to form an enclosed boundary. If set to closed, the first and last points in the sprite shape will not be connected
- SetTangents
	- Whether tangents should be set. This allows you to "blend" each sprite shape control point with the next.
- UseFillTesselation
	- This is an optional setting. It utilizes Unity's C# jobs to offload memory and CPU work. This setting helps with rapid generation of maps.
	- The default generation uses [LibNess.NET](https://github.com/speps/LibTessDotNet).
- SplineTangentMode
	- This setting helps in how the sprite shape transitions from one point to the next. This setting more specifically sets and defines how tangents appear at each control point.
	- Linear
		- There is no "smooth" transition between each point in the sprite shape. Each point is connected linearly.
	- Continuous
		- This setting will "smooth" the transitions to and from each point. This option provides to most natural looking boundaries. The angle between two tangents is also 180 degrees
	- Broken
		- Similar to continuous, but the angle does not have to be 180 degrees. This option allows you to define the angles of each tangent.
- Profile
	- You can read the documentation on sprite shape profiles [here](https://docs.unity3d.com/Packages/com.unity.2d.spriteshape@3.0/manual/SSProfile.html).
	- This setting takes a serialized SpriteShapeProfile and uses it in generating the boundaries for generated maps.
	- Per Unity documentation: "The **Sprite Shape Profile** contains the settings that determine which Sprites that appear on a Sprite Shape at specific Angle Ranges, as well as other display settings. You can use the same Profile for multiple **Sprite Shapes** in a Scene"
- EdgeMaterial
	- The material to use for the edge of the SpriteShapeProfile
- FillMaterial
	- The material to use to fill the SpriteShape per the SpriteShapeProfile
 
### Serialization

- What can be serialized is defined in the ProceduralGenerationConfig under the "Serialize & Deserialize" setting tab. 
- Each generated map is assigned a "Name-Seed-Iteration" identifier.
	- This identifier is stored in a text file called "seedTracker"
	- This file by default is stored under Assets -> SerializedData

<p align="center">
<img  src="https://i.imgur.com/0E8ApPp.png" width="350"/>
</p>

- Inside the seedTracker.txt file lies all generated map identifiers. The serializer looks for this identifier when deserializing data.

##### Name-Seed-Iteration Anatomy

- All Name-Seed-Iterations follow the same format:

<p align="center">
<img  src="https://i.imgur.com/Ls0YCun.png" width="200"/>
</p>

- "test-map" is defined in the 'MapNameId' setting under "Setup"
- "m2b7a9yztzkf4g" is the 'Seed' setting under "Setup"
- "uid-0" signifies a unique iteration identifier.
	- This value is always "uid-0" when "UseRandomSeed" is set to 'Yes'
	- When this setting is set to 'No', the iteration number replaces '0' and the 'MapNameId' & 'Seed' value remain the same

##### Generated Data

- Generated data can optionally (but recommended) to be serialized as JSON. 
	- IF you don't mind allocated memory on your HDD/SSD, a prefab of the generated map can work. This is not recommended. Runtime deserialization is very fast.
- Data is stored in a folder with the name equivalent to "Name-Seed-Iteration"

 <p align="center">
<img  src="https://i.imgur.com/BqgtfIa.png" width="200"/>
</p>

- These folders contain JSON files for the following:
	1) AstarGraph
		- Creating, modifying and scanning pathfinding graphs are expensive. Once this data is generated, it is simply cached and recalled at the time of deserialization.
	2) ColliderCoords
		- Vector2 coordinates that are used in generating colliders, pathfinding and SpriteShape borders.
	3) Mesh
		- dfafd
	4) SpriteShape
		- dafafs


### Deserialization

> Deserialization works just like serialization; choose the NameSeedIteration and what data you want to deserialize.
> Deserialization is typically done at runtime. Not everything needs to be deserialized at runtime. Select what data you want to persist in your scene; this data does not need to be deserialized again.
> The only data that needs (should) to be deserialized every time at runtime is the pathfinding data.

### Demo GIFs

_For more examples, please refer to the [Documentation](https://example.com)_

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<br/>

<!-- ROADMAP -->
# Roadmap
At this point in time , I do not intend to further this generator. This project was intended for personal learning. I will be more than happy to implement a feature request (within reason).

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- CONTRIBUTING -->
# Contributing

Contributions are absolutely welcome. This is an open source project. 

1. Fork the repository
2. Create a feature branch
```Shell
git checkout -b feature/your-feature-branch
```
3. Commit changes on your feature branch
```Shell
git commit -m 'Summary feature'
```
4. Push your changes to your branch
```Shell
git push origin feature/your-feature-branch
```
5. Open a pull request to merge/incorporate your feature

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- LICENSE -->
# License

Distributed under the MIT License.

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- CONTACT -->
# Contact

<p align="center">
<b><u>RyanIO</u></b> 
<br/><br/> 
<a href = "mailto:ryan.io@programmer.net?subject=[RIO]%20Procedural%20Generator%20Project" >[Email]</a>
<br/>
[LinkedIn]
<br/>
<a href="https://github.com/ryan-io">[GitHub]</a></p>

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ACKNOWLEDGMENTS -->
# Acknowledgments and Credit

* [Sebastian Lague's YouTube Channel](https://www.youtube.com/@SebastianLague)
* [Aron and the A* Pathfinding Project](https://arongranberg.com/astar/)
* [Pepijn Willekens's EasyWallCollider](https://twitter.com/PepijnWillekens)
* [Bruno from More Mountains](https://moremountains.com/unity-assets)

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/github_username/repo_name.svg?style=for-the-badge
[contributors-url]: https://github.com/github_username/repo_name/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/github_username/repo_name.svg?style=for-the-badge
[forks-url]: https://github.com/github_username/repo_name/network/members
[stars-shield]: https://img.shields.io/github/stars/github_username/repo_name.svg?style=for-the-badge
[stars-url]: https://github.com/github_username/repo_name/stargazers
[issues-shield]: https://img.shields.io/github/issues/github_username/repo_name.svg?style=for-the-badge
[issues-url]: https://github.com/github_username/repo_name/issues
[license-shield]: https://img.shields.io/github/license/github_username/repo_name.svg?style=for-the-badge
[license-url]: https://github.com/github_username/repo_name/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/linkedin_username
[product-screenshot]: images/screenshot.png
[Next.js]: https://img.shields.io/badge/next.js-000000?style=for-the-badge&logo=nextdotjs&logoColor=white
[Next-url]: https://nextjs.org/
[React.js]: https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB
[React-url]: https://reactjs.org/
[Vue.js]: https://img.shields.io/badge/Vue.js-35495E?style=for-the-badge&logo=vuedotjs&logoColor=4FC08D
[Vue-url]: https://vuejs.org/
[Angular.io]: https://img.shields.io/badge/Angular-DD0031?style=for-the-badge&logo=angular&logoColor=white
[Angular-url]: https://angular.io/
[Svelte.dev]: https://img.shields.io/badge/Svelte-4A4A55?style=for-the-badge&logo=svelte&logoColor=FF3E00
[Svelte-url]: https://svelte.dev/
[Laravel.com]: https://img.shields.io/badge/Laravel-FF2D20?style=for-the-badge&logo=laravel&logoColor=white
[Laravel-url]: https://laravel.com
[Bootstrap.com]: https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white
[Bootstrap-url]: https://getbootstrap.com
[JQuery.com]: https://img.shields.io/badge/jQuery-0769AD?style=for-the-badge&logo=jquery&logoColor=white
[JQuery-url]: https://jquery.com
