# Tower of Hanoi dev note

## Issues
The game of 'Tower of Hanoi'(as TOH) is a very good sample of implementation of the stack data structure because each pod is a stack that can stack disks. Every disk can be pushed into or popped out of a pod, following the rule of LIFO, Late In First Out. Another rule of TOH is that the In-disk must be smaller than top disk. In this case, the Push() function of normal Stack needed to be modified to comparing the Late-In disk and the peek() disk before pushing in. Code snippet of Push() as below:

``` C#
if(_disks.Count > 0)
{
    Disk topDisk = _disks.Peek();
    if (disk.ID <= topDisk.ID)
    {
        Console.WriteLine("Invalid move: Cannot place a larger disk on top of a smaller disk.");
        return false;
    }
}
```


## Strengthes and weaknesses
The Time Complexity of all Stack functions are O(1), that means no matter push(), pop() or peek() is called, it is very fast and efficient. While the Space Complexity of Stack is O(n), the more element it holds the more space it needs.

Unlike List, stack doesn't provide insert() or get(), we can not allocate an element of any position other than the top. We'll need to find our own way to iteria the stack. That means it take more time and more resource if we need to access certain elements of Stack.

## Real-world application
In most the programming language, the executing statements and primaritive variable are usually stored in a Stack for fast-access. Latest called function will be pushed into the top of Stack, when finished, it will be popped and return to previous function in the stack.

## Worst-case complexity
Push(), Pop() and Peek() have a same Time complexity of O(1), and Space complexity of O(n). There is not such a worst-case senario.