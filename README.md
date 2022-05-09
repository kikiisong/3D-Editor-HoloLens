# 3D-Editor

## Platforms and third party packages:
1. [Unity](https://unity.com) for development. Unity Editor version: 2020.3.20f1. Newer version may not work.
2. [Visual Studio 2019 Community](https://docs.microsoft.com/en-us/visualstudio/releases/2019/release-notes) for development.
3. [MRTK](https://github.com/microsoft/MixedRealityToolkit-Unity/releases) v 2.7.3  for voice command, voice dictation, and hand interaction.
4. [Vuforia](https://library.vuforia.com) for markerless tracking using the Model Target utility from Vuforia.
5. [AsImpL](https://github.com/gpvigano/AsImpL) for runtime custom 3D model import.
6. [Arrow WayPointer](https://assetstore.unity.com/packages/tools/particles-effects/arrow-waypointer-22642?locale=zh-CN) for the arrow 3D model.

## How to build:
1. Install 2020.3.20f1 Unity Editor if not already installed. 
2. Create a new empty 3D project from Unity Hub.
3. Unzip our package (3D-Editor.zip) to the empty 3D project. Overwrite anything if prompted.
4. Switch to the empty 3D project in the Unity Editor, wait for the importing to complete.
5. Open the Assets/Scenes/MenuScene scene.
6. Click the play button to run.

## Sample trainings:
There are some sample trainings in the Assets/Resources/Demo/ folder. After you started our system, go to editor mode, click load and select one of the demos to try.

## Custom 3D Model:

We support runtime custom 3D Model import. In the editor mode, click 3D>Import and then select your .obj 3D Model. Only Wavefront 3D models are supported at the moment.
