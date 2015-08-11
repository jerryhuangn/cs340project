# Requirements #

Add functionality to Add a node to a hypercube.


# Details #

  * There is no centralized controller.
  * given an address, all nodes with smaller values must exist.

### Algorithm for finding a node ###

```
  * c  = child
  * p  = parent
  * os = opposite surrogate (complement)
  * on = opposite natural (complement)
  * n  = neighbor
```

---


If you find and Edge Node that points to a surrogate, Insert is between Edge's Parent +1 and the surrogate.

  1. contact node and state that you want to be its child
  1. create node with binary value with leading 1 (ex: 00 => parent of 10)
  1. parent passes surrogates and natural neighbors
  1. new node contacts all the surrogates and natural neighbors and tells them to update (?)