<a name="readme-top"></a>
<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]



<!-- PROJECT LOGO -->

<p align="center">
<img width="300" height="125" src="C:\Development\[portfolio-website]\_logo\ryan-io-high-resolution-logo-transparent.png">
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
<br/>

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
<br/>

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
<br/>

# Built With
- Unity Game Engine
- JetBrains Rider
- Odin Inspector

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<br/>

<!-- GETTING STARTED -->
# Getting Started
This project can be used as a new, standalone Unity project, installed into an existing project using UPM, or manually imported into a Unity project. 

It needs to be stated that this generator is opinionated in how it is setup within a Unity scene. See the 'Usage' section for more information regarding this.

The generator is setup to run out of the box. You are more than welcome to define your own generation process describe in the 'Usage' section. 

It is opinionated. One of the primary goals was to create a new scene, import the generator and have the scene functionally ready within a few minutes. As such, I opted to implore the use of some outside projects that are defined in the 'Prerequisites and Dependencies' section.

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
<br/>

# Installation
> The generator and many dependencies can be installed via the UPM, a package manifest, or the asset store.

<br/>

### UPM Installation
> To install a package via UPM, open or create a project in Unity and open the Package Manager. Click "Add packge from git URL"
> ![[Pasted image 20231207153832.png]]
> Copy & paste the GIT url for the package you are installing and click "Add". You will need to navigate to "Packages/manifest.json" in your file explorer.

<br/>

### Manifest Installation
> To install a package via a package manifest, open or create a project in Unity. 
> Navigate to Packages/manifest.json in your file explorer
> ![[Pasted image 20231207154432.png]]
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

<br/>

### UniTask
```UPM
 https://github.com/Cysharp/UniTask
```

```Manifest
"com.cysharp.unitask": "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask"
```

<br/>

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
<br/>

### EasyWallCollider
```AssetStore
https://assetstore.unity.com/packages/tools/physics/easy-wall-collider-158206
```

<br/>

### Addressables
```
"com.unity.addressables": "1.19.19",
```

<br/>

### Burst
```
"com.unity.burst": "1.6.5",
```

<br/>

### URP
```
"com.unity.render-pipelines.universal": "12.1.6",
```

<br/>

### 2D Sprites
```
"com.unity.feature.2d": "2.0.0",
```

<br/>

### Collections
```
"com.unity.collections": "1.2.3",
```

<br/>

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

<br/>

<!-- USAGE EXAMPLES -->
# Usage

### Settings
### Generation
### Serialization
### Deserialization
### Demo GIFs

_For more examples, please refer to the [Documentation](https://example.com)_

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<br/>

<!-- ROADMAP -->
# Roadmap
At this point in time, I do not intend to further this generator. This project was intended for personal learning.

<p align="right">(<a href="#readme-top">back to top</a>)</p>
<br/>


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

<br/>


<!-- CONTACT -->
# Contact

<p align="center">
<b><u>
RyanIO</u></b> <br/> <a href = "mailto:ryan.io@programmer.net?subject=[RIO]%20Procedural%20Generator%20Project" >[Email]</a>
<br/>
<br/>
[LinkedIn]
<br/>
[GitHub]
</p>

<p align="right">(<a href="#readme-top">back to top</a>)</p>
<br/>


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
