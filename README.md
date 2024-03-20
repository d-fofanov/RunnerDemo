# RunnerDemo

##Goal
- Make myself a template for future projects
- Write a simplistic Runner game with multiple levels and easy to setup powerups

##Key desisions:
- Minimalistic RX implementation used to avoid import of a heavyweight UniRX library import
- UniTask used for async operations due to its convenience
- Binary serializable & encrypted in memory model is used to complicate cheating and simplify model's persistance/replication/etc
- Overall architecture is build with MVC approach in mind: only controllers are intended to be able to write to the model, asmdefs may be used to enforce this, while UI have full access to a read-only version of the model to simplify data access for visualizations
- Specific visuals and gameplay parameters are easiest to get: character is from Unity's starter pack, 'landscape' and coins are made from primitives
