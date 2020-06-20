# ConsensusKit

The consensus problem requires agreement among a number of processes (or agents) for a single data value. Some of the processes (agents) may fail or be unreliable in other ways, so consensus protocols must be fault tolerant or resilient. The processes must somehow put forth their candidate values, communicate with one another, and agree on a single consensus value.

A consensus protocol must satisfy the following:

**Termination**
* Eventually, every correct process decides some value.

**Integrity**
* If all the correct processes proposed the same value *v*, then any correct process must decide *v*.

**Agreement**
* Every correct process must agree on the same value.

In this repository you can find implementations for the following algorithms:

* [Chandra–Toueg](https://github.com/TCGV/ConsensusKit/tree/master/Tcgv.ConsensusKit/Algorithms/ChandraToueg)
* [Ben-Or](https://github.com/TCGV/ConsensusKit/tree/master/Tcgv.ConsensusKit/Algorithms/BenOr)
* [Basic Paxos](https://github.com/TCGV/ConsensusKit/tree/master/Tcgv.ConsensusKit/Algorithms/Paxos)
* [Nakamoto Consensus](https://github.com/TCGV/ConsensusKit/tree/master/Tcgv.ConsensusKit/Algorithms/Nakamoto)

## Sources

[1] Wikipedia. [Chandra–Toueg consensus algorithm](https://en.wikipedia.org/wiki/Chandra%E2%80%93Toueg_consensus_algorithm).

[2] Marcos Kawazoe Aguilera, Sam Toueg. [Correctness Proof of Ben-Or’s Randomized Consensus Algorithm](https://ecommons.cornell.edu/bitstream/handle/1813/7336/98-1682.pdf?sequence=1). 1998.

[3] Leslie Lamport. [Paxos Made Simple](https://lamport.azurewebsites.net/pubs/paxos-simple.pdf). 2001.

[4] James Aspnes. [Paxos - Class notes](https://www.cs.yale.edu/homes/aspnes/pinewiki/Paxos.html). 2003 to 2012.

[5] Jianyu Niu, Chen Feng, Hoang Dau, Yu-Chih Huang, and Jingge Zhu. [Analysis of Nakamoto Consensus, Revisited](https://arxiv.org/abs/1910.08510). 2019.

## Licensing

This code is released under the MIT License:

Copyright (c) TCGV.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
