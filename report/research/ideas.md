# Struct clustering

> This is similar to Barnes-Hut algorithms
> https://en.wikipedia.org/wiki/Barnes%E2%80%93Hut_simulation

If we have loads of entities with the same struct in close enough vicinity, could we create a struct which holds the average values of those for numeric values, and then that be used instead of each individual struct, for the system functionality?

eg. for a swarm of ships:

position1
position2
position3
position4
position5

Could we only use

positionAvg 

So that the `MoveShipSystem` only handles the `positionAvg`, and when later needed, the values get propagated into the real values, and the shift over time from when the clustering started to when the clustering ended is applied individually to each position?

If the `positionAvg` moved (2.3, 1.2), could we then:
- work out how much each ship moved, based on its own trajectory and how the cluster moved?
    - if `position2` was originally moving at (-2.3, -1.2), i.e. the opposite direction what would happen?
        - would this require custom clustering code based on what the struct does?

Some questions arise:
- how would we know when to de-cluster?
- if these objects move in a cluster and their structs aren't really updated, how would the entity itself know it is being moved?
    - can we temporarily redirect the entity/systems to read the cluster instead of the objects own struct?

# Data structures

Would hashsets work for this? If a big problem of the paradigm is the lookup of which entities have different combinations of components, could we create a hash from the components on an entity, and then use these as a key?

Then, to find all relevant entities of a system, all it needs to do is go through the hashset, and go to the specified entry with that hash

```c#

HashSet<SetItem> set = new();

struct SetItem{
    int hash;
    DataComponent[] components;
}

```

points against hashsets:
- we need "has at least A and B" whereas this provides "has exactly A and B" functionality for querying.
- hash sets scatter data in memory

# ECS-like

What if instead of revolving the entire engine around ECS, we allow it to be a separate thing, that may be ignored or used as the user pleases.

We can still have some sort of entity table which holds references to components, however components may either be `Component` or `DataComponent`.

Systems will be able to be used to query `DataComponents` (could it be made to work for normal components as well?)

This way, for trivial tasks you can still have a single object handle code and data, whilst for larger stuff you may want to use entities with data components instead.

You will also be able to mix and match these. Hopefully.