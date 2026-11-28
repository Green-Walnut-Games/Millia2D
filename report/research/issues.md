# Common issues in game development

These are some common performance issues found in game development, which may give developers performance headaches:
- object pooling - spawning and deletion of thousands of objects is very costly - we can make an easy API for this
    - even Void Strife has a basic object pooling mechanism, what if this was implemented In-Engine
- single-threaded loops - most games run like this - can we change this and can we make an easy API for this?
    - could we use ideas from ECS in order to create an API capable of simulating thousands of entities with little performance loss?
- rendering - rendering itself will need to be optimized for rendering so many things
    - how can we expose API to users to allow them to influence how groups of entities are rendered, for example
    - batching, other common techniques used
- pathfinding/AI evaluations