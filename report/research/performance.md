# Performance metrics

A performance target I could aim for is 60fps for ~100,000 actors in the scene.

## Entity Component System

An ECS will be a data oriented system which acts upon all entities of the same type at once, instead of each entity performing its own calculation.

## Multithreading

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

