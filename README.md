# Consistent Hashing, a .Net/C# implementation


This post is an attempt to create a demo/example for consistent hashing in .Net/C#. This is not an in-depth analysis of consistent hashing as a concept. There is a plethora of excellent articles online that does that. Below are a few that helped me to grasp the concept.

https://medium.com/system-design-blog/consistent-hashing-b9134c8a9062

https://itnext.io/introducing-consistent-hashing-9a289769052e

https://medium.com/@sent0hil/consistent-hashing-a-guide-go-implementation-fe3421ac3e8f

This is essentially a walkthrough of the consistent hashing concept from a .net developer’s perspective. As a developer, it has always been very helpful for me to grasp an idea when I create a proof of concept myself from a rudimentary analysis. The aim is to create a consistent hashing algorithm implementation that might help a .Net/C# developer to visualize the process and gain some insight into its inner mechanics.    
In a nutshell, consistent hashing is a solution for the rehashing problem in a load distribution process. In a typical rehashing process, the target node for a key is determined by taking the mod of the key hash value. Here, the divisor is the total number of nodes. For example, if the key hash value is 32 and there are 5 nodes in total, then the target node is calculated as 32 % 5 = 2. It is quite apparent from the process that any change in the total number of nodes will change the target node value for all data keys. Which means addition and removal of nodes from the nodes cluster will result in the rearrangement of the keys across the nodes space. Consistent hashing attempts to solve this problem by minimizing the key rearrangement process so that only a small fraction of the keys needs reassignment. The core idea of consistent hashing is to map all values in a ring-shaped space. It can be considered as a visual representation of the modular arithmetic process utilized by the consistent hashing algorithm. 
In the current example, the following approach is followed:
1.	Ring position is calculated for both node and data key by taking the mod of their individual hash value with the ring space as divisor. The ring space value should be large, greater than the total node count.    

2.	The nodes and keys are mapped on the ring based on the calculated ring position. 

3.	Keys are assigned to the next node in the ring in clock-wise direction (could be anti-clockwise as well). 

Three main methods are defined. 

AddNode – Adding a new node to the hash space.  In this case, the first node on the ring after the node to be added in the clockwise direction is identified. the subset of keys assigned to this node that are less than the node to be added are identified as well.  These set of keys are reassigned to the new node. Note that rest of the keys in the hash space remains unimpacted in this operation. This is the unique advantage of consistent hashing.   
      
RemoveNode – Removing a node from the hash space. Here, the first node on the ring after the node to be removed in the clockwise direction is identified as the target node. Now we simply reassign the keys belonging to the removed node to the target node. Finally, the node in question is removed from the hash space. As before, rest of the keys in the hash space remains unimpacted. Only the keys assigned to the node to be removed is affected.
 
AddKey – Adding a new key to the hash space. We start by calculating the hash value and ring position of the current key. We identify the node in the hash space that is strictly greater than the current key ring position. As per design, this node happens to be the first node in clockwise direction from the current key ring position.  We assign the current key to this node.
