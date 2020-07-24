# Achtung die kurve! (clone)
A multiplayer snake like game where you goal is to outlive or kill the opponent.

TO-DO
* [x] Basic game mechanics.
* [x] Enemy player (AI).
* [ ] P2P Multiplayer.
* [ ] Power-ups.

# Bot v1.0 (greedy, survival)

* Set-up: players appear at random positions & initial direction
* Goal: outlive all opponents.
* Agent Reward Function:
  * +0.01 at each step
  * +1.0 To the last alive agent.
  * -1.0 To all dead agents.
* Behavior Parameters:
  * Vector Observation space: Size of 210, corresponding to 17 ray casts (stack of 3) distributed over 60 degrees at max ray length 20.
  * Vector Action space: (Discrete) Size of 3, corresponding to left/right/forward movement.
  * Visual Observations: None
  
![Image](https://github.com/MaksymPylypenko/Achtung-die-kurve-/blob/master/dev.png)

The bot is able to survive in a friendly environment. 
![Image](https://github.com/MaksymPylypenko/Achtung-die-kurve/blob/master/training.png)

However, it fails versus an aggressive opponent. In the image below you may notice that the agent always wants to go clockwise. This can be easily exploited and will lead to an agent's death in the long run.
![Image](https://github.com/MaksymPylypenko/Achtung-die-kurve/blob/master/observation%201.png)
