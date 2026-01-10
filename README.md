# RWing airplane simulator

## Overview
The present Unity project represents open source Boeing 737-800 single player simulator with the georeferenced world with the following functionality:
* 3rd person view of an aircraft from several views with orbiting
* 2D user interface mimicking basic real cockpit controls (ailerons, flaps, spoilers, gears, rudder, elevator, reverses) and flight instruments (artificial horizon)
* Initially an aircraft is spawned at Saint-Petersburg, Pulkovo airport (LED), runway 28R
* Basic weather control (wind speed and direction, day time, clouds height and type)
* Physical motion of an aircraft is calculated based on an effective action of forces applied to different parts of an aircraft in including engines, tires, wings and other surfaces. Aerodynamic forces are calculated based on Cx, Cy curves and angle of atack

## Gameplay
A single player 3rd person view simulator without missions or tasks. An airplane is controlled via mouse and keyboard. For more information about controls and weather settings press Escape during the game. Terrain in the scene is relied on Cesium Georeference System (https://cesium.com/learn/unity/) therefore Internet connection is required

## Technical stack
* Unity 3D (6000.3.2f1)
  * Cinemachine
  * Cesium
  * Input System
  * HDRP

##  Contrinuting to the project
Any contribution to the project will be appreciated and welcomed. 

LinkedIn: https://www.linkedin.com/in/nikita-davydov-chemist/

Telegram: @nikitadavydov


