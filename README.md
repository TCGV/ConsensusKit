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
