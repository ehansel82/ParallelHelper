# ParallelHelper
Library that utilizes the TPL to assist with common programming needs normally handled on one thread.

## ParallelProcessor
Class that utilizes multi-threading to efficiently process data when a low memory footprint is needed. 

### Use case 1:

Say we need to read 100,000 records, perform some processing on each record, and insert or update records in another database. 
Sequentially reading this entire list into memory, and then processing each record, will take a while and use significant memory on 
the server.

The memory could be managed by reading and processing "chunks" of data at a time, perhaps through a skip/take query. 
The processing of each chunk could then be run in a Parallel loop to then increase throughput.

ParallelProcessor takes these improvements a step further.  The concept is that one thread is utilized to process a chunk
of data, while another thread is reading in a new chunk to proces next. A switching mechanism is utilize so that two threads
are costantly "busy" with one thread reading and one thread pocessing to maximize throughput.
