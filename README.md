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
<details>
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
<img  src="https://i.imgur.com/VO1LgrH.png">
</p>

> Copy & paste the GIT url for the package you are installing and click "Add". You will need to navigate to "Packages/manifest.json" in your file explorer.

### Manifest Installation
> To install a package via a package manifest, open or create a project in Unity. 
> Navigate to Packages/manifest.json in your file explorer

<p align="center">
<img  src="https://i.imgur.com/GCzGPou.png">
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
<img  src="https://i.imgur.com/u1sUljZ.png"/>
</p>

##### Using Included Prefab
1. If you are using the project repository (forked or cloned the entire Git repo), then you can also simply add the included Procedural Generator Prefab to a scene.
2. The prefab can be found at 

```Directory
Packages\ryan-io-procedural-generator\Assets\Prefabs\procedural-generator
```

   within the Unity Editor.

<p align="center">
<img  src="https://i.imgur.com/vlJnN5K.png"/>
</p>

### Settings

Settings are divided into two groups: 'Map Configuration' and 'SpriteShape Configuration'. 'Map Configuration' contains settings relevant to the map generation, serialization, deserialization, tiles, pathfinding, colliders, events and more. 'SpriteShape Configuration' contains settings relevent to sprite shape generation.

These configurations are isolated from each other for serialization purposes.

<p align="center">
<img  src="https://i.imgur.com/Zo29Hqy.png"/>
</p>

##### Setup

> Setup is intended for high level configuration of the generator. This is the entry point for this package.

<p align="center">
<img  src="https://i.imgur.com/tcauKhp.png"/>
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

<p align="center">
<img  src="https://i.imgur.com/hMQSyM0.png"/>
</p>

- UseRandomSeed
		- If 'Yes', a seed will be provided for you. If 'No', create your own or use a previously defined seed value
	- Seed
		- The value seed into the generated. Under the hood, is really just a hash code defined in an extension method.
	- LastSeed
		- ReadOnly; the last seed used by the generator
	- LastIteration
		- Readonly; if using the same seed, the iterator will simply increase its value by '1' each time you reuse a seed. This is to prevent overwriting data. Will always be '0' if using random seeds
- Monobehaviors
	- Any required monobehaviors are defined here.
	- At this point in time, the only required monobehavior is the Pathfinder component from the A* Pathfinding Project
	- If this is not set, one will be (try) created. Otherwise, no pathfinding will be calculated
	- Pathfinder
		- Scene reference to a game object that contains the Pathfinder component

<p align="center">
<img  src="https://i.imgur.com/Khho7Fy.png"/>
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

<p align="center">
<img  src="https://i.imgur.com/0bWaEKJ.png"/>
</p>

- Columns
	- The number of columns the map should generate
- Rows
	- The number of rows the map should gneerate
- CellSize
	- Scalar factor for how 'large' a cell should be
	- **Setting this to a value other than '1' make a few algorithms run with a quadratic time complexity
- **Columns X Rows = TotalNumberOfCells
- BorderSize
	- An optional border around the map
- SmoothingIterations
	- How many times to run all algorithms on a generated map
	- There are diminishing returns for high values, and not post-processing for lower values.
	- 'Correct' vales can give a more a map with a more purposeful layout
- WallRemovalThreshold
	- How many cells can be in a cluster that do NOT enclose open space (think a room).
	- And example of this:

<p align="center">
<img  src="https://i.imgur.com/urRNijf.png"/>
</p>

   - The larger the number, the less stand alone walls there will be. The small the number, the more there will be. This can make maps feel and appear more dense. 
   - Lower numbers are great if you want to a lot of obstacles in your map(s).
 
##### WallRemovalThreshold = 50

<p align="center">
<img  src="https://i.imgur.com/zzWnbZk.png"/>
</p>

##### WallRemovalThreshold = 3000

<p align="center">
<img  src="https://i.imgur.com/YUbKq0B.png"/>
</p>

- RoomRemovalThreshold
	- Analogous to WallRemovalThreshold, but for rooms
	- A room is defined as open/free map space that is enclose by a wall.
	- An example of a room:

<p align="center">
<img  src="https://i.imgur.com/MsQNy0m.png"/>
</p>

   - Notice the area circle in 'red' is not closed off. This is an example of a passage
   - **Rooms are not entirely isolated from other rooms. One of the generator's constraints is to create maps where all rooms are connected.

##### RoomRemovalThreshold = 50

<p align="center">
<img  src="https://i.imgur.com/5PqSAGA.png"/>
</p>

##### RoomRemovalThreshold = 3000
<p align="center">
<img  src="https://i.imgur.com/pEJPZzg.png"/>
</p>
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
##### WallFillPercentage = 47
<p align="center">
<img  src="https://i.imgur.com/yvdADGD.png"/>
</p>
##### WallFillPercentage  = 45
<p align="center">
<img  src="https://i.imgur.com/yo5kYrk.png"/>
</p>
##### WallFillPercentage = 40
<p align="center">
<img  src="https://i.imgur.com/C4dG53v.png"/>
</p>
##### WallFillPercentage = 49
<p align="center">
<img  src="https://i.imgur.com/jp8o0Uf.png"/>
</p>
##### WallFillPercentage = 55
<p align="center">
<img  src="https://i.imgur.com/M3wDv9v.png"/>
</p>
- CorridorWidth
	- How wide or narrow each connected room passageway is
	- Below is an example with CorridorWidth => {3, 5}
		- SOME passageways are circled in 'red'.

<p align="center">
<img  src="https://i.imgur.com/cdhfyEz.png"/>
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
<img  src="https://i.imgur.com/61Foupt.png"/>
</p>

### Generation
### Serialization
### Deserialization
### Demo GIFs

_For more examples, please refer to the [Documentation](https://example.com)_

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<br/>

<!-- ROADMAP -->
# Roadmap
At this point in time, I do not intend to further this generator. This project was intended for personal learning. I will be more than happy to implement a feature request (within reason).

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
