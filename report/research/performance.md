# Performance metrics

A performance target I could aim for is 60fps for ~100,000 actors in the scene.

TODO: Try come up with more evaluation metrics/performance targets, especially for things like performance graphs.

## Evaluation Metrics

There is no single evaluation metric which can be created to easily compare game engines. Due to the nature of game engines, a "good" developer may make a game with amazing performance, whilst a "bad" developer may make the same game with much worse performance, but on the same engine.

It is easier to compare engines based on the difficulty, or the process, of setting up specific game scenarios and game designs. For instance, with Unity, you have to learn and understand the Unity DOTS package in order to fully take advantage of it, and because it is such a massive subject, that can be quite difficult.

Where Millia2D can shine is in the ease of creating a simulation-based game. If the engine is focused and the API is focused to deliver good enough results, it may have a reason of existing.

Since the performance of a game in any engine depends on the developers' proficiency with that game engine and hardware, Millia2D should focus on clean, easy to use APIs and less so on raw strength. We can still compare things like frame time or FPS between similar scenarios, but the main target should be to create an engine which is usable and approachable.

## Entity Component System

An ECS will be a data oriented system which acts upon all entities of the same type at once, instead of each entity performing its own calculation.

ECS is generally thought of as a composition-over-inheritance pattern. Instead of deeply nested class hierarchies often used in OOP, ECS would provide a data-oriented design, which can provide great benefits to performance, and separation of concerns

### Unity DOTS

Unity DOTS uses the Entity Component System (ECS) design pattern. This is most often used in games, but sometimes used outside of the gaming industry.
Inside DOTS, entities and components compose are used for data, and systems for code interacting with that data.

> From Unity DOTS book

An entity is composed of components, where each component is usually C# structs. Components can be added/removed from an entity over its lifetime.

Unlike traditional object-oriented patterns in game programming, entities or their components do not usually have their own methods.
Instead, each "system" has an update method invoked once per frame, and these updates will read and modify the components of the desired entities.

For example, in a monster game, there may be a system called `MonsterMoveSystem` which modifies the Transform components of every monster entity.

#### Chunks and archetypes

In Unity's DOTS, each variation of components on an entity is called an "archetype", and each archetype is stored together in memory. Adding a component to an entity or removing a component from an entity moves the entity to a different archetype.

![alt text](image.png)

Within an archetype, entities and their components are stored in blocks of memory called chunks. Each chunk stores up to 128 entities, and are stored in their own array in the chunk.

![[Pasted image 20261119223543.png]]

The benefit of this system is incredibly fast querying over large numbers of entities. Also, because the arrays are tightly packed, the amount of cache misses decreases greatly, increasing efficiency and the potential ceiling of performance. 

Caching can also be used to further speed up this process.

##### C# Structs

Generally in C#, developers use `classes`. These are `reference types`, meaning they hold a reference which points to the actual data somewhere else on the heap.

Structs are `value types`. Instead of holding a pointer to the data, these **are** the data, and are stored on the stack.

The stack offers better cache locality, and overall better performance so structs should generally be used in high-performance scenarios. However, there are certain cases which they aren't the best for.

If a struct is quite large in size, copying its values may take some time and thus decrease performance, so it is recommended that structs are kept small and lightweight.

#### Memory

A very good target is having components of the same type be contiguous in memory - making iterating through all entities with that component incredibly easy. This is why Unity DOTS's approach below works so well - their table like data structure allows extremely easy querying.

> Harkonen: "The ideal layout can be achieved by breaking objects into different components, and
> grouping those components together in the memory by their type. This results in
> homogenous and sequentially processable data."

#### Multithreading

DOD (Data Oriented Design) has a big benefit when it comes to separation of concerns and multithreading. Classically, OOP struggles to take full advantage of multithreading sometimes - differenet components may be using the same data and there may be conflicts, making working on multiple threads much harder.

In DOD and ECS especially, it can be a lot easier splitting functionality into different threads.

> Maybe in the benchmark game, the people simulation is done on one thread, whilst the ship simulation is done on another. Then the master thread combines the result at the end of each frame/simulation tick?

### Unity DOTS

Unity DOTS brings in the C# Job System. From their Unity DOTS book, the C# job system is described with the following:

"
- The job system maintains a pool of worker threads, one for each additional core of the
target platform. For example, when Unity runs on eight cores, it creates one main thread
and seven worker threads.
- The worker threads execute units of work called jobs. When a worker thread is idle, it
pulls the next available job from the job queue to execute.
- Once a job starts execution on a worker thread, it runs to completion (in other words,
jobs are not preempted).
" (page 10)

Example code:
```c#
// A simple example job that multiplies the
// elements of two arrays.
// Implementing IJob makes this struct a job type.
struct MyJob : IJob
{
    // A NativeArray is “unmanaged”, meaning it
    // isn’t garbage collected.
    public NativeArray<float> Input;
    public NativeArray<float> Output;
    // The Execute method is called when the
    // job system executes this job.
    public void Execute()
    {
        // Multiply every value in Output by the
        // corresponding value in the Input array.
        for (int i = 0; i < Input.Length; i++)
        {
            Output[i] *= Input[i];
        }
    }   
```

The C# Jobs System is mainly a way for users to perform calculations and processes in a multithreaded way, because generally C# and Unity encourage single-threaded design - that's just what they were made for and that's why they're so easy. Everything gets garbage collected for you.

A common downfall when it comes to performance is methods having to be loaded from memory many times, if there is one object type that is spawned a lot in the game. As an example, bullets - these often have classes on each one, and when methods run on each bullet, they have to be loaded from memory. Once we have a couple hundred bullets loaded, a couple hundred methods have had to be loaded, and these bullets are transient, meaning they spawn and despawn and respawn yet again, almost in an endless cycle.
One solution to this that is a common fix is object pooling - instead of creating a bullet and then destroying it, we would like to create the bullets at load time, and then re-use them when necessary. If a bullet is no longer used, we can deactivate it and hide it, instead of removing it. This keeps the bullet in memory and removes the need for loading it and its methods from scratch.

