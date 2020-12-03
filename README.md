# Voxel
 
Voxel generation on Unity

## Voxels following spheres
Create cubes with a defined _cubeSize_. For each cube, if the cube or a part a the cube is in a sphere, display it.
![voxels](https://github.com/LorenzoMarnat/Voxel/blob/main/spheresToVoxel.PNG)

## Voxels with octrees
Create cubes with a defined _cubeSize_. On user input, each cube is divided in smaller cubes. Only cubes with at least an edge in the sphere are displayed. Cubes wich are fully in the sphere are not divided.
![octree](https://github.com/LorenzoMarnat/Voxel/blob/main/octree.gif)

## Voxles with potential
Create cubes with a defined _cubeSize_ and with potentials. The closest a cube is to the spheres, the higher is its potential. We can filter cubes displayed by choosing potential limit (cubes with potentials under limit are not displayed).
![potential](https://github.com/LorenzoMarnat/Voxel/blob/main/potential.gif)
