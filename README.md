# ParallelHelper
Library that utilizes the TPL to assist with common programming needs normally handled on one thread.

## ParallelProcessor
Class that utilizes multi-threading to efficiently process data when a low memory footprint is needed. 

### Use case:

Say we need to read 100,000 records, perform some processing on each record, and insert or update records in another database. 
Sequentially reading this entire list into memory, and then processing each record, will take a while and use significant memory on 
the server.

The memory could be managed by reading and processing "chunks" of data at a time, perhaps through a skip/take query. 
The processing of each chunk could then be run in a Parallel loop to further increase throughput.

ParallelProcessor takes these improvements a step further.  The concept is that one thread is utilized to process a chunk
of data, while another thread is simultaneously reading in a new chunk that will be processed next. A switching mechanism is utilized so that two threads are costantly "busy" with one thread reading and one thread pocessing to maximize throughput.

### Setup:
The unit test project can be reviewed to show an example of setup; however the basics are as follows.
+ Create a new instance of ParallelProcessor.
+ Assign a function that returns a List of the objects you wish to process to the LoadData delegate.
+ Assign a routine that processes an individual object to the ProcessData delegate.
+ Make a call to Start()

Start() begins reading and processing data per the description in the above Use Case. It will invoke the LoadData delegate
indefinitely until the delegate does not return new records. As mentioned some mechanism needs to be in place in the function 
referenced by LoadData to ensure it returns proper subsets of data and, if desired, completes by returning nothing. 
